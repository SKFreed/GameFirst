using PixelCrew.Components;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

namespace PixelCrew
{
    public class Hero : MonoBehaviour
    {
        [SerializeField] private float _speed;
        [SerializeField] private float _jumpSpeed;  
        [SerializeField] private float _damageJumpSpeed;
        [SerializeField] private int _damage;

        [SerializeField] private LayerMask _groundLayer;
        [SerializeField] private float _groundCheckRadius;
        [SerializeField] private Vector3 _groundCheckPositionDelta;

        [SerializeField] private float _interactionRadius;
        private Collider2D[] _interactionResult = new Collider2D[1];
        [SerializeField] private LayerMask _interactionLayer;
        [SerializeField] private CheckCircleOverlap _attackRange;

        [SerializeField] private AnimatorController _armed;
        [SerializeField] private AnimatorController _disarmed;

        [Space] [Header ("Particles:")]
        [SerializeField] private SpawnComponent _spawnStepParticles;
        [SerializeField] private ParticleSystem _hitParticles;
        [SerializeField] private SpawnComponent _spawnJumpParticles;
        [SerializeField] private SpawnComponent _spawnFallParticles;

        private Vector2 _direction;
        private Rigidbody2D _rigidbody;
        private Animator _animator;
        private bool _isGrounded;
        private bool _allowDoubleJump;


        private static readonly int _isGroundKey = Animator.StringToHash("is-ground");
        private static readonly int _isRunningKey = Animator.StringToHash("is-running");
        private static readonly int _velocityVerticalKey = Animator.StringToHash("vertical-velocity");
        private static readonly int _hit = Animator.StringToHash("hit");
        private static readonly int _attack = Animator.StringToHash("attack");

        private float _coin = 0;
        public bool _isMoving;
        private float _slamDownVelocity = 1f;
        private bool _isArmed;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _animator = GetComponent<Animator>();
        }
        public void SetDirection(Vector2 direction)
        {
            _direction = direction;
            
            if(_direction.x != 0)
            {
                _isMoving = true;
            }
            else 
            { _isMoving = false; }
        }
        private void Update()
        {
            _isGrounded = IsGrounded();
        }
        private void FixedUpdate()
        {   
            /*if (_isGrounded == true && _allowDoubleJump == false)
            {
                SpawnFallDust();
            }*/

            var xVelocity = _direction.x * _speed;
            var yVelocity = CalculateYVelocity();
            _rigidbody.velocity = new Vector2(xVelocity, yVelocity);                     
            _animator.SetFloat(_velocityVerticalKey,_rigidbody.velocity.y);
            _animator.SetBool(_isGroundKey, _isGrounded);
            _animator.SetBool(_isRunningKey,_direction.x != 0);
            UpdateSpriteDirection();     
           
        }
        private float CalculateYVelocity()
        {
            
            var isJumpPressing = _direction.y > 0;
            var yVelocity = _rigidbody.velocity.y;
            if (_isGrounded)
            {
                _allowDoubleJump = true;
            }
            
            if (isJumpPressing)
            {
                yVelocity = CalculateJumpVelocity(yVelocity);                
            }
            return yVelocity;
        }
        private float CalculateJumpVelocity(float yVelocity)
        {
            var isFalling = _rigidbody.velocity.y <= 0.1f;
            if (!isFalling) return yVelocity;
            if (_isGrounded) 
            {
                yVelocity += _jumpSpeed;
                SpawnJumpDust();
            }
            else if (_allowDoubleJump)
            {
                yVelocity = _jumpSpeed;
                _allowDoubleJump = false;
                SpawnJumpDust();
            }
            return yVelocity;
        }
        private bool IsGrounded()
        {            
            var hit = Physics2D.CircleCast(transform.position + _groundCheckPositionDelta,_groundCheckRadius,Vector2.down,0,_groundLayer);            
            return hit.collider != null;
        }
#if UNITY_EDITOR //��� ���������� �� ������ ��������� ���� ��� ���������
        private void OnDrawGizmos()
        {
            Handles.color = IsGrounded() ? HandlesUtils.TransparentGreen : HandlesUtils.TransparentRed;
            Handles.DrawSolidDisc(transform.position + _groundCheckPositionDelta, Vector3.forward, _groundCheckRadius);
        }
#endif

        private void UpdateSpriteDirection()
        {
            if (_direction.x > 0)
            {
                transform.localScale = Vector3.one;
               // _sprite.flipX = false;
            }
            else if (_direction.x < 0)
            {
                transform.localScale = new Vector3(-1, 1, 1);
               // _sprite.flipX = true;
            }
        }
        public void AddCoinsToHero(float coin)
        {
            _coin += coin;
            Debug.Log($"������: {_coin}");
        }
        public void TakeDamage()
        {
            _animator.SetTrigger(_hit);
            _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, _damageJumpSpeed);
            if (_coin > 0)
            {
                SpawnCoins();
                Debug.Log($"������: {_coin}");
                
            }
            
        }
        private void SpawnCoins()
        {
            var numCoinsToDispose = Mathf.Min(_coin, 5);
            _coin -= numCoinsToDispose;
            var burst = _hitParticles.emission.GetBurst(0);//������
            burst.count = numCoinsToDispose;
            _hitParticles.emission.SetBurst(0, burst);
            _hitParticles.gameObject.SetActive(true);
            _hitParticles.Play();
        }
        public void Interact()
        {
            var size = Physics2D.OverlapCircleNonAlloc(transform.position, _interactionRadius,_interactionResult,_interactionLayer);
            for (int i = 0; i < size; i++)
            {
                var interactable = _interactionResult[i].GetComponent<InteractableComponent>();
                if (interactable != null)
                {
                    interactable.Interact();
                }
            }
        }
        public void Attack()
        {
            if (!_isArmed) return;
            _animator.SetTrigger(_attack);

        }
        public void OnAttackToEnemy()//������ ����� ����� � ��������
        {
            var objects = _attackRange.GetObjectsInRange();
            foreach(var obj in objects)
            {
                var hp = obj.GetComponent<HealthComponent>();
                if(hp != null && obj.CompareTag("Enemy"))
                {
                    hp.ModifyHealth(_damage);
                }
            }
        }
        public void SpawnFootDust()
        {
            _spawnStepParticles.Spawn();
        }
        public void SpawnJumpDust()
        {
            _spawnJumpParticles.Spawn();
        }
        public void SpawnFallDust()
        {
            _spawnFallParticles.Spawn();
        }
        public void OffParticles()
        {
            _hitParticles.gameObject.SetActive(false);
        }
        private void OnCollisionEnter2D(Collision2D collision)
        {
            /*            if (_isGrounded)
                        {
                            var cotanct = collision.contacts[0];
                            if (cotanct.relativeVelocity.y >= _slamDownVelocity)
                            {
                                SpawnFallDust();
                            }
                        }*/
            if (collision.gameObject.IsInLayer(_groundLayer))
            {
                var cotanct = collision.contacts[0];
                if (cotanct.relativeVelocity.y >= _slamDownVelocity)
                {
                    SpawnFallDust();
                }
            }
        }
        public void ArmHero()
        {
            _isArmed = true;
            _animator.runtimeAnimatorController = _armed;
        }


    }
}

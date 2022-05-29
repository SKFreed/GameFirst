using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PixelCrew.Components
{
    public class TeleportComponent : MonoBehaviour
    {
        [SerializeField] private Transform _destTransform;
        private float _alphaTime = 1;
        private float _moveTime = 1;
        
        public void Teleport(GameObject target)
        {
           // target.transform.position = _destTransform.position;
            StartCoroutine(AnimateTeleport(target));
        }
        private IEnumerator AnimateTeleport(GameObject target)
        {
            var sprite = target.GetComponent<SpriteRenderer>();
            var input = target.GetComponent<PlayerInput>();
            target.GetComponent<Hero>().OffParticles();

            SetLocked(input, true);
            yield return AlphaAnimation(sprite, 0);
            target.SetActive(false);

            yield return MoveAnimation(target);

            target.SetActive(true);
            yield return AlphaAnimation(sprite, 1);
            SetLocked(input, false);
        }
        private IEnumerator AlphaAnimation(SpriteRenderer sprite, float destAlpha)
        {
            var time = 0f;
            var alphaSprite = sprite.color.a;
            while (time < _alphaTime)
            {
                time += Time.deltaTime;
                var progress = time / _alphaTime;
                var tmpAlpha = Mathf.Lerp(alphaSprite, destAlpha, progress);
                var color = sprite.color;
                color.a = tmpAlpha;
                sprite.color = color;

                yield return null;
            }
        }
        private IEnumerator MoveAnimation(GameObject target)
        {
            var moveTime = 0f;
            while (moveTime < _moveTime)
            {
                moveTime += Time.deltaTime;
                var progress = moveTime / _moveTime;
                target.transform.position = Vector3.Lerp(target.transform.position, _destTransform.position, progress);

                yield return null;
            }
        }
        private void SetLocked(PlayerInput input, bool isLocked)
        {
            if (input != null)
            {
                input.enabled = !isLocked;
            }
        }
    }

}

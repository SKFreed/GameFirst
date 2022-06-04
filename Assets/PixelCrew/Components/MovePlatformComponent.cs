using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PixelCrew.Components
{
    public class MovePlatformComponent : MonoBehaviour
    {
        [SerializeField] private Vector3 _point1;
        [SerializeField] private Vector3 _point2;
        [SerializeField] private float _speed;
        private Vector3 _lastPosition;
        [SerializeField] private Hero _hero;
        private bool _onPlat;
        private void Start()
        {
            _lastPosition = _point2;
        }
        
        void Update()
        {
            transform.position = Vector3.MoveTowards(transform.position, _lastPosition,_speed * Time.deltaTime );
            if(Vector3.Distance(_lastPosition, transform.position) < 0.02f)
            {
                _lastPosition = GetPoint();
            }
            
            if (!_hero._isMoving && _onPlat)
            {
                SetChild(_hero);
            }
            else
            {
                ResetChild(_hero);
            }            

        }
        private Vector3 GetPoint()
        {
            if(_lastPosition == _point2) return _point1;
            else return _point2;
        }
        private void SetChild(Hero childTranform)
        {
            childTranform.transform.parent = this.transform;
        }
        private void ResetChild(Hero childTranform)
        {
            if(childTranform.transform.parent != null)
            {
                childTranform.transform.parent = null;
            }
        }    
        public void OnPlat()
        {
            _onPlat = true;
        }
        public void NotOnPlat()
        {
            _onPlat = false;
        }

    }

}

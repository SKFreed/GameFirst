using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static PixelCrew.Components.EnterCollosoinComponent;

namespace PixelCrew.Components
{
    public class ExitCollisionComponent : MonoBehaviour
    {
        [SerializeField] private string _tag;
        [SerializeField] private EnterEvent _action;
        private void OnCollisionExit2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag(_tag))
            {
                _action?.Invoke(collision.gameObject);
            }
        }
        [Serializable]
        public class EnterEvent : UnityEvent<GameObject>
        {

        }
    }

}


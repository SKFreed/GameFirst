using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PixelCrew.Components
{
    public class SwitchComponent : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private bool _state;
        [SerializeField] private string _animationKey;

        public void Switch()
        {
            _state = !_state;
            _animator.SetBool(_animationKey, _state);
        }
        [ContextMenu("Switch")] //  в трёх точках появится этот метод, нажав на него, в нашем сучае, открывается и закрываем дверь
        public void SwitchIt()
        {
            Switch();
        }
    }
}


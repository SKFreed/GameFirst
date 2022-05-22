using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PixelCrew
{
    // Фиксирование камеры на цели, если не подключать Cinemachine
    // В проекте не используется
    public class FollowTarget : MonoBehaviour
    {
        [SerializeField] private Transform _target;
        [SerializeField] private float _damping;
        void LateUpdate()
        {
            var destination = new Vector3(_target.position.x, _target.position.y, transform.position.z);
            transform.position = Vector3.Lerp(transform.position, destination,Time.deltaTime * _damping);
        }
    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace PixelCrew // Запускает цикл при котором меняет спрайты из массива
{
    [RequireComponent (typeof(SpriteRenderer))]
    public class SpriteAnimation : MonoBehaviour
    {
        [SerializeField] private int _frameRate = 10;
        [SerializeField] private bool _loop;
        [SerializeField] private Sprite[] _sprits;
        [SerializeField] private UnityEvent _onComplete;

        private SpriteRenderer _renderer;
        private float _secondsPerFrame;
        private int _currentSpriteIndex;
        private float _nextFrameTime;
        private void Start()
        {
            _renderer = GetComponent<SpriteRenderer>();
            
        }
        private void OnEnable() // Тк настройки находятся здесь, то при запущенной игре можно изменять параметры, затем переподключить компонент и настройки применятся
        {
            _secondsPerFrame = 1f / _frameRate;
            _nextFrameTime = Time.time + _secondsPerFrame;
            _currentSpriteIndex = 0;
        }
        private void Update()
        {
            if ( _nextFrameTime > Time.time) return;
            if (_currentSpriteIndex >= _sprits.Length)
            {
                if (_loop)
                {
                    _currentSpriteIndex = 0;
                }
                else
                {
                    enabled = false;
                    _onComplete?.Invoke();
                    return;
                }
            }
            _renderer.sprite = _sprits[_currentSpriteIndex];
            _nextFrameTime += _secondsPerFrame;
            _currentSpriteIndex++;
        }
    }
}


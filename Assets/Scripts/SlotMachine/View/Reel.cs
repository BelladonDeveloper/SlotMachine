using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SlotMachine.View
{
    using Animators;
    
    public class Reel : MonoBehaviour
    {
        public Action OnStopped;
        
        public int SymbolsCount => _symbols.Count;
        
        [SerializeField] private List<Image> _symbols;

        private ReelAnimator _animator;
        private Sprite[] _sprites;
        private int _currentPrize = 0;
        private int _currentNextPrize = 0;
        
        public void Init(Sprite[] sprites)
        {
            _sprites = sprites;
            _animator = new ReelAnimator();
            _animator.Init(_symbols, _sprites);
        }
        
        public void Show(Sprite[] symbols, int prize)
        {
            _currentPrize = prize;
            for (int i = 0; i < _symbols.Count && i < symbols.Length; i++)
            {
                if (symbols[i] != null)
                {
                    _symbols[i].sprite = symbols[i];   
                }
                else
                {
                    Debug.LogWarning($"No sprite for symbol {i}");
                }
            }
            gameObject.SetActive(true);
        }

        public void Spin(int value)
        {
            _currentNextPrize = value;
            _animator.SpinQuickly(SpinToValue);
        }

        private void SpinToValue()
        {
            _animator.MakeSlowly();
            _animator.SpinOneStep(CheckStopped);
        }

        private void CheckStopped()
        {
            if (_animator.CurrentPrize == _currentNextPrize)
            {
                _currentPrize = _currentNextPrize;
                _currentNextPrize = -1;
                _animator.ResetSpeed();
                
                OnStopped?.Invoke();
            }
            else
            {
                SpinToValue();
            }
        }
    }
}
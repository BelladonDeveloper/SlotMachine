using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SlotMachine.View
{
    using Animators;
    
    public class Reel : MonoBehaviour
    {
        /// <summary>
        /// Called when the reel is stopped after spinning
        /// </summary>
        public Action OnStopped;
        
        /// <summary>
        /// Returns the number of symbols on the reel
        /// </summary>
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
            SetSlotStartPositions();
        }
        
        /// <summary>
        /// Set the start positions of the first and the last slots.
        /// Needs for the correct anchor position
        /// </summary>
        private void SetSlotStartPositions()
        {
            _symbols[0].rectTransform.anchoredPosition += Vector2.up * _symbols[0].rectTransform.rect.height;
            _symbols[^1].rectTransform.anchoredPosition -= Vector2.up * _symbols[^1].rectTransform.rect.height;
        }
        
        /// <summary>
        /// Show the reel with the symbols
        /// </summary>
        /// <param name="symbols"></param>
        /// <param name="middleSymbol"></param>
        public void Show(Sprite[] symbols, int middleSymbol)
        {
            _currentPrize = middleSymbol;
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

        /// <summary>
        /// Start spinning the reel
        /// </summary>
        /// <param name="middleValue"></param>
        public void Spin(int middleValue)
        {
            _currentNextPrize = middleValue;
            _animator.SpinQuickly(SpinToValue);
        }

        /// <summary>
        /// Spin slowly to the target value
        /// </summary>
        private void SpinToValue()
        {
            _animator.MakeSlowly();
            _animator.SpinOneStep(CheckStopped);
        }

        /// <summary>
        /// Approve the stopped state or continue slowly spinning
        /// </summary>
        private void CheckStopped()
        {
            if (_animator.CurrentSymbol == _currentNextPrize)
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
using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace SlotMachine.View.Animators
{
    public class ReelAnimator
    {
        private const float ONE_SPIN_QUICK_DURATION = 0.02f;
        
        /// <summary>
        /// Returns the current symbol number
        /// </summary>
        public int CurrentSymbol => _currentSymbol;
        
        /// <summary>
        /// Invoked when the reel is stopped after spinning
        /// </summary>
        private Action _onComplete;

        private List<Image> _symbols;
        private Sprite[] _sprites;
        
        private int _currentSymbol;
        private int _stepCounter;
        private int _spinCounter;
        private int _spinCount;
        
        private float _spinStepDuration = ONE_SPIN_QUICK_DURATION;

        public void Init(List<Image> symbols, Sprite[] sprites)
        {
            _symbols = symbols;
            _sprites = sprites;
        }

        /// <summary>
        /// Start spinning
        /// </summary>
        /// <param name="onComplete"> Will be invoked after the animation is completed </param>
        public void SpinQuickly(Action onComplete)
        {
            _onComplete = onComplete;
            _stepCounter = 0;
            _spinCounter = 0;
            _spinCount = Random.Range(_symbols.Count, _sprites.Length) * 2;
            SpinOneStep(IncrementStepCounter);
        }

        /// <summary>
        /// One step is only rotate one symbol
        /// </summary>
        /// <param name="onComplete"></param>
        public void SpinOneStep(Action onComplete)
        {
            Sequence spinOneStep = DOTween.Sequence();
            for (int j = 0; j < _symbols.Count - 1; j++)
            {
                spinOneStep.Join(_symbols[j].transform.DOLocalMoveY(
                    _symbols[j + 1].transform.localPosition.y, _spinStepDuration).SetEase(Ease.Linear));
            }

            spinOneStep.OnComplete(OnStepCompleted);
            spinOneStep.Play();
            
            void OnStepCompleted()
            {
                spinOneStep.Kill();
                RearrangeSymbols();
                onComplete?.Invoke();
            }
        }

        /// <summary>
        /// Teleport the last symbol to the top.
        /// Change the sprite of the top symbol
        /// </summary>
        private void RearrangeSymbols()
        {
            Image lastSymbol = _symbols[^1];
            ChangeIndex(lastSymbol);
            ChangeSprite();
            ChangePosition(lastSymbol);
        }

        private void ChangeIndex(Image lastSymbol)
        {
            _symbols.RemoveAt(_symbols.Count - 1);
            _symbols.Insert(0, lastSymbol);
        }

        private void ChangeSprite()
        {
            DecrementCurrentPrize();
            int topPrize = GetTopPrize();
            _symbols[0].sprite = _sprites[topPrize];
        }

        /// <summary>
        /// Determine the next symbol number always in the range of the sprites array
        /// </summary>
        private void DecrementCurrentPrize()
        {
            _currentSymbol--;
            if (_currentSymbol < 0)
            {
                _currentSymbol += _sprites.Length;
            }
        }

        /// <summary>
        /// Search the sprite for the top symbol
        /// </summary>
        /// <returns></returns>
        private int GetTopPrize()
        {
            int topPrize = _currentSymbol - _symbols.Count / 2;
            if (topPrize < 0)
            {
                topPrize += _sprites.Length;
            }

            return topPrize;
        }

        private void ChangePosition(Image lastSymbol)
        {
            Vector3 resetPosition = _symbols[1].transform.localPosition;
            resetPosition.y += lastSymbol.preferredHeight;
            lastSymbol.transform.localPosition = resetPosition;
        }

        /// <summary>
        /// Each step is one symbol rotation
        /// </summary>
        private void IncrementStepCounter()
        {
            _stepCounter++;
            if (_stepCounter < _symbols.Count)
            {
                SpinOneStep(IncrementStepCounter);
            }
            else
            {
                IncrementSpinCounter();
            }
        }
        
        /// <summary>
        /// Each spin is one full rotation of all symbols
        /// </summary>
        private void IncrementSpinCounter()
        {
            _spinCounter++;
            if (_spinCounter < _spinCount)
            {
                _stepCounter = 0;
                SpinOneStep(IncrementStepCounter);
            }
            else
            {
                _onComplete?.Invoke();
            }
        }

        public void MakeSlowly()
        {
            _spinStepDuration += ONE_SPIN_QUICK_DURATION;
        }

        /// <summary>
        /// Set the speed of the reel to the initial value
        /// </summary>
        public void ResetSpeed()
        {
            _spinStepDuration = ONE_SPIN_QUICK_DURATION;
        }
    }
}
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
        
        public int CurrentPrize => _currentPrize;
        
        private Action _onComplete;

        private List<Image> _symbols;
        private Sprite[] _sprites;
        
        private int _currentPrize;
        private int _stepCounter;
        private int _spinCounter;
        private int _spinCount;
        
        private float _spinStepDuration = ONE_SPIN_QUICK_DURATION;

        public void Init(List<Image> symbols, Sprite[] sprites)
        {
            _symbols = symbols;
            _sprites = sprites;
        }

        public void SpinQuickly(Action onComplete)
        {
            _onComplete = onComplete;
            _stepCounter = 0;
            _spinCounter = 0;
            _spinCount = Random.Range(_symbols.Count, _sprites.Length) * 2;
            SpinOneStep(IncrementStepCounter);
        }

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

        public void SpinOneStep(Action onComplete)
        {
            Sequence spinOneStep = DOTween.Sequence();
            for (int j = 0; j < _symbols.Count - 1; j++)
            {
                spinOneStep.Join(_symbols[j].transform.DOLocalMoveY(
                    _symbols[j + 1].transform.localPosition.y, _spinStepDuration).SetEase(Ease.Linear));
            }

            spinOneStep.OnComplete(() =>
            {
                spinOneStep.Kill();
                RearrangeSymbols();
                onComplete?.Invoke();
            });
            spinOneStep.Play();
        }

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

        private void DecrementCurrentPrize()
        {
            _currentPrize--;
            if (_currentPrize < 0)
            {
                _currentPrize += _sprites.Length;
            }
        }

        private int GetTopPrize()
        {
            int topPrize = _currentPrize - _symbols.Count / 2;
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

        public void MakeSlowly()
        {
            _spinStepDuration += ONE_SPIN_QUICK_DURATION;
        }

        public void ResetSpeed()
        {
            _spinStepDuration = ONE_SPIN_QUICK_DURATION;
        }
    }
}
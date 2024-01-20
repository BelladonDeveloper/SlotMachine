using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Random = UnityEngine.Random;

namespace SlotMachine.View
{
    public class Reel : MonoBehaviour
    {
        private const float ONE_SPIN_QUICK_DURATION = 0.02f;
        
        public Action OnStopped;
        
        public int SymbolsCount => _symbols.Count;
        
        [SerializeField] private List<Image> _symbols;
        
        private Sprite[] _sprites;
        private int _currentPrize = 0;
        private int _currentNextPrize = 0;
        private float _spinStepDuration = ONE_SPIN_QUICK_DURATION;
        
        public void Init(Sprite[] sprites)
        {
            _sprites = sprites;
        }
        
        public void Show(Sprite[] symbols, int prize)
        {
            Debug.Log($"Show prize {prize}");
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
            SpinQuickly(SpinToValue);
        }

        private void SpinQuickly(Action onComplete)
        {
            int stepCounter = 0;
            int spinCounter = 0;
            int spinCount = Random.Range(_symbols.Count, _sprites.Length) * 2;
            SpinOneStep(IncrementCounter);

            void IncrementCounter()
            {
                stepCounter++;
                if (stepCounter < _symbols.Count)
                {
                    SpinOneStep(IncrementCounter);
                }
                else
                {
                    spinCounter++;
                    if (spinCounter < spinCount)
                    {
                        stepCounter = 0;
                        SpinOneStep(IncrementCounter);
                    }
                    else
                    {
                        onComplete?.Invoke();
                    }
                }
            }
        }

        private void SpinOneStep(Action onComplete)
        {
            Sequence spinOneStep = DOTween.Sequence();
            for (int j = 0; j < _symbols.Count - 1; j++)
            {
                spinOneStep.Join(_symbols[j].rectTransform.DOAnchorPosY(_symbols[j + 1].rectTransform.anchoredPosition.y, _spinStepDuration).SetEase(Ease.Linear));
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

        private int GetTopPrize()
        {
            int topPrize = _currentPrize - _symbols.Count / 2;
            if (topPrize < 0)
            {
                topPrize += _sprites.Length;
            }

            return topPrize;
        }

        private void DecrementCurrentPrize()
        {
            _currentPrize--;
            if (_currentPrize < 0)
            {
                _currentPrize += _sprites.Length;
            }
        }

        private void ChangePosition(Image lastSymbol)
        {
            Vector2 resetPosition = _symbols[1].rectTransform.anchoredPosition;
            resetPosition.y += lastSymbol.rectTransform.sizeDelta.y;
            lastSymbol.rectTransform.anchoredPosition = resetPosition;
        }

        private void SpinToValue()
        {
            _spinStepDuration += ONE_SPIN_QUICK_DURATION;
            SpinOneStep(CheckStopped);
        }
        
        private void CheckStopped()
        {
            if (_currentPrize == _currentNextPrize)
            {
                _currentNextPrize = -1;
                _spinStepDuration = ONE_SPIN_QUICK_DURATION;
                
                OnStopped?.Invoke();
            }
            else
            {
                SpinToValue();
            }
        }
    }
}
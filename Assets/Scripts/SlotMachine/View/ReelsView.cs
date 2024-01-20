using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlotMachine.View
{
    public class ReelsView : MonoBehaviour
    {
        private const float REELS_START_INTERVAL = 0.5f;
        
        public Action OnAllReelsStopped;

        public int Count => _reels.Count;

        [SerializeField] private List<Reel> _reels;

        private Sprite[] _sprites;

        private int _stoppedReelsCount;

        public void Init(Sprite[] sprites)
        {
            _sprites = sprites;
            _reels.ForEach(r => r.Init(sprites));
        }

        #region SHOW_START_STATE

        public void Show(Dictionary<int, int> nextPrizes)
        {
            for (int i = 0; i < _reels.Count; i++)
            {
                if (nextPrizes.TryGetValue(i, out int prize))
                {
                    ShowReel(_reels[i], prize);
                }
                else
                {
                    Debug.LogWarning($"No next prizes for reel {i}");
                }
            }
        }

        private void ShowReel(Reel reel, int prize)
        {
            int count = reel.SymbolsCount;
            Sprite[] sprites = GetSprites(prize, count);
            reel.Show(sprites, prize);
        }

        private Sprite[] GetSprites(int middle, int length)
        {
            Sprite[] sprites = new Sprite[length];

            int startIndex = middle - length / 2;
            if (startIndex < 0)
            {
                startIndex += _sprites.Length;
            }

            for (int i = 0; i < length; i++)
            {
                int index = (startIndex + i) % _sprites.Length;
                sprites[i] = _sprites[index];
            }

            return sprites;
        }

        #endregion

        #region SPIN

        public void Spin(Dictionary<int, int> nextPrizes)
        {
            StartCoroutine(SpinReelsWithInterval(nextPrizes));
        }

        private IEnumerator SpinReelsWithInterval(Dictionary<int, int> nextPrizes)
        {
            for (int i = 0; i < _reels.Count; i++)
            {
                yield return new WaitForSeconds(REELS_START_INTERVAL);

                if (nextPrizes.TryGetValue(i, out int prize))
                {
                    _reels[i].OnStopped += IncreaseStoppedReels;
                    SpinReel(_reels[i], prize);
                }
                else
                {
                    Debug.LogWarning($"No next prizes for reel {i}");
                }
            }
        }

        private void IncreaseStoppedReels()
        {
            _stoppedReelsCount++;
            if (_stoppedReelsCount == _reels.Count)
            {
                _stoppedReelsCount = 0;
                _reels.ForEach(r => r.OnStopped -= IncreaseStoppedReels);

                OnAllReelsStopped?.Invoke();
            }
        }

        private void SpinReel(Reel reel, int prize)
        {
            reel.Spin(prize);
        }

        #endregion
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Base;
using SlotMachine.View.Animators;
using UnityEngine;

namespace SlotMachine.View
{
    /// <summary>
    /// Show all the reels start state and spin them
    /// </summary>
    public class ReelsView : MonoBehaviour
    {
        private const float REELS_START_INTERVAL = 0.5f;
        
        /// <summary>
        /// Invoked when all the reels are stopped
        /// </summary>
        public Action OnAllReelsStopped;

        /// <summary>
        /// Returns the number of reels in the slot machine
        /// </summary>
        public int Count => _reels.Count;

        [SerializeField] private List<Reel> _reels;
        
        private Sprite[] _sprites;

        private int _stoppedReelsCount;

        /// <summary>
        /// Init all reels
        /// </summary>
        /// <param name="sprites"> All these sprites will be used on reels </param>
        public void Init(Sprite[] sprites)
        {
            _sprites = sprites;
            _reels.ForEach(r => r.Init(sprites));
        }

        #region SHOW_START_STATE

        /// <summary>
        /// Set the start state of the reels
        /// </summary>
        /// <param name="startSymbols"></param>
        public void Show(Dictionary<int, int> startSymbols)
        {
            for (int i = 0; i < _reels.Count; i++)
            {
                if (startSymbols.TryGetValue(i, out int prize))
                {
                    ShowReel(_reels[i], prize);
                }
                else
                {
                    Debug.LogWarning($"No next Symbols for reel {i}");
                }
            }
        }

        /// <summary>
        /// Show the reel with the symbol in the middle
        /// </summary>
        /// <param name="reel"></param>
        /// <param name="symbol"> Symbol in the middle of the reel </param>
        private void ShowReel(Reel reel, int symbol)
        {
            int count = reel.SymbolsCount;
            Sprite[] sprites = GetSprites(symbol, count);
            reel.Show(sprites, symbol);
        }

        /// <summary>
        /// Get the sprites for the each image on the reel object
        /// </summary>
        /// <param name="middle"> Symbol in the middle of the reel </param>
        /// <param name="length"> Images count on the reel object </param>
        /// <returns></returns>
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

        /// <summary>
        /// Spin all reels to the next symbols one by one
        /// </summary>
        /// <param name="nextSymbols"> The symbols will be shown in the middle of reels </param>
        public void Spin(Dictionary<int, int> nextSymbols)
        {
            StartCoroutine(SpinReelsWithInterval(nextSymbols));
        }

        /// <summary>
        /// Set the interval between reels spinning
        /// </summary>
        /// <param name="nextSymbols"></param>
        /// <returns></returns>
        private IEnumerator SpinReelsWithInterval(Dictionary<int, int> nextSymbols)
        {
            for (int i = 0; i < _reels.Count; i++)
            {
                yield return new WaitForSeconds(REELS_START_INTERVAL);
                Register.Get<ISoundManager>().PlaySound(SoundName.ReelSpin);

                if (nextSymbols.TryGetValue(i, out int prize))
                {
                    _reels[i].OnStopped += IncreaseStoppedReels;
                    SpinReel(_reels[i], prize);
                }
                else
                {
                    Debug.LogWarning($"No next Symbols for reel {i}");
                }
            }
        }

        /// <summary>
        /// Count the stopped reels.
        /// </summary>
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

        public void ShowWinAnimation(Action onCompleted)
        {
            _reels.ForEach(r => r.PlayParticles());
            onCompleted?.Invoke();
        }
    }
}
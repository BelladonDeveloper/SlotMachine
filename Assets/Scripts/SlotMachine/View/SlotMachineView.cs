using System;
using System.Collections.Generic;
using UnityEngine;

namespace SlotMachine.View
{
    public class SlotMachineView : MonoBehaviour
    {
        /// <summary>
        /// Called when a player turns the handle
        /// </summary>
        public event Action OnHandleTurned;
        /// <summary>
        /// Called when all the reels are stopped.
        /// And we need to make decision about the win or continue the game
        /// </summary>
        public event Action OnAllReelsStopped;
        
        /// <summary>
        /// Returns the number of reels in the slot machine
        /// </summary>
        public int ReelsCount => _reels.Count;
        
        [SerializeField] private Handle _handle;
        [SerializeField] private ReelsView _reels;
        
        private Sprite[] _sprites;
        
        /// <summary>
        /// Init all components and subscribe to events
        /// </summary>
        /// <param name="sprites"> All these sprites will be used on reels </param>
        public void Init(Sprite[] sprites)
        {
            _sprites = sprites;
            _reels.Init(_sprites);
            _handle.Init();
            _handle.ChangeInteractable(false);
            _handle.OnClick += HandleTurned;
        }

        private void HandleTurned()
        {
            OnHandleTurned?.Invoke();
        }

        #region SHOW_START_STATE

        /// <summary>
        /// Show the slot machine in the start state
        /// </summary>
        /// <param name="currentSymbols"> Symbols for initialize reels </param>
        public void Show(Dictionary<int, int> currentSymbols)
        {
            ShowHandle();
            ShowReels(currentSymbols);
        }
        
        private void ShowHandle()
        {
            _handle.Show();
            _handle.ChangeInteractable(true);
        }
        
        private void ShowReels(Dictionary<int, int> nextSymbols)
        {
            _reels.Show(nextSymbols);
        }

        #endregion

        #region SPIN

        /// <summary>
        /// Spin all reels to the next symbols
        /// </summary>
        /// <param name="nextSymbols"></param>
        public void Spin(Dictionary<int, int> nextSymbols)
        {
            _reels.OnAllReelsStopped += InvokeAllReelsStopped;
            _reels.Spin(nextSymbols);
        }

        private void InvokeAllReelsStopped()
        {
            _reels.OnAllReelsStopped -= InvokeAllReelsStopped;
            OnAllReelsStopped?.Invoke();
        }

        #endregion
        
        /// <summary>
        /// Activate all interactable elements
        /// </summary>
        public void Activate()
        {
            _handle.ChangeInteractable(true);
        }
        
        public void Dispose()
        {
            _handle.OnClick -= HandleTurned;
        }
    }
}
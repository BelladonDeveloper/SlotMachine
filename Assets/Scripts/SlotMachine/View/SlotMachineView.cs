using System;
using System.Collections.Generic;
using UnityEngine;

namespace SlotMachine.View
{
    public class SlotMachineView : MonoBehaviour
    {
        public event Action OnHandleTurned;
        public event Action OnAllReelsStopped;
        
        public int ReelsCount => _reels.Count;
        
        [SerializeField] private Handle _handle;
        [SerializeField] private ReelsView _reels;
        
        private Sprite[] _sprites;
        
        public void Init(Sprite[] sprites)
        {
            _sprites = sprites;
            _reels.Init(_sprites);
            _handle.Init();
            _handle.ChangeInteractable(false);
            _handle.OnClick += TurnHandle;
        }

        private void TurnHandle()
        {
            OnHandleTurned?.Invoke();
        }

        #region SHOW_START_STATE

        public void Show(Dictionary<int, int> nextPrizes)
        {
            ShowHandle();
            ShowReels(nextPrizes);
        }
        
        private void ShowHandle()
        {
            _handle.Show();
            _handle.ChangeInteractable(true);
        }
        
        private void ShowReels(Dictionary<int, int> nextPrizes)
        {
            _reels.Show(nextPrizes);
        }

        #endregion

        #region SPIN

        public void Spin(Dictionary<int, int> nextPrizes)
        {
            _reels.OnAllReelsStopped += ActivateAfterSpin;
            _reels.Spin(nextPrizes);
        }

        private void ActivateAfterSpin()
        {
            _reels.OnAllReelsStopped -= ActivateAfterSpin;
            OnAllReelsStopped?.Invoke();
        }

        #endregion
        
        public void Activate()
        {
            _handle.ChangeInteractable(true);
        }
        
        public void Dispose()
        {
            _handle.OnClick -= TurnHandle;
        }
    }
}
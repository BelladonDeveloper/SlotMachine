using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

namespace SlotMachine
{
    using View;
    using Model;
    using Model.Data;
    
    public class SlotMachineController
    {
        public Action<int> OnWin;
        
        private readonly SlotMachineModel _model;
        private readonly SlotMachineView _view;
        private readonly SlotMachineConfig _config;

        private Dictionary<int, int> _nextPrizes;
        private bool _isOnlySamePrizes;

        public SlotMachineController(SlotMachineModel model, SlotMachineView view, SlotMachineConfig config)
        {
            _model = model;
            _view = view;
            _config = config;
        }

        public void Init()
        {
            _view.OnHandleTurned += HandleTurned;
            _view.OnAllReelsStopped += CheckPrizes;

            Sprite[] sprites = GetSymbolSprites(_config.SymbolsAtlas);
            _view.Init(sprites);
            
            Show();
        }

        private void HandleTurned()
        {
            _nextPrizes = GetNextPrizes();
            _view.Spin(_nextPrizes);
        }

        private void CheckPrizes()
        {
            bool isWin = true;
            
            for (int i = 1; i < _nextPrizes.Count; i++)
            {
                if (_nextPrizes[i] != _nextPrizes[0])
                {
                    isWin = false;
                    break;
                }
            }
            
            if (isWin)
            {
                int rewardAmount = _config.GetRewardAmount((PrizeType)_nextPrizes[0]);
                OnWin?.Invoke(rewardAmount);
            }
            else
            {
                _view.Activate();
            }
        }

        private Sprite[] GetSymbolSprites(SpriteAtlas spriteAtlas)
        {
            Sprite[] sprites = new Sprite[_config.Prizes.Count];
            
            if (spriteAtlas != null)
            {
                for (int i = 0; i < sprites.Length; i++)
                {
                    sprites[i] = spriteAtlas.GetSprite(_config.Prizes[i].ToString());
                }
            }

            return sprites;
        }

        private void Show()
        {
            _nextPrizes = GetNextPrizes();
            _view.Show(_nextPrizes);
        }

        private Dictionary<int, int> GetNextPrizes()
        {
            return _model.GetNextPrizes(_view.ReelsCount, _config.Prizes.Count, _isOnlySamePrizes);            
        }

        public void ActivateAfterReward()
        {
            _view.Activate();
        }
        
        public void GenerateOnlySamePrizes() => _isOnlySamePrizes = true;

        public void Dispose()
        {
            _view.OnHandleTurned -= HandleTurned;
            _view.OnAllReelsStopped -= CheckPrizes;
            _view.Dispose();
        }
    }
}

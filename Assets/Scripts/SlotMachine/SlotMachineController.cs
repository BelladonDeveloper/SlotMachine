using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using Base;

namespace SlotMachine
{
    using View;
    using Model;
    
    public class SlotMachineController : IInitiable
    {
        private readonly SlotMachineModel _model;
        private readonly SlotMachineView _view;
        private readonly SlotMachineConfig _config;

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
            Debug.Log("Handle turned");
            Dictionary<int, int> nextPrizes = GetNextPrizes();
            _view.Spin(nextPrizes);
        }

        private void CheckPrizes()
        {
            
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
            Dictionary<int, int> nextPrizes = GetNextPrizes();
            _view.Show(nextPrizes);
        }
        
        private Dictionary<int, int> GetNextPrizes() => _model.GetNextPrizes(_view.ReelsCount, _config.Prizes.Count);

        public void Dispose()
        {
            _view.OnHandleTurned -= HandleTurned;
            _view.OnAllReelsStopped -= CheckPrizes;
            _view.Dispose();
        }
    }
}

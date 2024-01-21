using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

namespace SlotMachine
{
    using View;
    using Model;
    using Model.Data;
    
    /// <summary>
    /// Control all the logic of the slot machine.
    /// Created as MVC pattern
    /// </summary>
    public class SlotMachineController
    {
        /// <summary>
        /// Invoked when a player wins any prize.
        /// The parameter is the amount of a reward
        /// </summary>
        public Action<int> OnWin;
        
        private readonly SlotMachineModel _model;
        private readonly SlotMachineView _view;
        private readonly SlotMachineConfig _config;

        /// <summary>
        /// Key is the reel number from left to right.
        /// Value is the prize number in the config symbols list
        /// </summary>
        private Dictionary<int, int> _nextSymbols;
        
        /// <summary>
        /// Only for testing.
        /// If true all the symbols will be the same
        /// </summary>
        private bool _isOnlySameSymbols;

        public SlotMachineController(SlotMachineModel model, SlotMachineView view, SlotMachineConfig config)
        {
            _model = model;
            _view = view;
            _config = config;
        }

        /// <summary>
        /// Init all the logic components.
        /// Show the slot machine view
        /// </summary>
        public void Init()
        {
            Sprite[] sprites = GetSymbolSprites(_config.SymbolsAtlas);
            _view.Init(sprites);
            _view.OnHandleTurned += Spin;
            _view.OnAllReelsStopped += CheckWin;
            
            Show();
        }

        /// <summary>
        /// Take the sprites from the sprite atlas.
        /// Only types of symbols are checked in the config
        /// </summary>
        /// <param name="spriteAtlas"> Sprite atlas only with symbols </param>
        /// <returns></returns>
        private Sprite[] GetSymbolSprites(SpriteAtlas spriteAtlas)
        {
            Sprite[] sprites = new Sprite[_config.Symbols.Count];
            
            if (spriteAtlas != null)
            {
                for (int i = 0; i < sprites.Length; i++)
                {
                    sprites[i] = spriteAtlas.GetSprite(_config.Symbols[i].ToString());
                }
            }

            return sprites;
        }

        /// <summary>
        /// Start to spin all reels
        /// </summary>
        private void Spin()
        {
            _nextSymbols = GetNextSymbols();
            _view.Spin(_nextSymbols);
        }

        /// <summary>
        /// Invoke win if the combination is correct.
        /// Otherwise activate the slot machine`s handle
        /// </summary>
        private void CheckWin()
        {
            if (IsHaveWinCombination())
            {
                InvokeWin();
            }
            else
            {
                _view.Activate();
            }
        }
        
        /// <summary>
        /// Check if all the symbols are the same
        /// </summary>
        /// <returns></returns>
        private bool IsHaveWinCombination()
        {
            bool isWin = true;
            
            for (int i = 1; i < _nextSymbols.Count; i++)
            {
                if (_nextSymbols[i] != _nextSymbols[0])
                {
                    isWin = false;
                    break;
                }
            }

            return isWin;
        }
        
        private void InvokeWin()
        {
            int rewardAmount = _config.GetRewardAmount((SymbolsType)_nextSymbols[0]);
            OnWin?.Invoke(rewardAmount);
        }

        /// <summary>
        /// Show start state of the slot machine
        /// </summary>
        private void Show()
        {
            _nextSymbols = GetNextSymbols();
            _view.Show(_nextSymbols);
        }

        /// <returns>
        /// Key is the reel number from left to right.
        /// Value is the prize number in the config symbols list
        /// </returns>
        private Dictionary<int, int> GetNextSymbols()
        {
            return _model.GetNextSymbols(_view.ReelsCount, _config.Symbols.Count, _isOnlySameSymbols);            
        }

        /// <summary>
        /// Activate the slot machine after any blockers.
        /// For example after the reward animation
        /// </summary>
        public void ActivateAfterReward()
        {
            _view.Activate();
        }
        
        /// <summary>
        /// Set always win combination for testing
        /// </summary>
        public void GenerateOnlySameSymbols() => _isOnlySameSymbols = true;

        /// <summary>
        /// Unsubscribe from events.
        /// Dispose all the logic components
        /// </summary>
        public void Dispose()
        {
            _view.OnHandleTurned -= Spin;
            _view.OnAllReelsStopped -= CheckWin;
            _view.Dispose();
        }
    }
}

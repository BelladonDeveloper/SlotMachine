using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

namespace SlotMachine
{
    using Model.Data;
    
    [CreateAssetMenu(fileName = "SlotMachineConfig", menuName = "SlotMachine/SlotMachineConfig", order = 0)]
    public class SlotMachineConfig : ScriptableObject
    {
        [SerializeField] private SpriteAtlas _symbolsAtlas;
        [SerializeField] private List<SymbolsType> _symbols;
        [SerializeField] private List<Reward> _rewardAmounts;
        
        /// <summary>
        /// Sprite atlas only with prize symbols
        /// </summary>
        public SpriteAtlas SymbolsAtlas => _symbolsAtlas;
        
        /// <summary>
        /// Include only symbols which you want to see on the reels
        /// </summary>
        public List<SymbolsType> Symbols => _symbols;

        /// <summary>
        /// The list of rewards for each prize type
        /// </summary>
        /// <param name="symbolsType"> Rewarded prize type</param>
        /// <returns> Reward amount </returns>
        public int GetRewardAmount(SymbolsType symbolsType)
        {
            Reward reward = _rewardAmounts.Find(x => x.WinType == symbolsType);
            if (reward == null)
            {
                Debug.LogError($"Reward for {symbolsType} not found");
                return 0;
            }
            
            return reward.Amount;
        }
    }
}
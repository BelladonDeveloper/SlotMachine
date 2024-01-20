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
        [SerializeField] private List<PrizeType> _prizes;
        [SerializeField] private List<Reward> _rewardAmounts;
        
        public SpriteAtlas SymbolsAtlas => _symbolsAtlas;
        public List<PrizeType> Prizes => _prizes;

        public int GetRewardAmount(PrizeType prizeType)
        {
            Reward reward = _rewardAmounts.Find(x => x.WinType == prizeType);
            if (reward == null)
            {
                Debug.LogError($"Reward for {prizeType} not found");
                return 0;
            }
            
            return reward.Amount;
        }
    }
}
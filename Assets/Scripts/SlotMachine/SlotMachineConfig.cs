using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

namespace SlotMachine
{
    using Model;
    
    [CreateAssetMenu(fileName = "SlotMachineConfig", menuName = "SlotMachine/SlotMachineConfig", order = 0)]
    public class SlotMachineConfig : ScriptableObject
    {
        [SerializeField] private SpriteAtlas _symbolsAtlas;
        [SerializeField] private List<PrizeType> _prizes;
        
        public SpriteAtlas SymbolsAtlas => _symbolsAtlas;
        public List<PrizeType> Prizes => _prizes;
    }
}
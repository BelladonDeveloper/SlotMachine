using System;

namespace SlotMachine.Model.Data
{
    [Serializable]
    public class Reward
    {
        public PrizeType WinType;
        public int Amount;
    }
}
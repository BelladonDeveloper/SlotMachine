using System;

namespace SlotMachine.Model.Data
{
    [Serializable]
    public class Reward
    {
        public SymbolsType WinType;
        public int Amount;
    }
}
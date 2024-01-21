using System;
using Base;

namespace SlotMachine
{
    public interface ISlotMachineProvider : IManager
    {
        /// <summary>
        /// Invoked when a player wins any prize
        /// The parameter is the amount of a reward
        /// </summary>
        event Action<int> OnWin;
        
        /// <summary>
        /// Just call it to show the slot machine
        /// </summary>
        void ShowSlotMachine();
        
        /// <summary>
        /// Generate always win combination for testing
        /// </summary>
        void GenerateOnlySameSymbols();
    }
}

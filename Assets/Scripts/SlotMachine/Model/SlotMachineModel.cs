using System.Collections.Generic;
using UnityEngine;

namespace SlotMachine.Model
{
    public class SlotMachineModel
    {
        /// <summary>
        /// Get a random symbols for each reel.
        /// Avoid zero prize because it is a "no prize" state
        /// </summary>
        /// <param name="reelCount"></param>
        /// <param name="maxNumber"> Symbols count </param>
        /// <param name="onlySameSymbols"> Debug set win </param>
        /// <returns>
        /// Key is the reel number from left to right.
        /// Value is the prize number in the config symbols list
        /// </returns>
        public Dictionary<int, int> GetNextSymbols(int reelCount, int maxNumber, bool onlySameSymbols = false)
        {
            Dictionary<int, int> nextSymbols = new Dictionary<int, int>();
            
            for (int i = 0; i < reelCount; i++)
            {
                nextSymbols.Add(i, Random.Range(1, maxNumber));
            }
            
            if (onlySameSymbols)
            {
                int prize = nextSymbols[0];
                for (int i = 1; i < reelCount; i++)
                {
                    nextSymbols[i] = prize;
                }
            }

            return nextSymbols;
        }
    }
}
using System.Collections.Generic;
using UnityEngine;

namespace SlotMachine.Model
{
    public class SlotMachineModel
    {
        public Dictionary<int, int> GetNextPrizes(int prizeCount, int maxNumber)
        {
            Dictionary<int, int> nextPrizes = new Dictionary<int, int>();
            
            for (int i = 0; i < prizeCount; i++)
            {
                nextPrizes.Add(i, Random.Range(0, maxNumber));
            }

            return nextPrizes;
        }
    }
}
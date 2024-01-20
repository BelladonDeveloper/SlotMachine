using System.Collections.Generic;
using UnityEngine;

namespace SlotMachine.Model
{
    public class SlotMachineModel
    {
        public Dictionary<int, int> GetNextPrizes(int prizeCount, int maxNumber, bool onlySamePrizes = false)
        {
            Dictionary<int, int> nextPrizes = new Dictionary<int, int>();
            
            for (int i = 0; i < prizeCount; i++)
            {
                nextPrizes.Add(i, Random.Range(1, maxNumber));
            }
            
            if (onlySamePrizes)
            {
                int prize = nextPrizes[0];
                for (int i = 1; i < prizeCount; i++)
                {
                    nextPrizes[i] = prize;
                }
            }

            return nextPrizes;
        }
    }
}
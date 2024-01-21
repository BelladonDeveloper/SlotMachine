using System.Collections;
using UnityEngine;
using SlotMachine;
using TMPro;
using Base;

namespace UI
{
    public class RewardCounterView : MonoBehaviour
    {
        private const float INTERVAL = 0.1f;
        private const float STEP_SCALE_INDEX = 0.03f;
        
        [SerializeField] private TextMeshProUGUI _rewardCounterText;
        [SerializeField] private Transform _transformImage;
        
        WaitForSeconds _waitInterval = new WaitForSeconds(INTERVAL);

        private void Start()
        {
            Register.Get<ISlotMachineProvider>().OnWin += SetRewardCounter;
        }

        private void SetRewardCounter(int rewardCounter)
        {
            StartCoroutine(ChangeRewardCounter(rewardCounter));
        }

        private IEnumerator ChangeRewardCounter(int rewardCounter)
        {
            int currentRewardCounter = int.Parse(_rewardCounterText.text);
            
            for (int i = 0; i < rewardCounter; i++)
            {
                currentRewardCounter++;
                _transformImage.localScale = Vector3.one * (1 + i * STEP_SCALE_INDEX);
            
                yield return _waitInterval;
            
                _rewardCounterText.text = currentRewardCounter.ToString();
                _transformImage.localScale = Vector3.one;
            }
        }
        
        public void Dispose()
        {
            Register.Get<ISlotMachineProvider>().OnWin -= SetRewardCounter;
        }
    }
}
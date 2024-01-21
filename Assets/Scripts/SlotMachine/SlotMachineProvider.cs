using System;
using UnityEngine;
using Base;
using Base.Animators;

namespace SlotMachine
{
    using View;
    using Model;
    
    public class SlotMachineProvider : MonoBehaviour, ISlotMachineProvider
    {
        private const string PREFAB_PATH = "Prefabs/SlotMachine";
        private const string CONFIG_PATH = "Configs/SlotMachineConfig";
        
        public event Action<int> OnWin;
        
        private RewardAnimator _rewardAnimator;
        private SlotMachineController _controller;

        public void Init()
        {
            _rewardAnimator = Register.Get<RewardAnimator>();
            _rewardAnimator.OnAnimationEnded += ActivateAfterReward;
            
            ShowSlotMachine();
        }

        /// <summary>
        /// The method provide the slot machine.
        /// Just call it to show.
        /// All the logic is inside the SlotMachineController
        /// </summary>
        public void ShowSlotMachine()
        {
            GameObject prefab = Resources.Load<GameObject>(PREFAB_PATH);
            Transform parent = Register.Get<IUIManager>().GetCanvasAsParent();
            SlotMachineConfig config = Resources.Load<SlotMachineConfig>(CONFIG_PATH);
            SlotMachineModel model = new SlotMachineModel();
            
            GameObject slotMachine = Instantiate(prefab, parent);
            SlotMachineView view = slotMachine.GetComponent<SlotMachineView>();
            
            _controller = new SlotMachineController(model, view, config);
            _controller.Init();
            _controller.OnWin += InvokeWin;
        }

        private void InvokeWin(int rewardAmount)
        {
            OnWin?.Invoke(rewardAmount);
        }

        private void ActivateAfterReward()
        {
            _controller.ActivateAfterReward();
        }

        /// <summary>
        /// Generate always win combination for testing
        /// </summary>
        public void GenerateOnlySameSymbols() => _controller.GenerateOnlySameSymbols();

        public void Dispose()
        {
            _controller.Dispose();
            _rewardAnimator.OnAnimationEnded -= ActivateAfterReward;
            _controller.OnWin -= InvokeWin;
        }
    }
}
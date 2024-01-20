using System;
using UnityEngine;
using Base;

namespace SlotMachine
{
    using View;
    using Model;
    
    public class SlotMachineProvider : MonoBehaviour
    {
        private const string PREFAB_PATH = "Prefabs/SlotMachine";
        private const string CONFIG_PATH = "Configs/SlotMachineConfig";
        
        public event Action<int> OnWin;
        
        [SerializeField] private Transform _parent;

        private SlotMachineController _controller;

        private void Start()
        {
            ShowSlotMachine();
        }

        public void ShowSlotMachine()
        {
            GameObject prefab = Resources.Load<GameObject>(PREFAB_PATH);
            GameObject slotMachine = Instantiate(prefab, _parent);
            //slotMachine.transform.SetAsFirstSibling();
            SlotMachineView view = slotMachine.GetComponent<SlotMachineView>();
            SlotMachineModel model = new SlotMachineModel();
            SlotMachineConfig config = Resources.Load<SlotMachineConfig>(CONFIG_PATH);
            _controller = new SlotMachineController(model, view, config);
            _controller.Init();
            _controller.OnWin = InvokeWin;
        }

        private void InvokeWin(int rewardAmount)
        {
            OnWin?.Invoke(rewardAmount);
        }

        /// <summary>
        /// Generate always win combination for testing
        /// </summary>
        public void GenerateOnlySamePrizes() => _controller.GenerateOnlySamePrizes();

        public void Dispose()
        {
            _controller.Dispose();
        }
    }
}

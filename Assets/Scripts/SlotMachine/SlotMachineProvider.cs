using Base;
using SlotMachine.Model;
using SlotMachine.View;
using UnityEngine;

namespace SlotMachine
{
    public class SlotMachineProvider : MonoBehaviour
    {
        private const string PREFAB_PATH = "Prefabs/SlotMachine";
        private const string CONFIG_PATH = "Configs/SlotMachineConfig";
        
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
            SlotMachineView view = slotMachine.GetComponent<SlotMachineView>();
            SlotMachineModel model = new SlotMachineModel();
            SlotMachineConfig config = Resources.Load<SlotMachineConfig>(CONFIG_PATH);
            _controller = new SlotMachineController(model, view, config);
            _controller.Init();
        }

        public void Dispose()
        {
            _controller.Dispose();
        }
    }
}

using Base;
using SlotMachine;
using UnityEngine;
using UnityEngine.UI;

namespace Test
{
    public class DebugWinButton : MonoBehaviour
    {
        [SerializeField] private Button _button;
        
        private void Start()
        {
            _button.onClick.AddListener(OnButtonClick);
        }

        private void OnButtonClick()
        {
            Register.Get<ISlotMachineProvider>().GenerateOnlySameSymbols();
        }
        
        private void OnDestroy()
        {
            _button.onClick.RemoveListener(OnButtonClick);
        }
    }
}

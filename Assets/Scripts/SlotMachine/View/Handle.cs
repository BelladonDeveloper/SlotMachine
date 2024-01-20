using System;
using UnityEngine;
using UnityEngine.UI;

namespace SlotMachine.View
{
    public class Handle : MonoBehaviour
    {
        public Action OnClick { get; set; }
        
        [SerializeField] private Button _button;
        
        public void ChangeInteractable(bool value)
        {
            _button.interactable = value;
        }

        public void Show()
        {
            _button.onClick.AddListener(Click);
            gameObject.SetActive(true);
        }

        private void Click()
        {
            ChangeInteractable(false);
            OnClick?.Invoke();
        }
    }
}
using System;
using UnityEngine;
using UnityEngine.UI;

namespace SlotMachine.View
{
    using Animators;

    public class Handle : MonoBehaviour
    {
        public Action OnClick { get; set; }
        public Action OnTurnedUp { get; set; }

        [SerializeField] private Image _image;
        [SerializeField] private Button _button;
        [SerializeField] private Animator _animator;

        private HandleAnimator _handleAnimator;

        public void Init()
        {
            _handleAnimator = new HandleAnimator();
            _handleAnimator.Init(_animator, _image);
        }

        public void ChangeInteractable(bool active)
        {
            _button.interactable = active;
            if (active)
            {
                _handleAnimator.MarkActive();
            }
        }

        public void Show()
        {
            _button.onClick.AddListener(Click);
            gameObject.SetActive(true);
        }

        private void Click()
        {
            ChangeInteractable(false);
            _handleAnimator.Turn();
            OnClick?.Invoke();
        }

        public void HandleReturned()
        {
            OnTurnedUp?.Invoke();
        }
    }
}
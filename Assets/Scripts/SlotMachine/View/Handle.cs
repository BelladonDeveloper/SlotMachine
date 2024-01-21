using System;
using UnityEngine;
using UnityEngine.UI;

namespace SlotMachine.View
{
    using Animators;

    public class Handle : MonoBehaviour
    {
        /// <summary>
        /// Called when a player turns the handle
        /// </summary>
        public Action OnClick { get; set; }
        
        /// <summary>
        /// Called when the handle is returned to the start position
        /// </summary>
        public Action OnTurnedUp { get; set; }

        [SerializeField] private Image _image;
        [SerializeField] private Button _button;
        [SerializeField] private Animator _animator;

        private HandleAnimator _handleAnimator;

        public void Init()
        {
            _handleAnimator = new HandleAnimator();
            _handleAnimator.Init(_animator, _image);
            _button.onClick.AddListener(Click);
        }

        /// <summary>
        /// Change the interactable state of the handle
        /// Change visual as active if needs
        /// </summary>
        /// <param name="active"></param>
        public void ChangeInteractable(bool active)
        {
            _button.interactable = active;
            if (active)
            {
                _handleAnimator.MarkActive();
            }
        }

        /// <summary>
        /// Show the Handle
        /// You can add here some animation and VFX
        /// </summary>
        public void Show()
        {
            gameObject.SetActive(true);
        }

        private void Click()
        {
            ChangeInteractable(false);
            _handleAnimator.Turn();
            OnClick?.Invoke();
        }

        /// <summary>
        /// Called from animation event when the handle is returned to the start position
        /// </summary>
        public void HandleReturned()
        {
            OnTurnedUp?.Invoke();
        }
    }
}
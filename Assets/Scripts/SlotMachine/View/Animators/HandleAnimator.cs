using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace SlotMachine.View.Animators
{
    public class HandleAnimator
    {
        private float FADE_DURATION = 1.15f;
        private float ACTIVATE_DURATION = 0.5f;
        private readonly Color FADED_COLOR = new Color(0.5f, 0.5f, 0.5f, 0.77f);
        private static readonly int TurnDown = Animator.StringToHash("TurnDown");

        private Image _image;
        private Animator _animator;

        public void Init(Animator animator, Image image)
        {
            _animator = animator;
            _image = image;
        }

        /// <summary>
        /// Start the handle turning animation and partly fade the handle
        /// </summary>
        public void Turn()
        {
            _animator.SetTrigger(TurnDown);
            _image.DOColor(FADED_COLOR, FADE_DURATION).SetEase(Ease.InOutFlash);
        }
        
        /// <summary>
        /// Return start color
        /// </summary>
        public void MarkActive()
        {
            _image.DOColor(Color.white, ACTIVATE_DURATION);
        }
    }
}
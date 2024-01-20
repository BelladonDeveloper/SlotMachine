using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using SlotMachine;

namespace Base.Animators
{
    public class RewardAnimator : MonoBehaviour
    {
        private const string PREFAB_PATH = "Prefabs/Reward";
        private const float INTERVAL = 0.1f;
        private const float FLY_DURATION = 1f;
        
        public event Action OnAnimationEnded;
        
        [SerializeField] private Transform _rewardParent;
        [SerializeField] private SlotMachineProvider _slotMachineProvider;
        
        private Vector3 _target;
        private GameObject _prefab;
        
        private void Start()
        {
            _slotMachineProvider.OnWin += Show;
            _target = _rewardParent.position;
            _prefab = Resources.Load<GameObject>(PREFAB_PATH);
        }

        private void Show(int rewardAmount)
        {
            Sequence flyTo = DOTween.Sequence();
            
            for (int i = 0; i < rewardAmount; i++)
            {
                GameObject reward = Instantiate(_prefab, transform);
                reward.transform.position = transform.position;
                flyTo.Join(reward.transform.DOMove(_target, FLY_DURATION).SetEase(Ease.InOutSine));
                //flyTo.AppendInterval(INTERVAL);
            }
            
            flyTo.OnComplete(OnAnimationEndedHandler);
            flyTo.Play();
        }

        private void OnAnimationEndedHandler()
        {
            OnAnimationEnded?.Invoke();
        }
        
        private void OnDestroy()
        {
            _slotMachineProvider.OnWin -= Show;
        }
    }
}

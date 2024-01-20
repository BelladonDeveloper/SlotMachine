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
        private const float FLY_DURATION = 0.5f;
        
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
            
            Vector3 position = transform.position;
            Vector3[] path = CreatePath(position, _target);
            for (int i = 0; i < rewardAmount; i++)
            {
                GameObject reward = Instantiate(_prefab, transform);
                reward.transform.position = position;
                flyTo.Join(reward.transform.DOPath(path, FLY_DURATION)
                    .SetEase(Ease.InOutQuad)
                    .SetDelay(INTERVAL)
                    .OnComplete(() => Destroy(reward)));
            }
            
            flyTo.OnComplete(OnAnimationEndedHandler);
            flyTo.Play();
        }
        
        private Vector3[] CreatePath(Vector3 start, Vector3 end, int resolution = 10)
        {
            Vector3[] path = new Vector3[resolution];
            float timeStep = 1f / (resolution - 1);
            float parabolaHeight = Mathf.Abs(end.x - start.x) / 2f;

            for (int i = 0; i < resolution; i++)
            {
                float t = i * timeStep;
                Vector3 pointOnLine = Vector3.Lerp(start, end, t);
                float parabolaX = 4 * parabolaHeight * t * (1 - t);

                path[i] = new Vector3(start.x + (end.x - start.x) * t - parabolaX, pointOnLine.y, pointOnLine.z);
            }

            return path;
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

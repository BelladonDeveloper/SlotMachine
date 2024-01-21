using System;
using UnityEngine;
using DG.Tweening;
using SlotMachine;
using UnityEngine.UI;

namespace Base.Animators
{
    public class RewardAnimator : MonoBehaviour, IManager
    {
        private const string PREFAB_PATH = "Prefabs/Reward";
        private const float INTERVAL = 0.1f;
        private const float FLY_DURATION = 0.5f;
        
        public event Action OnAnimationEnded;
        
        private Vector3 _target;
        private GameObject _prefab;
        private ISlotMachineProvider _slotMachineProvider;

        public void Init()
        {
            _slotMachineProvider = Register.Get<ISlotMachineProvider>();
            _slotMachineProvider.OnWin += Show;
            _target = Register.Get<IUIManager>().GetRewardTarget().position;
            _prefab = Resources.Load<GameObject>(PREFAB_PATH);
        }

        private void Show(int rewardAmount)
        {
            Register.Get<ISoundManager>().PlaySound(SoundName.RewardFlying);
            
            Sequence flyTo = DOTween.Sequence();
            Vector3 position = transform.position;
            Vector3[] path = CreatePath(position, _target);
            
            for (int i = 0; i < rewardAmount; i++)
            {
                Sequence flyToTarget = FlyToTarget(position, path);
                flyTo.Join(flyToTarget)
                    .SetDelay(INTERVAL);
            }
            
            flyTo.OnComplete(OnAnimationEndedHandler);
            flyTo.Play();
        }

        private Sequence FlyToTarget(Vector3 position, Vector3[] path)
        {
            Sequence flyTo = DOTween.Sequence();
            GameObject reward = Instantiate(_prefab, transform);
            reward.transform.position = position;
            reward.transform.localScale = Vector3.one / 4f;
            Image image = reward.GetComponentInChildren<Image>();
            image.color = new Color(image.color.r, image.color.g, image.color.b, 0f);
                
            flyTo.Append(image.DOFade(1f, FLY_DURATION / 2f));
            flyTo.Append(reward.transform.DOScale(Vector3.one, FLY_DURATION / 2f));
            flyTo.Join(reward.transform.DOPath(path, FLY_DURATION)
                .SetEase(Ease.InOutQuad));
            flyTo.Append(reward.transform.DOScale(Vector3.one / 4f, FLY_DURATION / 2f));
            flyTo.Join(image.DOFade(0f, FLY_DURATION / 2f)
                .OnComplete(() => Destroy(reward)));
            
            return flyTo;
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

        public void Dispose()
        {
            _slotMachineProvider.OnWin -= Show;
        }
    }
}

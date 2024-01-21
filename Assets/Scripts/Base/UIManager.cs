using UnityEngine;

namespace Base
{
    /// <summary>
    /// The simple template for UI manager
    /// </summary>
    public class UIManager : MonoBehaviour, IUIManager
    {
        [SerializeField] private Transform _rewardTarget;
        
        public void Init()
        {
            
        }

        public Transform GetCanvasAsParent() => transform;
        
        public Transform GetRewardTarget() => _rewardTarget;

        public void Dispose()
        {
            
        }
    }
}
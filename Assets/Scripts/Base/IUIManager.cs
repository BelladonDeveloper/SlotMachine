using UnityEngine;

namespace Base
{
    public interface IUIManager : IManager
    {
        Transform GetCanvasAsParent();
        Transform GetRewardTarget();
    }
}
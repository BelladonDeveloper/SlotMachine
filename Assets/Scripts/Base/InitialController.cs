using System.Collections.Generic;
using Base.Animators;
using SlotMachine;
using UnityEngine;

namespace Base
{
    public class InitialController : MonoBehaviour
    {
        [SerializeField] private RewardAnimator _rewardAnimator;
        [SerializeField] private SlotMachineProvider _slotMachineProvider;
        [SerializeField] private UIManager _uiManager;
    
        private List<IManager> _managers = new List<IManager>();

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
            RegisterManagers();
            InitializeManagers();
        }

        private void RegisterManagers()
        {
            Register.Add(_rewardAnimator);
            Register.Add<ISlotMachineProvider>(_slotMachineProvider);
            Register.Add<IUIManager>(_uiManager);
            _managers.Add(_rewardAnimator);
            _managers.Add(_slotMachineProvider);
            _managers.Add(_uiManager);
        }

        private void InitializeManagers()
        {
            _managers.ForEach(e => e.Init());
        }

        private void DisposeManagers()
        {
            _managers.ForEach(e => e.Dispose());
            _managers.Clear();
            Register.Remove(_rewardAnimator);
            Register.Remove<ISlotMachineProvider>(_slotMachineProvider);
            Register.Remove<IUIManager>(_uiManager);
        }

        private void OnDestroy()
        {
            DisposeManagers();
        }
    }
}
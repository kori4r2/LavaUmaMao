using UnityEngine;
using UnityEngine.Events;

namespace LavaUmaMao {
    [CreateAssetMenu(menuName = "LavaUmaMão/WashingStep", fileName = "WashingStep")]
    public class WashingStep : ScriptableObject {
        [SerializeField] private string animationTriggerName = string.Empty;
        public string AnimationTriggerName => animationTriggerName;
        private WashingStepState currentState = WashingStepState.Available;
        public WashingStepState CurrentState {
            get => currentState;
            private set {
                currentState = value;
                OnStateChanged?.Invoke(value);
            }
        }
        private WashingStepSlot selectionSlot = null;
        private WashingStepSlot placementSlot = null;
        private UnityEvent<WashingStepState> OnStateChanged = new UnityEvent<WashingStepState>();

        public void AddStateChangeListener(UnityAction<WashingStepState> actionAdded) {
            OnStateChanged?.AddListener(actionAdded);
        }

        public void RemoveStateChangeListener(UnityAction<WashingStepState> actionToRemove) {
            OnStateChanged?.RemoveListener(actionToRemove);
        }
    }
}

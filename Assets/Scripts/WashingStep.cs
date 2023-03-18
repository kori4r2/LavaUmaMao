using UnityEngine;
using UnityEngine.Events;

namespace LavaUmaMao {
    [CreateAssetMenu(menuName = "LavaUmaMão/WashingStep", fileName = "WashingStep")]
    public class WashingStep : ScriptableObject {
        [SerializeField, Range(0, 6)] private int stepId = 0;
        public int StepId => stepId;
        [SerializeField] private string animationTriggerName = string.Empty;
        public string AnimationTriggerName => animationTriggerName;
        [SerializeField] private WashingStepEvent playAnimationEvent;
        [SerializeField] private Sprite stepImage = null;
        public Sprite StepSprite => stepImage;
        private WashingStepState currentState = WashingStepState.Available;
        public WashingStepState CurrentState {
            get => currentState;
            set {
                currentState = value;
                OnStateChanged?.Invoke(value);
            }
        }
        private UnityEvent<WashingStepState> OnStateChanged = new UnityEvent<WashingStepState>();

        public void AddStateChangeListener(UnityAction<WashingStepState> actionAdded) {
            OnStateChanged?.AddListener(actionAdded);
        }

        public void RemoveStateChangeListener(UnityAction<WashingStepState> actionToRemove) {
            OnStateChanged?.RemoveListener(actionToRemove);
        }

        public void PlayAnimation() {
            if (playAnimationEvent != null)
                playAnimationEvent.Raise(this);
        }
    }
}

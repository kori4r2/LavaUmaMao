using UnityEngine;
using Toblerone.Toolbox;

namespace LavaUmaMao {
    public class HandAnimator : MonoBehaviour {
        [SerializeField] private Animator animator = null;
        [SerializeField] private string fullAnimationEventTrigger = "PlayFull";
        [SerializeField] private EventSO fullAnimationEvent = null;
        private EventListener fullAnimationEventListener = null;
        [SerializeField] private WashingStepEvent stepAnimationEvent = null;
        private GenericEventListener<WashingStep> stepAnimationEventListener = null;
        [SerializeField] private BoolVariable blockInput = null;

        private void Awake() {
            fullAnimationEventListener = new EventListener(fullAnimationEvent, AnimateFull);
            stepAnimationEventListener = new GenericEventListener<WashingStep>(stepAnimationEvent, AnimateStep);
        }

        private void AnimateFull() {
            blockInput.Value = true;
            animator.SetTrigger(fullAnimationEventTrigger);
        }

        private void AnimateStep(WashingStep washingStep) {
            blockInput.Value = true;
            animator.SetTrigger(washingStep.AnimationTriggerName);
        }

        private void OnEnable() {
            fullAnimationEventListener.StartListeningEvent();
            stepAnimationEventListener.StartListeningEvent();
        }

        private void OnDisable() {
            fullAnimationEventListener.StopListeningEvent();
            stepAnimationEventListener.StopListeningEvent();
        }
    }
}

using UnityEngine;

namespace LavaUmaMao {
    public class SelectionWashingStepSlot : WashingStepSlot {
        private WashingStep startingWashingStep = null;
        public override WashingStep WashingStep {
            get => startingWashingStep;
            protected set { }
        }

        protected override void WashingStepStateChanged(WashingStepState newState) {
            switch (newState) {
                case WashingStepState.Available:
                case WashingStepState.Dragging:
                    HideOverlay();
                    HideWrongIndicator();
                    HideCorrectIndicator();
                    break;
                case WashingStepState.Selected:
                    ShowOverlay();
                    HideWrongIndicator();
                    HideCorrectIndicator();
                    break;
                case WashingStepState.Correct:
                    ShowOverlay();
                    HideWrongIndicator();
                    ShowCorrectIndicator();
                    break;
                case WashingStepState.Wrong:
                    ShowOverlay();
                    ShowWrongIndicator();
                    HideCorrectIndicator();
                    break;
            }
        }

        public void PlayAnimation() {
            WashingStep.PlayAnimation();
        }

        public void ResetInitialWashingStep(WashingStep newInitialStep) {
            if (newInitialStep == null)
                return;

            if (WashingStep != null && newInitialStep != WashingStep)
                WashingStep.RemoveStateChangeListener(WashingStepStateChanged);
            if (newInitialStep != WashingStep)
                newInitialStep.AddStateChangeListener(WashingStepStateChanged);

            startingWashingStep = newInitialStep;
            if (slotImage.sprite != WashingStep.StepSprite)
                slotImage.sprite = WashingStep.StepSprite;
            WashingStepStateChanged(WashingStep.CurrentState);
        }

        public override void OnPointerEnter() {
            base.OnPointerEnter();
            Debug.Log($"Entered Selection Slot {transform.parent.name}");
        }

        public override void OnPointerExit() {
            base.OnPointerExit();
            Debug.Log($"Exited Selection Slot {transform.parent.name}");
        }

        public override void OnPointerDown() {
            Debug.Log($"Pointer down on selection slot {transform.parent.name}");
            if (WashingStep.CurrentState != WashingStepState.Available)
                return;
            draggedWashingStepReference.Value = WashingStep;
            WashingStep.CurrentState = WashingStepState.Dragging;
        }

        public override void OnPointerUp() {
            Debug.Log($"Pointer up on selection slot {transform.parent.name}");
            if (draggedWashingStepReference.Value != WashingStep || draggedWashingStepReference.Value == null)
                return;
            WashingStep.CurrentState = WashingStepState.Available;
            draggedWashingStepReference.Value = null;
        }
    }
}

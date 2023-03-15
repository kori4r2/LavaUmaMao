namespace LavaUmaMao {
    public class SelectionWashingStepSlot : WashingStepSlot {
        private WashingStep startingWashingStep = null;
        public override WashingStep WashingStep {
            get => startingWashingStep;
            protected set { }
        }

        protected override void WashingStepStateChanged(WashingStepState newState) {
            // Change Object based on current state
            switch (newState) {
                case WashingStepState.Available:
                    break;
                case WashingStepState.Selected:
                    break;
                case WashingStepState.Dragging:
                    break;
                case WashingStepState.Correct:
                    break;
                case WashingStepState.Wrong:
                    break;
            }
        }

        public void PlayAnimation() {
            WashingStep.PlayAnimation();
        }

        public void ResetInitialWashingStep(WashingStep newInitialStep) {
            if (newInitialStep == null || newInitialStep == WashingStep)
                return;

            if (WashingStep != null)
                WashingStep.RemoveStateChangeListener(WashingStepStateChanged);

            startingWashingStep = newInitialStep;
            WashingStep.AddStateChangeListener(WashingStepStateChanged);
            if (slotImage.sprite != WashingStep.StepSprite)
                slotImage.sprite = WashingStep.StepSprite;
            WashingStepStateChanged(WashingStep.CurrentState);
        }

        public override void OnPointerDown() {
            draggedWashingStepReference.Value = WashingStep;
            WashingStep.CurrentState = WashingStepState.Dragging;
        }

        public override void OnPointerUp() {
            if (draggedWashingStepReference.Value != WashingStep)
                return;
            draggedWashingStepReference.Value = null;
            WashingStep.CurrentState = WashingStepState.Available;
        }
    }
}

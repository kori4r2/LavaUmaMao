namespace LavaUmaMao {
    public class PlacementWashingStepSlot : WashingStepSlot {
        public override WashingStep WashingStep { get; protected set; }

        public void ResetSlot() {
            ChangeCurrentWashingStep(null);
        }

        private void ChangeCurrentWashingStep(WashingStep newStep) {
            if (WashingStep == newStep)
                return;
            if (WashingStep != null)
                WashingStep.RemoveStateChangeListener(WashingStepStateChanged);
            WashingStep = newStep;
            slotImage.sprite = newStep != null ? newStep.StepSprite : null;
        }

        protected override void WashingStepStateChanged(WashingStepState newState) {
            switch (newState) {
                case WashingStepState.Available:
                case WashingStepState.Dragging:
                    ChangeCurrentWashingStep(null);
                    break;
                case WashingStepState.Correct:
                    // To Do
                    break;
                case WashingStepState.Wrong:
                    // To Do
                    break;
            }
        }

        public override void OnPointerDown() {
            if (WashingStep == null || draggedWashingStepReference.Value != null)
                return;
            draggedWashingStepReference.Value = WashingStep;
            ChangeCurrentWashingStep(null);
            draggedWashingStepReference.Value.CurrentState = WashingStepState.Dragging;
        }

        public override void OnPointerUp() {
            if (draggedWashingStepReference.Value == null)
                return;
            ChangeCurrentWashingStep(draggedWashingStepReference.Value);
            draggedWashingStepReference.Value = null;
            WashingStep.CurrentState = WashingStepState.Selected;
        }
    }
}

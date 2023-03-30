using UnityEngine;
using Toblerone.Toolbox;

namespace LavaUmaMao {
    public class PlacementWashingStepSlot : WashingStepSlot {
        [SerializeField] private IntVariable placedStepsCount;
        public override WashingStep WashingStep { get; protected set; }
        private VariableObserver<WashingStep> draggedStepObserver;
        private WashingStep newStepSelected = null;
        private WashingStep previousStepSelected = null;

        private new void Awake() {
            base.Awake();
            draggedStepObserver = new VariableObserver<WashingStep>(draggedWashingStepReference, DraggingStatusChanged);
        }

        private void DraggingStatusChanged(WashingStep draggedStepNewValue) {
            if (draggedStepNewValue == null && newStepSelected != null) {
                ChangeCurrentWashingStep(newStepSelected);
                newStepSelected = null;
                draggedWashingStepReference.Value = null;
                WashingStep.CurrentState = WashingStepState.Selected;
                placedStepsCount.IncrementValue(1);
            }
            newStepSelected = null;
        }

        public void ResetSlot() {
            ChangeCurrentWashingStep(null);
            HideOverlay();
            HideWrongIndicator();
            HideCorrectIndicator();
        }

        private void ChangeCurrentWashingStep(WashingStep newStep) {
            if (WashingStep == newStep && newStep != null)
                return;
            if (WashingStep != null)
                WashingStep.RemoveStateChangeListener(WashingStepStateChanged);
            if (newStep != null)
                newStep.AddStateChangeListener(WashingStepStateChanged);
            WashingStep = newStep;
            slotImage.sprite = newStep != null ? newStep.StepSprite : null;
            slotImage.color = newStep != null ? Color.white : Color.clear;
        }

        protected override void WashingStepStateChanged(WashingStepState newState) {
            switch (newState) {
                case WashingStepState.Available:
                case WashingStepState.Dragging:
                    ChangeCurrentWashingStep(null);
                    HideOverlay();
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

        private new void OnEnable() {
            base.OnEnable();
            draggedStepObserver.StartWatching();
        }

        private new void OnDisable() {
            base.OnDisable();
            draggedStepObserver.StopWatching();
        }

        public override void OnPointerEnter() {
            base.OnPointerEnter();
            if (draggedWashingStepReference.Value != null)
                newStepSelected = draggedWashingStepReference.Value;
        }

        public override void OnPointerExit() {
            base.OnPointerExit();
            newStepSelected = null;
        }

        public override void OnPointerDown() {
            if (WashingStep == null || draggedWashingStepReference.Value != null)
                return;
            draggedWashingStepReference.Value = WashingStep;
            previousStepSelected = WashingStep;
            ChangeCurrentWashingStep(null);
            placedStepsCount.DecrementValue(1);
            draggedWashingStepReference.Value.CurrentState = WashingStepState.Dragging;
            newStepSelected = draggedWashingStepReference.Value;
        }

        public override void OnPointerUp() {
            if (previousStepSelected != null)
                previousStepSelected.CurrentState = WashingStepState.Available;
            previousStepSelected = null;
            draggedWashingStepReference.Value = null;
        }
    }
}

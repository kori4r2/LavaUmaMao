using UnityEngine;
using Toblerone.Toolbox;
using System.Collections;

namespace LavaUmaMao {
    public class PlacementWashingStepSlot : WashingStepSlot {
        [SerializeField] private IntVariable placedStepsCount;
        public override WashingStep WashingStep { get; protected set; }

        private new void Awake() {
            base.Awake();
        }

        public void ResetSlot() {
            ChangeCurrentWashingStep(null);
            HideOverlay();
            HideWrongIndicator();
            HideCorrectIndicator();
        }

        private void ChangeCurrentWashingStep(WashingStep newStep) {
            if (WashingStep != null)
                RemovePlacedStep();
            if (newStep != null)
                newStep.AddStateChangeListener(WashingStepStateChanged);
            WashingStep = newStep;
            slotImage.sprite = newStep != null ? newStep.StepSprite : null;
            slotImage.color = newStep != null ? Color.white : Color.clear;
        }

        private void RemovePlacedStep() {
            WashingStep.RemoveStateChangeListener(WashingStepStateChanged);
            WashingStep.CurrentState = WashingStepState.Available;
            placedStepsCount.DecrementValue(1);
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

        public void PlaceDraggedStep() {
            ChangeCurrentWashingStep(draggedWashingStepReference.Value);
            draggedWashingStepReference.Value = null;
            if (WashingStep != null) {
                placedStepsCount.IncrementValue(1);
                WashingStep.CurrentState = WashingStepState.Selected;
            }
        }

        public override void OnPointerEnter() {
            base.OnPointerEnter();
            selectedWashingStepSlotReference.Value = this;
        }

        public override void OnPointerExit() {
            base.OnPointerExit();
            StartCoroutine(PointerExitCoroutine());
        }

        private IEnumerator PointerExitCoroutine() {
            yield return null;
            if (draggedWashingStepReference.Value != null)
                selectedWashingStepSlotReference.Value = null;
        }

        public override void OnPointerDown() {
            if (WashingStep == null || draggedWashingStepReference.Value != null)
                return;
            draggedWashingStepReference.Value = WashingStep;
            ChangeCurrentWashingStep(null);
            draggedWashingStepReference.Value.CurrentState = WashingStepState.Dragging;
        }

        public override void OnPointerUp() {
            PlacementWashingStepSlot selectedSlot = selectedWashingStepSlotReference.Value as PlacementWashingStepSlot;
            if (selectedSlot != null) {
                selectedSlot.PlaceDraggedStep();
            } else {
                draggedWashingStepReference.Value.CurrentState = WashingStepState.Available;
                draggedWashingStepReference.Value = null;
            }
        }
    }
}

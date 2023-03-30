using System;
using UnityEngine;
using UnityRandom = UnityEngine.Random;

namespace LavaUmaMao {
    [Serializable]
    public class SlotShuffler {
        private const int stepNumber = 7;
        [SerializeField] private SelectionWashingStepSlot[] selectionSlots = new SelectionWashingStepSlot[stepNumber];
        [SerializeField] private PlacementWashingStepSlot[] placementSlots = new PlacementWashingStepSlot[stepNumber];
        [SerializeField] private WashingStep[] washingSteps = new WashingStep[stepNumber];
        private WashingStep[] shuffledSteps = new WashingStep[stepNumber];
        private int[] shuffleNumbers = new int[stepNumber];

        public SlotShuffler() {
            selectionSlots = new SelectionWashingStepSlot[stepNumber];
            placementSlots = new PlacementWashingStepSlot[stepNumber];
            washingSteps = new WashingStep[stepNumber];
            shuffledSteps = new WashingStep[stepNumber];
            shuffleNumbers = new int[stepNumber];
        }

        public void ValidateArrays() {
            if (selectionSlots.Length != stepNumber)
                AdjustSelectionSlotsArraySize();
            if (placementSlots.Length != stepNumber)
                AdjustPlacementSlotsArraySize();
            if (washingSteps.Length != stepNumber)
                AdjustWashingStepsArraySize();
        }

        private void AdjustSelectionSlotsArraySize() {
            SelectionWashingStepSlot[] copy = new SelectionWashingStepSlot[selectionSlots.Length];
            Array.Copy(selectionSlots, copy, selectionSlots.Length);
            selectionSlots = new SelectionWashingStepSlot[stepNumber];
            Array.Copy(copy, 0, selectionSlots, 0, Math.Min(copy.Length, stepNumber));
        }

        private void AdjustPlacementSlotsArraySize() {
            PlacementWashingStepSlot[] copy = new PlacementWashingStepSlot[placementSlots.Length];
            Array.Copy(placementSlots, copy, placementSlots.Length);
            placementSlots = new PlacementWashingStepSlot[stepNumber];
            Array.Copy(copy, 0, placementSlots, 0, Math.Min(copy.Length, stepNumber));
        }

        private void AdjustWashingStepsArraySize() {
            WashingStep[] copy = new WashingStep[washingSteps.Length];
            Array.Copy(washingSteps, copy, washingSteps.Length);
            washingSteps = new WashingStep[stepNumber];
            Array.Copy(copy, 0, washingSteps, 0, Math.Min(copy.Length, stepNumber));
        }

        public void ResetGame() {
            ShuffleStepOrder();
            for (int index = 0; index < stepNumber; index++) {
                selectionSlots[index].ResetInitialWashingStep(shuffledSteps[index]);
                placementSlots[index].ResetSlot();
            }
        }

        private void ShuffleStepOrder() {
            ResetIndexArray();
            for (int index = 0; index < stepNumber; index++) {
                int selectedIndex = SelectNewIndex(index);
                shuffledSteps[index] = washingSteps[selectedIndex];
            }
        }

        private void ResetIndexArray() {
            for (int index = 0; index < stepNumber; index++) {
                shuffleNumbers[index] = index;
            }
        }

        private int SelectNewIndex(int index) {
            int randomResult = UnityRandom.Range(0, stepNumber - 1 - index);
            int selectedIndex = shuffleNumbers[randomResult];
            shuffleNumbers[randomResult] = shuffleNumbers[stepNumber - 1 - index];
            shuffleNumbers[stepNumber - 1 - index] = selectedIndex;
            return selectedIndex;
        }

        public void InitSlots() {
            for (int index = 0; index < stepNumber; index++) {
                selectionSlots[index].ResetInitialWashingStep(washingSteps[index]);
                placementSlots[index].ResetSlot();
            }
        }

        public bool TestResults() {
            bool result = true;
            for (int index = 0; index < stepNumber; index++) {
                WashingStep placedStep = placementSlots[index].WashingStep;
                bool indexStepCorrect = placedStep != null && placedStep.StepId == index;
                placedStep.CurrentState = indexStepCorrect ? WashingStepState.Correct : WashingStepState.Wrong;
                result &= indexStepCorrect;
            }
            return result;
        }

        public int ResetWrongSlots() {
            int count = 0;
            for (int index = 0; index < stepNumber; index++) {
                WashingStep placedStep = placementSlots[index].WashingStep;
                if (placedStep.CurrentState == WashingStepState.Wrong) {
                    count++;
                    placedStep.CurrentState = WashingStepState.Available;
                }
            }
            return count;
        }
    }
}

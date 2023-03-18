using System;
using UnityEngine;
using UnityEngine.UI;
using UnityRandom = UnityEngine.Random;
using Toblerone.Toolbox;
using System.Collections;

namespace LavaUmaMao {
    public class SlotShuffler : MonoBehaviour {
        private const int stepNumber = 7;
        [SerializeField] private BoolVariable blockInput;
        [SerializeField] private IntVariable placedStepsCount;
        private VariableObserver<int> placedStepsCountObserver;
        [SerializeField] private EventSO playFullAnimationEvent;
        [SerializeField] private EventSO animationEndedEvent;
        private EventListener animationEndedEventListener;
        [SerializeField] private Button testResultButton;
        [SerializeField] private SelectionWashingStepSlot[] selectionSlots = new SelectionWashingStepSlot[stepNumber];
        [SerializeField] private PlacementWashingStepSlot[] placementSlots = new PlacementWashingStepSlot[stepNumber];
        [SerializeField] private WashingStep[] washingSteps = new WashingStep[stepNumber];
        private WashingStep[] shuffledSteps = new WashingStep[stepNumber];
        private int[] shuffleNumbers = new int[stepNumber];
        private WaitForSeconds placeholderDelay;

        private void OnValidate() {
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

        private void Awake() {
            animationEndedEventListener = new EventListener(animationEndedEvent, ResetGame);
            placedStepsCountObserver = new VariableObserver<int>(placedStepsCount, UpdateTestButtonStatus);
            placeholderDelay = new WaitForSeconds(2f);
        }

        private void ResetGame() {
            placedStepsCount.Value = 0;
            ShuffleStepOrder();
            for (int index = 0; index < stepNumber; index++) {
                selectionSlots[index].ResetInitialWashingStep(shuffledSteps[index]);
                placementSlots[index].ResetSlot();
            }
            blockInput.Value = false;
        }

        private void ShuffleStepOrder() {
            ResetIndexArray();
            for (int index = 0; index < stepNumber; index++) {
                int selectedIndex = SelectNewIndex(index);
                shuffledSteps[index] = washingSteps[selectedIndex];
            }
        }

        private void UpdateTestButtonStatus(int newValue) {
            testResultButton.interactable = newValue >= stepNumber;
        }

        private void OnEnable() {
            animationEndedEventListener.StartListeningEvent();
            placedStepsCountObserver.StartWatching();
        }

        private void OnDisable() {
            animationEndedEventListener.StopListeningEvent();
            placedStepsCountObserver.StopWatching();
        }

        private void Start() {
            blockInput.Value = true;
            placedStepsCount.Value = 0;
            InitSlots();
            playFullAnimationEvent.Raise();
            PlaceholderPlayFullAnimation();
        }

        private void InitSlots() {
            for (int index = 0; index < stepNumber; index++) {
                selectionSlots[index].ResetInitialWashingStep(washingSteps[index]);
                placementSlots[index].ResetSlot();
            }
        }

        private int SelectNewIndex(int index) {
            int randomResult = UnityRandom.Range(0, stepNumber - 1 - index);
            int selectedIndex = shuffleNumbers[randomResult];
            shuffleNumbers[randomResult] = shuffleNumbers[stepNumber - 1 - index];
            shuffleNumbers[stepNumber - 1 - index] = selectedIndex;
            return selectedIndex;
        }

        private void ResetIndexArray() {
            for (int index = 0; index < stepNumber; index++) {
                shuffleNumbers[index] = index;
            }
        }

        private void PlaceholderPlayFullAnimation() {
            StartCoroutine(PlacedholderAnimationCoroutine());
        }

        private IEnumerator PlacedholderAnimationCoroutine() {
            yield return placeholderDelay;
            animationEndedEvent.Raise();
        }

        public void TestResults() {
        }
    }
}

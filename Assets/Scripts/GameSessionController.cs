using System;
using UnityEngine;
using UnityEngine.UI;
using UnityRandom = UnityEngine.Random;
using Toblerone.Toolbox;
using System.Collections;

namespace LavaUmaMao {
    public class GameSessionController : MonoBehaviour {
        private const int stepNumber = 7;
        [SerializeField] private BoolVariable blockInput;
        [SerializeField] private IntVariable placedStepsCount;
        private VariableObserver<int> placedStepsCountObserver;
        [SerializeField] private EventSO playFullAnimationEvent;
        [SerializeField] private EventSO animationEndedEvent;
        private EventListener animationEndedEventListener;
        [SerializeField] private Button testResultButton;
        [SerializeField] private SlotShuffler slotShuffler = new SlotShuffler();
        private WaitForSeconds placeholderDelay;
        private WaitForSeconds wrongAnswerDelay;

        private void OnValidate() {
            slotShuffler.ValidateArrays();
        }

        private void Awake() {
            animationEndedEventListener = new EventListener(animationEndedEvent, ResetGame);
            placedStepsCountObserver = new VariableObserver<int>(placedStepsCount, UpdateTestButtonStatus);
            placeholderDelay = new WaitForSeconds(2f);
            wrongAnswerDelay = new WaitForSeconds(2f);
        }

        private void ResetGame() {
            placedStepsCount.Value = 0;
            slotShuffler.ResetGame();
            blockInput.Value = false;
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
            slotShuffler.InitSlots();
            playFullAnimationEvent.Raise();
            PlaceholderPlayFullAnimation();
        }

        private void PlaceholderPlayFullAnimation() {
            StartCoroutine(PlacedholderAnimationCoroutine());
        }

        private IEnumerator PlacedholderAnimationCoroutine() {
            yield return placeholderDelay;
            animationEndedEvent.Raise();
        }

        public void TestResults() {
            blockInput.Value = true;
            bool result = slotShuffler.TestResults();
            if (result) {
                playFullAnimationEvent.Raise();
                PlaceholderPlayFullAnimation();
            } else {
                StartCoroutine(WrongAnswerCoroutine());
            }
        }

        private IEnumerator WrongAnswerCoroutine() {
            yield return wrongAnswerDelay;
            int wrongSlotsCount = slotShuffler.ResetWrongSlots();
            placedStepsCount.DecrementValue(wrongSlotsCount);
            blockInput.Value = false;
        }
    }
}

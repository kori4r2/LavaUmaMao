using UnityEngine;
using UnityEngine.UI;
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
        private WaitForSeconds wrongAnswerDelay;

        private void OnValidate() {
            slotShuffler.ValidateArrays();
        }

        private void Awake() {
            animationEndedEventListener = new EventListener(animationEndedEvent, ResetGame);
            placedStepsCountObserver = new VariableObserver<int>(placedStepsCount, UpdateTestButtonStatus);
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
        }

        public void TestResults() {
            blockInput.Value = true;
            bool result = slotShuffler.TestResults();
            if (result) {
                playFullAnimationEvent.Raise();
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

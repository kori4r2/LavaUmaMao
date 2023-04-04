using UnityEngine;
using Toblerone.Toolbox;
using UnityEngine.UI;

namespace LavaUmaMao {
    public class InputBlocker : MonoBehaviour {
        [SerializeField] private Image overlay;
        [SerializeField] private BoolVariable blockInput;
        private VariableObserver<bool> inputBlockVariableObserver;

        private void Awake() {
            inputBlockVariableObserver = new VariableObserver<bool>(blockInput, shouldBlock => overlay.enabled = shouldBlock);
        }

        private void OnEnable() {
            inputBlockVariableObserver.StartWatching();
        }

        private void OnDisable() {
            inputBlockVariableObserver.StopWatching();
        }
    }
}

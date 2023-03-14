using UnityEngine;
using UnityEngine.Events;

namespace LavaUmaMao {
    public class RaiseEventOnAnimationExit : StateMachineBehaviour {
        [SerializeField] private UnityEvent callbackEvent = new UnityEvent();
        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
            callbackEvent?.Invoke();
        }
    }
}

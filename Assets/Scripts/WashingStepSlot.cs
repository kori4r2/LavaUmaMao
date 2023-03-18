using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace LavaUmaMao {
    public abstract class WashingStepSlot : UIBehaviour {
        [SerializeField] protected UIBehaviour slotOutline = null;
        [SerializeField] protected Image slotImage = null;
        [SerializeField] protected WashingStepVariable draggedWashingStepReference = null;
        public abstract WashingStep WashingStep { get; protected set; }

        protected abstract void WashingStepStateChanged(WashingStepState newState);

        protected new void OnDestroy() {
            if (WashingStep != null)
                WashingStep.RemoveStateChangeListener(WashingStepStateChanged);
            base.OnDestroy();
        }

        public virtual void OnPointerEnter() {
            slotOutline.enabled = true;
        }

        public virtual void OnPointerExit() {
            slotOutline.enabled = false;
        }

        public abstract void OnPointerDown();

        public abstract void OnPointerUp();
    }
}

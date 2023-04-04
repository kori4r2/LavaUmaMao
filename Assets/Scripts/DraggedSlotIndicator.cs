using UnityEngine;
using UnityEngine.UI;
using Toblerone.Toolbox;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace LavaUmaMao {
    public class DraggedSlotIndicator : UIBehaviour {
        [SerializeField] private Image image = null;
        [SerializeField] private WashingStepVariable draggedWashingStep;
        [SerializeField] private InputActionReference movePointerAction;
        private VariableObserver<WashingStep> draggedWashingStepObserver;

        private new void Awake() {
            base.Awake();
            UpdateImage(null);
            draggedWashingStepObserver = new VariableObserver<WashingStep>(draggedWashingStep, UpdateImage);
        }

        private void UpdateImage(WashingStep newValue) {
            image.sprite = newValue != null ? newValue.StepSprite : null;
            image.color = newValue != null ? Color.white : Color.clear;
        }

        private new void OnEnable() {
            base.OnEnable();
            movePointerAction.action.performed += UpdatePointerPosition;
            draggedWashingStepObserver.StartWatching();
        }

        private void UpdatePointerPosition(InputAction.CallbackContext callbackContext) {
            Vector2 screenSize = new Vector2(Screen.width, Screen.height);
            Vector2 screenPosition = ClampVector2(callbackContext.ReadValue<Vector2>(), Vector2.zero, screenSize);
            transform.position = new Vector3(screenPosition.x, screenPosition.y, transform.position.z);
        }

        private static Vector2 ClampVector2(Vector2 value, Vector2 minValue, Vector2 maxValue) {
            return new Vector2(Mathf.Clamp(value.x, minValue.x, maxValue.x), Mathf.Clamp(value.y, minValue.y, maxValue.y));
        }

        private new void OnDisable() {
            draggedWashingStepObserver.StopWatching();
            movePointerAction.action.performed -= UpdatePointerPosition;
            base.OnDisable();
        }
    }
}

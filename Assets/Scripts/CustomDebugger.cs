using UnityEngine;
using UnityEngine.UI;

namespace LavaUmaMao {
    public class CustomDebugger : MonoBehaviour {
        public static CustomDebugger Instance {get; private set;} = null;

        private void Awake(){
            if(Instance != null && Instance != this){
                Destroy(this);
                return;
            }

            Instance = this;
        }

        private void OnDestroy(){
            if(Instance == this){
                Instance = null;
            }
        }

        public static void Log(string line){
            if(Instance == null) return;
            Instance.LogNewLine(line);
        }

        [SerializeField] private Text textBox;
        [SerializeField] private Scrollbar scrollbar;

        private void LogNewLine(string line){
            textBox.text += $"\n  {line}";
            scrollbar.value = 0;
        }

        private void LateUpdate(){
            if(scrollbar.value > Mathf.Epsilon)
                scrollbar.value = 0;
        }
    }
}

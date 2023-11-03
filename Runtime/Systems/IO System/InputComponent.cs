using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.Events;

namespace IOActions
{
    public class InputComponent : MonoBehaviour
    {
        [Header("Base Input Component")]
        public UnityEvent<InputComponent> OnInputSignal;
        public UnityEvent<OutputComponent> OnOutputConnected;
        public List<OutputComponent> connectedOutputs = new List<OutputComponent>();

        private void Start()
        {
            if (!IOActionManager.Instance.inputComponents.Contains(this))
            {
                IOActionManager.Instance.inputComponents.Add(this);
            }
        }

        private void OnDestroy()
        {
            if (IOActionManager.Instance.inputComponents.Contains(this))
            {
                IOActionManager.Instance.inputComponents.Remove(this);
            }
        }

        [Button()]
        public void ConncetToOutput(OutputComponent outputComponent)
        {
            if (connectedOutputs.Contains(outputComponent)) { return; }

            connectedOutputs.Add(outputComponent);
            outputComponent.ConnectToInputComponent(this);
            OnOutputConnected.Invoke(outputComponent);
        }

        [Button()]
        public virtual void SendInputSignal()
        {
            OnInputSignal.Invoke(this);
        }
    }
}

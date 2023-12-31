using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IOActions
{
    public class OutputComponent : MonoBehaviour
    {
        [Header("Base Output Component")]
        [SerializeField] private List<InputComponent> connectedInputs = new List<InputComponent>();

        private void Start()
        {
            if (!IOActionManager.Instance.outputActionComponents.Contains(this)) {
                IOActionManager.Instance.outputActionComponents.Add(this);
            }
        }

        private void OnDestroy()
        {
            if (IOActionManager.Instance.outputActionComponents.Contains(this)) {
                IOActionManager.Instance.outputActionComponents.Remove(this);
            }
        }

        [Button()] public void ConnectToInputComponent(InputComponent inputComponent)
        {
            if (connectedInputs.Contains(inputComponent)) { return; }

            connectedInputs.Add(inputComponent);
            inputComponent.OnInputSignal.AddListener(InputSignalRecived);
        }

        [Button()] public void DisconnectFromInputComponet(InputComponent inputComponent)
        {
            if (!connectedInputs.Contains(inputComponent)) { return; }

            connectedInputs.Remove(inputComponent);
            inputComponent.OnInputSignal.RemoveListener(InputSignalRecived);
        }

        public virtual void InputSignalRecived(InputComponent inputComponent)
        {
            Debug.Log(this.gameObject.name + " recived a input from " + inputComponent.gameObject.name);
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Rufas
{

    [System.Serializable]
    [HideReferenceObjectPicker]
    public class Condition_MonoState : GenericCondition
    {
        [HorizontalGroup("V", Width = 80)]
        [HideLabel]
        [GUIColor("$GuiColour")]
        [DisableInPlayMode]
        public ActiveState compareTo = ActiveState.Active;

        [HorizontalGroup("V")]
        [HideLabel]
        [InlineEditor]
        [GUIColor("$CurrentGuiColour")]
        public MonoState stateValue;

        public enum ActiveState
        {
            Active,
            NotActive
        }

        private Color GuiColour()
        {
            if (compareTo == ActiveState.Active)
            {
                return new Color(0.8f, 1, 1f);

            }
            else
            {
                return new Color(1, 0.8f, 1);
            }
        }

        private Color CurrentGuiColour()
        {
            if (stateValue == null)
            {
                return new Color(1, 0.95f, 1);
            }

            if (stateValue.IsCurrentState == true)
            {
                return new Color(0.95f, 1, 1f);
            }
            else
            {
                return new Color(1, 0.95f, 1);
            }
        }

        public override void AwakeInput(Action<bool> onChanged)
        {
            base.AwakeInput(onChanged);
            // stateValue.onStateChanged += StateChanged;

            stateValue.AddListener(StateChanged);
        }

        public override void DestroyInput()
        {
            base.DestroyInput();
            //stateValue.onStateChanged -= StateChanged;
            stateValue.RemoveListener(StateChanged);
        }

        private void StateChanged(bool val)
        {
            callOnMetChanged.Invoke(GetConditionMet());
        }

        public override bool GetConditionMet()
        {
            if (stateValue == null) { Debug.Log("STATE NULL!"); return true; }

            if (stateValue.IsCurrentState == true && compareTo == ActiveState.Active
                || stateValue.IsCurrentState == false && compareTo == ActiveState.NotActive)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public override void SetMet(bool setMet)
        {
            Debug.Log("Cannot 'SetMet' for this object");
            return;
        }
    }
}

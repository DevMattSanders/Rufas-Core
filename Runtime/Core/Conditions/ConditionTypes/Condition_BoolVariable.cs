using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.YamlDotNet.Core.Tokens;
using UnityEngine;
using static UnityEngine.CullingGroup;

namespace Rufas
{
    public class Condition_BoolVariable : GenericCondition
    {

        [HorizontalGroup("V", Width = 80)]
        [HideLabel]
        [GUIColor("$GuiColour")]
        [DisableInPlayMode]
        public bool compareTo = false;

        [InlineEditor]
        [HorizontalGroup("V")]
        [HideLabel]
        [GUIColor("$CurrentGuiColour")]

        public BoolVariable Value;

        private Color GuiColour()
        {
            if (compareTo == true)
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
            if (Value == null)
            {
                return new Color(1, 0.95f, 1);
            }

            if (Value.Value == true)
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
            Value.AddListener(StateChanged);
        }

        public override void DestroyInput()
        {
            base.DestroyInput();
            Value.RemoveListener(StateChanged);
        }

        private void StateChanged(bool val)
        {
            callOnMetChanged.Invoke(GetConditionMet());
        }

        public override bool GetConditionMet()
        {
            if (Value == null) { Debug.Log("STATE NULL!"); return true; }

            if (Value.Value == compareTo)
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
            if (Value != null) Value.Value = setMet;
            // Debug.Log("Cannot 'SetMet' for this object");
            return;
        }
    }
}
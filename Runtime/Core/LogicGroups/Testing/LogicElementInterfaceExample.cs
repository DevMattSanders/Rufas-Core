using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rufas
{
    public class LogicElementInterfaceExample : MonoBehaviour
    {
      
        public LogicGroup myLogicGroup;

        public BoolWithCallback enableLogicGroup;

        private void OnEnable()
        {
            //enableLogicGroup.Value = true;
            myLogicGroup.RegisterEnabler(this, enableLogicGroup.Value, false);
            enableLogicGroup.AddListener(SetLogicGroupEnabled);
        }

        private void OnDisable()
        {
           // enableLogicGroup.Value = false;
            myLogicGroup.UnregisterEnabler(this);
            enableLogicGroup.RemoveListener(SetLogicGroupEnabled);
        }

        private void SetLogicGroupEnabled(bool val)
        {
            if (val)
            {
                myLogicGroup.EnableFromRegisteredEnabler(this);
            }
            else
            {
                myLogicGroup.DisableFromRegisteredEnabler(this);
            }
        }

    }
}

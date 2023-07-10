using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rufas
{
    public class LogicElementInterfaceExample : MonoBehaviour
    {
      
        public LogicGroup myLogicGroup;

        public BoolWithCallback enableLogicGroup;



        private void Start()
        {
            myLogicGroup.RegisterEnabler(this, enableLogicGroup.Value, false);
            enableLogicGroup.onValue += SetLogicGroupEnabled;
        }

        private void OnDestroy()
        {
            myLogicGroup.UnregisterEnabler(this);
            enableLogicGroup.onValue -= SetLogicGroupEnabled;
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

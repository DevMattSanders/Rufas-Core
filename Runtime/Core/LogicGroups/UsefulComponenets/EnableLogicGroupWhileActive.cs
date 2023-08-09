using Rufas;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlotCarVR
{
    public class EnableLogicGroupWhileActive : MonoBehaviour
    {
        public LogicGroup logicGroup;

        private void Awake()
        {
            logicGroup.RegisterEnabler(this, gameObject.activeInHierarchy, false);
        }

        private void OnEnable()
        {
            logicGroup.EnableFromRegisteredEnabler(this);

        }

        private void OnDisable()
        {
            logicGroup.DisableFromRegisteredEnabler(this);

        }

        private void OnDestroy()
        {
            logicGroup.UnregisterEnabler(this);
        }
    }
}

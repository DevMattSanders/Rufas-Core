using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rufas
{
    [RequireComponent(typeof(LogicElement))]
    public class LogicReactor : MonoBehaviour
    {
        [HideInInspector]
        public LogicElement logicElement;

        public virtual void GetLogicElement()
        {
            logicElement = GetComponent<LogicElement>();
        }

        public virtual void Awake()
        {
            logicElement = GetComponent<LogicElement>();

            logicElement.logicCaseChanged.AddListener(ConditionChangedOnLogicElement);
        }

        public virtual void OnDestory()
        {
            logicElement.logicCaseChanged.RemoveListener(ConditionChangedOnLogicElement);
        }      

        public virtual void LogicElementDisabled()
        {

        }

        public virtual void ConditionChangedOnLogicElement(int val)
        {

        }
    }
}

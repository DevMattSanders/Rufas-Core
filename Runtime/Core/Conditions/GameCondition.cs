using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

namespace Rufas
{
    [System.Serializable]
    [InlineProperty]
    public class GameCondition
    {
        public bool conditionMet
        {
            get
            {
                if (conType == ConditionType.i)
                {
                    if (conditionDataInstance == null)
                    {
                        return false;
                    }
                    return conditionDataInstance.GetMet();
                }
                else
                {
                    if (conditionDataScriptable == null)
                    {
                        return false;
                    }

                    return conditionDataScriptable.GetMet();
                }
            }
        }

        public void SetMet(bool setMet)
        {
            if (conType == ConditionType.i && conditionDataInstance != null)
            {
                conditionDataInstance.SetMet(setMet);
            }
            else if (conditionDataScriptable != null)
            {
                conditionDataScriptable.SetMet(setMet);
            }
        }

        public bool IsNull()
        {
            if (conType == ConditionType.i && conditionDataInstance.conditions.Count == 0)
            {
                return true;
            }
            else if (conType == ConditionType.so && conditionDataScriptable == null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        [HideLabel]
        [HorizontalGroup("H", Width = 40)]
        [DisableInPlayMode]
        [SerializeField] private ConditionType conType = ConditionType.i;

        [HorizontalGroup("H")]
        [ShowIf("ShowScriptableCon")]
        [SerializeField] private GameConditionData conditionDataScriptable;

        [HorizontalGroup("H")]
        [HideIf("ShowScriptableCon")]
        [SerializeField] private GameConditionDataInstance conditionDataInstance;

        public enum ConditionType
        {
            so,
            i
        }

        private bool ShowScriptableCon()
        {
            if (conType == ConditionType.so)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void AddListener(UnityAction<bool> onConditionMetChanged, bool invokeListenerNow)
        {
            if (conType == ConditionType.so && conditionDataScriptable == null) return;

            if (conType == ConditionType.so)
            {
                conditionDataScriptable.onConditionChanged.AddListener(onConditionMetChanged);
            }
            else
            {
                conditionDataInstance.AwakeInput(onConditionMetChanged);
            }

            if (invokeListenerNow)
            {
                onConditionMetChanged.Invoke(conditionMet);
            }
        }

        public void RemoveListener(UnityAction<bool> onConditionMetChanged)
        {
            if (conType == ConditionType.so && conditionDataScriptable == null) return;

            if (conType == ConditionType.so)
            {
                conditionDataScriptable.onConditionChanged.RemoveListener(onConditionMetChanged);
            }
            else
            {
                conditionDataInstance.DestroyInput(onConditionMetChanged);
            }
        }
    }
}

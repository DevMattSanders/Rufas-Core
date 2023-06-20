using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Rufas
{
    [System.Serializable]
    public class GenericCondition
    {

        public enum AndOr
        {
            And,
            Or
        }

        [HideLabel]
        [HorizontalGroup("V", Width = 70)]
        public AndOr andOr = AndOr.And;

        protected Action<bool> callOnMetChanged;


        public virtual void AwakeInput(Action<bool> onChanged)
        {
            callOnMetChanged = onChanged;
        }

        public virtual void DestroyInput()
        {
        }

        public virtual bool GetConditionMet()
        {
            Debug.Log("No Implementation of GET MET here");
            return false;
        }

        public virtual void SetMet(bool setMet)
        {
            Debug.Log("No Implementation of SET MET here");
        }
    }
}

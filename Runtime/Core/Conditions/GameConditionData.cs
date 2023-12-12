using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;
using UnityEngine.Events;

namespace Rufas
{
    [CreateAssetMenu(menuName = "Rufas/Game Condition/Game Condition Data")]
    [InlineEditor]
    [HideLabel]
    public class GameConditionData : SuperScriptable
    {
        [HideLabel]
        [SerializeField] private GameConditionDataInstance conditionDataInstance;

        [HideInInlineEditors]
        [SerializeField]
        private UnityEvent editorOnConditionMet = new UnityEvent();

        // [HideInInspector]
        // public System.Action<bool> onConditionChanged;

        public UnityEvent<bool> onConditionChanged
        {
            get
            {
                return conditionDataInstance.onConditionChanged;
            }
        }

        public override void SoOnAwake()
        {
            base.SoOnAwake();

            conditionDataInstance.AwakeInput(OnConditionChanged);
        }

        public override void SoOnEnd()
        {
            base.SoOnEnd();
            conditionDataInstance.DestroyInput(OnConditionChanged);
        }

        private void OnConditionChanged(bool val)
        {
            if (val) editorOnConditionMet.Invoke();
        }

        public bool GetMet()
        {
            if (conditionDataInstance != null)
            {
                return conditionDataInstance.GetMet();
            }

            return false;
        }

        public void SetMet(bool setMet)
        {
            if (conditionDataInstance != null)
            {
                conditionDataInstance.SetMet(setMet);
            }
        }
    }
    [System.Serializable]
    [HideLabel]
    public class GameConditionDataInstance
    {

     
        [HideInInspector]
        [SerializeField]
        private bool ifNoCondition_ReturnAs = true;

        [PropertyOrder(0)]
        [GUIColor("IfNoConditionColour")]
        [ShowIf("ShowIfNoConditionButton")]
        [Button(Name = "$IfNoConditionLabel")]
        private void IfNoCondition_ReturnAsButtonClicked()
        {
            ifNoCondition_ReturnAs = !ifNoCondition_ReturnAs;
        }

        private string IfNoConditionLabel()
        {
            if (ifNoCondition_ReturnAs)
            {
                return "Returns -- TRUE -- as there are no conditions";
            }
            else
            {
                return "Returns -- FALSE -- as there are no conditions";
            }
        }

        private Color IfNoConditionColour()
        {
            if (ifNoCondition_ReturnAs)
            {
                return new Color(0.8f, 0.8f, 1, 1);
            }
            else
            {
                return new Color(1,0.8f, 0.8f, 1);
            }
        }

        private bool ShowIfNoConditionButton()
        {
            if(conditions.Count == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        [PropertyOrder(1)]
        [ShowInInspector]
        [SerializeReference]
        [OdinSerialize]
        [HideReferenceObjectPicker]
        [ListDrawerSettings(ShowFoldout = false, HideRemoveButton = true)]
        public List<GenericCondition> conditions = new List<GenericCondition>();

        //public System.Action<bool> onConditionChanged;
        [HideInInspector]
        public UnityEvent<bool> onConditionChanged = new UnityEvent<bool>();
        public void SetMet(bool setMet)
        {
            foreach (GenericCondition next in conditions)
            {
                next.SetMet(setMet);
            }
        }

        public void AwakeInput(UnityAction<bool> onChanged)
        {
            // onConditionChanged += onChanged;
            onConditionChanged.AddListener(onChanged);

            foreach (GenericCondition next in conditions)
            {
                // Debug.Log("Awake input to actual generic conditions");
                next.AwakeInput(AnyConditionChanged);
            }
        }

        public void DestroyInput(UnityAction<bool> onChanged)
        {
            // onConditionChanged -= onChanged;

            foreach (GenericCondition next in conditions)
            {
                next.DestroyInput();
            }
        }

        private void AnyConditionChanged(bool conditionValue)
        {
            CheckAndInvokeEvent();
            //  Debug.Log("AnyConditionChanged");
        }

        public bool GetMet()
        {
            bool met = false;
            if (conditions.Count == 0)
            {
                return ifNoCondition_ReturnAs;
            }
            else
            {
                bool _met = false;
                bool lastMet = true;
                bool first = true;

                foreach (GenericCondition next in conditions)
                {
                    if (next.andOr == GenericCondition.AndOr.And) //Continuing
                    {
                        if (lastMet == false && first == false) continue; //Skipping to either end or next starting new point

                        if (next.GetConditionMet())
                        {
                            lastMet = true;
                        }
                        else
                        {
                            lastMet = false;
                        }

                    }
                    else //Starting new
                    {
                        if (lastMet && first == false)
                        {
                            _met = true;
                            break;
                        }
                        else
                        {
                            if (next.GetConditionMet())
                            {
                                lastMet = true;
                            }
                            else
                            {
                                lastMet = false;
                            }
                        }
                    }
                    first = false;
                }

                if (lastMet) _met = true;

                met = _met;
            }
            return met;
        }

        public void CheckAndInvokeEvent()
        {
            if (onConditionChanged == null)
            {
                //   Debug.Log("OnConditionChanged = null");
                return;
            }
            if (GetMet())
            {
                //  Debug.Log("Checked and true!");
                onConditionChanged.Invoke(true);
            }
            else
            {
                //  Debug.Log("Checked and false!");
                onConditionChanged.Invoke(false);
            }
        }
    }
}
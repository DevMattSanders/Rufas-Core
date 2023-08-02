using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace Rufas
{
  
    public class TweenElement : LogicReactor
    {       

        private BoolWithCallback elementActive;
        private BoolWithCallback elementInFinishedState;
     
        [PropertyOrder(2)]
        [GUIColor(1, 0.8f, 0.8f, 1)]
        public TweenCase defaultData;

        [OnInspectorInit]
        private void UpdateDefaultData()
        {
            defaultData.showCondition = false;
            defaultData.tweenElement = this;
            defaultData.tweenCaseIndex = -1;
        }

        [SerializeReference]
        [HideReferenceObjectPicker]
        [OnValueChanged("AddToTweenCases")]
        [HorizontalGroup("H")]
        private zBaseTween toAdd;

        [SerializeReference]
        [HideReferenceObjectPicker]
        [OnValueChanged("RemoveFromTweenCases")]
        [HorizontalGroup("H")]
        private zBaseTween toRemove;

        private void AddToTweenCases()
        {
            if (toAdd != null)
            {
                Type toAddType = toAdd.GetType();

                if (!defaultData.tweens.Any(tween => tween.GetType() == toAddType))
                {
                    defaultData.tweens.Add((zBaseTween)Activator.CreateInstance(toAddType));
                }

                foreach (TweenCase t in tweenCases)
                {
                    if (!t.tweens.Any(tween => tween.GetType() == toAddType))
                    {
                        t.tweens.Add((zBaseTween)Activator.CreateInstance(toAddType));
                    }
                }
            }

            toAdd = null;
        }

       

        private void RemoveFromTweenCases()
        {
            if (toRemove != null)
            {
                defaultData.tweens.RemoveAll(tween => tween.GetType() == toRemove.GetType());

                foreach (TweenCase t in tweenCases)
                {
                    t.tweens.RemoveAll(tween => tween.GetType() == toRemove.GetType());
                }
            }

            toRemove = null;
        }

        [Header("TWEEN CASES")]
        [PropertyOrder(3)]
        [ListDrawerSettingsAttribute(ShowFoldout = false, ShowIndexLabels = true)]
        [OnValueChanged("RefreshTweenCases")]
        public List<TweenCase> tweenCases = new List<TweenCase>();

        private void RefreshTweenCases()
        {
            int refreshIndex = 0;

            foreach (TweenCase tweenCase in tweenCases)
            {
                tweenCase.tweenElement = this;
                tweenCase.tweenCaseIndex = refreshIndex;
                refreshIndex++;
            }
        }

        public override void GetLogicElement()
        {
            base.GetLogicElement();

            RefreshTweenCases();
            Debug.Log("GotLogicElement");
        }



        //   [Button("Refresh")]
        [ContextMenu("Refresh Cases Size")]
        private void RefreshCasesSize()
        {
            if (logicElement == null) logicElement = GetComponent<LogicElement>();

            if (logicElement != null)
            {
                while (tweenCases.Count > logicElement.cases.Count)
                {
                    tweenCases.RemoveAt(tweenCases.Count - 1);
                }

                while (tweenCases.Count < logicElement.cases.Count)
                {
                    tweenCases.Add(new TweenCase());
                }

                Debug.Log("Here: " + tweenCases.Count);
                for (int i = 0; i < tweenCases.Count; i++)
                {
                    tweenCases[i].conditionValue = logicElement.cases[i].conditionEvent;
                }
            }

            RefreshTweenCases();
        }


        private int lastTweenIndex;



        public DG.Tweening.Tween moveTween, rotateTween, scaleTween, fadeTween;



        //--METHODS--//

        public override void Awake()
        {
            base.Awake();

            if (logicElement.cases.Count != tweenCases.Count)
            {
                Debug.Log("Reactor cases count not the same as logic element cases count!!!");
            }
        }

        [HideInEditorMode]
        [Button]
        public void ManualSetCase(int caseIndex)
        {
            if (gameObject.name != "Image" && gameObject.name != "Img_Garden")
            {
            }
            if (caseIndex < -1)
            {
                caseIndex = -1;
            }

            if (caseIndex >= (tweenCases.Count))
            {
                caseIndex = (tweenCases.Count - 1);
            }

            if (lastTweenIndex == caseIndex) return;

            if (lastTweenIndex != -1)
            {
                tweenCases[lastTweenIndex].StopActions();
            }
            else
            {
                defaultData.StopActions();
            }


            lastTweenIndex = caseIndex;

            elementInFinishedState.Value = false;

            if (caseIndex == -1)
            {
                defaultData.StartActions(transform);
            }
            else
            {
                tweenCases[caseIndex].StartActions(transform);
            }
        }


        private void ResetValues()
        {
            lastTweenIndex = -1;
            defaultData.GoToValuesImmediate();
        }

        private void OnEnable()
        {

            foreach (TweenCase tweenCase in tweenCases)
            {
                tweenCase.UpdateBaseTweens();
            }

            defaultData.UpdateBaseTweens();

            ResetValues(); 
        }

        private void OnDisable()
        {
        }


        public override void LogicElementDisabled()
        {
            base.LogicElementDisabled();

            ManualSetCase(-1);
        }

        public override void ConditionChangedOnLogicElement(int val)
        {
            base.ConditionChangedOnLogicElement(val);

            ManualSetCase(val);
        }
        public void IndexCompleted(int completedIndex)
        {
            if (completedIndex != lastTweenIndex) return;

            if (completedIndex == -1)
            {
                elementInFinishedState.Value = false;
            }
            else
            {
                elementInFinishedState.Value = true;
            }
        }
    }
}

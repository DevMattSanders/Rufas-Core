using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Rufas
{
    public class LogicGroupMonoEnabler : MonoBehaviour
    {

#if UNITY_EDITOR        
        private string debugState;


        [Title("$debugState",titleAlignment: TitleAlignments.Left,horizontalLine: true, bold: true)]
        [PropertySpace(5)]
#endif


        [GUIColor("GUIColour")]
        [DisableInPlayMode]
        [ListDrawerSettings(Expanded = true)]
        public LogicGroup[] logicGroups;

        [DisableInPlayMode]
        [SerializeField] private bool listenerOnly;

        [FoldoutGroup("EnablerRules")]
        [HideIf("listenerOnly")]
        [GUIColor("GUIColour")]
        [DisableInPlayMode]
        [HorizontalGroup("EnablerRules/H")]
        public bool EnableLogicInOnEnable;

        [HideIf("listenerOnly")]
        [GUIColor("GUIColour")]
        [DisableInPlayMode]
        [HorizontalGroup("EnablerRules/H")]
        public bool DisableLogicInOnDisable;

        private Color GUIColour()
        {
            if (listenerOnly)
            {
                if (IsOverriden)
                {
#if UNITY_EDITOR
                    debugState = "--OVERRIDEN--";
#endif
                    return new Color(1, 0.6f, 0.4f, 1); //Orange

                }
                else
                {
#if UNITY_EDITOR
                    debugState = "--LISTENER ONLY--";
#endif
                    return new Color(0.8f, 0.8f, 1, 1); //Blue
                }
            }
            else {
                if (IsEnabled)
                {
                    if (IsOverriden)
                    {
#if UNITY_EDITOR
                        debugState = "--OVERRIDEN--";
#endif
                        return new Color(1, 0.6f, 0.4f, 1); //Orange
                    }
                    else
                    {
#if UNITY_EDITOR
                        debugState = "--ENABLER ENABLED--";
#endif
                        return new Color(0.8f, 1, 0.8f, 1); //Green
                    }
                }
                else
                {
#if UNITY_EDITOR
                    debugState = "--DISABLED--";
#endif
                    return new Color(1, 0.8f, 0.8f, 1); //Red
                }
            }
        }
        
        public bool IsOverriden { get; private set; }

        public bool IsEnabled { get; private set; }

      

        [FoldoutGroup("Override Events")]
        public UnityEvent OnOverridenTrue = new UnityEvent();

        [FoldoutGroup("Override Events")]
        public UnityEvent OnOverridenFalse = new UnityEvent();

        private void OnEnable()
        {
          

            foreach(LogicGroup group in logicGroups)
            {
                group.RegisterEnabler(this, IsEnabled, listenerOnly);

                group.OverrideStateChanged += RefreshIfOverriden;
            }

            if (EnableLogicInOnEnable) EnableLogic();

                RefreshIfOverriden();
        }

        private void OnDisable()
        {

            if (DisableLogicInOnDisable) DisableLogic();

            foreach (LogicGroup group in logicGroups)
            {
                group.UnregisterEnabler(this);

                group.OverrideStateChanged -= RefreshIfOverriden;
            }
        }

        public void RefreshIfOverriden()
        {
            bool shouldOverride = false;

            foreach(LogicGroup group in logicGroups)
            {
                if (group.IsOverriden)
                {
                    shouldOverride = true;
                    break;
                }
            }

            if (shouldOverride && IsOverriden == false)
            {
                IsOverriden = true;
                OnOverridenTrue.Invoke();
            }
            else if (IsOverriden && shouldOverride == false)
            {
                IsOverriden = false;
                OnOverridenFalse.Invoke();
            }
        }

        private bool ShowEnableLogicButton()
        {
            if (listenerOnly || Application.isPlaying == false) return false;

            if (IsEnabled) return false;

            return true;
        }

        private bool ShowDisableLogicButton()
        {
            if (listenerOnly || Application.isPlaying == false) return false;

            if (IsEnabled) return true;

            return false;
        }

        [GUIColor("GUIColour")]
        [Button,ShowIf("ShowEnableLogicButton"),DisableInEditorMode]
        public void EnableLogic()
        {
            if (!listenerOnly)
            {
                foreach (LogicGroup next in logicGroups)
                {
                    next.EnableFromRegisteredEnabler(this);
                }

                IsEnabled = true;
            }

            RefreshIfOverriden();
        }

        [GUIColor("GUIColour")]
        [Button,ShowIf("ShowDisableLogicButton"), DisableInEditorMode]
        public void DisableLogic()
        {
            if (!listenerOnly)
            {
                foreach (LogicGroup next in logicGroups)
                {
                    next.DisableFromRegisteredEnabler(this);
                }

                IsEnabled = false;
            }
            RefreshIfOverriden();
        }

      
    }
}

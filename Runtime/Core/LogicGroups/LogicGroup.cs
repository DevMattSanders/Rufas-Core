using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.TextCore;

namespace Rufas
{
    [CreateAssetMenu(menuName = "Rufas/LogicGroups/LogicGroup")]
    public class LogicGroup : SuperScriptable
    {
     //   public System.Action EnabledStateChanged;
        [ShowInInspector,ReadOnly] public BoolWithCallback IsEnabled;// { get; private set; }

        [ShowInInspector, ReadOnly] public BoolWithCallback IsOverriden;
      //  public System.Action OverrideStateChanged;
         //public bool IsOverriden { get; private set; }

        private Dictionary<Object,bool> registeredEnablers = new Dictionary<Object, bool>();

#if UNITY_EDITOR

        private List<Object> registeredListeners = new List<Object>();

        [ReadOnly,HideLabel]
        [SerializeField] private List<LogicGroupEnablerEditorDebug> logicGroupEnablerEditorDebugView = new List<LogicGroupEnablerEditorDebug>();
#endif

        public override void SoOnAwake()
        {
            base.SoOnAwake();
            ResetValues();
        }

        public override void SoOnEnd()
        {
            base.SoOnEnd();
            ResetValues();
        }

        public void EnableFromRegisteredEnabler(Object enabler)
        {
            if (registeredEnablers.ContainsKey(enabler))
            {
                if (registeredEnablers[enabler] == true) return;

                registeredEnablers[enabler] = true;

                RefreshRegisteredEnablers();
            }           
        }

        public void DisableFromRegisteredEnabler(Object enabler)
        {
            if (registeredEnablers.ContainsKey(enabler))
            {
                if (registeredEnablers[enabler] == false) return;

                registeredEnablers[enabler] = false;

                RefreshRegisteredEnablers();
            }
        }

        private void RefreshRegisteredEnablers()
        {
            bool enableLogicGroup = false;

            foreach(KeyValuePair<Object,bool> next in registeredEnablers)
            {
                if (next.Value)
                {
                    enableLogicGroup = true;
                    break;
                }
            }


            if (enableLogicGroup && IsEnabled.Value == false)
            {
                EnableLogicGroup();
            }else if (enableLogicGroup == false && IsEnabled.Value)
            {
                DisableLogicGroup();
            }
#if UNITY_EDITOR
            RefreshEditorDebugListView();
#endif
        }

        private void EnableLogicGroup()
        {
            IsEnabled.Value = true;
            //EnabledStateChanged?.Invoke();
         
        }

        private void DisableLogicGroup()
        {
            IsEnabled.Value = false;
            //EnabledStateChanged?.Invoke();
          
        }


        //Should be a list (OverridenBy) of logic override groups that update
   
        public void OverrideLogicGroup()
        {
            IsOverriden.Value = true;
            //IsOverriden
           // OverrideStateChanged?.Invoke();
        }

        //Should remove the logic override group from OverridenBy list
     
        public void UnoverrideLogicGroup()
        {
            IsOverriden.Value = false;
          //  OverrideStateChanged?.Invoke();
        }

        
        public void RegisterEnabler(Object enabler, bool isEnablerEnabled, bool listenerOnly)
        {            
                if (!registeredEnablers.ContainsKey(enabler))
                {

                if (listenerOnly)
                {
                    registeredEnablers.Add(enabler, false);
                }
                else
                {
                    registeredEnablers.Add(enabler, isEnablerEnabled);
                }

#if UNITY_EDITOR
                if (listenerOnly)
                {
                    registeredListeners.Add(enabler);
                }
#endif

                //Debug.Log(registeredEnablers.Count);

                    if (isEnablerEnabled)
                    {
                        RefreshRegisteredEnablers();
                    }
                    else
                    {

#if UNITY_EDITOR
                        RefreshEditorDebugListView();
#endif
                    }
                }
            
        }

        public void UnregisterEnabler(Object enabler)
        {
            if (registeredEnablers.ContainsKey(enabler))
            {
                bool tempVal = registeredEnablers[enabler];

                registeredEnablers.Remove(enabler);

#if UNITY_EDITOR
                if (registeredListeners.Contains(enabler))
                {
                    registeredListeners.Remove(enabler);
                }
#endif

                if (tempVal)
                {
                    RefreshRegisteredEnablers();
                }
                else
                {
#if UNITY_EDITOR
                    RefreshEditorDebugListView();
#endif
                }
            }


        }


        private void ResetValues()
        {

            registeredEnablers.Clear();

#if UNITY_EDITOR
            registeredListeners.Clear();
            logicGroupEnablerEditorDebugView.Clear();
#endif

            IsEnabled.Value = false;
            IsOverriden.Value = false;
        }

#if UNITY_EDITOR
        private void RefreshEditorDebugListView()
        {
            logicGroupEnablerEditorDebugView.Clear();

            foreach(KeyValuePair<Object,bool> next in registeredEnablers)
            {
                if (next.Key == null) continue;

                bool listenerOnly = false;

                if (registeredListeners.Contains(next.Key)) listenerOnly = true;


                logicGroupEnablerEditorDebugView.Add(new LogicGroupEnablerEditorDebug(next.Key, next.Value, listenerOnly));
            }
        }

        [System.Serializable]
        public class LogicGroupEnablerEditorDebug
        {
            public LogicGroupEnablerEditorDebug(Object _enabler, bool _isEnabled, bool _listenerOnly)
            {
                enabler = _enabler;
                isEnabled = _isEnabled;
                listenerOnly = _listenerOnly;
            }

            [HorizontalGroup("H"), HideLabel, SerializeField, ReadOnly]
            public Object enabler;

            [HorizontalGroup("H"), HideLabel, ReadOnly, SerializeField, HideIf("listenerOnly")]
            public bool isEnabled = false;

            [HorizontalGroup("H"), HideLabel, ReadOnly, SerializeField, ShowIf("listenerOnly")]
            private string listenerOnlytext = "Listener Only";

            private bool listenerOnly = false;
        }

#endif

    }

    [System.Serializable]
    public class LogicGroupReference
    {
        [HorizontalGroup("H")]
        [HideLabel]
        [OnValueChanged("TrackUpdated")]
        public LogicGroup group;

        [HorizontalGroup("H")]
        [HideLabel]
        [ShowIf("ShowTrackName")]
        [ReadOnly]
        public string trackName;

        private void TrackUpdated()
        {
            if (group != null)
            {
                trackName = group.name;
            }
        }

        private bool ShowTrackName()
        {
            if (group == null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool TrackOverriden()
        {
            if (group == null)
            {
                return false;
            }

            return group.IsOverriden.Value;
        }
    }
}
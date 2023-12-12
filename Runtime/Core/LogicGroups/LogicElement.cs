using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Rufas
{

    [HideMonoScript]
    public class LogicElement : MonoBehaviour
    {
        /*
        public bool listenOnly = false;
        [ShowIf("listenOnly")]
        public bool requireAllLogicTracks = false;

        [HideInInspector]
        public List<LogicReactor> reactors = new List<LogicReactor>();

        [ListDrawerSettings(Expanded = true)]
        public List<LogicGroupReference> logicTracks = new List<LogicGroupReference>();


        [ListDrawerSettings(Expanded = true)]
        public List<LogicElementConditionCase> cases = new List<LogicElementConditionCase>();

        private bool elementLogicMet = false;


        private void Start()
        {
            elementLogicMet = false;
            CheckStatus();
        }

        private void OnEnable()
        {
#if UNITY_EDITOR
            if (Application.isPlaying == false) return;
#endif
            foreach (LogicGroupReference nextRef in logicTracks)
            {
                if (nextRef.group)
                {
                    nextRef.group.IsOverriden.onValue += OverrideChanged;
                    nextRef.group.RegisterEnabler(this, elementLogicMet, listenOnly);
                }


            }

            foreach (LogicElementConditionCase next in cases)
            {
                next.condition.AddListener(AnyConditionChanged);
            }

            Debug.Log("1");
            CheckStatus();

        }

        private void OnDisable()
        {
#if UNITY_EDITOR
            if (Application.isPlaying == false) return;
#endif

            foreach (LogicGroupReference nextRef in logicTracks)
            {
                if (nextRef.group)
                {
                    nextRef.group.IsOverriden.onValue -= OverrideChanged;
                    nextRef.group.UnregisterEnabler(this);//, elementLogicMet, listenOnly);
                }
            }

            foreach (LogicElementConditionCase next in cases)
            {
                next.condition.RemoveListener(AnyConditionChanged);
            }

            RemoveFromLogicTracks();

            CheckStatus();
        }


        private void AnyConditionChanged(bool val)
        {
            CheckStatus();
        }

        private void OverrideChanged(bool val)
        {
            CheckStatus();
        }

        public void CheckStatus()
        {
            Debug.Log("2");
            RefreshConditionCaseMet();
            Debug.Log("5 " + elementLogicMet);
            elementLogicMet = true;

            if (listenOnly == false)
            {
                foreach (LogicGroupReference trackRef in logicTracks)
                {
                    if (trackRef.TrackOverriden())
                    {
                        elementLogicMet = false;
                        break;
                    }
                }
            }
            Debug.Log("6 " + elementLogicMet);
            if (hasAnyCases == true && currentMetCase <= -1)
            {
                elementLogicMet = false;
            }
            Debug.Log("7 " + elementLogicMet);
            if (gameObject.activeInHierarchy == false || gameObject.activeSelf == false || enabled == false)
            {
                elementLogicMet = false;
            }
            Debug.Log("8 " + elementLogicMet);
            UpdateReactors();
        }



        private bool hasAnyCases = false;
        [ShowIf("hasAnyCases")]
        [SerializeField, ReadOnly] private int currentMetCase = -1;
        private void RefreshConditionCaseMet()
        {
            Debug.Log("3");
            currentMetCase = -1;

            if (cases.Count == 0)
            {
                hasAnyCases = false;
            }
            else
            {
                hasAnyCases = true;

                int counter = 0;
                foreach (LogicElementConditionCase nextCase in cases)
                {
                    if (nextCase.condition.conditionMet)
                    {
                        currentMetCase = counter;
                        break;
                    }
                    counter++;
                }
            }

            if (hasAnyCases)
            {
                if (currentMetCase <= -1)
                {
                    RemoveFromLogicTracks();
                }
                else
                {
                    AddToLogicTracks();
                }
            }
            else
            {
                AddToLogicTracks();
                Debug.Log("Here");
            }

            if (listenOnly == true)
            {
                bool logicTracksAllTrue = true;
                bool anyLogicTrackTrue = false;
                foreach (LogicGroupReference groupRef in logicTracks)
                {
                    if (groupRef.group.IsEnabled.Value && groupRef.TrackOverriden() == false)
                    {
                        anyLogicTrackTrue = true;
                    }
                    else
                    {
                        logicTracksAllTrue = false;
                    }
                }

                if (requireAllLogicTracks == true && logicTracksAllTrue == false)
                {
                    currentMetCase = -1;
                    Debug.Log("Not all logic tracks are enabled");
                }
                else if (anyLogicTrackTrue == true)
                {
                    Debug.Log("At least one logic track enabled");
                }
                else
                {
                    currentMetCase = -1;
                    Debug.Log("No logic tracks enabled");
                }
            }
        }

        private void UpdateReactors()
        {
            if (elementLogicMet == false)
            {
                foreach (LogicReactor reactor in reactors)
                {
                    reactor.ConditionChangedOnLogicElement(-1);
                }
            }
            else
            {
                foreach (LogicReactor reactor in reactors)
                {
                    reactor.ConditionChangedOnLogicElement(currentMetCase);
                }
            }
        }


        private bool onLogicTracks = false;

        private void AddToLogicTracks()
        {
            if (listenOnly == true) return;

            if (onLogicTracks == false)
            {
                onLogicTracks = true;
                foreach (LogicGroupReference groupRef in logicTracks)
                {
                    if (groupRef.group != null)
                    {
                        groupRef.group.EnableFromRegisteredEnabler(this);
                    }
                }
            }
        }

        private void RemoveFromLogicTracks()
        {
            if (onLogicTracks == true)
            {
                onLogicTracks = false;
                foreach (LogicGroupReference groupRef in logicTracks)
                {
                    if (groupRef.group != null)
                    {
                        groupRef.group.DisableFromRegisteredEnabler(this);
                    }
                }
            }
        }

       
        */

#if UNITY_EDITOR
        [HorizontalGroup("Top")]
        private string debugState = "";

#endif

        [Title("$debugState", titleAlignment: TitleAlignments.Left, horizontalLine: true, bold: true)]
        [PropertySpace(5)]
        [HorizontalGroup("Top")]
        [DisableInPlayMode]
        [SerializeField] private bool listenerOnly;

        [GUIColor("GUIColour")]
        [DisableInPlayMode]
        [ListDrawerSettings(ShowFoldout = false)]
        public LogicGroupReference[] logicGroups;

        [ListDrawerSettings(ShowFoldout = false)]//,ShowIndexLabels = true)]
        public List<LogicElementConditionCase> cases = new List<LogicElementConditionCase>();

        

       // [HideIf("listenerOnly")]
       // [GUIColor("GUIColour")]
       // [DisableInPlayMode]
       // public bool copyMonobehaviourEnabled;



        private Color GUIColour()
        {
            if (listenerOnly)
            {
                if (IsOverriden.Value)
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
            else
            {
                if (IsEnabled.Value)
                {
                    if (IsOverriden.Value)
                    {
                        if (enabled == false || gameObject.activeInHierarchy == false)
                        {
#if UNITY_EDITOR
                            debugState = "--OVERRIDEN && LOGIC OBJECT DISABLED--";

#endif
                            return new Color(0.8f, 0.8f, 1, 1); //Purple
                        }
                        else
                        {
#if UNITY_EDITOR
                            debugState = "--OVERRIDEN--";
#endif
                            return new Color(1, 0.6f, 0.4f, 1); //Orange
                        }
                    }
                    else
                    {

                        if (enabled == false || gameObject.activeInHierarchy == false)
                        {
#if UNITY_EDITOR
                            debugState = "--ENABLER ENABLED && LOGIC OBJECT DISABLED--";

#endif
                            return new Color(0.8f, 0.8f, 1, 1); //Purple
                        }
                        else
                        {
#if UNITY_EDITOR
                            debugState = "--ENABLER ENABLED--";

#endif
                            return new Color(0.8f, 1, 0.8f, 1); //Green
                        }
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

        [HideInInspector,SerializeField] private BoolWithCallback IsOverriden;
        [HideInInspector,SerializeField] private BoolWithCallback IsEnabled;

       [HideInInspector] public CodeEvent<int> logicCaseChanged;

        [ReadOnly] public int logicCase;
       // public BoolWithCallback IsLogicEnabledAndActive;
        // public bool IsOverriden { get; private set; }

        //public bool IsEnabled { get; private set; }

        //  public CodeEvent<bool> OnOverriden;

        private void OnEnable()
        {
            foreach (LogicGroupReference group in logicGroups)
            {
                group.group.RegisterEnabler(this, IsEnabled.Value, listenerOnly);

                group.group.IsOverriden.AddListener(RefreshOverridenState);
            }
            RefreshOverridenState(false);

            foreach (LogicElementConditionCase next in cases)
            {
                next.condition.AddListener(RefreshEnabledState,true);
            }

            RefreshEnabledState(false);

           // Refresh();
        }

        private void OnDisable()
        {

         //   if (copyMonobehaviourEnabled) DisableLogic();

            foreach (LogicGroupReference group in logicGroups)
            {
                group.group.UnregisterEnabler(this);

                group.group.IsOverriden.RemoveListener(RefreshOverridenState);
            }

            foreach (LogicElementConditionCase next in cases)
            {
                next.condition.RemoveListener(RefreshEnabledState);
            }

            Refresh();
        }

        public void RefreshEnabledState(bool __)
        {          
           

           // if (refreshMainStateAfter)
           // {
                Refresh();
           // }
        }
              

        public void RefreshOverridenState(bool __)
        {
          

           // if (refreshMainStateAfter)
           // {
                Refresh();
           // }
        }

       

        public void Refresh()
        {
            bool shouldOverride = false;

            foreach (LogicGroupReference group in logicGroups)
            {
                if (group.group.IsOverriden.Value)
                {
                    shouldOverride = true;
                    break;
                }
            }

            IsOverriden.Value = shouldOverride;

            if (IsOverriden == false)
            {

                bool casesReturnEnabled = false;

                int counter = -1;

                //Debug.Log(counter + " " + name);

                foreach (LogicElementConditionCase logicCase in cases)
                {
                    counter++;
                    if (logicCase.condition.conditionMet)
                    {
                        casesReturnEnabled = true;
                        break;
                    }
                }



                if (casesReturnEnabled == true)
                {
                    IsEnabled.Value = true;
                    logicCase = counter;
                }
                else
                {
                    IsEnabled.Value = false;
                    logicCase = -1;
                }

            }

            if (IsOverriden.Value || IsEnabled.Value == false)
            {
                logicCase = -1;
                logicCaseChanged.Raise(-1);

                foreach (LogicGroupReference l in logicGroups)
                {
                    l.group.DisableFromRegisteredEnabler(this);
                }
            }
            else if (enabled == false || gameObject.activeInHierarchy == false)
            {
                logicCase = -1;
                logicCaseChanged.Raise(-1);

                foreach (LogicGroupReference l in logicGroups)
                {
                    l.group.DisableFromRegisteredEnabler(this);
                }
            }
            else
            {
                foreach(LogicGroupReference l in logicGroups)
                {
                    l.group.EnableFromRegisteredEnabler(this);
                }

                logicCaseChanged.Raise(logicCase);
            }
        }

        /*
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
        */

        //[GUIColor("GUIColour")]
        //   [Button, ShowIf("ShowEnableLogicButton"), DisableInEditorMode]


       
        /*
        private void EnableLogic(int caseNumber)
        {
            if (!listenerOnly)
            {
                foreach (LogicGroupReference next in logicGroups)
                {
                    next.group.EnableFromRegisteredEnabler(this);
                }

                IsEnabled.Value = true;
            }

            RefreshIfOverriden(false);

        }

        //[GUIColor("GUIColour")]
        //[Button, ShowIf("ShowDisableLogicButton"), DisableInEditorMode]
        private void DisableLogic()
        {
            if (!listenerOnly)
            {
                foreach (LogicGroupReference next in logicGroups)
                {
                    next.group.DisableFromRegisteredEnabler(this);
                }

                IsEnabled.Value = false;
            }
            RefreshIfOverriden(false);

        }  
        */
    }

    [System.Serializable]
    [InlineProperty]
    public class LogicElementConditionCase
    {
        [HideLabel]
        public string conditionEvent;

        [HideLabel]
        public GameCondition condition;
    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

using System;
using System.IO;

#if UNITY_EDITOR
using UnityEditor;
using Sirenix.OdinInspector.Editor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
#endif

using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using System.Linq;
using Sirenix.Utilities;

namespace Rufas
{
    [ExecuteInEditMode]
    public class GameSystemManager : BootstrapBehaviour
    {
        public static GameSystemManager instance;

        [HideInEditorMode]
        public BoolWithCallback allSystemsInitialised;

        public static CodeEvent OnAllGameSystemsInitialized;

        [HideInEditorMode]
        public List<GameSystemParentClass> systemsInitializing = new List<GameSystemParentClass>();

        [HideInInspector]
        public GameSystemParentClass[] gameSystems;

        #region GameSystemsList
#if UNITY_EDITOR
        [TitleGroup("Game Systems")]
        [ListDrawerSettings(ShowFoldout = true, HideAddButton = true, HideRemoveButton = true)]        
        public List<GameSystemManagerToggle> ShowIndividualGameSystems = new List<GameSystemManagerToggle>();


        [TitleGroup("Game Systems")]
        [HideInPlayMode]
        [ListDrawerSettings(HideAddButton = true, HideRemoveButton = true)]
        public List<GameManagerSystemAddOptions> additionalGameSystems = new List<GameManagerSystemAddOptions>();
#endif
        #endregion

        #region GameSystemGroups
        [TitleGroup("Game System Groups")]

        [ListDrawerSettings(ShowFoldout = false, HideAddButton = true, HideRemoveButton = true)]
        public List<GameSystemGroup> GameSystemGroups = new List<GameSystemGroup>();

        [TitleGroup("Game System Groups")]
        [Button(name: "Create Group (Game System)")]
        private void CreateGroup(string groupName, int groupSize, SdfIconType icon)
        {
#if UNITY_EDITOR
            GameSystemGroup objectToCreate = (GameSystemGroup)ScriptableObject.CreateInstance("GameSystemGroup");
            Debug.Log("Create instance: " + groupName);
            Directory.CreateDirectory("Assets/Rufas/Systems/Groups");

            if (string.IsNullOrWhiteSpace(groupName))
            {
                groupName = "New Game System Group";
            }

            string path = "Assets/Rufas/Systems/Groups/" + groupName + ".asset";
            AssetDatabase.CreateAsset(objectToCreate, path);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            objectToCreate.gameSystems.SetLength(groupSize);
            objectToCreate.iconType = icon;
            Selection.activeObject = objectToCreate;

            GameSystemGroups.Add(objectToCreate);
            RefreshUI();
#endif
        }
#if UNITY_EDITOR
        [TitleGroup("Game System Groups")]
        [PropertyOrder(11)]
        [Button]
        private void SetGroupIcon(GameSystemGroup group, SdfIconType icon)
        {
            if (group) { group.iconType = icon; OdinEditorWindow.GetWindow<GameSystemManagerEditor>().ForceMenuTreeRebuild(); }
        }
#endif
        #endregion

        #region DefineSymbols
#if UNITY_EDITOR
        [TitleGroup("Define Symbols")]
        [PropertySpace(SpaceBefore = 15), PropertyOrder(10)]
        [InfoBox("Not all Rufas scriping define symbols are being used for the current build target. To include in the project, copy exact text into PlayerSettings -> Other -> ScriptingDefineSymbols", SdfIconType.ExclamationCircleFill, VisibleIf = "NotUsingAllDefineSymbols")]
        [ListDrawerSettings(HideAddButton = true, HideRemoveButton = true)]
        public List<RufasScriptingDefineSymbolsManager.DefineSymbolCheck> rufasDefineSymbols = RufasScriptingDefineSymbolsManager.AllDefineSymbols();
#endif
        #endregion

        public override void BehaviourToRunDuringBootstrap()
        {
         //   Debug.Log("GAME_SYSTEM_MANAGER: 1");
            base.BehaviourToRunDuringBootstrap();

            Debug.Log("GAME SYSTEM MANAGER SETTING INSTANCE: " + this.GetInstanceID());

            if (instance == null)
            {
                instance = this;
            }else if (instance != this)
            {
                Debug.LogError("Multiple GameSystemManagers found!");                
            }

#if UNITY_EDITOR
            //gameSystems = RufasStatic.GetAllScriptables_ToArray<GameSystemParentClass>();
#endif

           // Debug.Log("GAME_SYSTEM_MANAGER: 2");

            allSystemsInitialised.Value = false;

            foreach (GameSystemParentClass next in gameSystems)
            {
              //  Debug.Log("From Game Manager: " + next.name);
                systemsInitializing.Add(next);
                next.TriggerInstance();
            }

            //Loop once for rufas systems (they have priority)
            foreach (GameSystemParentClass next in gameSystems)
            {
                if (next.IsRufasSystem())
                {
                    next.PreInitialisationBehaviour();
                }
            }

            //Loop again for non rufas systems (just to make sure nothing comes before any rufas managers)
            foreach(GameSystemParentClass next in gameSystems)
            {
                if (next.IsRufasSystem() == false)
                {
                    next.PreInitialisationBehaviour();
                }
            }

            foreach(GameSystemParentClass next in gameSystems)
            {
                next.FinaliseInitialisation();               
            }

            CoroutineMonoBehaviour.StartCoroutine(WaitForAllInitialisationToFinish(), null);
        }


        private IEnumerator WaitForAllInitialisationToFinish()
        {
            while(systemsInitializing.Count > 0)
            {
                yield return null;
            }

            allSystemsInitialised.Value = true;
            OnAllGameSystemsInitialized.Raise();

            foreach (GameSystemParentClass next in gameSystems)
            {
                next.PostInitialisationBehaviour();
            }

            yield return null;

            GameObject gameSystemManagerLink = new GameObject("GameSystemManagerLink");
            gameSystemManagerLink.AddComponent<GameSystemManagerMonoLink>();
            DontDestroyOnLoad(gameSystemManagerLink);
        }


        public void TriggerOnAwakeBehaviour()
        {
            foreach (GameSystemParentClass next in gameSystems)
            {
                next.OnAwakeBehaviour();
            }
        }

        public void TriggerOnStartBehaviour()
        {
            foreach (GameSystemParentClass next in gameSystems)
            {
                next.OnStartBehaviour();
            }
        }

        public void TriggerOnUpdateBehaviour()
        {
            foreach(GameSystemParentClass next in gameSystems)
            {                
                next.OnUpdateBehaviour();
            }
        }

        public void TriggerEndOfApplicationBehaviour()
        {
            foreach (GameSystemParentClass next in gameSystems)
            {
                next.EndOfApplicaitonBehaviour();
            }
        }

        private void OnEnable()
        {
            if (Application.isPlaying)
            {
                //OnPlaymodeInit
            }
            else
            {
#if UNITY_EDITOR
                //OnEditorInit
                foreach(GameSystemParentClass next in gameSystems)
                {
                    next.OnEnable_EditorModeOnly();
                }

                ResetSymbolList();
#endif
            }
        }



#if UNITY_EDITOR
        private void RefreshWindowOnHiddenSystemsChange()
        {
            RefreshGameSystems();
            OdinEditorWindow.GetWindow<GameSystemManagerEditor>().ForceMenuTreeRebuild();
        }

        //[PropertyOrder(5)]
        public void RefreshGameSystems()
        {          
            gameSystems = RufasStatic.GetAllScriptables_ToArray<GameSystemParentClass>();

            //GameSys

            //  visibleGameSystems.Clear();
            ShowIndividualGameSystems.Clear();
            foreach(GameSystemParentClass parent in gameSystems)
            {
                ShowIndividualGameSystems.Add(new(this, parent));
            }

            if (Application.isPlaying == true) return;

            foreach (GameSystemParentClass next in gameSystems)
            {
                next.OnEnable_EditorModeOnly();
            }

            additionalGameSystems.Clear();
            var gameSystemsList = new List<GameSystemParentClass>();

            foreach (Type t in RufasStatic.FindDerivedTypes(typeof(GameSystemParentClass)))
            {
                if(t == typeof(GameSystem<>))
                {
                    continue;
                }

                GameSystemParentClass instance = (GameSystemParentClass)ScriptableObject.CreateInstance(t.Name);// (GameSystemParentClass)Activator.CreateInstance(t);
                gameSystemsList.Add(instance);
            }


            foreach(GameSystemParentClass next in gameSystemsList)
            {
                if(string.Compare(next.GetType().ToString(),"Rufas.GameSystem") == 0 || string.Compare(next.GetType().ToString(), "Rufas.GameSystemParentClass") == 0)
                {

                }
                else
                {
                    bool systemInstanceAlreadyExists = false;

                    foreach(GameSystemParentClass nextExistingSystem in gameSystems)
                    {
                        if(next.GetType() == nextExistingSystem.GetType())
                        {
                            systemInstanceAlreadyExists = true;
                            break;
                        }
                    }

                    if (systemInstanceAlreadyExists)
                    {

                    }
                    else
                    {
                        additionalGameSystems.Add(new GameManagerSystemAddOptions(this, next, next.GetType().ToString()));                       
                    }
                }
            }
        }




        private void ResetSymbolList()
        {
            rufasDefineSymbols = RufasScriptingDefineSymbolsManager.AllDefineSymbols();
        }


        private bool NotUsingAllDefineSymbols()
        {
            foreach (RufasScriptingDefineSymbolsManager.DefineSymbolCheck next in rufasDefineSymbols)
            {
                if (!next.Check()) return true;
            }

            return false;
        }

        // [InfoBox("Drag systems here to hide them in this editor")]
        // [OnValueChanged("RefreshGameSystemEditor")]
        [HideInInspector]
        public List<GameSystemParentClass> hiddenGameSystems = new List<GameSystemParentClass>();

        private void RefreshGameSystemEditor()
        {
            OdinEditorWindow.GetWindow<GameSystemManagerEditor>().ForceMenuTreeRebuild();
        }

        /*
        private void Create(string systemName, string displayName)
        {


            GameSystemParentClass objectToCreate = (GameSystemParentClass)ScriptableObject.CreateInstance(systemName);
            Debug.Log("Create instance: " + systemName);
            Directory.CreateDirectory("Assets/Rufas/Systems");

            if (string.IsNullOrWhiteSpace(displayName))
            {
                string[] nameSplit = systemName.Split(".");
                displayName = nameSplit[nameSplit.Length - 1];
            }

            string path = "Assets/Rufas/Systems/" + displayName + ".asset";
            AssetDatabase.CreateAsset(objectToCreate, path);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            RefreshGameSystems();

            objectToCreate.OnCreatedByEditor();
        }
        */

        [PropertyOrder(100)]
        [Button]
        public void RefreshUI()
        {
            OdinEditorWindow.GetWindow<GameSystemManagerEditor>().ForceMenuTreeRebuild();
        }


        [Serializable]
        public class GameManagerSystemAddOptions
        {
            public GameManagerSystemAddOptions(GameSystemManager _manager, GameSystemParentClass _systemToCreate, string _systemName)
            {
                manager = _manager;
                systemToCreate = _systemToCreate;
                systemName = _systemName;
                if (!string.IsNullOrEmpty(systemToCreate.DesiredName()))
                {
                    displayName = systemToCreate.DesiredName();
                }
                else
                {
                    displayName = systemToCreate.name;
                }
            }

            private GameSystemManager manager;

            [HideInInspector]
            public GameSystemParentClass systemToCreate;

            [HorizontalGroup("H")]
            [Button("Create")]
            private void CreateInstance()
            {
                systemToCreate.CreateGameSystem(systemName,displayName,manager);//
               // manager.Create(systemName, displayName);
            }

            private string systemName;

            [HorizontalGroup("H")]
            [HideLabel,ReadOnly]
            public string displayName;
        }

        
        [Serializable]
        public class GameSystemManagerToggle
        {
            public GameSystemManagerToggle(GameSystemManager _manager, GameSystemParentClass _gameSystem)//, bool _showGameSystem)
            {
                manager = _manager;
                gameSystem = _gameSystem;
               // showGameSystem = _showGameSystem;
            }

            private GameSystemManager manager;



            //  [HideInInspector]
            // public bool showGameSystem = true;

            [PropertyOrder(0)]
            [HorizontalGroup("H")]
            [Button(Name = "$Name", Style = ButtonStyle.Box, ButtonAlignment = 0, Stretch = true), GUIColor("$ButtonColour")]
            public void Toggle()
            {
                // showGameSystem = !showGameSystem;

                if (manager.hiddenGameSystems.Contains(gameSystem))
                {
                    manager.hiddenGameSystems.Remove(gameSystem);
                }
                else
                {
                    manager.hiddenGameSystems.Add(gameSystem);
                }

                UpdateGameSystem();
            }

            [PropertyOrder(5)]            
            [HideLabel, HorizontalGroup("H",width: 0.3f)]
            public GameSystemParentClass gameSystem;

            //private bool ShowG

            private Color ButtonColour()
            {
                if(manager == null || gameSystem == null)
                {
                    return Color.white;
                }

                if (!manager.hiddenGameSystems.Contains(gameSystem))
                {
                    return new Color(0.7f, 0.7f, 1, 1);
                }
                else
                {
                    return new Color(1, 0.7f, 0.7f, 1);
                }
            }            

            private string Name()
            {
                if (gameSystem)
                {
                    return gameSystem.DesiredName();
                }
                else
                {
                    return "GameSystem field null!";
                }
            }           


            private void UpdateGameSystem()
            {
                /*
                if (gameSystem != null)
                {
                    if (showGameSystem)
                    {
                        if (manager.hiddenGameSystems.Contains(gameSystem)) manager.hiddenGameSystems.Remove(gameSystem);
                    }
                    else
                    {

                    }
                    //_manager.
                    gameSystem.showInManager = showGameSystem;
                }
                */

                OdinEditorWindow.GetWindow<GameSystemManagerEditor>().ForceMenuTreeRebuild();
            }      
        }
        
#endif
    }
}



using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

using System;
using System.IO;

#if UNITY_EDITOR
using UnityEditor;
using Sirenix.OdinInspector.Editor;
#endif

using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using System.Linq;

namespace Rufas
{
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

        public override void BehaviourToRunBeforeStart()
        {
            base.BehaviourToRunBeforeStart();

            if (instance == null)
            {
                instance = this;
            }else if (instance != this)
            {
                Debug.LogError("Multiple GameSystemManagers found!");                
            }

#if UNITY_EDITOR
            gameSystems = RufasStatic.GetAllScriptables_ToArray<GameSystemParentClass>();
#endif

            allSystemsInitialised.Value = false;

            foreach (GameSystemParentClass next in gameSystems)
            {
                systemsInitializing.Add(next);
                next.TriggerInstance();
            }

            //Loop once for rufas systems (they have priority)
            foreach(GameSystemParentClass next in gameSystems)
            {
                if (next.IsRufasSystem())
                {
                    next.BehaviourToRunBeforeAwake();
                }
            }

            //Loop again for non rufas systems (just to make sure nothing comes before any rufas managers)
            foreach(GameSystemParentClass next in gameSystems)
            {
                if (next.IsRufasSystem() == false)
                {
                    next.BehaviourToRunBeforeAwake();
                }
            }

            foreach(GameSystemParentClass next in gameSystems)
            {
                next.FinaliseInitialisation();               
            }

            CoroutineMonoBehaviour.StartCoroutine(WaitForAllInitialisationToFinish(), null);
        }

        public void TriggerEndOfApplicationBehaviour()
        {
            foreach (GameSystemParentClass next in gameSystems)
            {
                next.EndOfApplicaitonBehaviour();
            }
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

            GameObject gameSystemManagerLink = new GameObject("GameSystemManagerLink");
            gameSystemManagerLink.AddComponent<GameSystemManagerMonoLink>();
        }

        private void OnEnable()
        {
            if (Application.isPlaying)
            {
                //OnPlaymodeInit
            }
            else
            {
                //OnEditorInit
                foreach(GameSystemParentClass next in gameSystems)
                {
                    next.OnEnable_EditorModeOnly();
                }
            }
        }

#if UNITY_EDITOR

        //[ShowIf()]
        // [HorizontalGroup("Lists")]
        // [VerticalGroup("Lists/Left")]
        // [TitleGroup("Lists/Left/Rufas Systems")]
        //[TitleGroup("Left/Game Systems")]
        [DisableInPlayMode]
        [FoldoutGroup("Rufas Core Systems")]
        [ListDrawerSettings(Expanded = true, HideAddButton = true, HideRemoveButton = true)]
        public List<GameSystemManagerToggle> rufasSystemToggles = new List<GameSystemManagerToggle>();

        [DisableInPlayMode]
        [HorizontalGroup("Lists")]
        [VerticalGroup("Lists/Left")]
        [TitleGroup("Lists/Left/Game Systems")]
        //[TitleGroup("Left/Game Systems")]
        [ListDrawerSettings(Expanded = true, HideAddButton = true, HideRemoveButton = true)]        
        public List<GameSystemManagerToggle> gameSystemToggles = new List<GameSystemManagerToggle>();


        [HideInPlayMode]
        [TitleGroup("Lists/Left/Game Systems")]
        //[TitleGroup("Left/Game Systems")]
        [ListDrawerSettings(HideAddButton = true, HideRemoveButton = true)]
        public List<GameManagerSystemAddOptions> additionalGameSystems = new List<GameManagerSystemAddOptions>();
                    

        private void RefreshWindowOnHiddenSystemsChange()
        {
            RefreshGameSystems();
            OdinEditorWindow.GetWindow<GameSystemManagerEditor>().ForceMenuTreeRebuild();
        }

        [Button]
        public void RefreshGameSystems()
        {
            additionalGameSystems.Clear();
            var gameSystemsList = new List<GameSystemParentClass>();

            foreach (Type t in RufasStatic.FindDerivedTypes(typeof(GameSystemParentClass)))
            {
                if(t == typeof(GameSystem<>))
                {
                    continue;
                }

              //  string scriptNamespace = t.Namespace;               

                GameSystemParentClass instance = (GameSystemParentClass)Activator.CreateInstance(t);
           
                gameSystemsList.Add(instance);
            }

            gameSystems = RufasStatic.GetAllScriptables_ToArray<GameSystemParentClass>();

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
                        if (next.AutogenerateGameSystem())
                        {
                            string desiredName = next.DesiredName();

                            if (string.IsNullOrWhiteSpace(desiredName)) desiredName = next.name;

                            Create(next.GetType().ToString(), desiredName);
                        }
                        else
                        {
                            additionalGameSystems.Add(new GameManagerSystemAddOptions(this, next, next.GetType().ToString()));
                        }
                    }
                }
            }

            rufasSystemToggles.Clear();
            gameSystemToggles.Clear();// = new GameSystemManagerToggle[standardToggleSize];//gameSystems.Length];
          //  rufasBackendSystems.Clear();

            foreach (GameSystemParentClass next in gameSystems)
            {

                if (next.IsRufasSystem() && next.AutogenerateGameSystem())
                {
                    rufasSystemToggles.Add(new GameSystemManagerToggle(this, next, next.showInManager));
                }
                else
                {
                    gameSystemToggles.Add(new GameSystemManagerToggle(this, next, next.showInManager));
                }
            }
        }


        private void Create(string systemName, string displayName)
        {
            GameSystemParentClass objectToCreate = (GameSystemParentClass)ScriptableObject.CreateInstance(systemName);
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
                manager.Create(systemName, displayName);
            }

            private string systemName;

            [HorizontalGroup("H")]
            [HideLabel,ReadOnly]
            public string displayName;
        }

        [Serializable]
        public class GameSystemManagerToggle
        {
            public GameSystemManagerToggle(GameSystemManager _manager, GameSystemParentClass _gameSystem, bool _showGameSystem)
            {
                manager = _manager;
                gameSystem = _gameSystem;
                showGameSystem = _showGameSystem;
            }


            private GameSystemManager manager;

            [HideLabel,HorizontalGroup("H")]
            public GameSystemParentClass gameSystem;

            [HideInInspector]
            public bool showGameSystem;

            [HorizontalGroup("H")]
            [Button(Name = "$Name",Style = ButtonStyle.Box,ButtonAlignment =0,Stretch = false),GUIColor("$ButtonColour")]
            public void Toggle()
            {
                showGameSystem = !showGameSystem;

                UpdateGameSystem();
            }


            private Color ButtonColour()
            {
                if (showGameSystem)
                {
                    return new Color(0.5f, 0.5f, 0.9f, 1);
                }
                else
                {
                    return new Color(0.9f, 0.5f, 0.5f, 1);
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
                if (gameSystem != null)
                {
                    gameSystem.showInManager = showGameSystem;
                }

                OdinEditorWindow.GetWindow<GameSystemManagerEditor>().ForceMenuTreeRebuild();
            }


        }
#endif
    }
}


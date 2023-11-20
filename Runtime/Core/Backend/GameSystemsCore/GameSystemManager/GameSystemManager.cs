
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

namespace Rufas
{

    public static class TriggerGameSystemManagerBeforeStart
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void FindAndTriggerGameSystemManager()
        {
            GameSystemManager[] loadableGameSystemManagers = Resources.LoadAll<GameSystemManager>("");

            if (loadableGameSystemManagers.Length == 0)
            {
                Debug.LogError("No game system managers found!");
            }
            else if (loadableGameSystemManagers.Length == 1)
            {
                loadableGameSystemManagers[0].BehaviourToRunBeforeStart();
            }
            else
            {
                Debug.LogError("Too many game system managers found!");
            }
        }
    }

    public class GameSystemManager : ScriptableObject
    {
        public static GameSystemManager instance;

        [HideInInspector]
        public GameSystemParentClass[] gameSystems;

        public void BehaviourToRunBeforeStart()
        {
#if UNITY_EDITOR
            gameSystems = RufasStatic.GetAllScriptables_ToArray<GameSystemParentClass>();
#endif
            //Debug.Log("Here!");
            foreach(GameSystemParentClass next in gameSystems)
            {
                next.BehaviourToRunBeforeAwake();
            }
        }

#if UNITY_EDITOR
        [HorizontalGroup("Lists")]
        [VerticalGroup("Lists/Left")]
        [TitleGroup("Lists/Left/Game Systems")]
        //[TitleGroup("Left/Game Systems")]
        [ListDrawerSettings(Expanded = true, HideAddButton = true, HideRemoveButton = true)]        
        public List<GameSystemManagerToggle> gameSystemToggles = new List<GameSystemManagerToggle>();

        [TitleGroup("Lists/Left/Game Systems")]
        //[TitleGroup("Left/Game Systems")]
        [ListDrawerSettings(HideAddButton = true, HideRemoveButton = true)]
        public List<GameManagerSystemAddOptions> additionalGameSystems = new List<GameManagerSystemAddOptions>();

        /*
        [VerticalGroup("Lists/Right")]
        [TitleGroup("Lists/Right/Rufas Systems")]
        //[TitleGroup("Right/Rufas Backend Systems")]
        [OnValueChanged("RefreshWindowOnHiddenSystemsChange")]
        public bool showRufasHiddenSystems;

        [TitleGroup("Lists/Right/Rufas Systems")]
        // [TitleGroup("Right/Rufas Backend Systems")]
        [ShowIf("showRufasHiddenSystems"), ListDrawerSettings(Expanded = true, HideAddButton = true, HideRemoveButton = true)]
        public List<GameSystemManagerToggle> rufasBackendSystems = new List<GameSystemManagerToggle>();
        */        

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

                GameSystemParentClass instance = (GameSystemParentClass)Activator.CreateInstance(t);
           //     if (showRufasHiddenSystems == false && instance.RufasBackendSystem() == true)
            //    {
            //        continue;
            //    }
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


            gameSystemToggles.Clear();// = new GameSystemManagerToggle[standardToggleSize];//gameSystems.Length];
          //  rufasBackendSystems.Clear();

            foreach (GameSystemParentClass next in gameSystems)
            {
           //     if (next.RufasBackendSystem())
           //     {
           //         rufasBackendSystems.Add(new GameSystemManagerToggle(this, next, next.showInManager));
           //     }
           //     else
           //     {
                    gameSystemToggles.Add(new GameSystemManagerToggle(this, next, next.showInManager));
           //     }
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


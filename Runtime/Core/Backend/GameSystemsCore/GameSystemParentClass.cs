using Sirenix.OdinInspector;

#if UNITY_EDITOR
using Sirenix.Utilities.Editor;
using System.Reflection;
using UnityEditor;

using UnityEngine.Windows;
#endif

using UnityEngine;

namespace Rufas
{
    public class GameSystemParentClass : SerializedScriptableObject
    {
#if UNITY_EDITOR

        [HideInInlineEditors]
        [SerializeField,HideLabel] private GameSystemParentClass selfRef;

        [PropertySpace(5)]
        [ShowInInlineEditors]
        [GUIColor("#7994a0")]
        [PropertyOrder(-1)]
        [ReadOnly,Button(ButtonHeight = 10,Style = ButtonStyle.Box,ButtonAlignment = 0.5f,Stretch = true,Name ="$TitleText")]
        private void Title()
        {
//#if UNITY_EDITOR
            Selection.activeObject = this;
//#endif
        }
        
        private string TitleText()
        {
            return "<b><size=14><color=white>" + DesiredName() + "</color></size></b>";
        }

        //public static SdfIconType editorI;
#endif
        public virtual SdfIconType EditorIcon()
        {            
            return SdfIconType.CircleFill;
        }
#if UNITY_EDITOR
        public string GetNamespace()
        {
            return this.GetType().Namespace;
        }

        public void CreateGameSystem(string systemName, string displayName, GameSystemManager toRefresh)
        {
            GameSystemParentClass objectToCreate = (GameSystemParentClass)ScriptableObject.CreateInstance(systemName);
          //  Debug.Log("Create instance: " + systemName);
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

            toRefresh.RefreshGameSystems();

            objectToCreate.OnCreatedByEditor();
        }
#endif

        /// <summary>
        /// Internal use! This will hide the system in the manager editor (revealed with the 'showRufasHiddenSystems' bool on the manager)
        /// </summary>
        /// <returns></returns>
        public virtual bool IsRufasSystem()
        {
            return false;
        }
        
        public virtual void TriggerInstance()
        {
            
        }

        //public virtual bool IsRufasSystem()
        //{

       // }



        /// <summary>
        /// Autogenerates this scriptable manager without asking
        /// </summary>
        /// <returns></returns>
       // public virtual bool AutogenerateGameSystem()
        //{
        //    return false;
        //}

        /// <summary>
        /// This method is called on initialization during the first frame. Before any monobehaviour callbacks!
        /// </summary>
        public virtual void PreInitialisationBehaviour()
        {

        }     

        public virtual void PostInitialisationBehaviour()
        {

        }

        public virtual void OnAwakeBehaviour()
        {

        }

        public virtual void OnStartBehaviour()
        {

        }

        public virtual void OnUpdateBehaviour()
        {

        }

        public virtual void EndOfApplicaitonBehaviour()
        {
            finishedIniting = false;
        }

        public bool finishedIniting = false;

        public virtual void FinaliseInitialisation()
        {
            finishedIniting = true;
            //GameSystemManager.instance.systemsInitializing.Remove(this);
        }

        public virtual void OnEnable_EditorModeOnly()
        {
#if UNITY_EDITOR
            selfRef = this;
#endif
        }

        public virtual string DesiredName()
        {
            string[] names = DesiredPath().Split("/"); this.GetType().ToString().Split(".");
            return names[names.Length - 1];
        }

        public virtual string DesiredPath()
        {
            return "GameSystems/" + this.name;
        }

        public virtual void OnCreatedByEditor()
        {

        }
        
    }
}

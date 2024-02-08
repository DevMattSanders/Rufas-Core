using JetBrains.Annotations;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

namespace Rufas
{
    [CreateAssetMenu(menuName = "Rufas/Functionality/SaveLoad")]
    public class SaveLoad : SuperScriptable
    {

        //A list of lost references and their last known path and name along with their ID

        [ReadOnly]
        public readonly Dictionary<SuperScriptableWithID, string> ObjectToString =
          new Dictionary<SuperScriptableWithID, string>();

        [ReadOnly]
        public readonly Dictionary<string, SuperScriptableWithID> StringToObject =
            new Dictionary<string, SuperScriptableWithID>();

        [Button]
        private void FindAndRefreshAll()
        {
#if UNITY_EDITOR

            ObjectToString.Clear();
            StringToObject.Clear();

            SuperScriptableWithID[] scriptables = RufasStatic.GetAllScriptables_ToArray<SuperScriptableWithID>();

            for (int i = 0; i < scriptables.Length; i++)
            {
                scriptables[i].RefreshID();
            }

            List<SuperScriptableWithID> superScriptablesToRemove = new List<SuperScriptableWithID>();
            List<string> idsToRemove = new List<string>();

            foreach (var kvp in ObjectToString)
            {
                if (kvp.Key == null)
                {
                    superScriptablesToRemove.Add(kvp.Key);
                    idsToRemove.Add(kvp.Value);
                }
            }

            foreach (var key in superScriptablesToRemove)
            {
                ObjectToString.Remove(key);
            }

            foreach(var val in idsToRemove)
            {
                StringToObject.Remove(val);
            }
#endif
        }

        public string fileName;

        private static SaveLoad editorInstance;

        private static SaveLoad instance;
        
        public static SaveLoad Instance
        {
            get
            {
                if (Application.isPlaying == false)
                {
#if UNITY_EDITOR
                    if (editorInstance == null)
                    {
                        editorInstance = RufasStatic.GetAllScriptables_ToList<SaveLoad>()[0];
                    }

                    return editorInstance;
#else
                return instance;
#endif
                }
                else
                {

                    return instance;
                }
            }
        }

        public override void SoOnAwake()
        {
            base.SoOnAwake();

            if(instance != null)
            {
                Debug.LogError("Multiple SaveLoad Scriptable Objects found!");
                return;
            }

            instance = this;
        }


        //OUT IF SUCCESSFUL FOR EACH LOAD JOB! SHOULD LOAD PACKET THAT CONTAINS A VALUE AND BOOL FOR IF ACTUALLY FOUND

        //Bool
       

        public void TrySave<T>(string varID, T value)
        {
                ES3.Save<T>(varID, value, fileName);
        }

        public void TryLoad<T>(string varID, out T value, out bool successful)
        {
            if (ES3.KeyExists(varID, fileName))
            {
                successful = true;
                value = ES3.Load<T>(varID, fileName);
            }
            else
            {
                successful = false;
                value = default(T);
            }
        }
    }
}

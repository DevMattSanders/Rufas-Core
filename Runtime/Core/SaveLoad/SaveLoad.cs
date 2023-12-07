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
            // try
            //{
            //ES3.Save
                ES3.Save<T>(varID, value, fileName);
           // }
           // catch
            ////{
            //    Debug.Log("Cannot save: " + varID + " " + value+ ". Most likely due to it not being serializable in ES3");
            //}
        }

        public void TryLoad<T>(string varID, out T value, out bool successful)
        {
            // try
            // {
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
           // }
            //catch
           // {
             //   value = default(T);
              //  return false;

              //  Debug.Log("Cannot save: " + varID + " " + value + ". Most likely due to it not being serializable in ES3");

           // }
        }

        /*
        public void SaveBool(string varID, bool value) { ES3.Save<bool>(varID, value, fileName); }

        public void LoadBool(string varID, out bool successful, out bool value)
        {
            if (ES3.KeyExists(varID, fileName))
            {
                successful = true;
               value = ES3.Load<bool>(varID, fileName);
            }
            else
            {
                successful = false;
                value = false;
            }
        }

        
        //Color
        public void SaveColor(string varID, Color value) { ES3.Save<Color>(varID, value, fileName); }
        public Color LoadColor(string varID) { return ES3.Load<Color>(varID, fileName); }

        //Double
        public void SaveDouble(string varID, double value) { ES3.Save<double>(varID, value, fileName); }
        public double LoadDouble(string varID) { return ES3.Load<double>(varID, fileName); }

        //Float
        public void SaveFloat(string varID, float value) { ES3.Save<float>(varID, value, fileName); }
        public float LoadFloat(string varID) { return ES3.Load<float>(varID, fileName); }

        //Int
        public void SaveInt(string varID, int value) { ES3.Save<int>(varID, value, fileName); }
        public int LoadInt(string varID) { return ES3.Load<int>(varID, fileName); }

        //String
        public void SaveString(string varID, string value) { ES3.Save<string>(varID, value, fileName); }
        public string LoadString(string varID) { return ES3.Load<string>(varID, fileName); }

        //Vector2
        public void SaveVector2(string varID, Vector2 value) { ES3.Save<Vector2>(varID, value, fileName); }
        public Vector2 LoadVector2(string varID) { return ES3.Load<Vector2>(varID, fileName); }

        //Vector3
        public void SaveVector3(string varID, Vector3 value) { ES3.Save<Vector3>(varID, value, fileName); }
        public Vector3 LoadVector3(string varID) { return ES3.Load<Vector3>(varID, fileName); }
        */
    }
}

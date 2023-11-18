using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Rufas
{
    [CreateAssetMenu]
    public class ScriptableCallbacksManager : ScriptableObject
    {
        /*
        public List<scriptable> allCallbackObjectsInFile = new List<IScriptableObjectCallbacks>();

        [Button]
        public void FindAll()
        {
            allCallbackObjectsInFile = GetAllScriptablesImplementingInterface<IScriptableObjectCallbacks>();
        }

        public static List<T> GetAllScriptablesImplementingInterface<T>() where T : IScriptableObjectCallbacks
        {
            string[] guids = AssetDatabase.FindAssets($"t:{typeof(ScriptableObject).Name}");

            List<T> scriptableObjects = new List<T>();

            foreach (string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                Object[] assets = AssetDatabase.LoadAllAssetsAtPath(path);

                foreach (Object asset in assets)
                {
                    if (asset is T scriptableObject)
                    {
                        scriptableObjects.Add(scriptableObject);
                    }
                }
            }

            return scriptableObjects;
        }
        */
    }
}

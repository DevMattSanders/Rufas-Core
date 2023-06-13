using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace Rufas
{

    public static class GeneralMethods
    {
#if UNITY_EDITOR
        public static List<T> FindAllScriptableObjectsOfType<T>() where T : ScriptableObject
        {
            string[] guids = AssetDatabase.FindAssets("t:" + typeof(T).Name);

            List<T> scriptableObjects = new List<T>();

            for (int i = 0; i < guids.Length; i++)
            {
                string path = AssetDatabase.GUIDToAssetPath(guids[i]);

                T scriptableObject = AssetDatabase.LoadAssetAtPath<T>(path);

                if (scriptableObject != null)
                {
                    scriptableObjects.Add(scriptableObject);
                }
            }

            return scriptableObjects;
        }
#endif
    }
}

using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Rufas
{
    public class SuperScriptableWithID : SuperScriptable, ISerializationCallbackReceiver
    {
        [HideInInlineEditors, TitleGroup("Save Load Options", Order = 1)]//, HorizontalGroup("ID")]
        public bool allowSaveLoad = false;

       

        [SerializeField,HideInInspector]
        private string uniqueID;

        [ShowInInspector,ReadOnly,HideLabel, TitleGroup("Save Load Options", Order = 1)]//, ShowIf("allowSaveAndLoad")]//, HorizontalGroup("ID")]
        public string UniqueID
        {
            get
            {
                return uniqueID;
            }
        }

        
        [Button, EnableIf("allowSaveLoad"), TitleGroup("Save Load Options", Order = 1)]
        [HideInInlineEditors]
        public void RefreshID()
        {
            ProcessRegistration(this);
        }

        public override void SoOnStart()
        {
            base.SoOnStart();       
            ProcessRegistration(this);
        }

        private static void ProcessRegistration(SuperScriptableWithID obj)
        {
            if (SaveLoad.Instance.ObjectToString.TryGetValue(obj, out var existingId))
            {
               Debug.Log(existingId);
               Debug.Log(obj.UniqueID);
             
                if (obj.UniqueID != existingId)
                {
                    Debug.LogError($"Inconsistency: {obj.name} {obj.UniqueID} / {existingId}");
                    obj.uniqueID = existingId;

#if UNITY_EDITOR
                    EditorUtility.SetDirty(obj);
                    AssetDatabase.SaveAssets();
#endif
                }

                if (SaveLoad.Instance.StringToObject.ContainsKey(existingId))
                {
                    return;
                }

                Debug.Log("Inconsistent database tracking.");
                SaveLoad.Instance.StringToObject.Add(existingId, obj);

                return;
            }

            if (string.IsNullOrEmpty(obj.UniqueID))
            {
                GenerateInternalId(obj);

                RegisterObject(obj);
                return;
            }

            if (!SaveLoad.Instance.StringToObject.TryGetValue(obj.UniqueID, out var knownObject))
            {
                RegisterObject(obj);
                return;
            }

            if (knownObject == obj)
            {
                Debug.Log("Inconsistent database tracking.");
                SaveLoad.Instance.ObjectToString.Add(obj, obj.UniqueID);
                return;
            }

            if (knownObject == null)
            {
                Debug.Log("Unexpected registration problem.");
                RegisterObject(obj, true);
                return;
            }

            GenerateInternalId(obj);

            RegisterObject(obj);
        }

        private static void RegisterObject(SuperScriptableWithID aID, bool replace = false)
        {
            if (replace)
            {
                SaveLoad.Instance.StringToObject[aID.UniqueID] = aID;
            }
            else
            {
                SaveLoad.Instance.StringToObject.Add(aID.UniqueID, aID);
            }

            SaveLoad.Instance.ObjectToString.Add(aID, aID.UniqueID);
        }

        private static void GenerateInternalId(SuperScriptableWithID obj)
        {
            obj.uniqueID = Guid.NewGuid().ToString();

#if UNITY_EDITOR
            EditorUtility.SetDirty(obj);
            AssetDatabase.SaveAssets();
#endif
        }
    }
}

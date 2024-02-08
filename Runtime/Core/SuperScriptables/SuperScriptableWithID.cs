using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace Rufas
{
    public class SuperScriptableWithID : SuperScriptable, ISerializationCallbackReceiver
    {
       // [HideInInlineEditors, TitleGroup("Save Load Options", Order = 1)]//, HorizontalGroup("ID")]
     //   public bool allowSaveLoad = false;

       

        [SerializeField,HideInInspector]
        private string uniqueID;


        [TitleGroup("Save Load Options", Order = 1)]
        [ShowInInspector,ReadOnly,HideLabel, HorizontalGroup("Save Load Options/H"), HideInInlineEditors]//, ShowIf("allowSaveAndLoad")]//, HorizontalGroup("ID")]
        public string UniqueID
        {
            get
            {
                return uniqueID;
            }
        }


        
        [Button,HorizontalGroup("Save Load Options/H"),GUIColor("RefreshIDButtonColour"), HideInInlineEditors]
        public void RefreshID()
        {
            ProcessRegistration(this);
        }

        private Color RefreshIDButtonColour()
        {
            if (string.IsNullOrEmpty(uniqueID))
            {
                return Color.red;
            }
            else
            {
                return Color.grey;
            }
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
               //Debug.Log(existingId);
               //Debug.Log(obj.UniqueID);
             
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
                if (obj.IsSaveLoadable())
                {
                    SaveLoad.Instance.StringToObject.Add(existingId, obj);
                }

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
                if (obj.IsSaveLoadable())
                {
                    SaveLoad.Instance.ObjectToString.Add(obj, obj.UniqueID);
                }
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

        public virtual bool IsSaveLoadable()
        {
            return false;
        }

        private static void RegisterObject(SuperScriptableWithID aID, bool replace = false)
        {
            if (replace)
            {
                SaveLoad.Instance.StringToObject[aID.UniqueID] = aID;
            }
            else
            {
                if (aID.IsSaveLoadable())
                {
                    SaveLoad.Instance.StringToObject.Add(aID.UniqueID, aID);
                }
            }
            if (aID.IsSaveLoadable())
            {
                SaveLoad.Instance.ObjectToString.Add(aID, aID.UniqueID);
            }
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

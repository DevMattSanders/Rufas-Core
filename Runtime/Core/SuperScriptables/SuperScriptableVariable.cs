using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rufas
{
    [InlineEditor(Expanded = false,DrawPreview = true,PreviewAlignment = PreviewAlignment.Right)]
    public class SuperScriptableVariable <T>: SuperScriptableWithID
    {
        [SerializeField, TitleGroup("Save Load Options", Order = 1), HideInInlineEditors]
        internal bool saveOnValueChanged;

        [SerializeField, TitleGroup("Save Load Options", Order = 1), HideInInlineEditors]
        internal bool loadOnAwake;

        public bool debug;

        [DisableInPlayMode, SerializeField,InlineProperty, TitleGroup("$GetName")]
        private T startingValue;
        private string GetName()
        {
            string toReturn = name;

            if (saveOnValueChanged) { toReturn += " | (Save On Change)"; } else { toReturn += " | (Direct Save Only)"; }

            if (loadOnAwake) { toReturn += " | (Load On Awake)"; } else { toReturn += " | (Direct Load Only)"; }

            return toReturn; 
        }

        private T _value;

        public T GetStartingValue()
        {
            return startingValue;
        }

        public override bool IsSaveLoadable()
        {
            //return base.IsSaveLoadable();
            return saveOnValueChanged;
        }

        [ShowInInspector, DisableInEditorMode, InlineProperty, TitleGroup("$GetName")]
        public T Value
        {
            get
            {
                return _value;
            }
            set
            {
                if (EqualityComparer<T>.Default.Equals(_value, value))
                {
                    if (debug)
                    {
                        Debug.Log("Changing: " + _value + " " + value);
                    }

                    _value = value;
                }
                else
                {
                    _value = value;

                    if (saveOnValueChanged)
                    {
                        Save();
                    }

                    OnValueChanged.Raise(_value);
                }                
            }
        }

        [SerializeField,LabelText("ValueChangedDebug"), HideInInlineEditors] private CodeEvent<T> OnValueChanged;

        public void AddListener(System.Action<T> listener,bool runEventNow = false)
        {
            OnValueChanged.AddListener(listener);

            if (runEventNow) listener.Invoke(Value);
        }

        public void RemoveListener(System.Action<T> listener)
        {
            OnValueChanged.RemoveListener(listener);
        }

        public override void SoOnAwake()
        {
            base.SoOnAwake();

            if (debug)
            {
                Debug.Log(_value + " " + startingValue);
            }

            _value = startingValue;                 
        }

        public override void SoOnStart()
        {
            if (loadOnAwake)
            {
                if (debug)
                {
                    Debug.Log("PreLoad: " + _value + " " + startingValue);
                }

                bool loadSuccessful = false;
                Load(out loadSuccessful);

                if(loadSuccessful == false)
                {
                    _value = startingValue;
                }


                if (debug)
                {
                    Debug.Log("Successful? " + loadSuccessful);

                    Debug.Log("Loaded: "+ _value + " " + startingValue);
                }
            }
        }

        public override void SoOnEnd()
        {
            base.SoOnEnd();
            _value = startingValue;
        }

        [Button("Save"), TitleGroup("Save Load Options", Order = 1), HideInInlineEditors]
        public void Save()
        {
            if (Application.isPlaying == false) { Debug.Log("Super Scriptable Variable saving & loading is runtime only"); return; }

            SaveLoad.Instance.TrySave<T>(UniqueID, _value);
        }

        [Button("Load"), TitleGroup("Save Load Options", Order = 1), HideInInlineEditors]
        public void Load(out bool successful)
        {
            if (Application.isPlaying == false) { Debug.Log("Super Scriptable Variable saving & loading is runtime only"); successful = false; return; }

            SaveLoad.Instance.TryLoad<T>(UniqueID, out _value, out successful);
        }

        [ShowInInspector,SerializeField, TitleGroup("Save Load Options", Order = 1), HideInInlineEditors]
        private (bool,string) DebugIfSaveDataExists()
        {
#if UNITY_EDITOR
            if (ES3.KeyExists(UniqueID, SaveLoad.Instance.fileName))
            {
                Debug.Log(UniqueID);
                Debug.Log(SaveLoad.Instance);

                T val = ES3.Load<T>(UniqueID, SaveLoad.Instance.fileName);


                if (val != null)
                {
                    return (true, val.ToString());
                }
                else
                {
                    return (true, "Null");
                }
            }
            else
            {
                return (false,"");
            }
#else

            return (false, "");
#endif
        }


    }
}

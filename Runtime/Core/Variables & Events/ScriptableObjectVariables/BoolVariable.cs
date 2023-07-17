using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rufas
{
    [CreateAssetMenu(menuName = "Rufas/Variable/Bool")]
    public class BoolVariable : SuperScriptableWithID
    {
        [SerializeField, EnableIf("allowSaveLoad"), TitleGroup("Save Load Options", Order = 1)]
        private bool saveOnValueChanged;

        [DisableInPlayMode, SerializeField,TitleGroup("$GetName")]
        private bool startingValue;
        private string GetName() { return name; }

        private bool _value;

        [ShowInInspector, DisableInEditorMode, TitleGroup("$GetName")]
        public bool Value
        {
            get
            {
                return _value;
            }
            set
            {
                _value = value;

                if (saveOnValueChanged)
                {
                    SoOnSave();
                }
            }
        }

      

        public override void SoOnAwake()
        {
            base.SoOnAwake();
            _value = startingValue;
        }

        public override void SoOnEnd()
        {
            base.SoOnEnd();
            _value = startingValue;
        }
        public override void SoOnSave()
        {
            if (!allowSaveLoad || Application.isPlaying == false) return;

            base.SoOnSave();

            SaveLoad.Instance.SaveBool(UniqueID, _value);
        }

        public override void SoOnLoad()
        {
            if (!allowSaveLoad || Application.isPlaying == false) return;

            base.SoOnLoad();

            _value = SaveLoad.Instance.LoadBool(UniqueID);
        }
    }
}
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rufas
{
    [System.Serializable,InlineProperty]
    public class BoolReference
    {
        public enum ReferenceType
        {
            boolWithCallback,
            boolVariable,
            localBoolVariable
        }

        [PropertyOrder(2), HorizontalGroup("H")]
        [DisableInPlayMode, HideLabel]
        [SerializeField] private ReferenceType referenceType;

        private bool ShowCallback() { return referenceType == ReferenceType.boolWithCallback; }
        [PropertyOrder(1), HorizontalGroup("H"), DisableInPlayMode, HideLabel]
        [SerializeField, ShowIf("ShowCallback")] private BoolWithCallback callback;

        private bool ShowScriptable() { return referenceType == ReferenceType.boolVariable; }
        [PropertyOrder(1),HorizontalGroup("H"), DisableInPlayMode, HideLabel,Required]
        [SerializeField, ShowIf("ShowScriptable")] private BoolVariable scriptable;

        private bool ShowLocal() { return referenceType == ReferenceType.localBoolVariable; }
        [PropertyOrder(1),HorizontalGroup("H"),DisableInPlayMode, HideLabel, Required]
        [SerializeField, ShowIf("ShowLocal")] private LocalBoolVariable local;


        private bool ShowValue() { if (referenceType == ReferenceType.boolVariable || referenceType == ReferenceType.localBoolVariable) return true; return false; }

        
        [PropertyOrder(0),HorizontalGroup("H",width: 20)]
        [ShowInInspector,HideLabel,ShowIf("ShowValue")]
        public bool Value
        {
            get
            {
                switch (referenceType)
                {
                    case ReferenceType.boolWithCallback: return callback.Value;
                    case ReferenceType.boolVariable: if (scriptable) { return scriptable.Value; } else { return false; }
                    case ReferenceType.localBoolVariable: if (local) { return local.Value; } else { return false; }
                    default: return false;
                }
            }
            set
            {
                switch (referenceType)
                {
                    case ReferenceType.boolWithCallback: callback.Value = value; break;
                    case ReferenceType.boolVariable: if (scriptable) scriptable.Value = value; break;
                    case ReferenceType.localBoolVariable: if(local) local.Value = value; break;
                }
            }
        }


        

        public void AddListener(System.Action<bool> listener)
        {
            switch (referenceType)
            {
                case ReferenceType.boolWithCallback: callback.AddListener(listener); break;
                case ReferenceType.boolVariable: if(scriptable) scriptable.AddListener(listener); break;
                case ReferenceType.localBoolVariable: if(local) local.AddListener(listener); break;
            }
        }

        public void RemoveListener(System.Action<bool> listener)
        {
            switch (referenceType)
            {
                case ReferenceType.boolWithCallback: callback.RemoveListener(listener); break;
                case ReferenceType.boolVariable: if(scriptable) scriptable.RemoveListener(listener); break;
                case ReferenceType.localBoolVariable: if(local) local.RemoveListener(listener); break;
            }
        }
    }
}

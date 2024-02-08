using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Build;
#endif
using UnityEngine;

namespace Rufas
{
    public class RufasScriptingDefineSymbolsManager
    {
#if UNITY_EDITOR
        public static List<DefineSymbolCheck> AllDefineSymbols()
        {
            return new List<DefineSymbolCheck>
            {
                 new DSC_RUFAS_UNITY_SYSTEMS(),
                 new DSC_RUFAS_XNODE()
            };
        }

        [System.Serializable]
        public class DefineSymbolCheck
        {
            public virtual string Name() { return ""; }
            public virtual bool Check() { return false; }

            [PropertyOrder(0)]
            [HorizontalGroup("H",order: 0,width: 15)]
            [HideLabel, HideIf("Check")]
            [Button(icon: SdfIconType.ExclamationTriangleFill)]
            
            private void NotFoundButton()
            {
                Debug.Log("The Scripting Define Symbol: " + Name() + " was not found in the current build settings! To include in the project, copy exact text into PlayerSettings -> Other -> ScriptingDefineSymbols");
            }

            [PropertyOrder(1)]
            [HorizontalGroup("H",order: 1)]
            [HideLabel, EnableIf("EnableText")]
            public string TextField;

            private bool CounterCheck()
            {
                return !Check();
            }

            private bool EnableText()
            {
                TextField = Name();
                return !Check();
            }
        }

        [System.Serializable]
        class DSC_RUFAS_UNITY_SYSTEMS : DefineSymbolCheck
        {
            public override string Name()
            {
                return "RUFAS_UNITY_SYSTEMS";
            }

            public override bool Check()
            {
#if RUFAS_UNITY_SYSTEMS
             return true;
#else
                return false;
#endif
            }
        }

        class DSC_RUFAS_XNODE : DefineSymbolCheck
        {
            public override string Name()
            {
                return "RUFAS_XNODE";
            }

            public override bool Check()
            {
#if RUFAS_XNODE
                return true;
#else
                return false;
#endif
            }
        }

#endif
            }


}

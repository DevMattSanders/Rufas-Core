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
                 new DSC_RUFAS_UNITY_SYSTEMS()
            };
        }

        [System.Serializable]
        public class DefineSymbolCheck
        {
            public virtual string Name() { return ""; }
            public virtual bool Check() { return false; }


            //  [HorizontalGroup("H"),ReadOnly,HideIf("Check"),HideLabel]
            //  public string NotIncludedWarning = "Copy Here -->";

            // [InfoBox("Symbol not found!", icon: SdfIconType.ExclamationCircleFill, VisibleIf = "CounterCheck")]

            //[HorizontalGroup("H")]
            //[HideLabel, ShowIf("Check")]
            // public EditorIcon foundIcon = EditorIcons.;


            //public EditorIcon notFoundIcon = EditorIcons.AlertCircle;

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
                //if (Check()) //
                //{              

                    TextField = Name();
                //}
               // else
               // {
                  //  TextField = Name();
               // }

                return! Check();
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
#endif
            }


}

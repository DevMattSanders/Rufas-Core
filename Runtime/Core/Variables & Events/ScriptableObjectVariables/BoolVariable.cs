using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rufas
{
    [CreateAssetMenu(menuName = "Rufas/Variable/Bool")]
    public class BoolVariable : SuperScriptable
    {
        private bool _value;

        [ShowInInspector,DisableInEditorMode, LabelText("$GetName")]
        public bool Value
        {
            get
            {
                return _value;
            }
            set
            {

                //If _value != value
                //Custom event for every time thevalue is changed
                               
                _value = value;
            }
        }
                       
        [DisableInPlayMode,SerializeField,LabelText("$GetName")] private bool startingValue;

        private string GetName() { return name; }

        public override void SoOnAwake()
        {
            base.SoOnAwake();            

            //Load stuff
            _value = startingValue; // loading          
        }

        public override void SoOnEnd()
        {
            base.SoOnEnd();
            _value = startingValue;
        }
    }
}

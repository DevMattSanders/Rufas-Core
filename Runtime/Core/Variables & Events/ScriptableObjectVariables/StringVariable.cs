using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rufas
{
    [CreateAssetMenu(menuName = "Rufas/Variable/String")]
    public class StringVariable : SuperScriptableVariable<string>
    {
        /*
        private string _value;

        [ShowInInspector, HideInEditorMode, LabelText("$GetName")]
        public string Value
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

        [HideInPlayMode, SerializeField, LabelText("$GetName")] private string startingValue;


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
        */
    }
}

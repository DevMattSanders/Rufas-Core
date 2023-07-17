using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rufas
{
    [CreateAssetMenu(menuName = "Rufas/Variable/Int")]
    public class IntVariable : SuperScriptableVariable<int>
    {
        /*
        private int _value;

        [ShowInInspector, HideInEditorMode, LabelText("$GetName")]
        public int Value
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

        [HideInPlayMode, SerializeField, LabelText("$GetName")] private int startingValue;


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

using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rufas
{
    [CreateAssetMenu(menuName = "Rufas/Variable/Float")]
    public class FloatVariable : SuperScriptableVariable<float>
    {/*
        private float _value;

        [ShowInInspector, HideInEditorMode, LabelText("$GetName")]
        public float Value
        {
            get
            { return _value; }
            set
            { _value = value; }
        }

        [HideInPlayMode, SerializeField, LabelText("$GetName")] private float startingValue;

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

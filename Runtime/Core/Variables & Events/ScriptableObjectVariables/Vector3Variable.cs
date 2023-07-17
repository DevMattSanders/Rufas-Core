using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rufas
{
    [CreateAssetMenu(menuName = "Rufas/Variable/Vector3")]
    public class Vector3Variable : SuperScriptableVariable<Vector3>
    {
        /*
        private Vector3 _value;

        [ShowInInspector, HideInEditorMode, LabelText("$GetName")]
        public Vector3 Value
        {
            get
            { return _value; }
            set
            { _value = value; }
        }

        [HideInPlayMode, SerializeField, LabelText("$GetName")] private Vector3 startingValue;

        private string GetName() { return name; }

        public override void SoOnAwake()
        {
            base.SoOnAwake();

            //Load stuff
            _value = startingValue; // loading          
        }
        */
    }
}

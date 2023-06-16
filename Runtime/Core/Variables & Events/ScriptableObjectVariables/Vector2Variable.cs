using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rufas
{
    [CreateAssetMenu(menuName = "Rufas/Variable/Vector2")]
    public class Vector2Variable : SuperScriptable
    {
        private Vector2 _value;

        [ShowInInspector, HideInEditorMode, LabelText("$GetName")]
        public Vector2 Value
        {
            get
            { return _value; }
            set
            { _value = value; }
        }

        [HideInPlayMode, SerializeField, LabelText("$GetName")] private Vector2 startingValue;

        private string GetName() { return name; }

        public override void SoOnAwake()
        {
            base.SoOnAwake();

            //Load stuff
            _value = startingValue; // loading          
        }
    }
}

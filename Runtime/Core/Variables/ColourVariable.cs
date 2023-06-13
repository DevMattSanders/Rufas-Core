using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rufas
{
    [CreateAssetMenu(menuName = "Rufas/Variable/Colour")]
    public class ColourVariable : SuperScriptable
    {
        private Color _value;

        [ShowInInspector, HideInEditorMode, LabelText("$GetName")]
        public Color Value
        {
            get
            { return _value; }
            set
            { _value = value; }
        }

        [HideInPlayMode, SerializeField, LabelText("$GetName")] private Color startingValue;

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

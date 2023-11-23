using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rufas
{
    public class PreAwakeBehaviour : ScriptableObject
    {
       // [InfoBox("MUST BE INSIDE A RESOURCES FOLDER TO FUNCTION", InfoMessageType = InfoMessageType.Warning)]

        [InfoBox("Lower values have higher priorities and get triggered first")]
        [Range(0, 10)]
        public int priority = 5;


        private void OnEnable()
        {
            if (Application.isPlaying == false)
            {

            }
        }


        public virtual void BehaviourToRunBeforeStart()
        {
            
        }
    }
}

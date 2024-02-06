using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rufas
{
    [CreateAssetMenu(menuName = "Rufas/Variable/GameObject")]
    public class GameObjectVariable : SuperScriptable
    {
        [InfoBox("Usecase is for storing instances generated at runtime or for referencing prefabs. There is no saving/loading feature for this type")]

        [DisableInPlayMode]
        public GameObject startingValue;
        [DisableInEditorMode]
        public GameObject value;

        //value needs to be a get/set event for cases where other systems cache it

        public override void SoOnAwake()
        {
            base.SoOnAwake();
            if (startingValue == null)
            {
                value = null;
            }
            else
            {
                value = startingValue;
            }

        }
    }
}

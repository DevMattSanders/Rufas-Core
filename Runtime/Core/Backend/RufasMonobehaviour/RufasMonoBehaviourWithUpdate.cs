using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rufas
{
    public class RufasMonoBehaviourWithUpdate : RufasMonoBehaviour
    {
        public override void Start()
        {
            base.Start();
            RufasMonoBehaviourHandler.updateBehaviours.Add(this);
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            RufasMonoBehaviourHandler.updateBehaviours.Remove(this);
        }


        public virtual void Update_AfterInitialisation()
        {

        }
    }
}
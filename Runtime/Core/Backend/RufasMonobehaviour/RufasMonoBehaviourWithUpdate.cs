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

            //if (RufasMonoBehaviourHandler.startCalled)
            // {
            //     Start_AfterInitialisation();
            // }
            // else
            // {
            Debug.Log("Here");
                RufasMonoBehaviourHandler.updateBehaviours.Add(this);
           // }
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rufas
{
    public class RufasMonoBehaviour : MonoBehaviour
    {
        internal bool initialisationCompleted = false;
        internal bool beingDestroyed = false;
        public virtual void Awake()
        {
            initialisationCompleted = false;

            if (RufasMonoBehaviourHandler.awakeCalled)
            {
                Awake_AfterInitialisation();
            }
            else
            {
                RufasMonoBehaviourHandler.waitingForAwake.Add(this);
            }
        }

        public virtual void Start()
        {
            if (RufasMonoBehaviourHandler.startCalled)
            {
                Start_AfterInitialisation();
            }
            else
            {
                RufasMonoBehaviourHandler.waitingForStart.Add(this);
            }
        }

        public virtual void OnDestroy()
        {
            beingDestroyed = true;
        }

        public virtual void Awake_AfterInitialisation()
        {

        }

        public virtual void Start_AfterInitialisation()
        {
            if (!beingDestroyed)
            {
                //Don't allow init marked as complete if being destroyed
                Debug.Log("Trying to finish initializing a Rufas MonoBehaviour that is marked to destroy!");
                initialisationCompleted = true;
            }
        }
    }
}

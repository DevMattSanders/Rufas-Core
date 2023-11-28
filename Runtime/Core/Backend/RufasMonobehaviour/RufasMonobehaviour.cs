using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rufas
{
    public class RufasMonoBehaviour : MonoBehaviour
    {
        public virtual void Awake()
        {
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

        public virtual void Awake_AfterInitialisation()
        {

        }

        public virtual void Start_AfterInitialisation()
        {

        }
    }
}

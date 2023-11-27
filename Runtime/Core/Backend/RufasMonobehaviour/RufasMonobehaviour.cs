using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rufas
{
    public class RufasMonobehaviour : MonoBehaviour
    {
        public virtual void Awake()
        {
            RufasMonoBehaviourHandler.Instance.Register(this);
        }

        public virtual void OnDestroy()
        {
            RufasMonoBehaviourHandler.Instance.Unregister(this);
        }

        public virtual void Awake_AfterInitialisation()
        {

        }

        public virtual void Start_AfterInitialisation()
        {

        }
    }
}

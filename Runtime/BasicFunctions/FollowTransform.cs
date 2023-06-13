using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rufas
{
    public class FollowTransform : MonoBehaviour
    {
        public Transform target;
        void Update()
        {
            if (target == null) return;

            transform.position = target.position;
            transform.rotation = target.rotation;

        }
    }
}

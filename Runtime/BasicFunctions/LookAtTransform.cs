using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Sirenix.OdinInspector;

namespace Rufas
{
    [ExecuteAlways]
    public class LookAtTransform : MonoBehaviour
    {
        public Transform target;

        void Update()
        {
            transform.LookAt(target);
        }
    }
}
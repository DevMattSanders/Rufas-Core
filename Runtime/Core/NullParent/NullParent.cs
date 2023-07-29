using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rufas
{
    public class NullParent : MonoBehaviour
    {
        public static Transform nullParent;

        private void Awake()
        {
            if (nullParent == null) nullParent = transform;
        }

        private void OnDestroy()
        {
            if (nullParent == transform) nullParent = null;
        }
    }
}

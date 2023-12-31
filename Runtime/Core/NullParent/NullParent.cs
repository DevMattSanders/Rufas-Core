using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rufas
{
    public class NullParent : MonoBehaviour
    {
        public static Transform nullParent { get; private set; }

        private void Awake()
        {
            if (gameObject.activeInHierarchy)
            {
                if (nullParent == null) nullParent = transform;
            }
        }

        private void OnDestroy()
        {
            if (nullParent == transform) nullParent = null;
        }
    }
}

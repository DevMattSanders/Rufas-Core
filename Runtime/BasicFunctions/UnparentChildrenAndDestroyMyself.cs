using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rufas
{
    public class UnparentChildrenAndDestroyMyself : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            transform.DetachChildren();
            Destroy(gameObject);
        }
    }
}

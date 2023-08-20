using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rufas
{
    public class Billboard : MonoBehaviour
    {
        public float additonalYRot = 0;
        void LateUpdate()
        {
            transform.forward = new Vector3(Camera.main.transform.forward.x, transform.forward.y, Camera.main.transform.forward.z);

            transform.Rotate(0, additonalYRot, 0, Space.Self);
        }
    }
}

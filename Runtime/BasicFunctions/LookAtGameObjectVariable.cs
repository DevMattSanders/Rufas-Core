using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rufas.BasicFunctions
{
    public class LookAtGameObjectVariable : MonoBehaviour
    {
        public GameObjectVariable target;
        public float localYRotOffset = 0;

        public bool onlyLookAtSameHeight = false;

        private void Update()
        {
            if (onlyLookAtSameHeight)
            {
                transform.LookAt(new Vector3(target.value.transform.position.x, transform.position.y, target.value.transform.position.z));
            }
            else
            {
                transform.LookAt(target.value.transform);
            }
            transform.Rotate(0, localYRotOffset, 0, Space.Self);
        }
    }
}

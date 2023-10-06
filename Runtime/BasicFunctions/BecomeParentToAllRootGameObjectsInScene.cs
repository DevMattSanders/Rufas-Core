using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Rufas.BasicFunctions
{
    public class BecomeParentToAllRootGameObjectsInScene : MonoBehaviour
    {
        [Tooltip("If null, will use this object")]
        public Transform parentToSet;

        private void Awake()
        {
            if (parentToSet == null) parentToSet = transform;
        }

        private void Start()
        {
            transform.SetParent(null);
            transform.SetPositionAndRotation(Vector3.zero,Quaternion.identity);

           // parentToSet.SetPositionAndRotation(Vector3.zero, Quaternion.identity);

            foreach (GameObject g in gameObject.scene.GetRootGameObjects())
            {
                g.transform.SetParent(parentToSet, true);
            }
        }
    }
}

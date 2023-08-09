using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Rufas.BasicFunctions
{
    public class BecomeParentToAllRootGameObjectsInScene : MonoBehaviour
    {
        private void Start()
        {
            transform.SetParent(null);
            transform.SetPositionAndRotation(Vector3.zero,Quaternion.identity);

            foreach(GameObject g in gameObject.scene.GetRootGameObjects())
            {
                g.transform.SetParent(transform, true);
            }
        }
    }
}

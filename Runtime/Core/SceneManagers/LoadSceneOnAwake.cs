using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rufas
{
    public class LoadSceneOnAwake : MonoBehaviour
    {
        public SoScene sceneToLoad;

        

        private void Awake()
        {
            sceneToLoad.LoadScene();
        }
    }
}

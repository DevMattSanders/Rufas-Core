using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rufas
{
    public class LoadSceneOnAwake : MonoBehaviour
    {
        public SoScene sceneToLoad;

        //public float delay = 0.5f;
        public int frames = 2;

        private void Awake()
        {

            if (frames <= 0)
            {
                sceneToLoad.LoadScene();
            }
            else
            {
                this.CallWithFrameDelay(sceneToLoad.LoadScene, frames);
            }
        }
    }
}

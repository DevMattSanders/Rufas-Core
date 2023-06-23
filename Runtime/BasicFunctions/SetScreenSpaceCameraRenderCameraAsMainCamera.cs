using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rufas.BasicFunctions
{
    public class SetScreenSpaceCameraRenderCameraAsMainCamera : MonoBehaviour
    {

        Camera mainCameraRef;

        private IEnumerator Start()
        {
            Canvas canvas = GetComponent<Canvas>();

            if(canvas == null)
            {
                Debug.Log("Canvas is null!");

                yield break;
            }

            while (mainCameraRef == null)
            {
                mainCameraRef = Camera.main;

                yield return null;
            }
            

            canvas.worldCamera = mainCameraRef;
            canvas.planeDistance = 0.02f;

        }

    }
}

using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rufas.BasicFunctions
{
    public class SetScreenSpaceCameraRenderCameraAsMainCamera : RufasMonoBehaviour
    {
        public static CodeEvent RefreshCameras;

        [ReadOnly,SerializeField]
        private Camera mainCameraRef;
        Canvas canvas;
        [InfoBox("Will use Camera.Main if this GameObjectVariable is null")]// (but not the GOV value!)")]
        public GameObjectVariable cameraGameObject;

        public override void Start()
        {
            base.Start();
            RefreshCameras.AddListener(Refresh);

            canvas = GetComponent<Canvas>();
            if (canvas == null)
            {
                Debug.Log("Canvas is null!");
              //  yield break;
            }
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            RefreshCameras.RemoveListener(Refresh);
        }

        public override void Start_AfterInitialisation()
        {
            base.Start_AfterInitialisation();
            Refresh();
        }

        // public override void Start_AfterInitialisation()
        // {
        //     base.Start_AfterInitialisation();        
        //  StartCoroutine(Routine());
        // }
        /*
        private IEnumerator Routine()
        {          
            if (canvas == null)
            {
                Debug.Log("Canvas is null!");
                yield break;
            }

            while (true)
            {
                Refresh();
                yield return new WaitForSecondsRealtime(1);
            }  
        }
        */
        public void Refresh()
        {
           // bool cameraNull = false;
            if (mainCameraRef == null)
            {
               // cameraNull = true;

                if (cameraGameObject != null)
                {
                    if (cameraGameObject.value != null)
                    {
                        mainCameraRef = cameraGameObject.value.GetComponent<Camera>();
                    }
                    else
                    {
                        mainCameraRef = Camera.main;
                    }
                }
                else
                {
                    mainCameraRef = Camera.main;
                }
            }


            if (mainCameraRef != null && canvas != null)// && cameraNull == true)
            {
                canvas.worldCamera = mainCameraRef;
                canvas.planeDistance = 0.02f;
            }
        }
    }
}

using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;

namespace Rufas
{
    public static class ScreenshotUtility
    {
        public static string screenshotFolderName = "GameplayScreenshots";

        public static int pixelWidth = 384;
        public static int pixelHeight = 216;

        public static void CaptureScreenshot()
        {
            CaptureScreenshot((Texture2D tex) =>
            {
                if (tex != null)
                {
                    Debug.Log("Captured texture successfully!");

                    Debug.Log(tex.mipmapCount + " " + tex.dimension);

                }
                else
                {
                    Debug.LogError("Failed to capture texture!");
                }
            });
        }

        public static void CaptureScreenshot(Action<Texture2D> returnTexture)
        {
            CoroutineMonoBehaviour.i.StartCoroutine(CoroutineScreenshot( pixelWidth, pixelHeight, returnTexture));
        }

        public static void CaptureScreenshot(int _pixelWidth, int _pixelHeight, Action<Texture2D> returnTexture)
        {

            CoroutineMonoBehaviour.i.StartCoroutine(CoroutineScreenshot(_pixelWidth, _pixelHeight, returnTexture));
        }

        private static IEnumerator CoroutineScreenshot(int _pixelWidth, int _pixelHeight, Action<Texture2D> onComplete)
        {

            yield return new WaitForEndOfFrame();

            Texture2D screenshotTexture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGBA32, true);

            // Capture the full screen
            screenshotTexture.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
            screenshotTexture.Apply();

            // Resize the texture to the lower resolution on the GPU
            Texture2D resizedTexture = ResizeTextureGPU(screenshotTexture, _pixelWidth, _pixelHeight);

            // Invoke the onComplete callback with the resized texture
            onComplete?.Invoke(resizedTexture);
        }
       
        private static Texture2D ResizeTextureGPU(Texture2D sourceTexture, int targetWidth, int targetHeight)
        {
            RenderTexture rt = RenderTexture.GetTemporary(targetWidth, targetHeight, 0, RenderTextureFormat.Default,RenderTextureReadWrite.Linear);
            rt.filterMode = FilterMode.Bilinear;

            Graphics.Blit(sourceTexture, rt);

            Texture2D resultTexture = new Texture2D(targetWidth, targetHeight);
            resultTexture.filterMode = FilterMode.Bilinear;
            resultTexture.wrapMode = TextureWrapMode.Clamp;
            RenderTexture.active = rt;
            resultTexture.ReadPixels(new Rect(0, 0, targetWidth, targetHeight), 0, 0);

            resultTexture.Apply();
            RenderTexture.active = null;
            RenderTexture.ReleaseTemporary(rt);


            Debug.Log(resultTexture.isReadable);
            return resultTexture;
        }      

    }
}

using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;

namespace Rufas
{
    public class ScreenshotUtility : GameSystem<ScreenshotUtility>
    {
        [SerializeField] private string screenshotFolderName = "GameplayScreenshots";

        [ShowInInspector, ReadOnly]
        public string DataPath()
        {
            return Application.dataPath + "/" + screenshotFolderName;
        }

        public int pixelWidth = 384;
        public int pixelHeight = 216;

        [PreviewField,ReadOnly] public Texture2D mostRecentScreenshot;
        [PreviewField,ReadOnly] public List<Texture2D> screenshots = new List<Texture2D>();

        public override string DesiredName()
        {
            return "Screenshot Utility";
        }

        public static void CaptureScreenshotNow() { ScreenshotUtility.Instance?.CaptureDefaultScreenshot(); }


        public override void PreInitialisationBehaviour()
        {
            base.PreInitialisationBehaviour();        
            RefreshFolder();
        }

        [Button]
        public void CaptureDefaultScreenshot()
        {
            CoroutineMonoBehaviour.i.StartCoroutine(CoroutineScreenshot("Screenshot" + screenshots.Count,(Texture2D screenshot) =>
            {
                if (screenshot != null)
                {
                    screenshots.Add(screenshot);
                    mostRecentScreenshot = screenshot;
                    Debug.Log("Screenshot captured with default width and height!");
                }
                else
                {
                    Debug.LogError("Failed to capture screenshot!");
                }
            }));
            /*
            CaptureScreenshot(pixelWidth, pixelHeight, "Screenshot: " + screenshots.Count, (Texture2D screenshot) =>
            {
                // Handle the captured screenshot
                if (screenshot != null)
                {
                    Debug.Log("Screenshot captured with default width and height!");
                }
                else
                {
                    Debug.LogError("Failed to capture screenshot!");
                }
            });
            */
        }

        private IEnumerator CoroutineScreenshot(string screenshotName, Action<Texture2D> onComplete)
        {
            yield return new WaitForEndOfFrame();

            Texture2D screenshotTexture = new Texture2D(Screen.width, Screen.height, TextureFormat.ARGB32, false);

            // Capture the full screen
            screenshotTexture.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
            screenshotTexture.Apply();

            // Resize the texture to the lower resolution on the GPU
            Texture2D resizedTexture = ResizeTextureGPU(screenshotTexture, pixelWidth, pixelHeight);

            // Save the resized texture
            byte[] byteArray = resizedTexture.EncodeToPNG();
            System.IO.File.WriteAllBytes(DataPath() + "/" + screenshotName + ".png", byteArray);

            // Invoke the onComplete callback with the resized texture
            onComplete?.Invoke(resizedTexture);
        }

        private Texture2D ResizeTextureGPU(Texture2D sourceTexture, int targetWidth, int targetHeight)
        {
            RenderTexture rt = RenderTexture.GetTemporary(targetWidth, targetHeight, 0, RenderTextureFormat.ARGB32);
            rt.filterMode = FilterMode.Bilinear;

            Graphics.Blit(sourceTexture, rt);

            Texture2D resultTexture = new Texture2D(targetWidth, targetHeight, TextureFormat.ARGB32, false);
            RenderTexture.active = rt;
            resultTexture.ReadPixels(new Rect(0, 0, targetWidth, targetHeight), 0, 0);
            resultTexture.Apply();

            RenderTexture.active = null;
            RenderTexture.ReleaseTemporary(rt);

            return resultTexture;
        }
        /*
        public void CaptureScreenshot(int width, int height, string screenshotName, Action<Texture2D> onComplete = null)
        {

            Texture2D screenshotTexture = null;

            if (width < 1 || width > 1080) width = pixelWidth;
            if (height < 1 || height > 1920) height = pixelHeight;

            screenshotTexture = new Texture2D(width, height, TextureFormat.ARGB32, false);
            Rect rect = new Rect(0, 0, width, height);
            screenshotTexture.ReadPixels(rect, 0, 0);
            screenshotTexture.Apply();

            byte[] byteArray = screenshotTexture.EncodeToPNG();
            System.IO.File.WriteAllBytes(Application.persistentDataPath + "/" + screenshotName + ".png", byteArray);
            onComplete?.Invoke(screenshotTexture);
        }

        public void CaptureScreenshot(string screenshotName, Action<Texture2D> onComplete = null)
        {
            Texture2D screenshotTexture = null;

            screenshotTexture = new Texture2D(pixelWidth, pixelHeight, TextureFormat.ARGB32, false);
            Rect rect = new Rect(0, 0, pixelWidth, pixelHeight);
            screenshotTexture.ReadPixels(rect, 0, 0);
            screenshotTexture.Apply();

            byte[] byteArray = screenshotTexture.EncodeToPNG();
            System.IO.File.WriteAllBytes(DataPath() + "/" + screenshotName + ".png", byteArray);


            onComplete?.Invoke(screenshotTexture);
        }
        */
        /*
        public async void CaptureScreenshot(int width, int height, string screenshotName, Action<Texture2D> onComplete = null)
        {
            Texture2D screenshotTexture = null;

            await Task.Run(() =>
            {
                if (width < 1 || width > 1080) width = pixelWidth;
                if (height < 1|| height > 1920) height = pixelHeight;

                screenshotTexture = new Texture2D(width, height, TextureFormat.ARGB32, false);
                Rect rect = new Rect(0, 0, width, height);
                screenshotTexture.ReadPixels(rect, 0, 0);
                screenshotTexture.Apply();

                byte[] byteArray = screenshotTexture.EncodeToPNG();
                System.IO.File.WriteAllBytes(DataPath() + "/" + screenshotName, byteArray);
            });

            onComplete?.Invoke(screenshotTexture);
        }

        public async void CaptureScreenshot(string screenshotName, Action<Texture2D> onComplete = null)
        {
            Texture2D screenshotTexture = null;

            await Task.Run(() =>
            {
                screenshotTexture = new Texture2D(pixelWidth, pixelHeight, TextureFormat.ARGB32, false);
                Rect rect = new Rect(0, 0, pixelWidth, pixelHeight);
                screenshotTexture.ReadPixels(rect, 0, 0);
                screenshotTexture.Apply();

                byte[] byteArray = screenshotTexture.EncodeToPNG();
                System.IO.File.WriteAllBytes(DataPath() + "/" + screenshotName, byteArray);
            });

            onComplete?.Invoke(screenshotTexture);
        }
        */


        [Button]
        private void RefreshFolder()
        {
            screenshots.Clear();

            if(!Directory.Exists(DataPath()))
            {
                Directory.CreateDirectory(DataPath());
            }

            //if (Directory.Exists(DataPath()))
           // {
                string[] files = Directory.GetFiles(DataPath(), "*.png");;

                foreach(string filePath in files)
                {
                    byte[] fileData = File.ReadAllBytes(filePath);
                    Texture2D texture = new Texture2D(2, 2); //Placeholder
                    texture.LoadImage(fileData);
                    screenshots.Add(texture);
                }
            //}
            //else
           /// {
           //     Debug.LogError("Folder does not exist: " + DataPath());
            //}
        }


    }
}

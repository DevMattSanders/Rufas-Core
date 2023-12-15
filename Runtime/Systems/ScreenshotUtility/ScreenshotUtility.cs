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

        public static void CaptureScreenshotNow() { ScreenshotUtility.Instance?.CaptureScreenshot(); }
        public override void PreInitialisationBehaviour() { base.PreInitialisationBehaviour(); RefreshFolder(); }

        [Button]
        public void CaptureScreenshot()
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

        public void CaptureScreenshot(Action<Texture2D> returnTexture)
        {
            CoroutineMonoBehaviour.i.StartCoroutine(CoroutineScreenshot("Screenshot" + screenshots.Count, pixelWidth, pixelHeight, returnTexture));
        }

        public void CaptureScreenshot(string _screenshotName, Action<Texture2D> returnTexture)
        {
            CoroutineMonoBehaviour.i.StartCoroutine(CoroutineScreenshot(_screenshotName, pixelWidth, pixelHeight, returnTexture));
        }

        public void CaptureScreenshot(string _screenshotName, int _pixelWidth, int _pixelHeight, Action<Texture2D> returnTexture)
        {

            CoroutineMonoBehaviour.i.StartCoroutine(CoroutineScreenshot(_screenshotName, _pixelWidth, _pixelHeight, returnTexture));

            /*
            CoroutineMonoBehaviour.i.StartCoroutine(CoroutineScreenshot(_screenshotName, _pixelWidth, _pixelHeight, (Texture2D screenshot) =>
            {
                if (screenshot != null)
                {
                    screenshots.Add(screenshot);
                    mostRecentScreenshot = screenshot;
                    Debug.Log("Screenshot captured with default width and height!");
                    //Call returnTexture here with result?
                }
                else
                {
                    Debug.LogError("Failed to capture screenshot!");
                    //Call returnTexture here with null?
                }
            }));
            */
        }

        private IEnumerator CoroutineScreenshot(string screenshotName, int _pixelWidth, int _pixelHeight, Action<Texture2D> onComplete)
        {

            yield return new WaitForEndOfFrame();

            Texture2D screenshotTexture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGBA32, true);

            // Capture the full screen
            screenshotTexture.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
            screenshotTexture.Apply();

            // Resize the texture to the lower resolution on the GPU
            Texture2D resizedTexture = ResizeTextureGPU(screenshotTexture, _pixelWidth, _pixelHeight);

            // Save the resized texture
            byte[] byteArray = resizedTexture.EncodeToPNG();

            string pathToCreate = DataPath() + "/" + screenshotName + ".png";

            // Split the path after the last "/"
            int lastSeparatorIndex = pathToCreate.LastIndexOf('/');
            string directoryPart = pathToCreate.Substring(0, lastSeparatorIndex);

            // Check if the directory exists, and create it if it doesn't
            if (!Directory.Exists(directoryPart))
            {
                Directory.CreateDirectory(directoryPart);
            }

            System.IO.File.WriteAllBytes(pathToCreate, byteArray);

            screenshots.Add(resizedTexture);
            mostRecentScreenshot = resizedTexture;

            // Invoke the onComplete callback with the resized texture
            onComplete?.Invoke(resizedTexture);
        }

        private Texture2D ResizeTextureGPU(Texture2D sourceTexture, int targetWidth, int targetHeight)
        {
            RenderTexture rt = RenderTexture.GetTemporary(targetWidth, targetHeight, 0, RenderTextureFormat.Default,RenderTextureReadWrite.Linear);
            rt.filterMode = FilterMode.Bilinear;

            Graphics.Blit(sourceTexture, rt);

            Texture2D resultTexture = new Texture2D(targetWidth, targetHeight);//, TextureFormat.RGBA32, true); // Set isReadable to true
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

#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

using Sirenix.OdinInspector;

namespace Rufas
{
    public class IconGenerator : MonoBehaviour
    {
        private Camera iconCamera;

        [Header("Icon Resoultion")]
        public int imageSizeInPixels;

        [Header("Icon Details")]
        public NamedGameObject[] iconsToGenerate;

        [Button()]
        public void GenerateSingleIcons(int index)
        {
            if (iconsToGenerate.Length == 0)
            {
                Debug.LogError("Can't generate icon because there is nothing in the list! Stupid! Stupid Stupid!");
                return;
            }

            // Disable All Icons
            foreach (NamedGameObject newIcon in iconsToGenerate)
            {
                // Spawn Item In
                newIcon.iconObjectPrefab.SetActive(false);
            }

            iconsToGenerate[index].iconObjectPrefab.SetActive(true);

            // Save Image
            GenerateIconImage(iconsToGenerate[index].iconFileName);

            iconsToGenerate[index].iconObjectPrefab.SetActive(false);

            // Renable First Icon
            iconsToGenerate[index].iconObjectPrefab.SetActive(true);
        }

        [Button()]
        public void GenerateBatchOfIcons()
        {
            if (iconsToGenerate.Length == 0)
            {
                Debug.LogError("Can't generate icon because there is nothing in the list! Stupid! Stupid Stupid!");
                return;
            }

            // Disable All Icons
            foreach (NamedGameObject newIcon in iconsToGenerate)
            {
                // Spawn Item In
                newIcon.iconObjectPrefab.SetActive(false);
            }

            // Enable Each Icon & Take Photo
            foreach (NamedGameObject newIcon in iconsToGenerate)
            {
                // Spawn Item In
                newIcon.iconObjectPrefab.SetActive(true);

                // Save Image
                GenerateIconImage(newIcon.iconFileName);

                newIcon.iconObjectPrefab.SetActive(false);
            }

            // Renable First Icon
            iconsToGenerate[0].iconObjectPrefab.SetActive(true);
        }

        public void GenerateFirstIconInList()
        {
            if (iconsToGenerate.Length == 0)
            {
                Debug.LogError("Can't generate icon because there is nothing in the list! Stupid! Stupid Stupid!");
                return;
            }

            // Disable All Icons
            foreach (NamedGameObject newIcon in iconsToGenerate)
            {
                // Spawn Item In
                newIcon.iconObjectPrefab.SetActive(false);
            }

            // Renable First Icon
            iconsToGenerate[0].iconObjectPrefab.SetActive(true);

            GenerateIconImage(iconsToGenerate[0].iconFileName);
        }

        public void GenerateIconImage(string iconFileName)
        {
            // Get Camera
            if (iconCamera == null)
            {
                iconCamera = GetComponent<Camera>();
            }

            // Set The Resoultion
            Screen.SetResolution(imageSizeInPixels, imageSizeInPixels, false);

            // Take Screenshot
            RenderTexture screenTexture = new RenderTexture(imageSizeInPixels, imageSizeInPixels, 16);
            iconCamera.targetTexture = screenTexture;
            RenderTexture.active = screenTexture;
            iconCamera.Render();
            Texture2D renderedTexture = new Texture2D(imageSizeInPixels, imageSizeInPixels);
            renderedTexture.ReadPixels(new Rect(0, 0, imageSizeInPixels, imageSizeInPixels), 0, 0);
            RenderTexture.active = null;
            byte[] byteArray = renderedTexture.EncodeToPNG();

            // Save Texture
            System.IO.File.WriteAllBytes(Application.dataPath + "/Render Output/" + iconFileName + ".png", byteArray);

            // Reset Camera Target Texture
            iconCamera.targetTexture = null;

            // Refresh The Assets Folder
            AssetDatabase.Refresh();
        }
    }

    // A struct that stores the prefab that needs an icon and the file name of the icon
    [System.Serializable]
    public struct NamedGameObject
    {
        public string iconFileName;
        public GameObject iconObjectPrefab;
    }
}
#endif
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Rufas
{
    public class SoSceneReference : ScriptableObject
    {
        public AssetReference sceneReference;

        [Button]
        public void LoadScene()
        {
            RufasSceneManager.LoadScene(sceneReference);
        }
    }
}

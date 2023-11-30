using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Rufas
{
    public class LoadScene : MonoBehaviour
    {
        [OnValueChanged("CheckIfScene")]
        [SerializeField] private SoSceneReference sceneToLoad;

        [Button]
        public void Load()
        {
            RufasSceneManager.LoadScene(sceneToLoad.sceneReference);
        }

        private void CheckIfScene()
        {
#if UNITY_EDITOR
            if(sceneToLoad != null)
            {
                if(!sceneToLoad.sceneReference.editorAsset is SceneAsset)
                {
                    Debug.LogError("Only Scene Assets Can Be References In This Field!");
                    sceneToLoad = null;
                }
            }
#endif
        }
    }
}

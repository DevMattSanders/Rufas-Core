using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Rufas
{
    public class LoadScene : MonoBehaviour
    {
        [OnValueChanged("CheckIfScene")]
        [SerializeField]private AssetReference sceneToLoad;

        [Button]
        public void Load()
        {
            RufasSceneManager.LoadScene(sceneToLoad);
        }

        private void CheckIfScene()
        {
#if UNITY_EDITOR
            if(sceneToLoad != null)
            {
                Debug.Log(sceneToLoad.editorAsset.GetType());
            }
#endif
        }
    }
}

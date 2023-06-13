using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Rufas
{
    [CreateAssetMenu(menuName = "Rufas/Scene/SoSceneManager")]
    public class SceneManager : SuperScriptable
    {
        //Scene Manager/scene tools folder structure

        public static SceneManager instance;

        public SoScene firstScene;
        public SoScene loadingScene;

        public override void SoOnAwake()
        {
            base.SoOnAwake();

            instance = this;
        }


        public void LoadScene(SoScene sceneScriptableObject)
        {
            LoadScene(sceneScriptableObject.SceneName);

           
        }

        public void LoadScene(string sceneName)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
        }
    }
}

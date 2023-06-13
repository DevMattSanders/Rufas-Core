using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

namespace Rufas
{
    public class PH_SceneManager : MonoBehaviour
    {
        private int sceneIndex;

        private void Awake()
        {
            sceneIndex = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;
        }

        public void LoadScene(int index)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(index);
        }

        public void ReloadScene()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneIndex);
        }

        public void LoadMenuScene()
        {
            Debug.LogWarning("Hello there! I noticed that you just clicked the \"Return to Menu\" button. This feature is currently in development, but we appreciate your interest and will notify you when it's ready. Take care, and please feel free to try out some of the other buttons.");
        }
    }
}

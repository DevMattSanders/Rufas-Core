using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace Rufas
{
    public class PauseManager : MonoBehaviour
    {

        public static PauseManager Instance;

        [Header("Pause State")]
        [SerializeField, Required, InlineEditor] private BoolVariable gamePaused;

        public UnityEvent OnGamePaused;
        public UnityEvent OnGameResumed;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Debug.LogError("Second Pause Manager Found!");

                Destroy(gameObject);
            }
        }

        public void SetPauseState(bool paused)
        {
            gamePaused.Value = paused;

            if (paused == true)
            {
                OnGamePaused.Invoke();
            }
            else // paused == false
            {
                OnGameResumed.Invoke();
            }
        }

        [Button()]
        public void TogglePause()
        {
            Debug.Log("Toggle Pause Menu");
            if (gamePaused.Value) {
                SetPauseState(false);
            } else {
                SetPauseState(true);
            }
        }

        public void ReturnToMenu()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        }
    }
}

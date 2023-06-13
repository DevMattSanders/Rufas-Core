using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Rufas.VR {
    public class VRPlayerSpawner : MonoBehaviour
    {
        public static GameObject playerInstance;
        
        [SerializeField] private GameObject playerPrefab;
              

        [Header("Settings")]
        [Tooltip("If true, this will set the position and rotation for the current player instance when detected. Any newly created player will always be set to this point regardless of this setting")]
        public bool setExistingPlayerPositionAndRotation = true;

        [Tooltip("If true, this will add the newly created player to the DontDestroyOnLoad scene")]
        [ShowIf("playerPrefab")]
        public bool setNewPlayerAsDontDestroyOnLoad = true;

        [Tooltip("If true, this will destroy the current player instance and create a new one in the same frame. Useful for ensuring any parented grabbables are destroyed between scenes")]
        [ShowIf("playerPrefab")]
        public bool alwaysCreateNewPlayerInstance = false;
        [PropertyTooltip("If true, this will match the player instance to the playerPrefab field, therefore destorying it if the prefab value is null")]
        public bool removePlayerInstanceIfPrefabNull = false;
      

        private void Awake()
        {
            if(playerInstance == null)
            {
                CreateNewPlayer();
            }
        }

        private void CreateNewPlayer()
        {
            if (playerInstance != null && alwaysCreateNewPlayerInstance)
            {
                GameObject.Destroy(playerInstance);
            }

            if (playerPrefab == null && removePlayerInstanceIfPrefabNull && playerInstance != null)
            {
                GameObject.Destroy(playerInstance);
            }

            if (playerPrefab != null && playerInstance == null)
            {
                //Player doesn't exist and needs to be created
                playerInstance = GameObject.Instantiate(playerPrefab, transform.position, Quaternion.Euler(0f, transform.rotation.eulerAngles.y, 0f));

                if (setNewPlayerAsDontDestroyOnLoad)
                {
                    DontDestroyOnLoad(playerInstance);
                }
            }
            else if (playerInstance != null)
            {
                //Player instance already exists
                if (setExistingPlayerPositionAndRotation)
                {
                    playerInstance.transform.SetPositionAndRotation(transform.position, Quaternion.Euler(0f, transform.rotation.eulerAngles.y, 0f));
                }
            }
            
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            transform.rotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y, 0f);

            Handles.DrawWireDisc(transform.position,transform.up, 0.5f,2);

            Handles.DrawLine(transform.position, transform.position + transform.forward);

            Handles.DrawDottedLine(transform.position, transform.position + Vector3.up * 1.8f, 5);

        }
#endif

    }
}
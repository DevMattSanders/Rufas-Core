using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Rufas.VR {
    public class VRPlayerSpawner : MonoBehaviour
    {
        public static GameObject playerInstance;

        [SerializeField, HideInInspector]
        private bool useScriptable;

        [HorizontalGroup("H")]
        [ShowIf("useScriptable")]
        [HideLabel]
        [Required, SerializeField]
        private GameObjectVariable scriptable;

        [HorizontalGroup("H")]
        [HideIf("useScriptable")]
        [HideLabel]
        [Required, SerializeField]
        private GameObject prefab;

        [HorizontalGroup("H")]
        [HideIf("useScriptable")]
        [Button]
        private void UseScriptable()
        {
            useScriptable = true;
        }

        [HorizontalGroup("H")]
        [ShowIf("useScriptable")]
        [Button]
        private void UsePrefab()
        {
            useScriptable = false;
        }

        //  public GameObject playerPrefab;

        public static CodeEvent<Vector3,Quaternion> newPlayerPosRot;

        public static bool lastPositionSet = false;
        public static Vector3 lastPlayerPosition;
        public static Quaternion lastPlayerRotation;

        [Header("Settings")]
        [Tooltip("If true, this will set the position and rotation for the current player instance when detected. Any newly created player will always be set to this point regardless of this setting")]
        public bool setExistingPlayerPositionAndRotation = true;

        [Tooltip("If true, this will add the newly created player to the DontDestroyOnLoad scene")]
        [ShowIf("ShowNewPlayerOptions")]
        public bool setNewPlayerAsDontDestroyOnLoad = true;



        [Tooltip("If true, this will destroy the current player instance and create a new one in the same frame. Useful for ensuring any parented grabbables are destroyed between scenes")]
        [ShowIf("ShowNewPlayerOptions")]
        public bool alwaysCreateNewPlayerInstance = false;
        [PropertyTooltip("If true, this will match the player instance to the playerPrefab field, therefore destorying it if the prefab value is null")]
        public bool removePlayerInstanceIfPrefabNull = false;


        private bool ShowNewPlayerOptions()
        {
            if (useScriptable)
            {
                if (scriptable)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                if (prefab)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        private void Awake()
        {
            //Debug.Log("Heres");
            CreateOrMovePlayer();

        }

       // public 

        private void CreateOrMovePlayer()
        {

            GameObject playerToBuild = null;

            if (useScriptable)
            {
                playerToBuild = scriptable.value;
            }
            else
            {
                playerToBuild = prefab;
            }

            if(playerToBuild == null)
            {
                Debug.LogError("Player prefab or scriptable reference missing on player spawner!");
            }

            if (playerInstance != null && alwaysCreateNewPlayerInstance)
            {
                GameObject.Destroy(playerInstance);
            }

            if (playerToBuild == null && removePlayerInstanceIfPrefabNull && playerInstance != null)
            {
                GameObject.Destroy(playerInstance);
            }

            if (playerToBuild != null && playerInstance == null)
            {
                //Player doesn't exist and needs to be created
                playerInstance = GameObject.Instantiate(playerToBuild, transform.position, Quaternion.Euler(0f, transform.rotation.eulerAngles.y, 0f));
                              

                if (setNewPlayerAsDontDestroyOnLoad)
                {
                    DontDestroyOnLoad(playerInstance);
                }

               // playerInstance = playerInstance.transform.GetChild(playerInstance.transform.childCount - 1).gameObject;

            }
            else if (playerInstance != null)
            {
                //Player instance already exists
                if (setExistingPlayerPositionAndRotation)
                {
                   // Debug.Log("Set pos & rot");
                    playerInstance.transform.SetPositionAndRotation(transform.position, Quaternion.Euler(0f, transform.rotation.eulerAngles.y, 0f));                  
                }
            }

            if (setExistingPlayerPositionAndRotation)
            {
                newPlayerPosRot.Raise(transform.position, Quaternion.Euler(0f, transform.rotation.eulerAngles.y, 0f));

                lastPlayerPosition = transform.position;
                lastPlayerRotation = transform.rotation;

                lastPositionSet = true;
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
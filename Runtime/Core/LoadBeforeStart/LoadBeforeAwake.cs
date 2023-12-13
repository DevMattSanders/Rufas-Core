using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Rufas
{
    [CreateAssetMenu(menuName = "Rufas/LoadBeforeStart")]
    public class LoadBeforeAwake : ScriptableObject
    {
        //This list is located and instantiated by the TriggerLoadBeforeScene Class before the game in fully initialized.
        //All objects are stored in the DontDestroyOnLoad scene
        //All new objects are also given a message of 'OnCreatedBeforeStart' for if they have to run any special functionality themselves

        //public static List<LoadBeforeAwake> preAwakeLoaders = new List<LoadBeforeAwake>();

        //public BoolWithCallback finishedLoading = new BoolWithCallback(false);

        //[Obsolete]
        // public List<GameObject> ddolBeforeScene = new List<GameObject>();

        [ListDrawerSettings(ShowFoldout = false)]
        public List<AssetReferenceGameObject> addressablesToLoad = new List<AssetReferenceGameObject>();

        public static Transform loadBeforeStartParent;

       // public bool organizeInDdolAsManager = false;
        public void BehaviourToRunBeforeStart()
        {
           // finishedLoading.Value = false;
            //base.BehaviourToRunBeforeStart();
            /*
            if (organizeInDdolAsManager)
            {
                if (loadBeforeStartParent == null)
                {
                    loadBeforeStartParent = new GameObject("-- MANAGERS ABOVE --").transform;
                    DontDestroyOnLoad(loadBeforeStartParent.gameObject);
                    loadBeforeStartParent.SetAsFirstSibling();
                }
            }
            */

            foreach (AssetReferenceGameObject assetReference in addressablesToLoad)
            {
                //assetReference.is
                assetReference.LoadAssetAsync().Completed += createdObjectHandle =>
                {
                    GameObject createdObject = Instantiate(createdObjectHandle.Result);
                    DontDestroyOnLoad(createdObject);
                    createdObject.SendMessage("OnCreatedBeforeScene", SendMessageOptions.DontRequireReceiver);
                };
            }
            /*
            foreach(GameObject nextPrefab in ddolBeforeScene)
            {
                return;

                GameObject newGameobject = GameObject.Instantiate(nextPrefab);
                                
               
                DontDestroyOnLoad(newGameobject);

             //   if (organizeInDdolAsManager)
             //   {
             //       newGameobject.transform.SetAsFirstSibling();
             //   }
                newGameobject.SendMessage("OnCreatedBeforeScene", SendMessageOptions.DontRequireReceiver);
                
            }
            */
        }        
    }
}
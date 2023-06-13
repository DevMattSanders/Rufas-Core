using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rufas
{
    [CreateAssetMenu(menuName = "Rufas/LoadBeforeStart")]
    public class LoadBeforeStart : PreAwakeBehaviour
    {
        //This list is located and instantiated by the TriggerLoadBeforeScene Class before the game in fully initialized.
        //All objects are stored in the DontDestroyOnLoad scene
        //All new objects are also given a message of 'OnCreatedBeforeStart' for if they have to run any special functionality themselves

        public List<GameObject> ddolBeforeScene = new List<GameObject>();

        public static Transform loadBeforeStartParent;

        public override void BehaviourToRunBeforeStart()
        {
            base.BehaviourToRunBeforeStart();

            foreach(GameObject nextPrefab in ddolBeforeScene)
            {
                if (loadBeforeStartParent == null)
                {
                    loadBeforeStartParent = new GameObject("-- MANAGERS ABOVE --").transform;
                    DontDestroyOnLoad(loadBeforeStartParent.gameObject);
                    loadBeforeStartParent.SetAsFirstSibling();
                }

                GameObject newGameobject = GameObject.Instantiate(nextPrefab);

                
               
                DontDestroyOnLoad(newGameobject);

                newGameobject.transform.SetAsFirstSibling();
                
                newGameobject.SendMessage("OnCreatedBeforeScene", SendMessageOptions.DontRequireReceiver);
                
            }
        }        
    }
}
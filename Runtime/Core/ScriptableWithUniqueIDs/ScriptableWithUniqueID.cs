using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Rufas
{
    [CreateAssetMenu(menuName = "Rufas/GameContent/GameContentObject")]
    public class ScriptableWithUniqueID : ScriptableObjectWithCallbacks
    {
       

        //SuperScriptable
        [HorizontalGroup("TopLine", order: 0)]
        [ReadOnly, SerializeField]        
        private string uniqueID;
                
        public string UniqueID
        {
            get
            {
                return uniqueID;
            }
        }



#if UNITY_EDITOR

        [HideInInspector] public string proposed_NameValue;
        [HideInInspector] public string proposed_ID;
        [HideInInspector] public bool markedForDeletion;
        [HideInInspector] public bool IDAlreadyExists;

#endif

        [HorizontalGroup("TopLine", width: 30, order: 2)]
        [Button("R")]
        void RefreshButton()
        {
#if UNITY_EDITOR
            AuthorisedRefresh();
#endif
        }

        /*
        public void SetIDFromConfirmationWindow(string id, ScriptableIDConfirmation authorisingConfirmationWindow)
        {
            if(authorisingConfirmationWindow != null)
            {
                uniqueID = id;
                EditorUtility.SetDirty(this);
            }
        }
        */

        public void ManuallySetID_OnlyForDatabase(string newID, ScriptablesUniqueIDDatabase authorisingIDDatabase)
        {
#if UNITY_EDITOR
            if (authorisingIDDatabase != null)
            {
                uniqueID = newID;
                EditorUtility.SetDirty(this);
            }
#endif
        }

        public void AuthorisedRefresh()
        {
#if UNITY_EDITOR
            ScriptablesUniqueIDDatabase.Instance.RefreshDatabase();
#endif
        }

       
    }
}

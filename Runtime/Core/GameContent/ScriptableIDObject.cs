using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Rufas
{
    [CreateAssetMenu(menuName = "Rufas/GameContent/GameContentObject")]
    public class ScriptableIDObject : SuperScriptable
    {

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

        [HorizontalGroup("TopLine", width: 30, order: 2)]
        [Button("R")]
        void RefreshButton()
        {
            AuthorisedRefresh();
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

        public void ManuallySetID_OnlyForDatabase(string newID, ScriptableIDDatabase authorisingIDDatabase)
        {
            if (authorisingIDDatabase != null)
            {
                uniqueID = newID;
                EditorUtility.SetDirty(this);
            }
        }

        public void AuthorisedRefresh()
        {
            ScriptableIDDatabase.Instance.RefreshDatabase();
        }
#endif

    }
}

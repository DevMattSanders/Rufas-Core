using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Rufas
{
    [CreateAssetMenu(menuName = "Rufas/GameContent/GameContentObject")]
    [InitializeOnLoad]
    public class GameContentObject : SuperScriptable
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

        [ShowIf("AlwaysShow")]
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
            // ScriptableIDConfirmation.ShowWindow(this, newID, authorisingIDDatabase);
            if (authorisingIDDatabase != null)
            {
                uniqueID = newID;
                EditorUtility.SetDirty(this);
            }
        }

        public void AuthorisedRefresh()
        {
            ScriptableIDDatabase.Instance.RefreshReplicationKey(this);
        }

        private int counter = -1;
        private bool AlwaysShow()
        {
            //Debug.Log(counter);
            if (counter < 3)
            {
                counter++;
                if (counter >= 3)
                {
                    counter = 0;
                   // AuthorisedRefresh();
                }                
            }

            return true;
        }

#endif

    }
}

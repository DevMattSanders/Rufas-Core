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

        //[HorizontalGroup("TopLine", order: 0)]
        [ReadOnly, SerializeField]        
        public string uniqueID;


        /*
        public string UniqueID
        {
            get
            {
                return uniqueID;
            }
        }
        */

       // public string GetID()
       // {
        //    return uniqueID;
       // }

#if UNITY_EDITOR

        //[HorizontalGroup("TopLine", width: 100, order: 1)]
        //[ReadOnly,SerializeField,HideLabel] private GameContentObject myself;

        [HorizontalGroup("TopLine", width: 100, order: 1)]
        [ReadOnly, SerializeField, HideLabel] private int recordedInstanceID = -1;

        [ShowIf("AlwaysShow")]
        [HorizontalGroup("TopLine", width: 30, order: 2)]
        [Button("R")]
        void RefreshButton()
        {
            AuthorisedRefresh();
        }

      //  private void OnEnable()
        //{
        //    Debug.Log(this.name + " * " + uniqueID);
           // Debug.Log(this.name + " " + counter);
       // }


        public void ManuallySetID_OnlyForDatabase(string newID)
        {
            uniqueID = newID;
        }
        public void Refresh()
        {
            if (recordedInstanceID == -1)
            {
                AuthorisedRefresh();
            }
            else if (recordedInstanceID != this.GetInstanceID())
            {
                AuthorisedRefresh();
            }
        }

        public void AuthorisedRefresh(bool refreshAssetDatabase = true)
        {

            recordedInstanceID = this.GetInstanceID();

            EditorUtility.SetDirty(this);
            GameContentDatabase.Instance.RefreshReplicationKey(this);
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
                    //Debug.Log(name + " CREATED!");
                    Refresh();
                }                
            }

            return true;
        }

#endif

    }
}

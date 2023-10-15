using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rufas
{
    [SerializeField]
    public class DatabaseEntryInstance : MonoBehaviour
    {                
        public string uniqueID;
        
        public ApplicationPlayer owner;

        private event System.Action<object> onChangeFromDatabase;

        //Local enties
        public List<DatabaseEntry> entries;// = new List<DatabaseEntry>();

        public void RegisterToDatabase(Action listener)
        {            
            //Database.instance.RegisterDatabaseBridge();
                        
        }

        public void Destroy()
        {
            
        }

        public void DestroyFromApplicationPlayer()
        {

        }
    }
}

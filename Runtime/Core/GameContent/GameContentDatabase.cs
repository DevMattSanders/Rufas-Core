using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Rufas
{
    [CreateAssetMenu(menuName = "Rufas/GameContent/Database")]
    public class GameContentDatabase : SuperScriptable
    {
        public static GameContentDatabase instance;

        private static GameContentDatabase editorInstance;
        public static GameContentDatabase Instance
        {
            get
            {
                if (Application.isPlaying == false)
                {
#if UNITY_EDITOR
                    if (editorInstance == null)
                    {
                        editorInstance = RufasStatic.GetAllScriptables_ToList<GameContentDatabase>()[0];
                    }

                    return editorInstance;
#else
                return instance;
#endif
                }
                else
                {

                    return instance;
                }
            }
        }

        public List<DatabaseEntry> databaseEntries = new List<DatabaseEntry>();

        public override void SoOnAwake()
        {
            base.SoOnAwake();
            instance = this;
        }

        public void RegisterDatabaseEntry(DatabaseEntry entry)
        {

        }

        public void UnregisterDatabaseEntry(DatabaseEntry entry)
        {

        }


     //   public List<GameContentObject> gameContentObjects = new List<GameContentObject>();

        [PropertySpace(spaceBefore: 20)]

        [Header("Replication Objects")]
        [ReadOnly]
        public Dictionary<string, GameContentObject> replication_KeyToObject = new Dictionary<string, GameContentObject>();
       
        [ReadOnly]
        public Dictionary<GameContentObject, string> replication_ObjectToKey = new Dictionary<GameContentObject, string>();

        [FoldoutGroup("ReplicaitonObject History & Recovery")]
        [ReadOnly]
        public Dictionary<string, string> IDandNameRecord = new Dictionary<string, string>();


        [FoldoutGroup("ReplicaitonObject History & Recovery")]
        [Button]
        public void SyncFlippedDictionary()
        {
            GameContentObject[] allContent = RufasStatic.GetAllScriptables_ToArray<GameContentObject>();

            foreach(GameContentObject content in allContent)
            {
                content.AuthorisedRefresh();
            }

            //replication_KeyToObject.Clear();

          //  List<GameContentObject> objectsToRemove = new List<GameContentObject>();
            List<string> keysToRemove = new List<string>();

            foreach (string next in replication_KeyToObject.Keys)
            {
                if (replication_KeyToObject[next] == null)
                {
                    Debug.Log("Found destroyed objects. Adding to record");
                    if (replication_KeyToObject.ContainsKey(next))
                    {
                        if (IDandNameRecord.ContainsKey(next))
                        {
                            IDandNameRecord[next] = "-[Deleted]-" + IDandNameRecord[next];
                        }
                        else
                        {
                            Debug.Log("Next value lost");
                        }

                        keysToRemove.Add(next);
                    }
                }
                else
                {

                }
            }

            foreach(string key in keysToRemove)
            {
                replication_KeyToObject.Remove(key);
            }

            foreach (var key in replication_ObjectToKey.Keys.ToArray())
            {
                if (key == null)
                {
                    replication_ObjectToKey.Remove(key);
                }
            }

            // foreach(KeyValuePair<GameContentObject, string> next in replication_ObjectToKey)
            // {
            //  Debug.Log(next.Key);
            //    if(next.Key == null)
            //   {
            // Debug.Log("Is Null");
            /*
            if (replication_KeyToObject.ContainsKey(next.Value))
            {
                if (IDandNameRecord.ContainsKey(next.Value))
                {
                    IDandNameRecord[next.Value] = "-[Deleted]-" + IDandNameRecord[next.Value];

                }
                else
                {
                    Debug.Log("Next value lost");
                }

                replication_KeyToObject.Remove(next.Value);
            }
            */
            // }
            // else
            // {
            //   Debug.Log("Is Not Null");
            // }
            //  }

            /*
            foreach (KeyValuePair<GameContentObject, string> next in replication_ObjectToKey)
            {
                if (next.Key == null)
                {
                    Debug.Log(next.Key);
                    Debug.Log(next.Value);
                    if (replication_KeyToObject.ContainsKey(next.Value))
                    {
                        if (IDandNameRecord.ContainsKey(next.Value))
                        {
                            IDandNameRecord[next.Value] = "-[Deleted]-" + IDandNameRecord[next.Value];

                        }
                        else
                        {
                            Debug.Log("Next value lost");
                        }

                        replication_KeyToObject.Remove(next.Value);
                    }
                }
                else
                {
                    IDandNameRecord[next.Value] = next.Key.name;
                    
                }
            }
    */
            /*
          //  replication_KeyToObject.Clear();

            foreach (KeyValuePair<DatabaseReplicationObject, string> next in replication_ObjectToKey)
            {
                if (next.Key == null)
                {

                }
                else
                {
                   // replication_KeyToObject.Add(next.Value, next.Key);
                }
            }
            */
        }

        private void RemoveNullKeyEntries(Dictionary<object, string> inputDictionary)
        {
            List<object> keysToRemove = new List<object>();

            foreach (var kvp in inputDictionary)
            {
                if (kvp.Key == null)
                {
                    keysToRemove.Add(kvp.Key);
                }
            }

            foreach (var key in keysToRemove)
            {
                inputDictionary.Remove(key);
            }
        }

        public void RefreshReplicationKey(GameContentObject replicationObject)
        {
            /*
            if (replication_ObjectToKey.ContainsKey(replicationObject))
            {
                string readKey = replication_ObjectToKey[replicationObject];

               // Debug.Log(replicationObject.name + " * " + replicationObject.UniqueID + " * " + readKey);
                if (replicationObject.uniqueID != readKey)
                {
                    Debug.Log(replicationObject.uniqueID + "-|-" + readKey);

                    Debug.Log("Registered replication objects Key has changed since last check. Resetting back to database value...");
                    replicationObject.ManuallySetID_OnlyForDatabase(readKey);
                }

                if (replication_KeyToObject.ContainsKey(readKey))
                {
                    if (replication_KeyToObject[readKey] == replicationObject)
                    {
                        //All good
                        //Debug.Log(replicationObject.name + " | No issues!");
                    }
                    else
                    {
                        Debug.Log(replication_KeyToObject[readKey] + " " + replicationObject);
                        Debug.Log(replicationObject.name +  " * Registered replication objects Key has different value in flipped database. Resetting to primary database");
                        replication_KeyToObject[readKey] = replicationObject;
                    }
                }
                else
                {
                    Debug.Log(replicationObject.name + " * Registered replication objects Key is not present is flipped database. Adding...");
                    replication_KeyToObject.Add(readKey, replicationObject);
                }
            }
            else
            {
            */
            //Need to add it to the database.
            if (string.IsNullOrEmpty(replicationObject.uniqueID))
            {
                //if(gameContentObjects.Contains(repl))

                if (replication_ObjectToKey.ContainsKey(replicationObject))
                {
                    Debug.Log(replicationObject.name + " * Found existing ID in database via the object as a key");
                    replicationObject.ManuallySetID_OnlyForDatabase(replication_ObjectToKey[replicationObject]);
                }
                else
                {
                    Debug.Log(replicationObject.name + " * No ID, assigning");
                    //Not present in the primary database and has no unique ID. Most likely new.
                    AddToDatabase(replicationObject,true);
                }
            }else if (replication_ObjectToKey.ContainsKey(replicationObject))
            {
              
                if (replicationObject.uniqueID == replication_ObjectToKey[replicationObject])
                {
                    //all good.
                    Debug.Log(replicationObject.name + " * All Good, but refreshing against database anyway...");
                    AddToDatabase(replicationObject, false);
                }
                else
                {
                    Debug.Log(replicationObject.name + " * Exists in database but has wrong ID assigned. Assigning ID found in database");
                    replicationObject.uniqueID = replication_ObjectToKey[replicationObject];
                }
            }
            else if (replication_KeyToObject.ContainsKey(replicationObject.uniqueID))
            {
               // Debug.Log(replicationObject.uniqueID);

              //  Debug.Log(replication_KeyToObject[replicationObject.uniqueID].GetInstanceID() + " * " + replicationObject.GetInstanceID());
                if (replication_KeyToObject[replicationObject.uniqueID] == replicationObject)
                {                    
                    Debug.Log(replicationObject.name + " * Exists in Key database but not found in Object. Adding");
                    AddToDatabase(replicationObject, false);
                }
                else
                {
                    Debug.Log(replicationObject.name + " * Non registered objects key is found in flipped database. This is most likely a duplicated object! Giving new ID...");
                    AddToDatabase(replicationObject,true);
                }
                //replication_KeyToObject[replicationObject.UniqueID] = replicationObject;
            }
            else
            {
                Debug.Log(replicationObject.name + " * Very odd. Replication object has a Unique ID but is not found in replicaiton database or flipped vairent. Adding to database using its existing ID...");
                //Not present in the primary or secondary database and has no unique ID. Most likely new.
                AddToDatabase(replicationObject, false);
            }
            //}


            if (IDandNameRecord.ContainsKey(replicationObject.uniqueID))
            {
                IDandNameRecord[replicationObject.uniqueID] = replicationObject.name;
            }
            else
            {
                IDandNameRecord.Add(replicationObject.uniqueID, replicationObject.name);
                //  Debug.Log("Adding new value to ID and Name Record: " + replicationObject.name + " " + replicationObject.UniqueID);
            }

            AssetDatabase.Refresh();
        }

        private void AddToDatabase(GameContentObject replicationObject, bool giveNewID)
        {
            //Not present in the primary database and has no unique ID. Most likely new.
            if (giveNewID) { replicationObject.ManuallySetID_OnlyForDatabase(System.Guid.NewGuid().ToString()); }
            GameContentObject toAdd = replicationObject as GameContentObject;
           // Debug.Log(toAdd.name + " * " + toAdd.GetInstanceID());

            if (replication_ObjectToKey.ContainsKey(toAdd))
            {
                replication_ObjectToKey[toAdd] = toAdd.uniqueID;
            }
            else
            {
                replication_ObjectToKey.Add(toAdd, toAdd.uniqueID);
            }

            if (replication_KeyToObject.ContainsKey(toAdd.uniqueID))
            {
                replication_KeyToObject[toAdd.uniqueID] = toAdd;
            }
            else
            {
                replication_KeyToObject.Add(toAdd.uniqueID, toAdd);
            }

            EditorUtility.SetDirty(this);
          
           // gameContentObjects.Add(toAdd);

            //this.
        }
    }
}

using Rufas;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Rufas/Database/Database")]
public class Database : SuperScriptable
{
    public static Database instance;

    private static Database editorInstance;
    public static Database Instance
    {
        get
        {
            if (Application.isPlaying == false)
            {
#if UNITY_EDITOR
                if (editorInstance == null)
                {
                    editorInstance = RufasStatic.GetAllScriptables_ToList<Database>()[0];
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

    [PropertySpace(spaceBefore: 20)]
    [Header("Replicaiton Objects")]
    [ReadOnly]
    public Dictionary<GameContentObject,string> replication_ObjectToKey = new Dictionary<GameContentObject,string>();

    [ReadOnly]
    public Dictionary<string, GameContentObject> replication_KeyToObject = new Dictionary<string, GameContentObject>();


    [FoldoutGroup("ReplicaitonObject History & Recovery")]
    [ReadOnly]
    public Dictionary<string, string> IDandNameRecord = new Dictionary<string, string>();


   // [FoldoutGroup("ReplicaitonObject History & Recovery")]
   // [ReadOnly]
    //public List<string> previouslyUsedIDs = new List<string>();


    [FoldoutGroup("ReplicaitonObject History & Recovery")]
    [Button]
    public void SyncFlippedDictionary()
    {
        foreach(KeyValuePair<GameContentObject,string> next in replication_ObjectToKey)
        {
            if(next.Key == null)
            {
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
                /*
                if (previouslyUsedIDs.Contains(next.Key))
                {
                    previouslyUsedIDs.Remove(next.Key);
                }
                */
            }
        } 
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
        if (replication_ObjectToKey.ContainsKey(replicationObject))
        {
            string readKey = replication_ObjectToKey[replicationObject];

            if(replicationObject.UniqueID != readKey)
            {
                Debug.Log(replicationObject.UniqueID + "-|-" + readKey);

                Debug.Log("Registered replication objects Key has changed since last check. Resetting back to database value...");
                replicationObject.UniqueID = readKey;
            }

            if(replication_KeyToObject.ContainsKey(readKey))
            {
                if (replication_KeyToObject[readKey] == replicationObject)
                {
                    //All good
                    //Debug.Log(replicationObject.name + " | No issues!");
                }
                else
                {
                    Debug.Log("Registered replication objects Key has different value in flipped database. Resetting to primary database");
                    replication_KeyToObject[readKey] = replicationObject;
                }
            }
            else
            {
                Debug.Log("Registered replication objects Key is not present is flipped database. Adding...");
                replication_KeyToObject.Add(readKey, replicationObject);
            }
        }
        else
        {
            //Need to add it to the database.
            if (string.IsNullOrEmpty(replicationObject.UniqueID))
            {
                Debug.Log("No ID, assigning");
                //Not present in the primary database and has no unique ID. Most likely new.
                NewIDAndAddToReplicationDatabase(replicationObject);
            }
            else if (replication_KeyToObject.ContainsKey(replicationObject.UniqueID))
            {
                if (replication_KeyToObject[replicationObject.UniqueID] == replicationObject)
                {
                    //all good.
                }
                else
                {
                    Debug.Log("Non registered replication objects key is found in flipped database. This is most likely a duplicated object! Giving new ID...");
                    NewIDAndAddToReplicationDatabase(replicationObject);
                }
                //replication_KeyToObject[replicationObject.UniqueID] = replicationObject;
            }
            else
            {
                Debug.Log("Very odd. Replication object has a Unique ID but is not found in replicaiton database or flipped vairent. Giving ID and adding as new entry...");
                //Not present in the primary or secondary database and has no unique ID. Most likely new.
                NewIDAndAddToReplicationDatabase(replicationObject,false);
            }
        }


        if (IDandNameRecord.ContainsKey(replicationObject.UniqueID))
        {
            IDandNameRecord[replicationObject.UniqueID] = replicationObject.name;
        }
        else
        {
            IDandNameRecord.Add(replicationObject.UniqueID, replicationObject.name);

            Debug.Log("Adding new value to ID and Name Record: " + replicationObject.name + " " + replicationObject.UniqueID);
        }


    }

    private void NewIDAndAddToReplicationDatabase(GameContentObject replicationObject, bool giveNewID = true)
    {
        //Not present in the primary database and has no unique ID. Most likely new.
        if (giveNewID) { replicationObject.UniqueID = Guid.NewGuid().ToString(); }
        replication_ObjectToKey.Add(replicationObject, replicationObject.UniqueID);
        replication_KeyToObject.Add(replicationObject.UniqueID, replicationObject);
    }
}

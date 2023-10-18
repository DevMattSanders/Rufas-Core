using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.VersionControl;
using UnityEngine;

namespace Rufas
{
    [CreateAssetMenu(menuName = "Rufas/GameContent/Database")]
    public class ScriptableIDDatabase : SuperScriptable
    {
        public static ScriptableIDDatabase instance;
        private static ScriptableIDDatabase editorInstance;
        public static ScriptableIDDatabase Instance
        {
            get
            {
                if (Application.isPlaying == false)
                {
#if UNITY_EDITOR
                    if (editorInstance == null)
                    {
                        editorInstance = RufasStatic.GetAllScriptables_ToList<ScriptableIDDatabase>()[0];
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
                
        [PropertySpace(spaceBefore: 20)]

        [Header("Editor Game Content Objects")]
        [ReadOnly]
        public Dictionary<string, GameContentObject> gameContentObjects_KeyToObject = new Dictionary<string, GameContentObject>();
       
        [ReadOnly]
        public Dictionary<GameContentObject, string> gameContentObjects_ObjectToKey = new Dictionary<GameContentObject, string>();

        [FoldoutGroup("Editor Game Content Objects History & Recovery")]
        [ReadOnly]
        public Dictionary<string, string> IDAndNameRecord = new Dictionary<string, string>();


        [FoldoutGroup("Editor Game Content Objects History & Recovery")]
        [Button]
        public void SyncFlippedDictionary()
        {
            GameContentObject[] allContent = RufasStatic.GetAllScriptables_ToArray<GameContentObject>();

            foreach(GameContentObject content in allContent)
            {
                content.AuthorisedRefresh();
            }

            List<string> keysToRemove = new List<string>();

            foreach (string next in gameContentObjects_KeyToObject.Keys)
            {
                if (gameContentObjects_KeyToObject[next] == null)
                {
                    Debug.Log("Found destroyed objects. Adding to record");
                    if (gameContentObjects_KeyToObject.ContainsKey(next))
                    {
                        if (IDAndNameRecord.ContainsKey(next))
                        {
                            IDAndNameRecord[next] = "-[Deleted]-" + IDAndNameRecord[next];
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
                gameContentObjects_KeyToObject.Remove(key);
            }

            foreach (var key in gameContentObjects_ObjectToKey.Keys.ToArray())
            {
                if (key == null)
                {
                    gameContentObjects_ObjectToKey.Remove(key);
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
            if (string.IsNullOrEmpty(replicationObject.UniqueID))
            {
                //if(gameContentObjects.Contains(repl))

                if (gameContentObjects_ObjectToKey.ContainsKey(replicationObject))
                {
                    Debug.Log(replicationObject.name + " * Found existing ID in database via the object as a key");
                    replicationObject.ManuallySetID_OnlyForDatabase(gameContentObjects_ObjectToKey[replicationObject],this);

                    UpdateIDRecord(replicationObject);
                }
                else
                {
                    Debug.Log(replicationObject.name + " * No ID, assigning");
                    //Not present in the primary database and has no unique ID. Most likely new.
                    AddToDatabaseViaConfirmationWindow(replicationObject,true);
                }
            }else if (gameContentObjects_ObjectToKey.ContainsKey(replicationObject))
            {
              
                if (replicationObject.UniqueID == gameContentObjects_ObjectToKey[replicationObject])
                {
                    if (!gameContentObjects_KeyToObject.ContainsKey(replicationObject.UniqueID))
                    {
                        Debug.Log(replicationObject.name + " * Exists in Object database correctly but not in Key. Adding...");
                        AddToDatabase(replicationObject);
                    }
                    else
                    {
                        //All good!
                    }                   
                }
                else
                {
                    Debug.Log(replicationObject.name + " * Exists in database but has wrong ID assigned. Assigning ID found in database");
                    replicationObject.ManuallySetID_OnlyForDatabase(gameContentObjects_ObjectToKey[replicationObject], this);

                    UpdateIDRecord(replicationObject);
                }
            }
            else if (gameContentObjects_KeyToObject.ContainsKey(replicationObject.UniqueID))
            {
                if (gameContentObjects_KeyToObject[replicationObject.UniqueID] == replicationObject)
                {                    
                    Debug.Log(replicationObject.name + " * Exists in Key database but not found in Object. Adding...");
                    AddToDatabase(replicationObject);
                }
                else
                {
                    Debug.Log(replicationObject.name + " * Non registered objects key is found in flipped database. This is most likely a duplicated object! Giving new ID...");
                    AddToDatabaseViaConfirmationWindow(replicationObject,true);
                }
            }
            else
            {
                Debug.Log(replicationObject.name + " * Very odd. Replication object has a Unique ID but is not found in replicaiton database or flipped vairent. Adding to database using its existing ID...");
                //Not present in the primary or secondary database and has no unique ID. Most likely new.
                AddToDatabaseViaConfirmationWindow(replicationObject,false);
            }
            //}


                    
        }

        private void AddToDatabaseViaConfirmationWindow(GameContentObject replicationObject, bool giveNewID)
        {
            //Not present in the primary database and has no unique ID. Most likely new.
            if (giveNewID) { replicationObject.ManuallySetID_OnlyForDatabase(System.Guid.NewGuid().ToString(),this); } //Pass in that a message needs to be passed back to 
            GameContentObject toAdd = replicationObject as GameContentObject;

            ScriptableIDConfirmation.ShowWindow(toAdd, toAdd.UniqueID, this);

            /*
            if (gameContentObjects_ObjectToKey.ContainsKey(toAdd))
            {
                gameContentObjects_ObjectToKey[toAdd] = toAdd.UniqueID;
            }
            else
            {
                gameContentObjects_ObjectToKey.Add(toAdd, toAdd.UniqueID);
            }

            if (gameContentObjects_KeyToObject.ContainsKey(toAdd.UniqueID))
            {
                gameContentObjects_KeyToObject[toAdd.UniqueID] = toAdd;
            }
            else
            {
                gameContentObjects_KeyToObject.Add(toAdd.UniqueID, toAdd);
            }

            EditorUtility.SetDirty(this);
            AssetDatabase.Refresh();
            */
        }

      
        public bool CheckIfIDExists(string idToCheck)
        {
            return gameContentObjects_KeyToObject.ContainsKey(idToCheck);            
        }

        public void PassToDatabaseFromAuthorisedConfirmationWindow(string newID, string nameID, GameContentObject objectToAdd, ScriptableIDConfirmation authorisingConfirmationWindow)
        {
            if(authorisingConfirmationWindow != null)
            {
                bool saveAssets = false;
                if(objectToAdd.name != nameID)
                {
                    string assetPath = AssetDatabase.GetAssetPath(objectToAdd.GetInstanceID());
                    AssetDatabase.RenameAsset(assetPath, nameID);
                    saveAssets = true;
                }
                               
                objectToAdd.ManuallySetID_OnlyForDatabase(newID, this);

                AddToDatabase(objectToAdd,saveAssets);              
            }
        }

        private void AddToDatabase(GameContentObject objectToAdd, bool saveAssets = false)
        {
            if (gameContentObjects_ObjectToKey.ContainsKey(objectToAdd))
            {
                gameContentObjects_ObjectToKey[objectToAdd] = objectToAdd.UniqueID;
            }
            else
            {
                gameContentObjects_ObjectToKey.Add(objectToAdd, objectToAdd.UniqueID);
            }

            if (gameContentObjects_KeyToObject.ContainsKey(objectToAdd.UniqueID))
            {
                gameContentObjects_KeyToObject[objectToAdd.UniqueID] = objectToAdd;
            }
            else
            {
                gameContentObjects_KeyToObject.Add(objectToAdd.UniqueID, objectToAdd);
            }

            UpdateIDRecord(objectToAdd);

            EditorUtility.SetDirty(this);
            AssetDatabase.Refresh();

            if (saveAssets)
            {
                AssetDatabase.SaveAssets();
            }
        }

        private void UpdateIDRecord(GameContentObject objectToUpdate)
        {
            if (IDAndNameRecord.ContainsKey(objectToUpdate.UniqueID))
            {
                IDAndNameRecord[objectToUpdate.UniqueID] = objectToUpdate.name;
            }
            else
            {
                IDAndNameRecord.Add(objectToUpdate.UniqueID, objectToUpdate.name);
            }
        }
    }
}

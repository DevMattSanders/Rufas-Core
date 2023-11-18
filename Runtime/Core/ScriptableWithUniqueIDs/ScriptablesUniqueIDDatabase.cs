using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;

namespace Rufas
{
#if UNITY_EDITOR
    [InitializeOnLoad]
#endif
    [CreateAssetMenu(menuName = "Rufas/GameContent/Database")]

    public class ScriptablesUniqueIDDatabase : SuperScriptable
    {
        private static ScriptablesUniqueIDDatabase instance;
        private static ScriptablesUniqueIDDatabase editorInstance;
        public static ScriptablesUniqueIDDatabase Instance
        {
            get
            {
                if (Application.isPlaying == false)
                {

                    if (editorInstance == null)
                    {
#if UNITY_EDITOR
                        editorInstance = RufasStatic.GetAllScriptables_ToList<ScriptablesUniqueIDDatabase>()[0];
#endif
                    }

                    return editorInstance;

                    //#else
                    //return instance;

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
        public Dictionary<string, ScriptableWithUniqueID> gameContentObjects_KeyToObject = new Dictionary<string, ScriptableWithUniqueID>();
       
        [ReadOnly]
        public Dictionary<ScriptableWithUniqueID, string> gameContentObjects_ObjectToKey = new Dictionary<ScriptableWithUniqueID, string>();

        private void OnEnable()
        {
            RefreshEditorDatabase();
        }

        public override void SoOnAwake()
        {
            base.SoOnAwake();
            RefreshEditorDatabase();
        }

        private void RefreshEditorDatabase()
        {
#if UNITY_EDITOR
            RefreshDatabase();
#endif
        }



#if UNITY_EDITOR
        [FoldoutGroup("Editor Game Content Objects History & Recovery")]
        [ReadOnly]
        public Dictionary<string, string> IDAndNameRecord = new Dictionary<string, string>();

        [PropertySpace(spaceBefore: 10)]
        [Button]
        public void RefreshDatabase()
        {
            ScriptableWithUniqueID[] allContent = RufasStatic.GetAllScriptables_ToArray<ScriptableWithUniqueID>();

            potentialNewObjects.Clear();
            potentialExistingObjectsThatNeedAdding.Clear();
            potentialDuplications.Clear();

            Debug.Log("Refreshing Database");

            foreach(ScriptableWithUniqueID content in allContent)
            {
                RefreshReplicationKey(content, true);
                //content.AuthorisedRefresh();
            }

            int totalCount = potentialNewObjects.Count + potentialExistingObjectsThatNeedAdding.Count + potentialDuplications.Count;

            if(totalCount == 0)
            {

            }
            else
            {
                OpenBuldConfirmationWindow();
            }
            
            //Sorting record list and removing null values
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

        public List<ScriptableWithUniqueID> potentialNewObjects = new List<ScriptableWithUniqueID>();
        public List<ScriptableWithUniqueID> potentialExistingObjectsThatNeedAdding = new List<ScriptableWithUniqueID>();
        public List<ScriptableWithUniqueID> potentialDuplications = new List<ScriptableWithUniqueID>();

        public void RefreshReplicationKey(ScriptableWithUniqueID replicationObject, bool buildingBulkConfirmationList = false)
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
                    //Not present in the primary database and has no unique ID. Most likely new.
                    if (buildingBulkConfirmationList)
                    {
                        potentialNewObjects.Add(replicationObject);
                        replicationObject.proposed_NameValue = replicationObject.name;
                        replicationObject.proposed_ID = System.Guid.NewGuid().ToString();
                    }
                    else
                    {
                        Debug.Log(replicationObject.name + " * No ID, assigning");
                        AddToDatabaseViaConfirmationWindow(replicationObject, true);
                        
                    }
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
                 
                    if (buildingBulkConfirmationList)
                    {
                        potentialDuplications.Add(replicationObject);
                        replicationObject.proposed_NameValue = replicationObject.name;
                        replicationObject.proposed_ID = System.Guid.NewGuid().ToString();
                    }
                    else
                    {
                        Debug.Log(replicationObject.name + " * Non registered objects key is found in flipped database. This is most likely a duplicated object! Giving new ID...");
                        AddToDatabaseViaConfirmationWindow(replicationObject, true);
                    }
                }
            }
            else
            {
                //Debug.Log(replicationObject.name + " * Very odd. Replication object has a Unique ID but is not found in replicaiton database or flipped vairent");//. Adding to database using its existing ID...");
                //Not present in the primary or secondary database and has no unique ID. Most likely new.
                if (buildingBulkConfirmationList)
                {
                    potentialExistingObjectsThatNeedAdding.Add(replicationObject);
                    replicationObject.proposed_NameValue = replicationObject.name;
                    replicationObject.proposed_ID = replicationObject.UniqueID;
                }
                else
                {
                    AddToDatabaseViaConfirmationWindow(replicationObject, false);
                }
            }      
        }

        private void AddToDatabaseViaConfirmationWindow(ScriptableWithUniqueID replicationObject, bool giveNewID)
        {
            //Not present in the primary database and has no unique ID. Most likely new.
            if (giveNewID) { replicationObject.ManuallySetID_OnlyForDatabase(System.Guid.NewGuid().ToString(),this); } //Pass in that a message needs to be passed back to 
            ScriptableWithUniqueID toAdd = replicationObject as ScriptableWithUniqueID;
//#if UNITY_EDITOR
            ScriptableIDConfirmationWindow.ShowWindow(toAdd, toAdd.UniqueID, this);
//#endif
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

        private void OpenBuldConfirmationWindow()
        {
//#if UNITY_EDITOR
            BulkScriptableIDConfirmationWindow.ShowWindow(this);
//#endif
        }

        public void PassToDatabaseFromBulkConfirmationWindow()
        {
            foreach (ScriptableWithUniqueID next in potentialNewObjects)
            {
                PassToDatabaseFromAuthorisedConfirmationWindow(next.proposed_ID, next.proposed_NameValue, next, false);
            }

            foreach (ScriptableWithUniqueID next in potentialExistingObjectsThatNeedAdding)
            {
                PassToDatabaseFromAuthorisedConfirmationWindow(next.proposed_ID, next.proposed_NameValue, next, false);
            }

            foreach (ScriptableWithUniqueID next in potentialDuplications)
            {
                PassToDatabaseFromAuthorisedConfirmationWindow(next.proposed_ID, next.proposed_NameValue, next, false);
            }

            EditorUtility.SetDirty(this);
            AssetDatabase.Refresh();
            AssetDatabase.SaveAssets();

        }
      
        public bool CheckIfIDExists(string idToCheck)
        {
            return gameContentObjects_KeyToObject.ContainsKey(idToCheck);            
        }

        public void PassToDatabaseFromAuthorisedConfirmationWindow(string newID, string nameID, ScriptableWithUniqueID objectToAdd, bool refreshDatabaseNow = true)
        {
           // if(authorisingConfirmationWindow != null)
           // {
                bool saveAssets = false;
                if(objectToAdd.name != nameID)
                {
                    string assetPath = AssetDatabase.GetAssetPath(objectToAdd.GetInstanceID());
                    AssetDatabase.RenameAsset(assetPath, nameID);
                    saveAssets = true;
                }
                               
                objectToAdd.ManuallySetID_OnlyForDatabase(newID, this);

            if (refreshDatabaseNow)
            {
                AddToDatabase(objectToAdd, saveAssets);
                RefreshDatabase();
            }
            else
            {
                AddToDatabase(objectToAdd, refreshDatabaseNow);
            }
          //  }
        }

        private void AddToDatabase(ScriptableWithUniqueID objectToAdd, bool saveAssets = false)
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
                    
            

            if (saveAssets)
            {
                EditorUtility.SetDirty(this);
                AssetDatabase.Refresh();
                AssetDatabase.SaveAssets();
            }

           // RefreshDatabase();
        }

        private void UpdateIDRecord(ScriptableWithUniqueID objectToUpdate)
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
#endif
    }

}

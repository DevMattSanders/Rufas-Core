
using JetBrains.Annotations;
using Rufas;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameSaveManager : GameSystem<GameSaveManager>
{
    public static CodeEvent<GameSaveFile, SaveTypeToken> SaveFileCollecting;
    public static CodeEvent<GameSaveFile, SaveTypeToken> SaveFileLoading;
    public static CodeEvent saveFilesRefreshed;



    [SerializeField] private string saveFolder = "GameSaves";
    [SerializeField] private GameObject thumbnailPrefab;    //Replace with addressable asset reference later!
    [SerializeField] private Texture2D defaultThumbnail;

    public Dictionary<string, SaveFilePointer> pointersByFileName = new Dictionary<string, SaveFilePointer>();

    public List<SaveFilePointer> localDeviceFiles = new List<SaveFilePointer>();

    //Ignore this for now
    public List<SaveFilePointer> ugcContentFiles = new List<SaveFilePointer>();

    public List<UnityEngine.Object> delaySavingWhilstAnyInList = new List<UnityEngine.Object>();

    //Current saving instance
    [BoxGroup("Save Instance")][SerializeField,ReadOnly] private bool savingNow = false;
    [BoxGroup("Save Instance")][SerializeField, ReadOnly] private string currentFileName;
    [BoxGroup("Save Instance")][SerializeField, ReadOnly] private SaveTypeToken currentToken;
    [BoxGroup("Save Instance")][SerializeField, ReadOnly] private string currentScreenshotID;
    [BoxGroup("Save Instance")][ShowInInspector,ReadOnly] public string currentThumbnailName { get; private set; }

    public override string DesiredName()
    {
        return "Game Save Manager";
    }

    public override void PreInitialisationBehaviour()
    {
        base.PreInitialisationBehaviour();
        savingNow = false;
        ClearCurrentSaveStamp();
        localDeviceFiles.Clear();
    }

    public override void OnStartBehaviour()
    {
        base.OnStartBehaviour();
        RefreshSavePointers();
    }

    public override void EndOfApplicaitonBehaviour()
    {
        base.EndOfApplicaitonBehaviour();
        savingNow = false;
        ClearCurrentSaveStamp();
        localDeviceFiles.Clear();
    }

    private void ClearCurrentSaveStamp()
    {
        currentFileName = "";
        currentToken = null;
        currentScreenshotID = "";
        currentThumbnailName = "";
    }

    

    [Button]
    public void SaveNow(string fileName,SaveTypeToken token, bool useScreenshot)
    {
        if(savingNow || token == null || string.IsNullOrEmpty(fileName)) return;

        savingNow = true;

        currentFileName = fileName;
        currentToken = token;

        string fileNameWithNoSpaces =  (currentFileName + "-" + DateTime.Now).Replace(" ", "").Replace("/","").Replace(":","");

        currentThumbnailName = "Thumbnails/" + fileNameWithNoSpaces;//Short GUID thing here?//;

        if (thumbnailPrefab == null || useScreenshot == false)
        {
            PostThumnail("");
        }
        else
        {
            //Replace with addressable asset reference later!
            //This system generates a thumbnail before calling PostThumnail
            GameObject thumbnailGenerator = GameObject.Instantiate(thumbnailPrefab);
        }
    }

    public void PostThumnail(string _screenshotID)
    {      
        currentScreenshotID = _screenshotID;

        //Method to check if any files aleady exist with this name?    
        if (ES3.KeyExists(currentFileName, filePath: saveFolder +"/"+ currentToken.additionalSavePath + "/" + currentFileName))
        {
            //No popup yet so just silently save over for now
            CoroutineMonoBehaviour.i.StartCoroutine(ConfirmSave());
        }
        else
        {
            CoroutineMonoBehaviour.i.StartCoroutine(ConfirmSave());
        }
    }

    private IEnumerator ConfirmSave()
    {
       
            GameSaveFile saveContainer = new GameSaveFile();
                        
            SaveFileCollecting.Raise(saveContainer, currentToken);

        while (delaySavingWhilstAnyInList.Count > 0)
        {
            for (int i = 0; i < delaySavingWhilstAnyInList.Count; i++)
            {
                if (delaySavingWhilstAnyInList[i] == null)
                {
                    delaySavingWhilstAnyInList.RemoveAt(i);
                }
            }

            yield return null;
        }

        //*? maybe a system here to lock the rest of the method whilst various systems are async placing data into the save container?
        //A list of 'holders' that won't progress this script unless it's empty?

        try
        {

            //Now create or update the actual save pointer
            SaveFilePointer pointer = null;

            if (pointersByFileName.ContainsKey(currentFileName))
            {
                // pointersByFileName[currentFileName].UpdateValues(currentFileName, currentToken.UniqueID, DateTime.Now.ToString(), currentScreenshotID);
                // pointersByFileName[currentFileName].thumbnailID = currentScreenshotID;
                pointersByFileName.Remove(currentFileName);
            }
          //  else
          //  {
                pointer = new SaveFilePointer(currentFileName, currentToken.UniqueID, DateTime.Now.ToString(), currentScreenshotID);
                pointersByFileName.Add(currentFileName, pointer);
          //  }

            //ES3.se
            //Saving a save pointer so that I can later access the name, thumbnail and
            //dateTime without having to load the full save file
            ES3.Save(currentFileName, pointer, filePath: "SavePointers");

            var settings = new ES3Settings();// ES3.CompressionType.Gzip);

            string filePath = saveFolder + "/" + currentToken.additionalSavePath + "/" + currentFileName;

            //settings
            //settings.com


            //Save the full save file!
            ES3.Save(currentFileName, saveContainer, filePath, settings);

            outputJson = ES3.LoadRawString(filePath, settings);

            //var settings = ;
            ES3.SaveRaw(outputJson,"TempLoaded", new ES3Settings(ES3.Location.Cache));

            file = ES3.Load<GameSaveFile>(currentFileName, "TempLoaded", new ES3Settings(ES3.Location.Cache));

            //file = JsonUtility.FromJson<GameSaveFile>(outputJson);

            Debug.Log(file.dataKeyValues.Count);

            foreach(KeyValuePair<string,string> next in file.dataKeyValues)
            {
                Debug.Log(next.Key + " | " + next.Value);
            }

            RefreshSavePointers();
            ClearCurrentSaveStamp();
            savingNow = false;

        }
        catch (Exception e) 
        {
            Debug.Log("Error! " + e);
            RefreshSavePointers();
            ClearCurrentSaveStamp();
            savingNow = false;
        }
    }

    [TextArea(minLines: 20, maxLines: 50)]
    public string outputJson;

    public GameSaveFile file;

    [Button]
    private void RefreshSavePointers()
    {
        localDeviceFiles.Clear();

        if (ES3.FileExists("SavePointers"))
        {
            string[] pointersInMemory = ES3.GetKeys("SavePointers");

            foreach (string key in pointersInMemory)
            {
                if (ES3.KeyExists(key, filePath: "SavePointers"))
                {
                    try
                    {
                        SaveFilePointer loaded = ES3.Load<SaveFilePointer>(key, filePath: "SavePointers");
                        if (loaded != null)
                        {
                            localDeviceFiles.Add(loaded);
                        }
                    }
                    catch (Exception e)
                    {
                        Debug.LogError($"Error loading SaveFilePointer for key {key}: {e.Message}");
                    }
                }
            }
        }

        saveFilesRefreshed.Raise();
    }

    public void LoadByFileName(string fileName, SaveTypeToken token)
    {
        RefreshSavePointers();

        //JsonUtilityArrays
        //JsonUtility.ToJson()

        SaveFilePointer foundPointer = null;

        foreach(SaveFilePointer pointer in localDeviceFiles)
        {
            if(string.Compare(fileName,pointer.fileName) == 0)
            {
                if (string.Compare(pointer.saveTypeToken, token.UniqueID) == 0)
                {
                    foundPointer = pointer;
                    break;
                }
            }            
        }

        if(foundPointer != null)
        {
            Load(foundPointer,token);
        }
    }

    public void Load(SaveFilePointer saveFilePointer,SaveTypeToken token)
    {        
        if (ES3.KeyExists(saveFilePointer.fileName, filePath: saveFolder + "/" + token.additionalSavePath + "/" + saveFilePointer.fileName))
        {
            GameSaveFile file = ES3.Load<GameSaveFile>(saveFilePointer.fileName,filePath: saveFolder + "/" + token.additionalSavePath + "/" + saveFilePointer.fileName);
        
            if (file != null)
            {
                SaveFileLoading.Raise(file, token);
            }
            else
            {
                Debug.Log("Pointer pointed to non existent save file!");
            }
        }
    }
}
//
// 

[Serializable]
public class GameSaveFile
{
    public Dictionary<string, string> dataKeyValues = new Dictionary<string, string>();
    //public List<string> data = new List<string>();
    public void Add(string key, string val)
    {
        dataKeyValues.Add(key, val);
       // data.Add(key + "," + value);
    }    

  //  public bool ContainsKey(string key)
   // {
        //Way to confirm here from data?
   // }
}

//A data container class used to make note of local save files without actually loading their
//content from the device. Useful for generating lists of content taht could be loaded if requested
[Serializable]
public class SaveFilePointer
{
    public SaveFilePointer(string _fileName, string _saveTypeToken, string _dateTime, string _thumbnailID)
    {     
        fileName = _fileName;
        saveTypeToken = _saveTypeToken;
        dateTime = _dateTime;
        thumbnailID = _thumbnailID;
    }  
    
    //The name used to find the actual save file (GameSaveFile)
    [HideLabel,ReadOnly,HorizontalGroup("LineOne")] public string fileName;
    //Unique ID for finding the save type token used for this save
    [HideLabel, ReadOnly, HorizontalGroup("LineOne")] public string saveTypeToken;
    //Used to store when the save was made
    [HideLabel, ReadOnly, HorizontalGroup("LineTwo")] public string dateTime;
    //Used to find the thumbnail from another system
    [HideLabel, ReadOnly, HorizontalGroup("LineTwo")] public string thumbnailID;

    [HorizontalGroup("LineTwo")]
    [Button]
    public void Load()
    {
        GameSaveManager.Instance.Load(this, (SaveTypeToken)ScriptablesUniqueIDDatabase.Instance.gameContentObjects_KeyToObject[saveTypeToken]);
    }

   // public void Delete()
  //  {
        //GameSaveManager.Instance.Delete(this,)
       // GameSaveManager.Instance.pointersByFileName.Remove(fileName);
       // GameSaveManager.Instance
   // }
}

/*
    UpdateValues(_fileName, _saveTypeToken, _dateTime, _thumbnailID);
  
  public void UpdateValues(string _fileName, string _saveTypeToken, string _dateTime, string _thumbnailID)
    {
       
        if (!string.IsNullOrEmpty(thumbnailID))
        {
            ScreenshotUtility.Instance.DeleteScreenshot(thumbnailID);
        }

       
    }
 */

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
    public static CodeEvent<SaveFile, SaveTypeToken> SaveFileCollecting;
    public static CodeEvent<SaveFile, SaveTypeToken> SaveFileLoading;
    public static CodeEvent saveFilesRefreshed;



    [SerializeField] private string saveFolder = "GameSaves";
    [SerializeField] private GameObject thumbnailPrefab;    //Replace with addressable asset reference later!
    [SerializeField] private Texture2D defaultThumbnail;

    Dictionary<string, SaveFilePointer> pointersByFileName = new Dictionary<string, SaveFilePointer>();

    public List<SaveFilePointer> localDeviceFiles = new List<SaveFilePointer>();

    //Ignore this for now
    public List<SaveFilePointer> ugcContentFiles = new List<SaveFilePointer>();



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
        if (ES3.KeyExists(currentFileName, filePath: saveFolder + currentToken.additionalSavePath))
        {
            //No popup yet so just silently save over for now
            ConfirmSave();
        }
        else
        {
            ConfirmSave();
        }
    }

    private void ConfirmSave()
    {
        try
        {
            SaveFile saveContainer = new SaveFile();
                        
            SaveFileCollecting.Raise(saveContainer, currentToken);

            //*? maybe a system here to lock the rest of the method whilst various systems are async placing data into the save container?
            //A list of 'holders' that won't progress this script unless it's empty?


            //Now create or update the actual save pointer
            SaveFilePointer pointer = null;

            if (pointersByFileName.ContainsKey(currentFileName))
            {
               // pointersByFileName[currentFileName].UpdateValues(currentFileName, currentToken.UniqueID, DateTime.Now.ToString(), currentScreenshotID);
                pointersByFileName[currentFileName].thumbnailID = currentScreenshotID;
            }
            else
            {
                pointer = new SaveFilePointer(currentFileName, currentToken.UniqueID, DateTime.Now.ToString(), currentScreenshotID);
                pointersByFileName.Add(currentFileName, pointer);
            }          

            //Saving a save pointer so that I can later access the name, thumbnail and
            //dateTime without having to load the full save file
            ES3.Save(currentFileName, pointer, filePath: "SavePointers");

            //Save the full save file!
            ES3.Save(currentFileName, saveContainer, filePath: saveFolder + currentToken.additionalSavePath);

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
                    /*
                    SaveFilePointer loaded = ES3.Load<SaveFilePointer>(key, filePath: "SavePointers");

                    if (loaded != null)
                    {
                        localDeviceFiles.Add(loaded);
                    }
                    */
                }
            }
        }

       // fileByName = ES3.GetFiles("GameSavesTrackFiles"); //What do I need to write here?

        

        saveFilesRefreshed.Raise();
    }

    public void LoadByFileName(string fileName, SaveTypeToken token)
    {
        RefreshSavePointers();
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

   // public string[] fileByName;

    [SerializeField]
    public Byte[] brokenDown;
    public void Load(SaveFilePointer saveFilePointer,SaveTypeToken token)
    {     
    

        if (ES3.KeyExists(saveFilePointer.fileName, filePath: saveFolder + token.additionalSavePath))
        {
            SaveFile file = ES3.Load<SaveFile>(saveFilePointer.fileName,filePath: saveFolder + token.additionalSavePath);
        //    string byString = ES3.LoadString(saveFilePointer.fileName,null, filePath: saveFolder + token.additionalSavePath);

            //ES3.Load()
            brokenDown = ES3.Serialize<SaveFile>(file);

            ES3.Save<byte[]>("BrokenDown", brokenDown, filePath: "BrokenDown");

           // Debug.Log(byString);

            if (file != null)
            {
                SaveFileLoading.Raise(file, token);
                /* Debuging loaded data
                Debug.Log("Loading!");
                Debug.Log(saveFilePointer.fileName);
                Debug.Log(saveFilePointer.dateTime);
                Debug.Log(saveFilePointer.thumbnailID);

                foreach (KeyValuePair<string, string> next in file.dataKeyValues)
                {
                    Debug.Log(next.Key + " | " + next.Value);
                }
                */
            }
            else
            {
                Debug.Log("Pointer pointed to non existent save file!");
            }
        }
    }
}

[Serializable]
public class SaveFile
{
    

    public Dictionary<string, string> dataKeyValues = new Dictionary<string, string>();
    public void Add(string key,string value)
    {        
        dataKeyValues.Add(key, value);
    }
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
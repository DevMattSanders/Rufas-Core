
using JetBrains.Annotations;
using Rufas;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class GameSaveManager : GameSystem<GameSaveManager>
{
 //   public static CodeEvent<SaveFile,FileCategory>

  //  public static CodeEvent<FileChunk,SaveFileType> SaveFileCollecting;

  //  public static CodeEvent<FileTypeID, FileChunk,SaveFileType> SaveFileLoading;
    public static CodeEvent saveFilesRefreshed;

    /*
    public class FileTypeID
    {
        public FileTypeID(string _id)
        {
            id = _id;
        }

        public bool Compare(FileTypeID file)
        {
            if (String.Compare(file.ID, ID) == 0)
            {
                return true;
            }
            return false;
        }

        [SerializeField, ReadOnly]
        private string id;
        public string ID
        {
            get
            {
                return id;
            }
        }
    }
    */

    //public ES3Settings settings;// = new ES3Settings();

    public ES3.CompressionType compressionType;

    [SerializeField] private string saveFolder = "GameSaves";
    [SerializeField] private GameObject thumbnailPrefab;    //Replace with addressable asset reference later!
    [SerializeField] private Texture2D defaultThumbnail;



    //public List<>
    // public Dictionary<string, SaveFileHeader> saveFilesByName = new Dictionary<string, SaveFileHeader>();
    public Dictionary<string, SaveFile> saveFilesByID = new Dictionary<string, SaveFile>();
    
  //  public List<SaveFile> localDeviceFiles = new List<SaveFile>();

    //Ignore this for now
    //public List<SaveFile> ugcContentFiles = new List<SaveFile>();

    public List<UnityEngine.Object> delaySavingWhilstAnyInList = new List<UnityEngine.Object>();

    //Current saving instance
    [BoxGroup("Save Instance")][SerializeField,ReadOnly] private bool savingNow = false;
    [BoxGroup("Save Instance")][SerializeField, ReadOnly] private string currentFileName;
    [BoxGroup("Save Instance")][SerializeField, ReadOnly] private string currentFileID;
    [BoxGroup("Save Instance")][SerializeField, ReadOnly] private SaveFileType currentSaveFileType;
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
        //  localDeviceFiles.Clear();
        onDevice.Clear();
        saveFilesByID.Clear();
    }

    public override void OnStartBehaviour()
    {
        base.OnStartBehaviour();
        RefreshSavePointers();
        onDevice.Clear();
        saveFilesByID.Clear();
    }

    public override void EndOfApplicaitonBehaviour()
    {
        base.EndOfApplicaitonBehaviour();
        savingNow = false;
        ClearCurrentSaveStamp();
       // localDeviceFiles.Clear();
    }

    private void ClearCurrentSaveStamp()
    {
        currentFileName = "";
        currentFileID = ""; 
        currentSaveFileType = null;
        currentScreenshotID = "";
        currentThumbnailName = "";
    }

    

    [Button]
    public void SaveNow(string fileName,SaveFileType saveFileType, bool useScreenshot)
    {
        if(savingNow || saveFileType == null || string.IsNullOrEmpty(fileName)) return;

        savingNow = true;

        currentFileName = fileName;
        currentFileID = GenerateShortId();// System.Guid.NewGuid().ToString().Replace("-", "");
        currentSaveFileType = saveFileType;

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
    private static readonly System.Random random = new System.Random();
    public static string GenerateShortId()
    {
        Debug.Log(DateTime.UtcNow.Ticks);
        long timestamp = DateTime.UtcNow.Ticks;
        byte[] randomBytes = new byte[4];
        random.NextBytes(randomBytes);

        byte[] combinedBytes = new byte[12];
        Array.Copy(BitConverter.GetBytes(timestamp), combinedBytes, 8);
        Array.Copy(randomBytes, 0, combinedBytes, 8, 4);

        string shortId = BitConverter.ToString(combinedBytes).Replace("-", "");
        return shortId;
    }
    public void PostThumnail(string _screenshotID)
    {      
        currentScreenshotID = _screenshotID;

        //Method to check if any files aleady exist with this name?    
        if (ES3.KeyExists(currentFileName, filePath: saveFolder +"/"+ currentSaveFileType.UniqueID + "/" + currentFileName))
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
        SaveFile saveFile = new SaveFile(new SaveFileHeader(currentFileName, currentFileID, currentSaveFileType.UniqueID,
            DateTime.Now.ToString(),
            currentScreenshotID));
        //FileChunk saveContainer = new FileChunk();

        // SaveFileCollecting.Raise(saveContainer.data, currentSaveFileType);

        SaveFile.SaveFileSaving.Raise(saveFile);

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
            //  SaveFileHeader pointer = null;

            //  if (pointersByFileName.ContainsKey(currentFileName))
            //  {
            // pointersByFileName[currentFileName].UpdateValues(currentFileName, currentToken.UniqueID, DateTime.Now.ToString(), currentScreenshotID);
            // pointersByFileName[currentFileName].thumbnailID = currentScreenshotID;
            //     pointersByFileName.Remove(currentFileName);
            // }
            //  else
            //  {
            //  pointer = new FileHeader(currentFileName, currentSaveFileType.UniqueID, DateTime.Now.ToString(), currentScreenshotID);
            // saveFilesByName.Add(currentFileName, pointer);
            //  }

            //ES3.se
            //Saving a save pointer so that I can later access the name, thumbnail and
            //dateTime without having to load the full save file
            ES3Settings settings = new ES3Settings();
            settings.typeChecking = false;



            string filePath = FileSavePath(saveFile.header.timeStamp,saveFile.header.savefileID, saveFile.header.saveFileType);
            Debug.Log(filePath);
            

            foreach(KeyValuePair<string, string> saveData in saveFile.data)
            {
                ES3.Save<string>(saveData.Key, saveData.Value, filePath);
            }

            ES3.Save<SaveFileHeader>("Header", saveFile.header, filePath);

            outputJson = ES3.LoadRawString(filePath);
            //Clues here for how to convert to JSON upload!
            /*
            //

           // var tempSettings = new ES3Settings();// ES3.CompressionType.Gzip);
           // string filePath = saveFolder + "/" + currentSaveFileType.additionalSavePath + "/" + currentFileName;            
            //Save the full save file!
            ES3.Save(currentFileName, saveContainer, filePath, tempSettings);            
            //writer.w
            outputJson = ES3.LoadRawString(filePath, tempSettings);                        
            //var settings = ;
            ES3.SaveRaw(outputJson,"TempLoaded", new ES3Settings(ES3.Location.Cache));
            file = ES3.Load<SaveFileData>(currentFileName, "TempLoaded", new ES3Settings(ES3.Location.Cache));
            //file = JsonUtility.FromJson<GameSaveFile>(outputJson);
            */

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

    public SaveFile file;

    [Button]
    public void RefreshSavePointers()
    {
       // localDeviceFiles.Clear();
        onDevice.Clear();
        saveFilesByID.Clear();
        foreach (SaveFileType nextSaveType in SaveFileType.saveFileTypes)
        {
            SaveFileTypesOnDevice saveTypeOnDevice = new SaveFileTypesOnDevice();
            onDevice.Add(saveTypeOnDevice);
            saveTypeOnDevice.saveFileType = nextSaveType;

            string[] fileNames = ES3.GetFiles(saveFolder + "/" + nextSaveType.UniqueID + "/");
            Debug.Log(fileNames.Length);
            foreach (string fileName in fileNames)
            {
               // Debug.Log(fileName);
                string directoryFilePath = saveFolder + "/" + nextSaveType.UniqueID + "/" + fileName;
                SaveFileHeader header = ES3.Load<SaveFileHeader>("Header", filePath: directoryFilePath);
                SaveFile saveFile = new SaveFile(header);
                saveFile.directoryFilePath = directoryFilePath;
                saveTypeOnDevice.saveFiles.Add(saveFile);
                saveFilesByID.Add(saveFile.header.savefileID, saveFile);
            }
        }
        //string[] directories = ES3.GetDirectories();
        //ES3.GetFiles()

        saveFilesRefreshed.Raise();
        return;
        if (ES3.FileExists("SavePointers"))
        {
            string[] pointersInMemory = ES3.GetKeys("SavePointers");

            foreach (string key in pointersInMemory)
            {
                if (ES3.KeyExists(key, filePath: "SavePointers"))
                {
                    try
                    {
                        SaveFileHeader loaded = ES3.Load<SaveFileHeader>(key, filePath: "SavePointers");
                        if (loaded != null)
                        {
            //               localDeviceFiles.Add(loaded);
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

    /*
    public void LoadByFileName(string fileName, FileToken token)
    {
        RefreshSavePointers();

        //JsonUtilityArrays
        //JsonUtility.ToJson()

        FileHeader foundPointer = null;

        foreach(FileHeader pointer in localDeviceFiles)
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
    */

    public List<SaveFileTypesOnDevice> onDevice = new List<SaveFileTypesOnDevice>();

    //public 
    [Serializable]
    public class SaveFileTypesOnDevice
    {
        public SaveFileType saveFileType;
        public List<SaveFile> saveFiles = new List<SaveFile>();
    }


    public void Load(SaveFile saveFile)// FileHeader saveFilePointer,FileCategory token)
    {
        saveFile.data.Clear();
       // string filePath = FileSavePath(saveFile.header.timeStamp,saveFile.header.savefileID,saveFile.header.saveFileType);
        if (ES3.FileExists(saveFile.directoryFilePath))
        {           
            string[] keys = ES3.GetKeys(saveFile.directoryFilePath);

            foreach(string key in keys)
            {
                if (string.Equals(key,"Header")) continue;

                string loadedValue = ES3.Load<string>(key, saveFile.directoryFilePath, "#NO#VALUE#FOUND#");

                if(string.Equals(loadedValue, "#NO#VALUE#FOUND#"))
                {
                    Debug.LogError($"KeyNotFoundInSaveFile:{key}");
                    continue;
                }

                saveFile.data.Add(key, loadedValue);                
            }

            SaveFile.SaveFileLoading.Raise(saveFile);
        }        
        
        /*
        if (ES3.KeyExists(saveFilePointer.fileName, filePath: saveFolder + "/" + token.additionalSavePath + "/" + saveFilePointer.fileName))
        {
            FileChunk file = ES3.Load<FileChunk>(saveFilePointer.fileName,filePath: saveFolder + "/" + token.additionalSavePath + "/" + saveFilePointer.fileName);
        
            if (file != null)
            {
                SaveFileLoading.Raise(file,file.id, token);
            }
            else
            {
                Debug.Log("Pointer pointed to non existent save file!");
            }            
        }     
        */
    }

    public void Delete(SaveFile saveFile)
    {
        ES3.DeleteFile(saveFile.directoryFilePath);// FileSavePath(saveFile.header.timeStamp, saveFile.header.savefileID, saveFile.header.saveFileType));

        RefreshSavePointers();
    }

    public string FileSavePath(string timeStamp, string fileID, string saveFileTypeID)
    {
        return CleanupFileName(saveFolder) + "/"  + CleanupFileName(saveFileTypeID) + "/" + CleanupFileName(timeStamp) + "-" + CleanupFileName(fileID);
    }

    private string CleanupFileName(string fileName)
    {
        return fileName.Replace("/", "").Replace(":","").Replace(" ","");
    }
}
//
// 

//Generated - not saved
[Serializable]
public class SaveFile
{
    public SaveFile(SaveFileHeader _header)
    {
        header = _header;
    }

    public static CodeEvent<SaveFile> SaveFileSaving;
    public static CodeEvent<SaveFile> SaveFileLoading;

    public string directoryFilePath;

    public SaveFileHeader header;
    [ShowInInspector]
    public Dictionary<string, string> data = new Dictionary<string, string>();
        

    public void AddData(string key, string value)
    {
        if (data.ContainsKey(key))
        {
            data[key] = value;
        }
        else
        {
            data.Add(key, value);
        }
    }

    public bool ValueExists(string key, out string value)
    {
        if (data.ContainsKey(key))
        {
            value = data[key];
            return true;// (true, data[key]);
        }
        else
        {
            value = null;
            return false;// (false, null); // or any other appropriate value for non-existence
        }
    }

    [Button]
    public void Load()
    {
        GameSaveManager.Instance.Load(this);
    }

    [Button]
    public void Delete()
    {
        GameSaveManager.Instance.Delete(this);
    }
}
/*
//The object that gets saved and loaded by ES3. This helps a little with future proofing as all
//the games data is just a collection of save chunks, each with a string-string dictionary
[Serializable]
public class SaveFileChunk
{
    public Dictionary<string, string> data = new Dictionary<string, string>();

    public void AddData(string key, string value)
    {
        if (data.ContainsKey(key))
        {
            data[key] = value;
        }
        else
        {
            data.Add(key, value);
        }
    }

    public (bool,string) ValueExists(string key)
    {
        if (data.ContainsKey(key))
        {
            return (true, data[key]);
        }
        else
        {
            return (false, null); // or any other appropriate value for non-existence
        }
    }
}
*/

//A data container class used to make note of local save files without actually loading their
//content from the device. Useful for generating lists of content taht could be loaded if requested
[Serializable]
public class SaveFileHeader
{
    public SaveFileHeader(string _saveFileName, string _saveFileID, string _saveFileType, string _timeStamp, string _thumbnailID)
    {     
        savefileName = _saveFileName;
        savefileID = _saveFileID;
        saveFileType = _saveFileType;
        timeStamp = _timeStamp;
        thumbnailID = _thumbnailID;
    }       

    [HideLabel, ReadOnly, HorizontalGroup("LineZero")] public string savefileName;
    [HideLabel, ReadOnly, HorizontalGroup("LineOne")] public string savefileID;
    [HideLabel, ReadOnly, HorizontalGroup("LineTwo")] public string saveFileType;
    [HideLabel, ReadOnly, HorizontalGroup("LineTwo")] public string timeStamp;
    [HideLabel, ReadOnly, HorizontalGroup("LineTwo")] public string thumbnailID;
}
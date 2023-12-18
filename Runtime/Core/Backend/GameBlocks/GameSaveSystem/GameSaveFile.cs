using Rufas;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameSaveFile
{
    public string fileName;
    public DateTime dateTime;

    public object saveData;
    public string thumnailID;

     private bool savingLockedWhilstCompletingProcess = false;
     private bool saveLocallyNext = false;

    [Button]
    public void SetFileData(string dataToSave,bool captureNewScreenshot = false, bool _saveLocallyNext = false)
    {
        if (savingLockedWhilstCompletingProcess) return;

        saveLocallyNext = false;

        saveData = dataToSave;
        dateTime = DateTime.Now;

        if (captureNewScreenshot)
        {
            CaptureScreenshot();
        }
        else
        {
            PostScreenshot();
        }
    }

    [Button]
    public void CaptureScreenshot()
    {
        if (savingLockedWhilstCompletingProcess) return;

        savingLockedWhilstCompletingProcess = true;
        string thumnailID = System.Guid.NewGuid().ToString();

        ScreenshotUtility.Instance.CaptureScreenshot("GameSaves/"+thumnailID,(Texture2D texture) =>
        {
            savingLockedWhilstCompletingProcess = false;
            if (texture != null)
            {
                PostScreenshot();
            }
            else
            {
                
            }
        });
    }

    private void PostScreenshot()
    {
        if (saveLocallyNext)
        {
            //string json = SaveFile            
        }
    }
}

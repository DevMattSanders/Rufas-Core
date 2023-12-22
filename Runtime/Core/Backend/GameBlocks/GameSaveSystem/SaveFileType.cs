using Rufas;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Used to identify what the rest of the project should do. This is for custom behaviours.
/// For example, a SaveTypeToken called SlotCarTrack will be passed into the GameSaveManager
/// in the Save method. The manager will then use the same token to alert the rest of the project.
/// The SlotCarTrackSaveLoadTool will pick up on this event, check the token is the same as
/// SlotCarTrack and then pass in the correct data needed to save a track. The same will happen in 
/// reverse when loading
/// </summary>

[CreateAssetMenu(menuName = "Rufas/SaveLoad/SaveTypeToken")]
public class SaveFileType : ScriptableWithUniqueID
{
    public static List<SaveFileType> saveFileTypes = new List<SaveFileType>();

    public override void SoOnAwake()
    {
        base.SoOnAwake();
        saveFileTypes.Add(this);
    }

    /*
    public static string GetPathFromToken(SaveFileType category)
    {
        if(string.IsNullOrEmpty(category.additionalSavePath) || string.IsNullOrWhiteSpace(category.additionalSavePath))
        {
            return "";
        }
        else
        {
            return category.additionalSavePath;
        }
    }
    public string additionalSavePath;
    */
}
using Rufas;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[Serializable]
public class SaveFile
{
    public static CodeEvent<SaveFile, SaveTypeToken> Loaded;
    public static CodeEvent<SaveFile,SaveTypeToken> Saving;

    public static Metadata LoadMetadata(string fileName)
    {
        if (File.Exists(fileName))
        {
            using (StreamReader reader = new StreamReader(fileName))
            {
                string json = reader.ReadToEnd();
                Metadata metadata = JsonUtility.FromJson<Metadata>(json);
                return metadata;
            }
        }
        else
        {
            Debug.LogError($"File not found: {fileName}");
            return null;
        }
        //return JsonUtility.FromJson<Metadata>(fileName);
    }

    public static SaveFile LoadSaveFile(string fileName)
    {
        return JsonUtility.FromJson<SaveFile>(fileName);
    }

    public Metadata metadata; 

    [SerializeField]
    public Dictionary<string,Section> sections = new Dictionary<string,Section>();


    public bool Contains(string key, out Section val)
    {
        if (sections.ContainsKey(key))
        {
            val = sections[key];
            return true;
        }
        else
        {
            val = null;
            return false;
        }
    }

    public void Add(string key,Section section,bool replaceIfExists = false)
    {
        if (!sections.ContainsKey(key))
        {
            sections.Add(key, section);
        }
        else if (replaceIfExists) //Probably dangerous to use this
        {
            sections[key] = section;
        }
        else
        {
            Debug.LogError("Multiple values added by same value name in save file!");
        }        
    }

    [Serializable]
    public class Section
    {
        public Dictionary<string, string> sectionData = new Dictionary<string, string>();
    }

    [Serializable]
    public class Metadata
    {
        public string name;
        public string saveTypeID;
        public string dateTime;
        public string thumbnailID;
    }
}

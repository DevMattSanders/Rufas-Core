#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.PackageManager.UI;
using UnityEditor.VersionControl;
using UnityEngine;

namespace Rufas
{
    public class ScriptableIDConfirmationWindow : EditorWindow
    {
        public ScriptableIDDatabase database;
        public ScriptableIDObject IDObject;
        public string nameValue;
        public string idValue;
        
        public static void ShowWindow(ScriptableIDObject _IDObject, string _givenID, ScriptableIDDatabase _database)
        {
            ScriptableIDConfirmationWindow window = null;

            
                window = (ScriptableIDConfirmationWindow)GetWindow(typeof(ScriptableIDConfirmationWindow), true, "Create New: " + _IDObject.GetType().ToString(), true);
            

            window.IDObject = _IDObject;
            window.nameValue = _IDObject.name;
            window.idValue = _givenID;
            window.database = _database;
            window.minSize = new Vector2(300, 150);
            if (window != null)
            {
                window.ShowModal();
            }
        }

        //private void Setup

        private void OnGUI()
        {
            //Add a scriptable object reference here

            EditorGUI.BeginDisabledGroup(true);
            IDObject = (ScriptableIDObject)EditorGUILayout.ObjectField("Object:", IDObject,typeof(ScriptableIDObject),false);
            EditorGUI.EndDisabledGroup();

            GUILayout.Label("Confirm Name");
            nameValue = EditorGUILayout.TextField(nameValue);
            GUILayout.Label("Confirm ID");
            idValue = EditorGUILayout.TextField(idValue);

            GUILayout.FlexibleSpace();

            GUILayout.BeginHorizontal();

            bool alreadyExists = database.CheckIfIDExists(idValue);

            if (alreadyExists)
            {
                GUI.backgroundColor = Color.red;
                GUILayout.Label("ID Already Exists In Database");
                GUI.backgroundColor = Color.white;
            }
            else
            {
                GUI.backgroundColor = Color.green;
                if (GUILayout.Button("Confirm"))
                {
                    database.PassToDatabaseFromAuthorisedConfirmationWindow(idValue, nameValue, IDObject);
                    Close();
                }
            }

            GUI.backgroundColor = Color.red;
            if (GUILayout.Button("Delete"))
            {
                Selection.activeObject = null;
                AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(IDObject));
                Close();
            }

            GUI.backgroundColor = Color.grey;
            if (GUILayout.Button("Ignore"))
            {
                Close();
            }

            GUI.backgroundColor = Color.white;

            GUILayout.EndHorizontal();
        }
    }
}
#endif
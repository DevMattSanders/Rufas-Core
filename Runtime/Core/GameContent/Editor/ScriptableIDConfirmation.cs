using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.PackageManager.UI;
using UnityEditor.VersionControl;
using UnityEngine;

namespace Rufas
{
    public class ScriptableIDConfirmation : EditorWindow
    {
        public ScriptableIDDatabase database;
        public GameContentObject IDObject;
        public string nameValue;
        public string idValue;
        
        public static void ShowWindow(GameContentObject _IDObject, string _givenID, ScriptableIDDatabase _database)
        {
            ScriptableIDConfirmation window = null;

            //GetWindow<ScriptableIDConfirmation>(true, "S", true);

            if (_IDObject == null)
            {
                window = (ScriptableIDConfirmation)GetWindow(typeof(ScriptableIDConfirmation), true, "Create New: " + _IDObject.GetType().ToString(), true);
            }
            else
            {
                window = (ScriptableIDConfirmation)GetWindow(typeof(ScriptableIDConfirmation), true, "Create New: " + _IDObject.GetType().ToString(), true);
            }

            window.IDObject = _IDObject;
            window.nameValue = _IDObject.name;
            window.idValue = _givenID;
            window.database = _database;
            window.minSize = new Vector2(300, 150);
           // window.ShowAuxWindow();
            window.ShowModal();
        }

        private void OnGUI()
        {
            //Add a scriptable object reference here

            EditorGUI.BeginDisabledGroup(true);
            IDObject = (GameContentObject)EditorGUILayout.ObjectField("Object:", IDObject,typeof(GameContentObject),false);
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
                    database.PassToDatabaseFromAuthorisedConfirmationWindow(idValue, nameValue, IDObject, this);
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

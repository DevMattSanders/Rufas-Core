#if UNITY_EDITOR
using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.PackageManager.UI;
using UnityEditor.VersionControl;
using UnityEngine;

namespace Rufas
{
    public class BulkScriptableIDConfirmationWindow : EditorWindow
    {

        public static BulkScriptableIDConfirmationWindow confirmationWindow;
        public static void ShowWindow(ScriptableIDDatabase _database)
        {
            /*
            BulkScriptableIDConfirmationWindow window = (BulkScriptableIDConfirmationWindow)EditorWindow.GetWindow(
        typeof(BulkScriptableIDConfirmationWindow), true, "Bulk Confirmation: " + _database.GetType().ToString(), true);
            */
            if (confirmationWindow == null)
            {
                confirmationWindow = (BulkScriptableIDConfirmationWindow)GetWindow(typeof(BulkScriptableIDConfirmationWindow), true, "Bulk Confirmation: " + _database.GetType().ToString(), true);
            }
            else
            {
                Debug.Log("Called open twice");
            }

            confirmationWindow.database = _database;
            confirmationWindow.minSize = new Vector2(300, 450);
            if (confirmationWindow != null)
            {
                confirmationWindow.ShowAuxWindow();
            }
        }

        //public void Setup

        public ScriptableIDDatabase database;

        private void OnGUI()
        {
            GUILayout.Space(10);

            IndividualGroup("New Objects Found: ", database.potentialNewObjects);//,
                //potentialNewObjects_NameValue, potentialNewObjects_IDValue,
               // ref potentialNewObjects_Index);

            GUILayout.Space(10);

            IndividualGroup("Existing Objects Found: ", database.potentialExistingObjectsThatNeedAdding);//,
                //potentialExistingObjects_NameValue, potentialExistingObjects_IDValue,
               // ref potentialExistingObjects_Index);

            GUILayout.Space(10);

            IndividualGroup("Duplicates Found: ", database.potentialDuplications);//,
                //potentialDuplications_NameValue, potentialDuplications_IDValue,
               // ref potentialDuplications_Index);

            GUILayout.FlexibleSpace();

            GUILayout.BeginHorizontal();

            GUI.backgroundColor = Color.gray;
            if (GUILayout.Button("Ignore & Close"))
            {
                Close();
            }

            GUI.backgroundColor = new Color(0.5597f,0.9056f,0.3708f);//Blue
            if (GUILayout.Button("Confirm & Update Database"))
            {
                Close();
                database.PassToDatabaseFromBulkConfirmationWindow();              
            }

           

            GUILayout.EndHorizontal();
        }

        private void IndividualGroup(string titleText, List<ScriptableIDObject> reference)//, ref int currentIndex)
        {
            

            if (reference.Count == 0)
            {
                GUIStyle titleStyle = new GUIStyle(GUI.skin.label);
                titleStyle.fontSize = 16; // Set the font size to make it larger
                titleStyle.fontStyle = FontStyle.Bold; // Make the text bold
                titleStyle.normal.textColor = Color.gray;

                GUILayout.Label(titleText + reference.Count, titleStyle);
            }
            else
            {

                GUIStyle titleStyle = new GUIStyle(GUI.skin.label);
                titleStyle.fontSize = 16; // Set the font size to make it larger
                titleStyle.fontStyle = FontStyle.Bold; // Make the text bold
                titleStyle.normal.textColor = Color.blue;

                GUILayout.Label(titleText + reference.Count, titleStyle);

                for (int i = 0; i < reference.Count; i++)
                {



                    ScriptableIDObject IDObject = reference[i];


                    GUILayout.BeginHorizontal();

                    EditorGUI.BeginDisabledGroup(true);
                    IDObject = (ScriptableIDObject)EditorGUILayout.ObjectField("", IDObject, typeof(ScriptableIDObject), false);
                    EditorGUI.EndDisabledGroup();
                    GUILayout.Label("Confirm Name");
                    IDObject.proposed_NameValue = EditorGUILayout.TextField(IDObject.proposed_NameValue);
                    GUILayout.Label("Confirm ID");
                    IDObject.proposed_ID = EditorGUILayout.TextField(IDObject.proposed_ID);

                    bool alreadyExists = database.CheckIfIDExists(IDObject.proposed_ID);

                    if (alreadyExists)
                    {
                        IDObject.IDAlreadyExists = true;
                        GUI.backgroundColor = new Color(0.8584f,0.3278f,0.2705f);//Red
                        GUILayout.Label("ID Already Exists In Database");
                        GUI.backgroundColor = Color.white;
                    }
                    else
                    {
                        IDObject.IDAlreadyExists = false;                      

                        GUI.backgroundColor = Color.white;
                      

                        if (IDObject.markedForDeletion)
                        {
                            GUI.backgroundColor = new Color(0.3845f, 0.5820f, 0.7358f);//Blue
                            if (GUILayout.Button("Keep"))
                            {
                                IDObject.markedForDeletion = false;
                            }
                        }
                        else
                        {
                            GUI.backgroundColor = new Color(0.8584f, 0.3278f, 0.2705f); //Red
                            if (GUILayout.Button("Delete?"))
                            {
                                IDObject.markedForDeletion = true;
                            }
                        }
                        GUI.backgroundColor = Color.white;

                        /*
                        GUI.backgroundColor = Color.white;
                        if (currentIndex == (reference.Count - 1)) EditorGUI.BeginDisabledGroup(true);

                        if (GUILayout.Button(">"))
                        {
                            currentIndex++;
                        }

                        if (currentIndex == (reference.Count - 1)) EditorGUI.EndDisabledGroup();
                        */

                    }
                    GUILayout.EndHorizontal();
                }

            }
        }
    }
}
#endif
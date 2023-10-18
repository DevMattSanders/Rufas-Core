using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Rufas
{
    [InitializeOnLoad]
    public class IDObjectSelectionChanged
    {
        static IDObjectSelectionChanged()
        {
            Selection.selectionChanged += OnSelectionChanged;
        }

        private static void OnSelectionChanged()
        {
            // Code to execute when the selection changes
            if (Selection.activeObject != null)
            {
              //  Debug.Log("Selected Object: " + Selection.activeObject.name);
                if(Selection.activeObject is GameContentObject)
                {
                    GameContentObject gameContentObject = (GameContentObject)Selection.activeObject;

                    gameContentObject.AuthorisedRefresh();
                }
            }
            else
            {
              //  Debug.Log("No object selected.");
            }
        }
    }
}

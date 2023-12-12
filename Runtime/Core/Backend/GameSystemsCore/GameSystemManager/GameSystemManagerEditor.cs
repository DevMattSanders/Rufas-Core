using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor;
using Sirenix.OdinInspector.Editor;
#endif
using System.IO;

namespace Rufas
{
#if UNITY_EDITOR
    public class GameSystemManagerEditor : OdinMenuEditorWindow
    {
        [MenuItem("Rufas/Game System Manager")]
        private static void OpenWindow()
        {
            GetWindow<GameSystemManagerEditor>().Show();
        }

        protected override OdinMenuTree BuildMenuTree()
        {
            var tree = new OdinMenuTree();

            GameSystemManager[] managers = RufasStatic.GetAllScriptables_ToArray<GameSystemManager>();

            GameSystemManager foundManager = null;


            if(managers.Length == 0)
            {
                foundManager = ScriptableObject.CreateInstance<GameSystemManager>();

                Directory.CreateDirectory("Assets/Rufas");
                string path = "Assets/Rufas/GameSystemManager.asset";
                AssetDatabase.CreateAsset(foundManager, path);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();

                tree.Add("+", foundManager);
            }else if (managers.Length > 1)
            {
                Debug.LogError("MULTIPLE GameSystemManager Scriptable Objects Found!");
                return null;
            }
            else
            {
                foundManager = managers[0];
                tree.Add("+", foundManager);
            }

            foundManager.RefreshGameSystems();

            var sortedList = foundManager.gameSystems.OrderBy(obj => obj.DesiredPath()).ToList();

            foreach (GameSystemParentClass next in sortedList)
            {
                // if (next.showInManager)
                // {
                // if (next.IsRufasSystem())// && next.AutogenerateGameSystem())
                // {
                //     tree.Add("Rufas Core Systems/" + next.DesiredPath(), next, next.EditorIcon());
                // }
                // else
                // {

                // GUIContent iconContent = EditorGUIUtility.IconContent(next.GetType().ToString() + " Icon");
                // string desiredPath = next.DesiredPath();
                if (!foundManager.hiddenGameSystems.Contains(next))
               {
                    string[] split = next.DesiredPath().Split("/");
                    tree.Add(
                        //next.DesiredPath().Replace("/", "-"),
                        split[split.Length - 1],
                        next, next.EditorIcon());
                }
                   // }

                    //tree.
               // }
            }

            return tree;
        }
    }
#endif
}
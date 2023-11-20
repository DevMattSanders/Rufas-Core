using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Sirenix.OdinInspector.Editor;
using System.IO;

namespace Rufas
{
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

                Directory.CreateDirectory("Assets/Rufas/Resources");
                string path = "Assets/Rufas/Resources/GameSystemManager.asset";
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

            foreach (GameSystemParentClass next in foundManager.gameSystems)
            {
              //  if(foundManager.showRufasHiddenSystems == false && next.RufasBackendSystem())
              // {
              //      continue;
              //  }

                if (next.showInManager)
                {
                    tree.Add(next.DesiredPath(), next);
                }
            }

            return tree;
        }
    }
}
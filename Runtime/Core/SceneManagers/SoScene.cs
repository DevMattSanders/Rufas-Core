using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
#endif


namespace Rufas
{
    [CreateAssetMenu(menuName = "Rufas/Scene/SoScene")]
    public class SoScene : SuperScriptable
#if UNITY_EDITOR
        , IPreprocessBuildWithReport
#endif
    { 
#if UNITY_EDITOR
        [SerializeField,ShowInInspector, OnValueChanged("RefreshSceneName")] private SceneAsset editorSceneFile;
#endif

       // [ShowInInspector,ReadOnly]
        //public string sceneName { get; private set; ; }

        private string sceneName;

        [ShowInInspector,ReadOnly]
        public string SceneName
        {
            get
            {
#if UNITY_EDITOR
                //When playing in the editor, just make sure the file name is correct. This is automatically done before creating a build
                if (Application.isPlaying == true)
                {
                    RefreshSceneName();
                }
#endif

                return sceneName;
            }
        }

        [Button]
        public void LoadScene()
        {
            SceneManager.instance.LoadScene(this);
        }

#if UNITY_EDITOR

        //This automatically refreshes the scene name reference prior to creating a build
        public int callbackOrder { get { return 0; } }
        public void OnPreprocessBuild(BuildReport report)
        {
            Debug.Log("MyCustomBuildProcessor.OnPreprocessBuild for target " + report.summary.platform + " at path " + report.summary.outputPath);

            RefreshSceneName();
        }


        private void RefreshSceneName()
        {
           // Debug.Log("Refreshing name");
            if (editorSceneFile == null)
            {
               // Debug.LogError("Missing Scene File on SoScene object!");
            }
            else
            {
                if (string.Compare(editorSceneFile.name, sceneName) != 0)
                {
                    Debug.Log("Refreshed scene name from " + sceneName + " to " + editorSceneFile.name);

                    sceneName = editorSceneFile.name;
                }
            }
        }

        [Button]
        private void RefreshSceneNameAndRenameFile()
        {
            RefreshSceneName();

            if (string.IsNullOrEmpty(sceneName))
            {
                Debug.LogError("Cannot rename SoScene file as the sceneName string is null or empty");
            }
            else
            {
                this.name = sceneName;

                string assetPath = AssetDatabase.GetAssetPath(this);
                AssetDatabase.RenameAsset(assetPath, sceneName);

                EditorUtility.SetDirty(this);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }


        }

       
#endif
    }
}

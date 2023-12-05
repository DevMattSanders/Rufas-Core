using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.AddressableAssets;


#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
#endif


namespace Rufas
{
    
    [CreateAssetMenu(menuName = "Rufas/Scene/SoScene")]
    public class SoScene : ScriptableObject// SuperScriptable
    {
        public static UnityEvent<SoScene> SoSceneLoaderCalled = new UnityEvent<SoScene>();

        [HideLabel, Title("SceneByReference")]
       // [InlineButton("LoadScene")]
        // public SceneReference sceneByReference;

        [OnValueChanged("Refresh")]
        public AssetReference sceneAssetReference;

        [SerializeField,ReadOnly] private string sceneName;

        

        [HorizontalGroup("H")]
        [Button]
        public void LoadScene()
        {
            Refresh();

            SoSceneLoaderCalled.Invoke(this);
            if (sceneAssetReference != null)
            {
             //  SoSceneManager.Instance.LoadScene(sceneAssetReference,sceneName);
            }
            else
            {
                Debug.Log("No Scene Found");
            }
        }

        [HorizontalGroup("H")]
        [Button]
        private void Refresh()
        {
#if UNITY_EDITOR

            if (sceneAssetReference != null)
            {

                if (sceneAssetReference.editorAsset is not SceneAsset)
                {
                    sceneAssetReference = null;
                    Debug.LogError("Field: SceneAssetReference is for scene files only!");
                }
                else
                {
                    sceneName = sceneAssetReference.editorAsset.name;
                }
            }
            else
            {
                sceneName = "";
            }
#endif
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            Refresh();
        }
#endif
    }

    [System.Serializable]
    [InlineProperty]
#if UNITY_EDITOR
    [InitializeOnLoad]
#endif
    public class SceneReference
    {



#if UNITY_EDITOR

        bool PrivateChecker()
        {
            UpdateSceneName();
            return false;
        }

        [ShowIf("PrivateChecker")]
        public bool checkerCounterpart;

        public SceneReference(Object _scene)
        {
            scene = _scene;
            UpdateSceneName();
        }

        //[OnValueChanged("SceneChanged")]
        [HideLabel]
        [SerializeField]
        [OnValueChanged("UpdateSceneName")]
        private Object scene;

        public Object SceneObjectWhichShouldOnlyBeAccessedInEditor
        {
            get
            {
                return scene;
            }
            set
            {
                scene = value;
                UpdateSceneName();
            }
        }

        private void UpdateSceneName()
        {
            if (scene != null)
            {
                if (string.Compare(scene.name, SceneName) != 0)
                {
                    Debug.Log("Updated Scene Name Field");
                    SceneName = scene.name;
                }
            }
            else
            {
                SceneName = "NO_SCENE";
            }


        }
#endif
        [HideLabel]
        [ReadOnly]
        public string SceneName;

    }
    /*

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
        RufasSceneManager.instance.LoadScene(this);
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
    */
}

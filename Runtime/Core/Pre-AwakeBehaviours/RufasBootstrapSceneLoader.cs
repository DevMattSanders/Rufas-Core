using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Rufas
{
    public class RufasBootstrapSceneLoader : MonoBehaviour
    {
        //public static RufasBootstrapSceneLoader instance;

        public AssetReference sceneToLoad;

      //  public CanvasGroup hideCurrentScene;

        public Image percentageImage;

       

        float additionalPercent = 0;
        float assetListLoadVal = 0;
        float sceneLoadVal = 0;

      //  [SerializeField] private string sceneName;

       // public FadeScreenOnSceneTransition fadeScreen;

        //public 

     //   public UnityEvent onCompletedAssetLoad;
      //  public AssetListToRun assetList;

        //public GameObject load

      //  private void Awake()
      //  {
            //#if UNITY_EDITOR
            //   onCompletedAssetLoad.Invoke();
            //#else



         //   DontDestroyOnLoad(gameObject);

          //  Init();

        

            /*
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
               
            }
            else
            {
                GameObject.Destroy(gameObject);
            }
            */
//#endif
     //   }

        private IEnumerator AdditonalAmount()
        {
            yield return new WaitForSeconds(0.2f);
            additionalPercent += 0.2f;

            yield return new WaitForSeconds(0.5f);
            additionalPercent += 0.55f;

            yield return new WaitForSeconds(0.3f);
            additionalPercent += 0.25f;

           // yield return new WaitForSeconds(0.6f);
          //  additionalPercent += 0.1f;


        }

        private void Update()
        {
            if(percentageImage != null)
            {
                if (additionalPercent > 1) additionalPercent = 1;

                float targetPercent = (additionalPercent + assetListLoadVal + sceneLoadVal) / 3f;

                if (percentageImage.fillAmount < targetPercent)
                {
                    percentageImage.fillAmount += Time.deltaTime * 5f;
                }

                if(percentageImage.fillAmount > targetPercent)
                {
                    percentageImage.fillAmount = targetPercent;
                }
            }
        }

        //  private bool loadingAssets;

        //  private AsyncOperationHandle<RunAllPreawakeBehaviours> loadOp;

        // [Button]
        // public void Init()
        // {

        // loadOp = RunAllPreawakeBehaviours.loadOp; // Addressables.LoadAssetAsync<RunAllPreawakeBehaviours>("AssetListToRun");// on completed, send to another method  

        // loadOp.Completed += OnCompletedMethod;

        // StartCoroutine(LoadingAssetList());
        // }
        AsyncOperationHandle<SceneInstance> sceneInst;
        public IEnumerator Start()
        {
            // sceneName = gameObject.scene.name;

            DontDestroyOnLoad(gameObject);

            StartCoroutine(AdditonalAmount());


           // Debug.Log("HERE: 1");
            // if (Application.isPlaying == false) return;

            // RunAllPreawakeBehaviours runAll = Resources.LoadAll<RunAllPreawakeBehaviours>();

           // loadingAssets = true;

            while (RufasBootstrapper.loadOp.IsDone == false)
            {
                assetListLoadVal = RufasBootstrapper.loadOp.PercentComplete;
                yield return null;                
            }

           // Debug.Log("HERE: 2");

            //  Debug.Log("Starting wait for init");
            while (GameSystemManager.instance.allSystemsInitialised.Value == false)
            {                
                yield return null;
            }

            yield return null;
           // Debug.Log("HERE: 3");

            assetListLoadVal = 1;

           // sceneLoadVal = 1;
           // RufasSceneManager.LoadScene(sceneToLoad);
            
           

            sceneInst = Addressables.LoadSceneAsync(sceneToLoad, LoadSceneMode.Single, activateOnLoad: false);

            //Debug.Log("HERE: 4");

            sceneInst.Completed += sceneObj =>
            {
                //Debug.Log("HERE: 6");
                sceneLoadVal = 1;
                additionalPercent = 1;


                RufasSceneManager.Instance.isCurrentlyLoadingScene.Value = true;
              //  Debug.Log("HERE: 7");

                this.CallWithDelay(ActivateScene,0.1f);                
            };

           

            while (sceneInst.IsDone == false)
            {
                sceneLoadVal = sceneInst.PercentComplete;
                yield return null;
            }
           // Debug.Log("HERE: 5");
            sceneLoadVal = 1;
           
        }

        private void ActivateScene()
        {
           // Debug.Log("HERE: 8");
            sceneInst.Result.ActivateAsync().completed += obj =>
            {
               // Debug.Log("HERE: 9");
                RufasSceneManager.Instance.ResetValues();
                RufasSceneManager.Instance.openScenes.Add(sceneInst.Result);
            };
            //RufasSceneManager.Instance.


        }
        /*
        IEnumerator HideAlpha(AsyncOperationHandle<SceneInstance> obj)
        {
            while (hideCurrentScene.alpha < 1)
            {
                hideCurrentScene.alpha += Time.deltaTime;
                yield return null;
            }

            SoSceneManager.Instance.GetCurrentScenes();

#if UNITY_EDITOR
           // Debug.Log(gameObject.scene.name);
            if (SoSceneManager.Instance.currentOpenScenes.Contains(sceneName))
            {
                SoSceneManager.Instance.currentOpenScenes.Remove(sceneName);
            }
#endif
          //  Debug.Log(obj.Result.Scene.name);
            SoSceneManager.Instance.currentlyOpenScenes.Add(obj);
          //  Debug.Log(SoSceneManager.Instance.currentlyOpenScenes.Count);

          //  obj.Result.ActivateAsync().completed += CompletedNewSceneLoad;

          //  SoSceneManager.Instance.currentOpenScenes.Add(obj.Result.Scene.name);


        }
        */

      //  private void CompletedNewSceneLoad(AsyncOperation obj)
       // {
       //     StartCoroutine(ShowAlpha());
       // }
    //
        /*
        IEnumerator ShowAlpha()
        {
            while(hideCurrentScene.alpha > 0)
            {
                hideCurrentScene.alpha -= Time.deltaTime;
                yield return null;
            }           
        }
        */
    }
}

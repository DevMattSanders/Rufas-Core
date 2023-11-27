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
        public static RufasBootstrapSceneLoader instance;

        public AssetReference sceneToLoad;

        public CanvasGroup hideCurrentScene;

        public Image percentageImage;

        float targetPercent;


        float additionalPercent = 0;
        float assetListLoadVal = 0;
        float sceneLoadVal = 0;

        [SerializeField] private string sceneName;

       // public FadeScreenOnSceneTransition fadeScreen;

        //public 

     //   public UnityEvent onCompletedAssetLoad;
      //  public AssetListToRun assetList;

        //public GameObject load

        private void Awake()
        {
            //#if UNITY_EDITOR
            //   onCompletedAssetLoad.Invoke();
            //#else

            sceneName = gameObject.scene.name;

            DontDestroyOnLoad(gameObject);

            Init();

            StartCoroutine(AdditonalAmount());

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
        }

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

                targetPercent = (additionalPercent + assetListLoadVal + sceneLoadVal) / 3f;

                if (percentageImage.fillAmount < targetPercent)
                {
                    percentageImage.fillAmount += Time.deltaTime * 1f;
                }

                if(percentageImage.fillAmount > targetPercent)
                {
                    percentageImage.fillAmount = targetPercent;
                }
            }
        }

        private bool loadingAssets;

      //  private AsyncOperationHandle<RunAllPreawakeBehaviours> loadOp;

        [Button]
        public void Init()
        {
            if (Application.isPlaying == false) return;

           // RunAllPreawakeBehaviours runAll = Resources.LoadAll<RunAllPreawakeBehaviours>();

            loadingAssets = true;
           // loadOp = RunAllPreawakeBehaviours.loadOp; // Addressables.LoadAssetAsync<RunAllPreawakeBehaviours>("AssetListToRun");// on completed, send to another method  

           // loadOp.Completed += OnCompletedMethod;

            StartCoroutine(LoadingAssetList());
        }

        private IEnumerator LoadingAssetList()
        {
            while (RufasBootstrapper.loadOp.IsDone == false)
            {
                assetListLoadVal = RufasBootstrapper.loadOp.PercentComplete;
                yield return null;                
            }

          //  Debug.Log("Starting wait for init");
            while(GameSystemManager.instance.allSystemsInitialised.Value == false)
            {                
                yield return null;
            }

         //   Debug.Log("Finishing wait for init");
            OnCompletedMethod();
        }

        private bool loadingScene;

        AsyncOperationHandle<SceneInstance> sceneInst;
        private void OnCompletedMethod()//AsyncOperationHandle<RunAllPreawakeBehaviours> operation)
        {
            loadingAssets = false;
            assetListLoadVal = 1;

  //          operation.Result.LoadAddressablesAndRunPreawakeBehaviours();



            loadingScene = true;
           sceneInst = Addressables.LoadSceneAsync(sceneToLoad, LoadSceneMode.Single, activateOnLoad: false);
            
            sceneInst.Completed += AssetListToRunMono_Completed;

            StartCoroutine(LoadingScene());
        }

        private IEnumerator LoadingScene()
        {
            while (loadingScene)
            {
                sceneLoadVal = sceneInst.PercentComplete;
                yield return null;
            }
        }


        private void AssetListToRunMono_Completed(AsyncOperationHandle<SceneInstance> obj)
        {
            loadingScene = false;
            sceneLoadVal = 1;

            additionalPercent = 1;

            StartCoroutine(HideAlpha(obj));
        }

        IEnumerator HideAlpha(AsyncOperationHandle<SceneInstance> obj)
        {
            while (hideCurrentScene.alpha < 1)
            {
                hideCurrentScene.alpha += Time.deltaTime;
                yield return null;
            }

            SoSceneManager.Instance.GetCurrentScenes();

#if UNITY_EDITOR
            Debug.Log(gameObject.scene.name);
            if (SoSceneManager.Instance.currentOpenScenes.Contains(sceneName))
            {
                SoSceneManager.Instance.currentOpenScenes.Remove(sceneName);
            }
#endif

            SoSceneManager.Instance.currentlyOpenScenes.Add(obj);

            obj.Result.ActivateAsync().completed += CompletedNewSceneLoad;


        }

        private void CompletedNewSceneLoad(AsyncOperation obj)
        {
           

            StartCoroutine(ShowAlpha());
        }

        IEnumerator ShowAlpha()
        {
            while(hideCurrentScene.alpha > 0)
            {
                hideCurrentScene.alpha -= Time.deltaTime;
                yield return null;
            }           
        }
    }
}

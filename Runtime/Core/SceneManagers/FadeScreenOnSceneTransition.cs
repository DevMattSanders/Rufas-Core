using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rufas
{
    [RequireComponent(typeof(CanvasGroup))]
    public class FadeScreenOnSceneTransition : RufasMonoBehaviour
    {        
    
        private CanvasGroup canvasGroup;
        private Tween canvasGroupTween;

        [SerializeField]
        private bool startFaded;

        public override void Awake_AfterInitialisation()
        {
            base.Awake_AfterInitialisation();
            canvasGroup = GetComponent<CanvasGroup>();
        }

        public override void Start_AfterInitialisation()
        {
            base.Start_AfterInitialisation();
         RufasSceneManager.Instance.isCurrentlyLoadingScene.AddListener(CurrentlyLoadingScene); SetScreenOnStart();
        }
        public void OnDestroy()
        {
            RufasSceneManager.Instance.isCurrentlyLoadingScene.RemoveListener(CurrentlyLoadingScene);
        }

        private void SetScreenOnStart()
        {
            if (RufasSceneManager.Instance.isCurrentlyLoadingScene.Value || startFaded)
            {
                Debug.Log("Already started");
                canvasGroup.alpha = 1;
                canvasGroup.blocksRaycasts = true;
            }
            else
            {
                canvasGroup.alpha = 0;
                canvasGroup.blocksRaycasts = true;
            }

            CurrentlyLoadingScene(RufasSceneManager.Instance.isCurrentlyLoadingScene.Value);
        }

        private void CurrentlyLoadingScene(bool loadingScene)
        {
            if (loadingScene)
            {
                FadeToBlack();
            }
            else
            {
                FadeToClear();
            }           
        }

        [DisableInEditorMode]
        [Button]
        private void FadeToBlack()
        {
            if (!RufasSceneManager.Instance.sceneLoadStallers.Contains(this))
            {
                RufasSceneManager.Instance.sceneLoadStallers.Add(this);
            }

            if (canvasGroupTween != null) canvasGroupTween.Kill();

            canvasGroupTween = canvasGroup.DOFade(1, RufasSceneManager.Instance.fadeToBlackDuration).OnComplete(OnFinishedFadeToBlack);
            canvasGroup.blocksRaycasts = true;

            canvasGroupTween.OnKill(ClearFromSceneLoadStallers);
        }

        private void OnFinishedFadeToBlack()
        {
            ClearFromSceneLoadStallers();
        }

        [DisableInEditorMode]
        [Button]
        private void FadeToClear()
        {
            ClearFromSceneLoadStallers();

            if (canvasGroupTween != null) canvasGroupTween.Kill();

            canvasGroupTween = canvasGroup.DOFade(0, RufasSceneManager.Instance.fadeToBlackDuration);
            canvasGroup.blocksRaycasts = false;
        }

        private void ClearFromSceneLoadStallers()
        {
            if (RufasSceneManager.Instance.sceneLoadStallers.Contains(this))
            {
                RufasSceneManager.Instance.sceneLoadStallers.Remove(this);
            }
        }
    }
}

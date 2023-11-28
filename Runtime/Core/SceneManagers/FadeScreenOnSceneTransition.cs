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
        [SerializeField] private float duration = 0.5f;
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
         SoSceneManager.Instance.isCurrentlyLoadingScene.AddListener(CurrentlyLoadingScene); SetScreenOnStart();
        }
        public void OnDestroy()
        {
            SoSceneManager.Instance.isCurrentlyLoadingScene.RemoveListener(CurrentlyLoadingScene);
        }

        private void SetScreenOnStart()
        {
            if (SoSceneManager.Instance.isCurrentlyLoadingScene.Value || startFaded)
            {
                canvasGroup.alpha = 1;
                canvasGroup.blocksRaycasts = true;
            }
            else
            {
                canvasGroup.alpha = 0;
                canvasGroup.blocksRaycasts = true;
            }

            CurrentlyLoadingScene(SoSceneManager.Instance.isCurrentlyLoadingScene.Value);
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
            if (!SoSceneManager.Instance.sceneLoadStallers.Contains(this))
            {
                SoSceneManager.Instance.sceneLoadStallers.Add(this);
            }

            if (canvasGroupTween != null) canvasGroupTween.Kill();

            canvasGroupTween = canvasGroup.DOFade(1, duration).OnComplete(OnFinishedFadeToBlack);
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

            canvasGroupTween = canvasGroup.DOFade(0, duration);
            canvasGroup.blocksRaycasts = false;
        }

        private void ClearFromSceneLoadStallers()
        {
            if (SoSceneManager.Instance.sceneLoadStallers.Contains(this))
            {
                SoSceneManager.Instance.sceneLoadStallers.Remove(this);
            }
        }
    }
}

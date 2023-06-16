using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rufas
{
    [RequireComponent(typeof(CanvasGroup))]
    public class FadeScreenOnSceneTransition : MonoBehaviour
    {        
        [SerializeField] private float duration = 0.5f;
        private CanvasGroup canvasGroup;
        private Tween canvasGroupTween;

        private void Awake() { canvasGroup = GetComponent<CanvasGroup>(); }
        private void Start() { SoSceneManager.instance.isCurrentlyLoadingScene.onValue += CurrentlyLoadingScene; SetScreenOnStart();}
        private void OnDestroy() { SoSceneManager.instance.isCurrentlyLoadingScene.onValue -= CurrentlyLoadingScene; }

        private void SetScreenOnStart()
        {
            if (SoSceneManager.instance.isCurrentlyLoadingScene.Value)
            {
                canvasGroup.alpha = 1;
                canvasGroup.blocksRaycasts = true;
            }
            else
            {
                canvasGroup.alpha = 0;
                canvasGroup.blocksRaycasts = true;
            }
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
            if (!SoSceneManager.instance.sceneLoadStallers.Contains(this))
            {
                SoSceneManager.instance.sceneLoadStallers.Add(this);
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
            if (SoSceneManager.instance.sceneLoadStallers.Contains(this))
            {
                SoSceneManager.instance.sceneLoadStallers.Remove(this);
            }
        }
    }
}

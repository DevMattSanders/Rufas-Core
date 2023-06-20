using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
namespace Rufas
{
    public class TweenContainer : MonoBehaviour
    {
        public static Dictionary<Transform, TweenContainer> allTweenContainers = new Dictionary<Transform, TweenContainer>();

        public static TweenContainer Get(Transform inputTransform)
        {
            if (allTweenContainers.ContainsKey(inputTransform))
            {
                return allTweenContainers[inputTransform];
            }

            TweenContainer container = inputTransform.gameObject.AddComponent<TweenContainer>();

            allTweenContainers.Add(inputTransform, container);


            return container;
        }

        public Tween moveTween;
        public Tween rotateTween;
        public Tween scaleTween;
        public Tween canvasGroupFadeTween;

        private CanvasGroup canvasGroup;
        public CanvasGroup CanvasGroup()
        {
            if (canvasGroup == null)
            {
                canvasGroup = gameObject.GetComponent<CanvasGroup>();
            }

            if (canvasGroup == null)
            {
                canvasGroup = gameObject.AddComponent<CanvasGroup>();
            }

            // canvasGroup.DOFade(1,1).

            return canvasGroup;
        }

        private void OnDestroy()
        {
            if(moveTween != null) { moveTween.Kill(); }
            if(rotateTween != null) { rotateTween.Kill(); }
            if(scaleTween != null) {  scaleTween.Kill(); }
            if(canvasGroupFadeTween != null) { canvasGroupFadeTween.Kill(); }

            allTweenContainers.Remove(transform);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;
using DG.Tweening;
using UnityEngine.UI;

namespace Rufas
{
    public class RadialMenuEntry : MonoBehaviour
    {
        public Button myButton;
        public Image myIcon;

        private Tween positionTween;
        private Tween scaleTween;


        public void TweenOpen(Vector3 position, float tweenTime, float delayTime)
        {
            if (positionTween != null) { positionTween.Kill(); }
            if (scaleTween != null) { scaleTween.Kill(); }

            this.gameObject.SetActive(true);

            RectTransform rect = GetComponent<RectTransform>();
            positionTween = rect.DOAnchorPos(position, tweenTime).SetEase(Ease.OutQuad).SetDelay(delayTime);
            scaleTween = rect.DOScale(Vector3.one, tweenTime).SetEase(Ease.OutQuad).SetDelay(delayTime);
        }


        public void TweenClose(float tweenTime, float delayTime)
        {
            if (positionTween != null) { positionTween.Kill(); }

            RectTransform rect = GetComponent<RectTransform>();
            scaleTween = rect.DOScale(Vector3.zero, tweenTime).SetEase(Ease.InOutQuad).SetDelay(delayTime);
            positionTween = rect.DOAnchorPos(Vector3.zero, tweenTime).SetEase(Ease.OutQuad).SetDelay(delayTime);
            scaleTween.onComplete =
                    delegate ()
                    {
                        this.gameObject.SetActive(false);
                    };
        }
    }
}

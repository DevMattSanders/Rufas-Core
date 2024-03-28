using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.Events;
namespace Rufas
{
    [System.Serializable]
    public class zBaseTween
    {
        [HideInInspector] public bool useDelay;
        [ShowIf("useDelay"), PropertyRange(0, 60)] public float delay;

        [HideInInspector] public bool useDuration;
        [ShowIf("useDuration"), PropertyRange(0, 60)] public float duration;

        [HideInInspector] public bool useEase;
        [ShowIf("useEase")] public Ease ease = Ease.InOutSine;

        [HideInInspector]
        public bool tweenComplete = true;

        public virtual bool TweenComplete()
        {
            return tweenComplete;
        }

        System.Action onComplete;

        internal DG.Tweening.Tween tween;

        public void StartTween(Transform transform, System.Action _onComplete)
        {
            onComplete = _onComplete;
            tweenComplete = false;
            StartSpecificTween(transform);
        }

        public virtual void StartSpecificTween(Transform transform)
        {

        }

        public virtual void StopTween()
        {
            if (tween != null) tween.Kill();

            tweenComplete = false;
            onComplete = null;
        }

        public virtual void FastAction(Transform transform)
        {
            tweenComplete = true;

            TweenComplete();
        }

        public virtual void GetCurrentValues(Transform transform)
        {

        }

        public virtual void TweenCompleted()
        {
            if (onComplete != null)
            {
                tweenComplete = true;
                onComplete.Invoke();
                onComplete = null;
            }
        }
    }

    [System.Serializable]
    public class MoveTo : zBaseTween
    {
        public Vector3 moveTo;

        public override void StartSpecificTween(Transform transform)
        {
            base.StartSpecificTween(transform);

            tween = TweenContainer.Get(transform).moveTween;

            if (tween != null) tween.Kill();

            tween = transform.DOLocalMove(moveTo, duration).SetDelay(delay).SetEase(ease).OnComplete(TweenCompleted);
        }

        public override void FastAction(Transform transform)
        {
            base.FastAction(transform);

            if (tween != null) tween.Kill();

            transform.localPosition = moveTo;
        }

        public override void GetCurrentValues(Transform transform)
        {
            base.GetCurrentValues(transform);

            moveTo = transform.localPosition;
        }
    }

    [System.Serializable]
    public class RotateTo : zBaseTween
    {
        public Vector3 rotateTo;
        public RotateMode rotateMode = RotateMode.Fast;

        public bool rotateLoopAfter;

        [ShowIf("rotateLoopAfter")]
        public Vector3 rotateLoopAddition;

        [ShowIf("rotateLoopAfter")]
        public float rotateLoopDuration = 2;

        private Transform targetTransform;

        public override void StartSpecificTween(Transform transform)
        {
            targetTransform = transform;
            base.StartSpecificTween(transform);


            tween = TweenContainer.Get(transform).rotateTween;

            if (tween != null) tween.Kill();

            tween = transform.DOLocalRotate(rotateTo, duration, rotateMode).SetDelay(delay).SetEase(ease).OnComplete(TweenCompleted);
        }

        public override void TweenCompleted()
        {
            base.TweenCompleted();


            GoToAfterTween();
        }

        private void GoToAfterTween()
        {
            if (rotateLoopAfter)
            {
                if (tween != null) tween.Kill();

                tween = targetTransform.DOBlendableRotateBy(rotateLoopAddition, rotateLoopDuration, RotateMode.FastBeyond360).SetEase(Ease.Linear).SetLoops(-1, LoopType.Incremental);
            }
        }

        public override void FastAction(Transform transform)
        {
            base.FastAction(transform);

            if (tween != null) tween.Kill();

            transform.localEulerAngles = rotateTo;

            if (Application.isPlaying)
            {
                GoToAfterTween();
            }
        }

        public override void GetCurrentValues(Transform transform)
        {
            base.GetCurrentValues(transform);

            rotateTo = transform.localEulerAngles;
        }
    }
    /*
    [System.Serializable]
    public class RotateLoop : zBaseTween
    {

        // public RotateMode rotateMode = RotateMode.FastBeyond360;

        public Vector3 rotateLoopAddition;

        public float rotateLoopDuration = 2;

        public override void StartSpecificTween(Transform transform)
        {
            base.StartSpecificTween(transform);


            tween = TweenContainer.Get(transform).rotateTween;

            if (tween != null) tween.Kill();

            tween = transform.DOBlendableRotateBy(rotateLoopAddition, rotateLoopDuration, RotateMode.FastBeyond360).SetEase(Ease.Linear).SetLoops(-1, LoopType.Incremental);
        }

        public override void FastAction(Transform transform)
        {
            base.FastAction(transform);


            if (Application.isPlaying)
            {
                StartSpecificTween(transform);
            }
        }

        public override void GetCurrentValues(Transform transform)
        {
            base.GetCurrentValues(transform);
        }
    }
    */
    [System.Serializable]
    public class ScaleTo : zBaseTween
    {
        public Vector3 scaleTo = Vector3.one;

        public bool scaleLoopAfter;

        [ShowIf("scaleLoopAfter")]
        public Vector3 scaleLoopTarget;

        [ShowIf("scaleLoopAfter")]
        public float scaleLoopDuration = 2;

        [ShowIf("scaleLoopAfter")]
        public Ease scaleLoopEase = Ease.InOutSine;

        [ShowIf("scaleLoopAfter")]
        public LoopType scaleLoopType = LoopType.Incremental;

        [ShowIf("scaleLoopAfter")]
        public int scaleLoops = -1;

        private Transform targetTransform;


        public override void StartSpecificTween(Transform transform)
        {
            targetTransform = transform;

            base.StartSpecificTween(transform);

            tween = TweenContainer.Get(transform).scaleTween;

            if (tween != null) tween.Kill();

            tween = transform.DOScale(scaleTo, duration).SetDelay(delay).SetEase(ease).OnComplete(TweenCompleted);
        }

        public override void TweenCompleted()
        {
            base.TweenCompleted();


            GoToAfterTween();
        }

        private void GoToAfterTween()
        {
            if (scaleLoopAfter)
            {
                if (tween != null) tween.Kill();

                tween = targetTransform.DOScale(scaleLoopTarget, scaleLoopDuration).SetEase(scaleLoopEase).SetLoops(scaleLoops, scaleLoopType);

            }
        }

        public override void FastAction(Transform transform)
        {
            base.FastAction(transform);

            if (tween != null) tween.Kill();

            transform.localScale = scaleTo;


            if (Application.isPlaying)
            {
                GoToAfterTween();
            }
        }

        public override void GetCurrentValues(Transform transform)
        {
            base.GetCurrentValues(transform);

            scaleTo = transform.localScale;
        }
    }

    [System.Serializable]
    public class FadeTo : zBaseTween
    {

        [HorizontalGroup("H")] public float fadeTarget = 1;

        private TweenContainer container;

        public override void StartSpecificTween(Transform transform)
        {
            base.StartSpecificTween(transform);

            if (container == null)
            {
                container = TweenContainer.Get(transform);
            }

            tween = container.canvasGroupFadeTween;

            if (tween != null) tween.Kill();

            tween = container.CanvasGroup().DOFade(fadeTarget, duration).SetDelay(delay).SetEase(ease).OnComplete(TweenCompleted).OnUpdate(OnUpdate);

            if (fadeTarget <= 0.8f)
            {
                container.CanvasGroup().blocksRaycasts = false;
            }
        }

        public override void TweenCompleted()
        {
            base.TweenCompleted();



            SetInteractableOnEndOfTween();
        }

        private void OnUpdate()
        {
            // Debug.Log("On Fade Updated: " + container.CanvasGroup().alpha);

            if (fadeTarget > 0.8f)
            {
                if (container.CanvasGroup().alpha > 0.8f)
                {
                    //  container.CanvasGroup().interactable = true;
                    container.CanvasGroup().blocksRaycasts = true;
                }
                else
                {
                    container.CanvasGroup().blocksRaycasts = false;
                }
            }
        }

        private void SetInteractableOnEndOfTween()
        {
            if (fadeTarget <= 0.8f)
            {
                container.CanvasGroup().blocksRaycasts = false;
            }
        }

        public override void FastAction(Transform transform)
        {
            base.FastAction(transform);

            if (tween != null) tween.Kill();

            if (Application.isPlaying)
            {
                if (container == null)
                {
                    container = TweenContainer.Get(transform);
                }

                container = TweenContainer.Get(transform);
                container.CanvasGroup().alpha = fadeTarget;

                SetInteractableOnEndOfTween();
            }
            else
            {
                CanvasGroup cg = transform.GetComponent<CanvasGroup>();
                if (cg != null)
                {
                    transform.GetComponent<CanvasGroup>().alpha = fadeTarget;
                }
            }
        }

        public override void GetCurrentValues(Transform transform)
        {
            base.GetCurrentValues(transform);
            CanvasGroup cg = transform.GetComponent<CanvasGroup>();
            if (cg != null)
            {
                fadeTarget = transform.GetComponent<CanvasGroup>().alpha;
            }
        }
    }

    [System.Serializable]
    public class SetActiveState : zBaseTween
    {

        [HorizontalGroup("H")] public bool setToActiveState = true;// = 1;
        [Required(MessageType = InfoMessageType.Error), LabelText("Target (shouldn't be this object!)")] public GameObject target;

        private TweenContainer container;

        public override void StartSpecificTween(Transform transform)
        {
            base.StartSpecificTween(transform);

            if (container == null)
            {
                container = TweenContainer.Get(transform);
            }

            tween = container.canvasGroupFadeTween;

            if (tween != null) tween.Kill();

            //if (target != null)
            //{
                shouldRun = true;
               // Debug.Log("Active State here: " + target.name + " " + setToActiveState);
                container.CallWithDelay(SetActiveAfterDelay, delay + duration);
            
        }

        bool shouldRun = false;

        private void SetActiveAfterDelay()
        {
            if (shouldRun)
            {
                shouldRun = false;
                target.SetActive(setToActiveState);
                TweenCompleted();
            }
        }

        public override void TweenCompleted()
        {
            base.TweenCompleted();
            shouldRun = false;
        }

        public override void StopTween()
        {
            base.StopTween();
        }

        public override void FastAction(Transform transform)
        {
            base.FastAction(transform);

            if (Application.isPlaying == false)
            {
                Debug.Log("Fast action not applicable to tween type SetActive");
            }
            else
            {
                target.SetActive(setToActiveState);
            }            
            return;
        }

        public override void GetCurrentValues(Transform transform)
        {
            base.GetCurrentValues(transform);
            Debug.Log("GetCurrentValues not applicable to tween type SetActive");
            return;
        }


        [System.Serializable]
        public class CallEvent : zBaseTween
        {

            // [HorizontalGroup("H")] public bool setToActiveState = true;// = 1;
            // [Required(MessageType = InfoMessageType.Error), LabelText("Target (shouldn't be this object!)")] public GameObject target;

            [HideLabel]
            public UnityEvent unityEvent;
            private TweenContainer container;

            public override void StartSpecificTween(Transform transform)
            {
                base.StartSpecificTween(transform);

                if (container == null)
                {
                    container = TweenContainer.Get(transform);
                }

                tween = container.canvasGroupFadeTween;

                if (tween != null) tween.Kill();

                shouldRun = true;
                container.CallWithDelay(CallEventAfterDelay, delay + duration);
            }

            bool shouldRun = false;

            private void CallEventAfterDelay()
            {
                if (shouldRun)
                {
                    shouldRun = false;
                    unityEvent.Invoke();
                    TweenCompleted();
                }
            }

            public override void TweenCompleted()
            {
                base.TweenCompleted();
                shouldRun = false;
            }

            public override void StopTween()
            {
                base.StopTween();
            }

            public override void FastAction(Transform transform)
            {
                base.FastAction(transform);
                if (Application.isPlaying)
                {
                    Debug.Log("Fast action not applicable to tween type CallEvent");
                }
                else
                {
                    unityEvent.Invoke();
                }
                return;
            }

            public override void GetCurrentValues(Transform transform)
            {
                base.GetCurrentValues(transform);
                Debug.Log("GetCurrentValues not applicable to tween type CallEvent");
                return;
            }
        }

    }
}

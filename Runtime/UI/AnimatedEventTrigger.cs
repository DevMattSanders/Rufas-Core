using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
namespace Rufas
{
    public class AnimatedEventTrigger : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        private BoolWithCallback hovering;
        private bool clickAnimationRunning;

        [Tooltip("If null, set to this transform")]
        public Transform targetTransform;

        [TitleGroup("Default")] public TweenCase defaultCase;
        [TitleGroup("Hover")] public TweenCase onHover;
        [TitleGroup("Click")] public TweenCase onClick;

        public UnityEvent OnClick;

        protected void Awake()
        {
            if (targetTransform == null) targetTransform = transform;
        }

        protected void Start()
        {
            onClick.onTweenCaseCompleted.AddListener(OnClickCaseCompleted);

            if(onClick.tweenCaseRunning.Value == false && hovering == false)
            {
                UnhoverAnimation();
            }
        }

        private void OnValidate()
        {
            if (targetTransform == null) targetTransform = transform;

            defaultCase.showCondition = false;
            defaultCase.tweenTarget = targetTransform;
            //defaultCase
            onHover.showCondition = false;
            onHover.tweenTarget = targetTransform;

            onClick.showCondition = false;
            onClick.tweenTarget = targetTransform;
        }


        protected void OnDestroy()
        {
            onClick.onTweenCaseCompleted.RemoveListener(OnClickCaseCompleted);
        }

        //Runs as long as OnClick isn't also running
       public void OnPointerEnter(PointerEventData eventData)
        {
            Debug.Log("Hover");
            hovering.Value = true;

            if (onClick.tweenCaseRunning.Value == false)
            {
                HoverAnimation();
            }
        }

        //Runs as long as OnClick isn't also running
        public void OnPointerExit(PointerEventData eventData)
        {
            Debug.Log("Unhover");
            hovering.Value = false;

            if (onClick.tweenCaseRunning.Value == false)
            {
                UnhoverAnimation();
            }
        }          

        //Always runs, stops hover animations first
        public void OnPointerClick(PointerEventData eventData)
        {
            Debug.Log("Click");
            defaultCase.StopActions();
            onHover.StopActions();
            onClick.StartActions(targetTransform);

            OnClick.Invoke();
        }

        private void UnhoverAnimation()
        {
            onHover.StopActions();
            defaultCase.StartActions(targetTransform);
        }

        private void HoverAnimation()
        {
            defaultCase.StopActions();
            onHover.StartActions(targetTransform);
        }

        private void OnClickCaseCompleted()
        {
            if (hovering.Value)
            {
                HoverAnimation();
            }
            else
            {
                UnhoverAnimation();
            }
        }

     
    }
}
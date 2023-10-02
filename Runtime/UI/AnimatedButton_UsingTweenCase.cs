using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Rufas
{
    [RequireComponent(typeof(ButtonWithCallback))]
    public class AnimatedButton_UsingTweenCase : MonoBehaviour
    {
        public BoolWithCallback hovering_debug;

        public ButtonWithCallback button;

        [Required]
        public Transform targetTransform;

        //NoPointer
        [Title("Default")]
        public TweenCase defaultTweens;

        //Hovering
        [Title("Hovering")]
        public TweenCase hoverTweens;

        public Tween click_MovePunch;
        public Tween click_RotatePunch;
        public Tween click_ScalePunch;


        public bool defaultRunning;
        public bool hoverRunning;
        public bool clickRunning;

        private void Update()
        {
            defaultRunning = defaultTweens.tweenCaseRunning.Value;
            hoverRunning = hoverTweens.tweenCaseRunning.Value;
            clickRunning = clickTweens.tweenCaseRunning.Value;
        }

        //ClickSequence

        bool clickSequenceTakingPlace;
        [Title("Click")]
        public TweenCase clickTweens;

        private void Start()
        {

            //button.pointerHovering
                hovering_debug.AddListener(HoverChanged);
            clickTweens.onTweenCaseCompleted.AddListener(RefreshCheck);
            button.pointerClick.AddListener(Click);
        }

        private void OnDestroy()
        {
            //button.pointerHovering
                hovering_debug.RemoveListener(HoverChanged);
            clickTweens.onTweenCaseCompleted.RemoveListener(RefreshCheck);
            button.pointerClick.RemoveListener(Click);
        }

        private void OnValidate()
        {
            if (targetTransform != null)
            {
                defaultTweens.tweenTarget = targetTransform;
                hoverTweens.tweenTarget = targetTransform;
                clickTweens.tweenTarget = targetTransform;
            }
        }

        private void HoverChanged(bool hover)
        {
            RefreshCheck();
        }

        [ShowInInspector, Button]
        private void Click_Debug()
        {
            Click();
        }

        private void Click()
        {
            RefreshCheck();

            /*
            hoverTweens.StopActions();
            defaultTweens.StopActions();

            if (clickTweens.tweenCaseRunning == false)
            {
                Debug.Log("Start: Click");
                clickTweens.StartActions(transform);
            }
            */
        }

        private void RefreshCheck()
        {
            //Check all click sequence tweens
            if (clickTweens.tweenCaseRunning.Value)
            {
                defaultTweens.StopActions();
                hoverTweens.StopActions();
            }
            //else if (button.pointerHovering.Value)
            else if (hovering_debug.Value)
            {
                clickTweens.StopActions();
                defaultTweens.StopActions();

                if (hoverTweens.tweenCaseRunning == false)
                {
                    Debug.Log("Start: Hover");
                    hoverTweens.StartActions(transform);
                }
            }
            else
            {
                clickTweens.StopActions();
                hoverTweens.StopActions();

                if (defaultTweens.tweenCaseRunning == false)
                {
                    Debug.Log("Start: Unhover");
                    defaultTweens.StartActions(transform);
                }
            }
        }
    }
}

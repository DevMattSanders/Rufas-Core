using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rufas
{
    [System.Serializable]
    [HideLabel]
    public class TweenCase
    {

        [HideInInspector]
        public bool showCondition = false;
        [HideInInspector]
        public TweenElement tweenElement;

        [HideInInspector]
        public Transform tweenTarget;

        [HideInInspector]
        public int tweenCaseIndex;

        private bool ShowCondition()
        {
            if (showCondition == true)// && manualToggle == false)
            {
                return true;
            }

            return false;
        }

        public string CaseTitle()
        {
            return "--- TWEEN CASE ---";
        }

        [ShowIf("ShowCondition")]
        [HideLabel]
        [GUIColor(0.8f, 0.8f, 1, 1)]
        public string conditionValue;

        #region Delay, Duration & Ease

        [FoldoutGroup("Group Settings")]
        [GUIColor("DelayColour")]
        [InlineButton("OverrideDelay", label: "Override")]
        [PropertyRange(0, 60)]
        [OnValueChanged("RefreshTweenValues")]
        public float delay;

        private Color DelayColour() { return BoolToColour(!overrideDelay); }

        private void OverrideDelay()
        {
            overrideDelay = !overrideDelay;
            UpdateBaseTweens();
        }


        [FoldoutGroup("Group Settings")]
        [GUIColor("DurationColour")]
        [InlineButton("OverrideDuration", label: "Override")]
        [PropertyRange(0, 60)]
        [OnValueChanged("RefreshTweenValues")]
        public float duration = 1;

        private Color DurationColour() { return BoolToColour(!overrideDuration); }

        private void OverrideDuration()
        {
            overrideDuration = !overrideDuration;
            UpdateBaseTweens();
        }

        [FoldoutGroup("Group Settings")]
        [GUIColor("EaseColour")]
        [InlineButton("OverrideEase", label: "Override")]
        [OnValueChanged("RefreshTweenValues")]
        public Ease ease = Ease.InOutSine;

        private Color EaseColour() { return BoolToColour(!overrideEase); }
        private void OverrideEase()
        {
            overrideEase = !overrideEase;
            UpdateBaseTweens();
        }

        private Color BoolToColour(bool val)
        {
            if (val)
            {
                return Color.white;
            }
            else
            {
                return Color.red;
            }
        }

        [HideInInspector, SerializeField] public bool overrideDelay, overrideDuration, overrideEase;

        #endregion

        [SerializeReference]
        [HideReferenceObjectPicker]
        [ListDrawerSettings(Expanded = true, HideRemoveButton = true)]
        [OnValueChanged("UpdateBaseTweens")]
        [InlineButton("GoToValuesImmediate", label: "Go")]
        [InlineButton("GetCurrentValues", label: "Get")]
        public List<zBaseTween> tweens = new List<zBaseTween>();

       [HideInInspector] public BoolWithCallback tweenCaseRunning;
        [HideInInspector] public CodeEvent onTweenCaseCompleted;

        public void TweenCompleted()
        {
            bool allComplete = true;

            foreach (zBaseTween tween in tweens)
            {
                if (tween.TweenComplete() == false)
                {
                    allComplete = false;
                    break;
                }
            }

            if (allComplete == true)
            {
                tweenElement?.IndexCompleted(tweenCaseIndex);
                tweenCaseRunning.Value = false;
                onTweenCaseCompleted.Raise();
            }
        }

        public void GoToValuesImmediate()
        {
            if(tweenTarget == null)
            {
                if (tweenElement == null) return;

                tweenTarget = tweenElement.transform;
            }

            foreach (zBaseTween tween in tweens)
            {
                tween.FastAction(tweenTarget);// tweenElement.transform);
            }

            TweenCompleted();
        }

        private void GetCurrentValues()
        {
            if (tweenTarget == null)
            {
                if (tweenElement == null) return;

                tweenTarget = tweenElement.transform;
            }

            foreach (zBaseTween tween in tweens)
            {
                tween.GetCurrentValues(tweenTarget);// tweenElement.transform);
            }
        }

        public void UpdateBaseTweens()
        {
            foreach (zBaseTween tween in tweens)
            {
                tween.useDelay = overrideDelay;


                tween.useDuration = overrideDuration;


                tween.useEase = overrideEase;

            }

            RefreshTweenValues();

            //if(tweenElement != null)
            //{
                //if(tweenElement.)
            //}
        }

        private void RefreshTweenValues()
        {
            foreach (zBaseTween tween in tweens)
            {
                //tween. = manualToggle;

                if (overrideDelay == false) tween.delay = delay;

                if (overrideDuration == false) tween.duration = duration;

                if (overrideEase == false) tween.ease = ease;
            }
        }

        public void StartActions(Transform transform)
        {
            tweenCaseRunning.Value = true;

            foreach (zBaseTween tween in tweens) tween.StartTween(transform, TweenCompleted);

            
        }

        public void StopActions()
        {
            foreach (zBaseTween tween in tweens) tween.StopTween();

            tweenCaseRunning.Value = false;
        }
    }
}
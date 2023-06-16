using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rufas
{
    public static class MonoBehaviourExtensions
    {
        public static void CallWithDelay(this MonoBehaviour mono, Action method, float delay)
          => mono.StartCoroutine(CallWithDelayRoutine(method, delay));
       
        private static IEnumerator CallWithDelayRoutine(Action method, float delay)
        {
            yield return new WaitForSeconds(delay);
            method();
        }

        public static void CallWithFrameDelay(this MonoBehaviour mono, Action method, int framesToSkip)
            => mono.StartCoroutine(CallWithDelayRoutine(method, framesToSkip));

        private static IEnumerator CallWithDelayRoutine(Action method, int framesToSkip)
        {
            yield return new WaitForFrames(framesToSkip);

            method();
        }

        public static void CallAtEndOfFrame(this MonoBehaviour mono, Action method)//, int frames)
            => mono.StartCoroutine(CallAtEndOfFrameRoutine(method));//, frames));

        private static IEnumerator CallAtEndOfFrameRoutine(Action method)//, int frames)
        {
            yield return new WaitForEndOfFrame();
            method();
        }

        public static void EnsureCoroutineStopped(this MonoBehaviour value, ref Coroutine routine)
        {
            if (routine != null)
            {
                value.StopCoroutine(routine);
                routine = null;
            }
        }

        public static Coroutine CreateAnimationRoutine(this MonoBehaviour value, float delay, float duration, Action<float> changeFunction, Action onComplete = null)
        {
            return value.StartCoroutine(GenericAnimationRoutine(delay, duration, changeFunction, onComplete));
        }

        private static IEnumerator GenericAnimationRoutine(float delay, float duration, Action<float> changeFunction, Action onComplete)
        {
            float elapsedTime = 0;
            float progress = 0;
            while (progress <= 1)
            {
                changeFunction(progress);
                elapsedTime += Time.unscaledDeltaTime;
                progress = elapsedTime / (delay + duration);
                yield return null;
            }

            changeFunction(1);
            onComplete?.Invoke();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rufas
{
    public class ScriptableCallbacksMonobehaviourLink : MonoBehaviour
    {
        //public static ScriptableCallbacksHandler scriptableCallbacksHandler;


        private void Awake()
        {
         //   ScriptableCallbacksHandler.Instance.TriggerAll_SoOnAwake();
        }
        private void Start()
        {
         //   ScriptableCallbacksHandler.Instance.TriggerAll_SoOnStart();
        }

        private void OnDestroy()
        {
       //     ScriptableCallbacksHandler.Instance.TriggerAll_SoOnEnd();
        }
    }
}

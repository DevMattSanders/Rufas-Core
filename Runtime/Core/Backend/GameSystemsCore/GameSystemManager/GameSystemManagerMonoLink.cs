using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rufas
{
    public class GameSystemManagerMonoLink : MonoBehaviour
    {
        private void Awake()
        {
            GameSystemManager.instance.TriggerOnAwakeBehaviour();
        }

        private void Start()
        {
            GameSystemManager.instance.TriggerOnStartBehaviour();
        }

        private void Update()
        {
            GameSystemManager.instance.TriggerOnUpdateBehaviour();
        }

        private void OnDestroy()
        {
            GameSystemManager.instance.TriggerEndOfApplicationBehaviour();
        }
    }
}

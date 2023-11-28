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

        private void OnDestroy()
        {
            GameSystemManager.instance.TriggerEndOfApplicationBehaviour();
        }
    }
}

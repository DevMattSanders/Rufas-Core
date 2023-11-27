using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rufas
{
    public class GameSystemManagerMonoLink : MonoBehaviour
    {
        private void OnDestroy()
        {
            GameSystemManager.instance.TriggerEndOfApplicationBehaviour();
        }
    }
}

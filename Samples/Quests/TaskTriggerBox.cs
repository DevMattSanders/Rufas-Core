using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rufas.Quests
{
    public class TaskTriggerBox : MonoBehaviour
    {
        [SerializeField] private QuestTask task;
        [Space]
        [SerializeField] private QuestTracker questTracker;

        private void OnTriggerEnter(Collider other)
        {

            if (other.gameObject.tag == "Player")
            {
                task.completeEvent.Raise();
            }
        }
    }
}

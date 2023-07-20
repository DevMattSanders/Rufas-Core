using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rufas.Quests
{
    public class QuestTaskCompleter : MonoBehaviour
    {
        [SerializeField] private QuestTask task;

        public void CompleteTask()
        {
            if (task != null)
            {
                task.completeEvent.Raise();
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rufas.Quests
{
    public class QuestTaskCompleter : MonoBehaviour
    {
        [SerializeField] private QuestTracker _tracker;
        [SerializeField] private QuestTask task;

        public void CompleteTask()
        {
            if (task != null && _tracker.currentQuest.currentTask == task)
            {
                task.completeEvent.Raise();
            }
        }
    }
}

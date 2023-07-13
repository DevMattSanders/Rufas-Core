using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Rufas.Quests
{
    public class QuestTracker : MonoBehaviour
    {
        [InlineEditor] public QuestData currentQuest;

        public CodeEvent<QuestData> OnQuestStarted;
        public CodeEvent<QuestTask> OnTaskComplete;
        public CodeEvent<QuestTask> OnTaskStarted;
        public CodeEvent<QuestData> OnQuestComplete;

        private void Start()
        {
            if (currentQuest == null) { return; }

            currentQuest.tracker = this;
            currentQuest.currentTask = currentQuest.tasks[0];

            OnQuestStarted.Raise(currentQuest);
            OnTaskStarted.Raise(currentQuest.currentTask);
        }
    }
}

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

        public LocalBoolVariable questActive;
        public LocalBoolVariable questCompleted;
        [Header("Delay")]
        public float taskStartDelay = 1f;

        public CodeEvent<QuestData> OnQuestStarted;
        public CodeEvent<QuestTask> OnTaskComplete;
        public CodeEvent<QuestTask> OnTaskStarted;
        public CodeEvent<QuestData> OnQuestComplete;

        private void Awake()
        {
            questCompleted.Value = false;
            OnQuestComplete.AddListener(QuestCompleted);
        }

        private void OnDestroy()
        {
            OnQuestComplete.RemoveListener(QuestCompleted);
        }

        private IEnumerator Start()
        {
            questActive.Value = false;

            if (currentQuest == null) { yield break; ; }

            foreach (var task in currentQuest.tasks) {
                task.completed = false;
            }

            currentQuest.questCompleted = false;

            currentQuest.tracker = this;
            currentQuest.currentTask = currentQuest.tasks[0];
            currentQuest.taskIndex = 0;

            yield return new WaitForSeconds(taskStartDelay * 3);

            questActive.Value = true;


            OnQuestStarted.Raise(currentQuest);
            OnTaskStarted.Raise(currentQuest.currentTask);
        }

        public void StartNextTaskDelayRoutine()
        {
            StartCoroutine(NextTaskDelay());
        }


        IEnumerator NextTaskDelay()
        {
            yield return new WaitForSeconds(taskStartDelay);

            currentQuest.StartNextTask();
        }

        private void QuestCompleted(QuestData completedQuest)
        {
            questCompleted.Value = true;
        }
    }
}

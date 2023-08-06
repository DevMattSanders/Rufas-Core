using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace Rufas.Quests
{
    [CreateAssetMenu(fileName = "New Quest Data", menuName = "Rufas/Quests/QuestData", order = 0)]
    public class QuestData : SuperScriptable
    {
        public string questName = "Unnamed Quest";
        public bool questCompleted = false;


        [Header("Trackers")]
        [ReadOnly] public QuestTracker tracker;

        [Header("Tasks")]
        public QuestTask currentTask;
        public int taskIndex;
        [InlineEditor] public List<QuestTask> tasks = new List<QuestTask>();


        public override void SoOnStart()
        {
            taskIndex = 0;
            questCompleted= false;
        }

        public void MoveToNextTasK()
        {
            taskIndex++;
            if (taskIndex >= tasks.Count)
            {
                QuestCompleted();
                return;
            }
            else
            {
                tracker.StartNextTaskDelayRoutine();
            }
        }

        public void QuestCompleted()
        {
            Debug.Log("Quest Completed");

            tracker.OnQuestComplete.Raise(this);
            questCompleted= true;
            //tracker.currentQuest = null;
        }

    
        public void StartNextTask()
        {
            currentTask = tasks[taskIndex];
            tracker.OnTaskStarted.Raise(currentTask);
            Debug.Log("NEW TASK: " + currentTask.taskDescription);
        }
    }
}
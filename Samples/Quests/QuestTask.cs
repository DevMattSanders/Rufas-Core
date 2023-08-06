using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rufas.Quests
{
    [CreateAssetMenu(fileName = "New Quest Data", menuName = "Rufas/Quests/QuestTask", order = 1)]
    public class QuestTask : SuperScriptable
    {
        [TextArea] public string taskDescription;
        public AudioClip taskAudioClip;
        [Space]
        public QuestData parentQuest;
        public bool completed = false;
        public CodeEvent completeEvent;

        public override void SoOnAwake()
        {
            base.SoOnAwake();

            completed = false;
        }

        public override void SoOnStart()
        {
            base.SoOnStart();

            completeEvent.AddListener(OnTaskComplete);
        }

        public override void SoOnEnd()
        {
            base.SoOnEnd();

            completeEvent.RemoveListener(OnTaskComplete);
        }

        private void OnTaskComplete()
        {
            parentQuest.tracker.OnTaskComplete.Raise(this);
            completed = true;
            if (parentQuest.currentTask != this) 
            { 
                Debug.Log("Not current task"); 
                return; 
            }

            parentQuest.MoveToNextTasK();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Rufas.Quests
{
    public class QuestUserInterface : MonoBehaviour
    {
        [SerializeField] private QuestTracker questTracker;

        [SerializeField] private TextMeshProUGUI questTitleText;
        [SerializeField] private TextMeshProUGUI questHintText;

        private void OnEnable()
        {
            questTracker.OnQuestStarted.AddListener(ShowQuestStarted);
            questTracker.OnTaskStarted.AddListener(UpdateQuestHintText);
            questTracker.OnQuestComplete.AddListener(ShowQuestCompleted);
        }

        private void OnDisable()
        {
            questTracker.OnQuestStarted.RemoveListener(ShowQuestStarted);
            questTracker.OnTaskStarted.RemoveListener(UpdateQuestHintText);
            questTracker.OnQuestComplete.RemoveListener(ShowQuestCompleted);
        }

        private void ShowQuestStarted(QuestData questData)
        {
            questTitleText.SetText("Quest Started: " + questData.questName);
            questTitleText.GetComponent<TextFadeOut>().FadeText();
        }

        private void UpdateQuestHintText(QuestTask questTask)
        {
            questHintText.SetText(questTask.taskDescription);
        }

        private void ShowQuestCompleted(QuestData questData)
        {
            questTitleText.SetText("Quest Complete: " + questData.questName);
            questTitleText.GetComponent<TextFadeOut>().FadeText();
        }
    }
}

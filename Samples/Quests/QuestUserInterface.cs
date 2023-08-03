using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using MPUIKIT;


namespace Rufas.Quests
{
    public class QuestUserInterface : MonoBehaviour
    {
        [SerializeField] private QuestTracker questTracker;

        [SerializeField] private TextMeshProUGUI questTitleText;
        [SerializeField] private TextMeshProUGUI questHintText;
        [SerializeField] private GameObject questHintBackground;

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
            questTitleText.SetText("Current Activity: " + questData.questName);
            questTitleText.GetComponent<TextFadeOut>().FadeText();
        }

        private void UpdateQuestHintText(QuestTask questTask)
        {
            questHintText.SetText(questTask.taskDescription);
        }

        private void ShowQuestCompleted(QuestData questData)
        {
            Debug.Log("Show quest completed text");

            questHintBackground.gameObject.SetActive(false);

            questTitleText.SetText("Activity Complete: " + questData.questName);
            questTitleText.GetComponent<TextFadeOut>().FadeText();
        }
    }
}

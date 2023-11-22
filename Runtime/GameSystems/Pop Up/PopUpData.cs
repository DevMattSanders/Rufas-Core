using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rufas
{
    public enum PopUpType
    {
        Duration,
        Acknowledge,
        Choice
    }

    [CreateAssetMenu(fileName = "New Pop Up", menuName = "Rufas/Pop Up/Pop Up Data")]
    public class PopUpData : ScriptableObject
    {
        public PopUpType popUpType = PopUpType.Duration;
        public string popUpTitle;
        [TextArea()] public string popUpDescription;
        [Space]
        [ShowIf("popUpType", PopUpType.Duration)] public float popUpDuration;
        [ShowIf("popUpType", PopUpType.Acknowledge)] public string acknowldgeButtonText;

        public void InitDuractionPopUp(string title, string description, float duration)
        {
            popUpType = PopUpType.Duration;

            this.name = title;
            popUpTitle = title;
            popUpDescription = description;
            popUpDuration = duration;
        }

        public void InitAcknowledgePopUp(string title, string description, string buttonText)
        {
            popUpType = PopUpType.Acknowledge;

            this.name = title;
            popUpTitle = title;
            popUpDescription = description;
            popUpDuration = Mathf.Infinity;
            acknowldgeButtonText = buttonText;
        }

        [Button()] public void Show()
        {

        }

        [Button()] public void ShowNow()
        {

        }
    }
}

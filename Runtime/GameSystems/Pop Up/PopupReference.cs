using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rufas
{
  //  public enum PopUpType
   // {
  //      Duration,
   //     Acknowledge,
   //     Choice
  //  }

    [CreateAssetMenu(fileName = "New Pop Up", menuName = "Rufas/Pop Up/Pop Up Data")]
    public class PopupReference : ScriptableObject
    {
        public GameObject popupPrefab;

        // public PopUpType popUpType = PopUpType.Duration;
        // public string popUpTitle;
        // [TextArea()] public string popUpDescription;
        // [Space]
        // [ShowIf("popUpType", PopUpType.Duration)] public float popUpDuration;
        //  [ShowIf("popUpType", PopUpType.Acknowledge)] public string acknowldgeButtonText;


        /*
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
        */


        [Button]
        public void CreatePopup(bool priority = false,bool ddol = false)
        {
            if (popupPrefab == null) { Debug.LogError("No prefab found on popupReference!"); return; }
            PopupMonoBehaviour popupInstance = GameObject.Instantiate(popupPrefab.GetComponent<PopupMonoBehaviour>());
            popupInstance.isPriority = priority;
        }

        /*
        [Button()] public void CreateAndShow()
        {
            if (PopUpManager.Instance != null) {
                PopUpManager.Instance.QueuePopUp(this);
            }
        }

        [Button()] public void CreateAndShowAsPriority()
        {
            if (PopUpManager.Instance != null) {
                PopUpManager.Instance.ShowPopUpNow(this);
            }
        }
        */
    }

}

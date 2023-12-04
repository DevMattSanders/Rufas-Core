using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rufas
{
    public class ShowPopUpOnStartWithDelay : MonoBehaviour
    {
        public PopupReference popup;

        [SerializeField] private float delay = 0.1f;
     //   [Space]
       // [SerializeField] private PopUpType popUpType;
        //[SerializeField] private string popUpName;
      //  [TextArea(), SerializeField] private string popUpDescription;
      //  [ShowIf("popUpType", PopUpType.Acknowledge), SerializeField] private string buttonText;
      //  [ShowIf("popUpType", PopUpType.Duration), SerializeField] private float duration;

        private void Start()
        {
            this.CallWithDelay(QueuePopUp, delay);
        }

        private void QueuePopUp()
        {
           // PopupReference popUpData = ScriptableObject.CreateInstance<PopupReference>() as PopupReference;
            //popUpData.In

            // if (popUpType == PopUpType.Acknowledge)
            // {
            //     popUpData.InitAcknowledgePopUp(popUpName, popUpDescription, buttonText);
            // }
            // else if (popUpType == PopUpType.Duration)
            // {
            //     popUpData.InitDuractionPopUp(popUpName, popUpDescription, duration);
            // }

            popup.CreatePopup();

            //PopUpManager.Instance.QueuePopUp(popUpData);
        }
    }
}

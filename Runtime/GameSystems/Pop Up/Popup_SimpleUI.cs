using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Rufas
{
    public class Popup_SimpleUI : PopupMonoBehaviour
    {      

        public TextMeshProUGUI Title;
        public TextMeshProUGUI SubTitle;
        public TextMeshProUGUI TextField;

        public override void Start_AfterInitialisation()
        {
            base.Start_AfterInitialisation();

            //HidePopUpWindow();
        //    PopUpManager.Instance.OnNewCurrentPopUpSet.AddListener(ShowNewPopUpData);
       //     PopUpManager.Instance.OnPopUpRemoved.AddListener(HidePopUpWindow);
           // acknowledgementButton.onClick.AddListener(OnAcknowlegementClicked);
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            
        //    PopUpManager.Instance.OnNewCurrentPopUpSet.RemoveListener(ShowNewPopUpData);
        //    PopUpManager.Instance.OnPopUpRemoved.RemoveListener(HidePopUpWindow);
            //acknowledgementButton.onClick.RemoveListener(OnAcknowlegementClicked);

            
        }


        

        public void Init(string _title, string _subTitle, string _textField)
        {                        
            // Debug.Log("Updating current pop up UI");
            //popUpWindow.gameObject.SetActive(true);

           // title?.SetText(popUpData.popUpTitle);
           // textFieldOne?.SetText(popUpData.popUpDescription);

            /*
            if (popUpData.popUpType != PopUpType.Acknowledge)
            {
                acknowledgementButton.gameObject.SetActive(false);
            }
            else if (popUpData.popUpType == PopUpType.Acknowledge)
            {
                acknowledgementButton.GetComponentInChildren<TMP_Text>().SetText(popUpData.acknowldgeButtonText);
            }           
            */
        }


        //private void HidePopUpWindow()
        // {
        //    popUpWindow.gameObject.SetActive(false);
        //}
      

    }
}

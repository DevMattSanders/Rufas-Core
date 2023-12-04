using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace Rufas
{
    public class Popup_YesNoCancel : PopupMonoBehaviour
    {
        [System.Serializable]
        public class Popup_YesNoCancelData
        {
            string title = "";
            string subTitle = "";
            string info = "";

        }

        public UnityEvent<string> title;
        public UnityEvent<string> subTitle;
        public UnityEvent<string> info;

        public UnityEvent<string> yesText;
        public UnityEvent<string> noText;
        public UnityEvent<string> cancelText;

        public Action onYes;
        public Action onNo;
        public Action onCancel;

        public void Init(
            string _title,
            string _subTitle,
            string _info, Action _onYes, Action _onNo, Action _onCancel)
        {
            title.Invoke(_title);
            subTitle.Invoke(_subTitle);
            info.Invoke(_info);

         //   yesText.Invoke

            onYes += _onYes;
            onNo += _onNo;
            onCancel += _onCancel;
        }

        public void Command_Yes()
        {

        }

        public void Command_No()
        {

        }

        public void Command_Cancel()
        {

        }        
    }



}

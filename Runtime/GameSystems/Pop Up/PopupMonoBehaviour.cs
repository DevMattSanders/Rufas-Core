using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Rufas
{
    public class PopupMonoBehaviour : RufasMonoBehaviour
    {
        public static PopupMonoBehaviour MainPopupInstance;
        public bool isMainPopup => MainPopupInstance == this;

        [SerializeField] bool alwaysDDOL = false;
        [SerializeField] bool alwaysPriority;
        [ReadOnly]public bool isPriority;

        [ReadOnly] public bool popupClosed = false;

        [SerializeField] private bool hideAfterDelay = false;
        [ShowIf("hideAfterDelay"), SerializeField,ReadOnly] private float delayCounter = 0;
        [ShowIf("hideAfterDelay"), SerializeField,Range(1,20)] private float delayDuration = -1;

        public BoolReference popupVisualsEnabled;
        public UnityEvent<bool> enableVisuals = new UnityEvent<bool>();

        [SerializeField] private float delayToDestroyAfterClose = 5;

        public override void Awake()
        {
            base.Awake();
            popupVisualsEnabled.Value = false;
            enableVisuals.Invoke(false);
            popupClosed = false;
            if(alwaysDDOL) GameObject.DontDestroyOnLoad(gameObject);
        }

        public override void Start_AfterInitialisation()
        {
            base.Start_AfterInitialisation();

            bool priority = isPriority;
            if (alwaysPriority) priority = true;
            

            PopUpManager.Instance.RegisterPopupGameObject(this,priority);
        }   

        public virtual void Update()
        {
            if (hideAfterDelay && popupVisualsEnabled.Value == true)
            {
                if (isMainPopup && initialisationCompleted)
                {
                    delayCounter += Time.deltaTime;

                    if(delayCounter > delayDuration)
                    {
                        ClosePopup();
                    }
                }
                else
                {
                    delayCounter = 0;
                }
            }
        }

        [Button]
        public virtual void ClosePopup()
        {
            popupVisualsEnabled.Value = false;
            enableVisuals.Invoke(false);
            if (popupClosed == false)
            {
                popupClosed = true;
                this.CallWithDelay(() => { GameObject.Destroy(gameObject); }, delayToDestroyAfterClose);
            }
        }
    }
}

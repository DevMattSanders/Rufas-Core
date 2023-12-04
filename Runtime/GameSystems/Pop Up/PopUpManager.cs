using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
//using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;

namespace Rufas
{
    public class PopUpManager : GameSystem<PopUpManager>
    {
        //public static PopUpManager Instance;

        [SerializeField, ReadOnly] private PopupReference currentPopUp;
        [SerializeField, ReadOnly] private List<PopupReference> queuedPopUpData = new List<PopupReference>();
        [SerializeField, ReadOnly] private List<PopupMonoBehaviour> instantiatedPopups = new List<PopupMonoBehaviour>();

        [HideInInspector] public CodeEvent<PopupReference> OnNewCurrentPopUpSet;
        [HideInInspector] public CodeEvent OnPopUpRemoved;

        public GameObject defaultPopup_YesNoCancel; 

        private float delayBetweenPopUps = 1f;

        public void RegisterPopupGameObject(PopupMonoBehaviour popup, bool priority)
        {
            if (!instantiatedPopups.Contains(popup))
            {
                if (priority)
                {
                    instantiatedPopups.Insert(0, popup);
                }
                else
                {
                    instantiatedPopups.Add(popup);
                }
            }
        }

        public override void OnStartBehaviour()
        {
            base.OnStartBehaviour();
            CoroutineMonoBehaviour.StartCoroutine(Updater(),null);
        }

        IEnumerator Updater()
        {
            while (true)
            {
                if(instantiatedPopups.Count == 0)
                {
                    PopupMonoBehaviour.MainPopupInstance = null;
                    yield return null;
                    continue;
                }

                if (instantiatedPopups[0] == null)
                {
                    PopupMonoBehaviour.MainPopupInstance = null;
                    instantiatedPopups.RemoveAt(0);
                    yield return null;
                    continue;
                }

                if (instantiatedPopups[0].popupClosed)
                {
                    PopupMonoBehaviour.MainPopupInstance = null;
                    instantiatedPopups.RemoveAt(0);
                    yield return null;
                    continue;
                }

                int loopCounter = 0;

                for (int i = 0; i < instantiatedPopups.Count; i++)
                {
                    PopupMonoBehaviour popup = instantiatedPopups[i];

                    if(popup == null)
                    {
                        continue;
                    }

                    if(i == 0)
                    {
                        PopupMonoBehaviour.MainPopupInstance = popup;
                        popup.popupVisualsEnabled.Value = true;
                        popup.enableVisuals.Invoke(true);
                    }
                    else
                    {                        
                        popup.popupVisualsEnabled.Value = false;
                        popup.enableVisuals.Invoke(false);
                    }

                    loopCounter++;
                    if(loopCounter > 10)
                    {
                        loopCounter = 0;
                        yield return null;
                    }
                }
                yield return null;
            }
        }

        //From popup data

        [Button] public void QueuePopUp(PopupReference popUpData)
        {
            Debug.Log("Queuing New Pop Up: " + popUpData.name);
            if (currentPopUp == null) {
                SetCurrentPopUp(popUpData);
            } else if (!queuedPopUpData.Contains(popUpData)) {
                queuedPopUpData.Add(popUpData);
            }
        }

        private void SetCurrentPopUp(PopupReference newPopUp)
        {
            Debug.Log("Setting New Current Pop Up: " + newPopUp.name);
            currentPopUp = newPopUp;
           // if (newPopUp.popUpType == PopUpType.Duration)
           // {
            //    CoroutineMonoBehaviour.i.CallWithDelay(RemoveCurrentPopUp, newPopUp.popUpDuration);
           // }

            OnNewCurrentPopUpSet.Raise(newPopUp);
        }

        public void RemoveCurrentPopUp()
        {
            if (currentPopUp == null) {
                Debug.LogError("No current pop up.");//, this.gameObject); 
                return;
            }

            currentPopUp = null;
            OnPopUpRemoved.Raise();

            if (queuedPopUpData.Count > 0)
            {
                CoroutineMonoBehaviour.i.CallWithDelay(SetNextUpToActive, delayBetweenPopUps);
            }
        }

        private void SetNextUpToActive()
        {
            PopupReference nextPopUp = queuedPopUpData[0];
            queuedPopUpData.Remove(nextPopUp);
            SetCurrentPopUp(nextPopUp);
        }

        public void ShowPopUpNow(PopupReference popUpData)
        {
            queuedPopUpData.Insert(0, popUpData);

            PopupReference cacheCurrent = null;
            if (currentPopUp != null) { }
            {
                cacheCurrent = currentPopUp;
                RemoveCurrentPopUp();
            }

            SetNextUpToActive();

            queuedPopUpData.Insert(0, cacheCurrent);
        }
    }
}

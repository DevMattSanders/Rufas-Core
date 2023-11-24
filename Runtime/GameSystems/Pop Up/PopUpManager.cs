using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
//using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Events;

namespace Rufas
{
    public class PopUpManager : MonoBehaviour
    {
        public static PopUpManager Instance;

        [SerializeField, ReadOnly] private PopUpData currentPopUp;
        [SerializeField, ReadOnly] private List<PopUpData> queuedPopUpData = new List<PopUpData>();

        [HideInInspector] public CodeEvent<PopUpData> OnNewCurrentPopUpSet;
        [HideInInspector] public CodeEvent OnPopUpRemoved;

        private float delayBetweenPopUps = 1f;

        private void Awake()
        {
            if (Instance == null) { Instance = this; }
            else { Debug.LogError("One or more Pop Up Managers found!", this.gameObject); }
        }

        [Button] public void QueuePopUp(PopUpData popUpData)
        {
            Debug.Log("Queuing New Pop Up: " + popUpData.name);
            if (currentPopUp == null) {
                SetCurrentPopUp(popUpData);
            } else if (!queuedPopUpData.Contains(popUpData)) {
                queuedPopUpData.Add(popUpData);
            }
        }

        private void SetCurrentPopUp(PopUpData newPopUp)
        {
            Debug.Log("Setting New Current Pop Up: " + newPopUp.name);
            currentPopUp = newPopUp;
            if (newPopUp.popUpType == PopUpType.Duration)
            {
                this.CallWithDelay(RemoveCurrentPopUp, newPopUp.popUpDuration);
            }

            OnNewCurrentPopUpSet.Raise(newPopUp);
        }

        public void RemoveCurrentPopUp()
        {
            if (currentPopUp == null) { 
                Debug.LogError("No current pop up.", this.gameObject); 
                return;
            }

            currentPopUp = null;
            OnPopUpRemoved.Raise();

            if (queuedPopUpData.Count > 0)
            {
                this.CallWithDelay(SetNextUpToActive, delayBetweenPopUps);
            }
        }

        private void SetNextUpToActive()
        {
            PopUpData nextPopUp = queuedPopUpData[0];
            queuedPopUpData.Remove(nextPopUp);
            SetCurrentPopUp(nextPopUp);
        }
    }
}

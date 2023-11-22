using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Rufas
{
    public class PopUpUserInterface : MonoBehaviour
    {
        [Header("UI Elements")]
        [SerializeField] private TMP_Text titleTextField;
        [SerializeField] private TMP_Text descriptionTextField;
        [SerializeField] private RectTransform popUpWindow;
        [SerializeField] private Button acknowledgementButton;

        private void Start()
        {
            HidePopUpWindow();
            PopUpManager.Instance.OnNewCurrentPopUpSet.AddListener(ShowNewPopUpData);
            PopUpManager.Instance.OnPopUpRemoved.AddListener(HidePopUpWindow);
            acknowledgementButton.onClick.AddListener(OnAcknowlegementClicked);
        }

        private void OnDestroy()
        {
            PopUpManager.Instance.OnNewCurrentPopUpSet.RemoveListener(ShowNewPopUpData);
            PopUpManager.Instance.OnPopUpRemoved.RemoveListener(HidePopUpWindow);
            acknowledgementButton.onClick.RemoveListener(OnAcknowlegementClicked);
        }

        private void ShowNewPopUpData(PopUpData popUpData)
        {
            Debug.Log("Updating current pop up UI");
            popUpWindow.gameObject.SetActive(true);

            titleTextField.SetText(popUpData.popUpTitle);
            descriptionTextField.SetText(popUpData.popUpDescription);

            if (popUpData.popUpType != PopUpType.Acknowledge) {
                acknowledgementButton.gameObject.SetActive(false);
            } else if (popUpData.popUpType == PopUpType.Acknowledge) {
                acknowledgementButton.GetComponentInChildren<TMP_Text>().SetText(popUpData.acknowldgeButtonText);
            }
        }

        private void HidePopUpWindow()
        {
            popUpWindow.gameObject.SetActive(false);
        }

        [Button] private void OnAcknowlegementClicked()
        {
            PopUpManager.Instance.RemoveCurrentPopUp();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DG.Tweening;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using System.Linq;
using UnityEngine.EventSystems;

namespace Rufas
{
    public class RadialMenu : MonoBehaviour
    {
        private EventSystem eventSystem;
        [SerializeField] private bool menuOpen;
        public RadialMenuEntry selectedEntry;
        [SerializeField] private float menuRadius = 100f;

        [SerializeField] private List<RadialMenuEntry> entries;

        private void Start()
        {
            entries = GetEntries();
            eventSystem = EventSystem.current;
            if (eventSystem ==  null ) { Debug.LogError("Cannot find event system!", this.gameObject); }
        }

        private List<RadialMenuEntry> GetEntries()
        {
            RadialMenuEntry[] entiresArray = GetComponentsInChildren<RadialMenuEntry>();
            List<RadialMenuEntry> entriesList = entiresArray.ToList();
            return entriesList;
        }

        public void UpdateRadialMenu(Vector2 inputVector)
        {
            if (menuOpen == false) { return; }

            Vector2 joystickInput = inputVector;

            if (joystickInput.magnitude < 0.1f)
            {
                selectedEntry = entries[0];
            }
            else
            {
                float inputAngle = Mathf.Atan2(joystickInput.y, joystickInput.x) * Mathf.Rad2Deg;
                float minAngleDifference = float.MaxValue;
                foreach (RadialMenuEntry entry in entries)
                {
                    if (entry != selectedEntry)
                    {
                        entry.myButton.interactable = false;
                    }

                    // Calculate the angle of the rectTransform
                    Vector2 direction = entry.GetComponent<RectTransform>().position - transform.position;
                    float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

                    // Calculate the difference in angles
                    float angleDifference = Mathf.Abs(Mathf.DeltaAngle(inputAngle, angle));

                    // Check if the current rectTransform has a smaller angle difference
                    if (angleDifference < minAngleDifference)
                    {
                        minAngleDifference = angleDifference;
                        selectedEntry = entry;
                        //eventSystem.SetSelectedGameObject(selectedEntry.gameObject);
                    }
                }

                selectedEntry.GetComponent<Button>().interactable = true;
            }
        }

        [Button] public void OpenRadialMenu()
        {
            menuOpen = true;
            
            //entries = GetEntries();
            float radiansOfSeperation = (Mathf.PI * 2) / entries.Count;
            for (int i = 0; i < entries.Count; i++)
            {
                entries[i].myButton.interactable = false;

                entries[i].gameObject.SetActive(true);
                float x = Mathf.Sin(radiansOfSeperation * i) * menuRadius;
                float y = Mathf.Cos(radiansOfSeperation * i) * menuRadius;

                float delayTime = 0.025f * i;
                entries[i].TweenOpen(new Vector3(x, y, 0), 0.3f, delayTime);
            }

            selectedEntry = entries[0];
            selectedEntry.myButton.interactable = true;
        }

        [Button] public void CloseRadialMenu()
        {
            menuOpen = false;

            //entries = GetEntries();
            for (int i = 0; i < entries.Count; i++)
            {
                float delayTime = 0.025f * i;
                entries[i].TweenClose(0.2f, delayTime);
            }

            if (selectedEntry != null)
            {
                selectedEntry.myButton.onClick.Invoke();
            }
        }
    }
}

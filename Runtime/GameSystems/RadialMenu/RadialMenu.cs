using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DG.Tweening;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Rufas
{
    public class RadialMenu : MonoBehaviour
    {
        [SerializeField] private bool menuOpen;
        [SerializeField] private RadialMenuEntry selectedEntry;
        [SerializeField] private float menuRadius = 100f;
        [Space]
        [SerializeField] private List<RadialMenuEntry> entries;

        [Header("Placeholder")]
        [SerializeField] private PlayerInput playerInput;

        public void ToggleMenu()
        {
            if (playerInput.actions["Trigger"].WasPressedThisFrame())
            {
                OpenRadialMenu();
            }
            else if (playerInput.actions["Trigger"].WasReleasedThisFrame())
            {
                CloseRadialMenu();
            }
        }

        public void UpdateRadialMenu()
        {
            if (menuOpen == false) { return; }

            Vector2 joystickInput = playerInput.actions["Joystick"].ReadValue<Vector2>().normalized;

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
                    entry.myButton.interactable = false;

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

                    }
                }

                selectedEntry.GetComponent<Button>().interactable = true;
            }
        }

        public void OpenRadialMenu()
        {
            menuOpen = true;
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

        public void CloseRadialMenu()
        {
            menuOpen = false;
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

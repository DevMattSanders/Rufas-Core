using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Rufas
{
    public class VRInput : MonoBehaviour
    {
        public static VRInput instance;

        private void Awake()
        {      
            if(instance != null)
            {
                Debug.Log("Multiple VR Input Scriptable Objects Found");
            }

            instance = this;
        }

        [Header("Left Hand")]
        public InputActionProperty L_PrimaryButton;
        public InputActionProperty L_SecondaryButton;
        public InputActionProperty L_Trigger;
        public InputActionProperty L_TriggerButton;
        public InputActionProperty L_Grip;
        public InputActionProperty L_GripButton;
        public InputActionProperty L_PrimaryAxis;
        public InputActionProperty L_PrimaryAxisClick;
        public InputActionProperty L_MenuButton;

        [Header("Right Hand")]
        public InputActionProperty R_PrimaryButton;
        public InputActionProperty R_SecondaryButton;
        public InputActionProperty R_Trigger;
        public InputActionProperty R_TriggerButton;
        public InputActionProperty R_Grip;
        public InputActionProperty R_GripButton;
        public InputActionProperty R_PrimaryAxis;
        public InputActionProperty R_PrimaryAxisClick;
        public InputActionProperty R_MenuButton;

    }
}

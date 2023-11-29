using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IOActions
{
    public class Gate : OutputComponent
    {
        private Animator animator;

        private void Awake()
        {
            animator = GetComponent<Animator>();
        }

        public override void InputSignalRecived(InputComponent inputComponent)
        {
            base.InputSignalRecived(inputComponent);
            ToggleGateState();
        }

        private void ToggleGateState()
        {
            if (animator == null) { animator = GetComponent<Animator>(); }

            animator.SetTrigger("ToggleState");
        }
    }
}

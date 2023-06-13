using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rufas
{
    public class DestroyHingeOnInput : MonoBehaviour
    {
        [SerializeField] private HingeJoint joint;

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.H))
            {
                Destroy(joint);
            } 
            else if (joint == null)
            {
                joint = GetComponent<HingeJoint>();
            }
        }
    }
}


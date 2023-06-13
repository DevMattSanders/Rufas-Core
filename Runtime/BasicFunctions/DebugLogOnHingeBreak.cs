using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rufas
{
    [RequireComponent(typeof(Joint))]
    public class DebugLogOnHingeBreak : MonoBehaviour
    {
        private void OnJointBreak(float breakForce)
        {
            Debug.Log("A joint has just been broken!, force: " + breakForce);
        }
    }
}

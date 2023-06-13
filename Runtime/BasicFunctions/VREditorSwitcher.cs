using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rufas
{
    public class VREditorSwitcher : MonoBehaviour
    {
        public static VREditorSwitcher Instance;

        public enum DebugState { VR, Editor }
        public DebugState debugState;

        [Space]

        [SerializeField] private GameObject vrRig;
        [SerializeField] private GameObject editorRig;

        private void Awake()
        {
            if (Instance != null)
            {
                Debug.LogError("Two Instances of the Solar System Simulation Exist. Time To Destroy My Self...");
                Destroy(this.gameObject);
            }
            else
            {
                Instance = this;
            }
        }

        private void OnValidate()
        {
            UpdateDebugState();
        }

        private void UpdateDebugState()
        {
            if (debugState == DebugState.VR)
            {
                vrRig.SetActive(true);
                editorRig.SetActive(false);
            }
            else
            {
                vrRig.SetActive(false);
                editorRig.SetActive(true);
            }
        }
    }
}

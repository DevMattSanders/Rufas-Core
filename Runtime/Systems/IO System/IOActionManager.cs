using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IOActions
{
    public class IOActionManager : MonoBehaviour
    {
        public static IOActionManager Instance;

        [FoldoutGroup("Active IO Components")] public List<InputComponent> inputComponents = new List<InputComponent>();
        [Space]
        [FoldoutGroup("Active IO Components")] public List<OutputComponent> outputActionComponents = new List<OutputComponent>();

        private void Awake()
        {
            if (Instance == null) { Instance = this; }
            else
            {
                Debug.LogError("One or more instances of IOActionManager found!", this.gameObject);
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rufas
{
    [RequireComponent(typeof(CanvasGroup))]
    public class ShowCanvasGroupOnState : MonoBehaviour
    {
        public SoState state;        
        private CanvasGroup canvasGroup;

        private void Awake() { canvasGroup = GetComponent<CanvasGroup>(); }

        private void Start() { state.onStateActiveChanged.AddListener(RefreshVisuals); }
        private void OnDestroy() { state.onStateActiveChanged.AddListener(RefreshVisuals); }


        private void RefreshVisuals(bool val)
        {
            canvasGroup.interactable = val;
            canvasGroup.blocksRaycasts = val;

            if (val)
            {
                canvasGroup.alpha = 1;
            }
            else
            {
                canvasGroup.alpha = 0;
            }
            
        }
    }
}

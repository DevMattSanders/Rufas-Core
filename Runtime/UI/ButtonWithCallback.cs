using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
namespace Rufas
{
    public class ButtonWithCallback : Button
    {        
        public BoolWithCallback pointerHovering;
        public CodeEvent pointerEntered;
        public CodeEvent pointerExit;
        public CodeEvent pointerClick;
                
        public override void OnPointerEnter(PointerEventData eventData)
        {
            base.OnPointerEnter(eventData);

            pointerHovering.Value = true;

            pointerEntered.Raise();
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            base.OnPointerExit(eventData);

            pointerHovering.Value = false;

            pointerExit.Raise();
        }

        public override void OnPointerClick(PointerEventData eventData)
        {
            base.OnPointerClick(eventData);
            pointerClick.Raise();
        }       
      

        //public LocalBoolVariable 
      
    }
}

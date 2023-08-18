using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rufas
{
    [CreateAssetMenu(menuName = "Rufas/Events/VoidEvent")]
    public class VoidEvent : ScriptableObject
    {
        private CodeEvent voidEvent;

        public void AddListener(Action listener)
        {
            voidEvent.AddListener(listener);
        }
        
        public void RemoveListener(Action listener)
        {
            voidEvent.RemoveListener(listener);
        }

        //Ease of use for adapting existing events to voidEvents
        public void Call() { voidEvent.Raise(); }
        public void Invoke() { voidEvent.Raise(); }

        [Button]
        public void Raise() { voidEvent.Raise(); }

       

    }
}

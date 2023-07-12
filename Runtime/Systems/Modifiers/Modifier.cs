using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rufas.Modifiers
{
    [CreateAssetMenu(fileName = "New Modifier", menuName = "Hilda/Modifiers/Modifier")]
    public class Modifier : ScriptableObject
    {
        public bool shouldTick = false;

        public float duration;
        public bool IsActive => duration > 0;

        [SerializeField] public float damagePerTick;

        [Header("Modifier Data")]
        public Sprite icon;

        public virtual bool CanAddModifer(ModifierTarget target)
        {
            return true;
        }

        public virtual void EnableModifier(ModifierTarget target)
        {
            
        }

        public virtual void TickModifier(ModifierTarget target)
        {
            duration -= Time.deltaTime;
        }

        public virtual void DisableModifier(ModifierTarget target)
        {

        }

       
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rufas.Modifiers
{
    [CreateAssetMenu(fileName = "New Modifier", menuName = "Hilda/Modifiers/Modifier")]
    public class Modifier : ScriptableObject
    {
        public bool shouldTick = false;
        [SerializeField] private float duration;
        public bool IsActive => duration > 0;

        [SerializeField] public float damagePerTick;

        [Header("Modifier Data")]
        public GameObject iconPrefab;

        public virtual bool CanAddModifer(ModifierTarget target)
        {
            return true;
        }

        public virtual void EnableModifier(ModifierTarget target)
        {
            
        }

        public virtual void DisableModifier()
        {

        }

        public virtual void TickModifer(ModifierTarget target)
        {
            duration -= Time.deltaTime;
        }
    }
}

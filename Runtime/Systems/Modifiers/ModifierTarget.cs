using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using Rufas;

namespace Rufas.Modifiers
{
    [System.Serializable] 
    public class ModifierTarget
    {
        [HideInInspector] public Transform transform;
        public List<Modifier> modifiers = new List<Modifier>();

        public CodeEvent<Modifier> OnModifierAdded;
        public CodeEvent<Modifier> OnModifierRemoved;

        private void UpdateCurrentModifiers()
        {
            for (int index = modifiers.Count - 1; index >= 0; index--)
            {
                if (modifiers[index].shouldTick) { modifiers[index].TickModifier(this); }
                
                if (modifiers[index].IsActive == false)
                {
                    modifiers[index].DisableModifier(this);
                    RemoveModifier(modifiers[index]);
                }
            }
        }

        public void AddModifier(Modifier templateModifier)
        {
            Modifier newModifier = ScriptableObject.Instantiate(templateModifier);
            bool canAdd = newModifier.CanAddModifer(this);
            if (canAdd)
            {
                modifiers.Add(newModifier);
                newModifier.EnableModifier(this);
                OnModifierAdded.Raise(newModifier);
            }
        }

        public void RemoveModifier(Modifier modifierInstance)
        {
            modifierInstance.DisableModifier(this);
            modifiers.Remove(modifierInstance);
            OnModifierRemoved.Raise(modifierInstance);
        }
    }
}
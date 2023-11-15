using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rufas.Achivements
{
    [CreateAssetMenu(fileName = "New Achivement", menuName = "Rufas/Achivement System/Achivement")]
    public class Achievement : ScriptableObject
    {
        public string apiName;
        public bool unlocked = false;
        [Space]
        public UnlockType unlockType;

        [ShowIf("unlockType", UnlockType.Count), SerializeField, ReadOnly] private ulong counter;
        
        [ShowIf("unlockType", UnlockType.Bitfield), SerializeField, ReadOnly] private string bitField = "0000000";

        public ulong GetCounterProgress() {
            return counter;
        }

        public string GetBitFieldProgress() { 
            return bitField;
        }

        public bool IsUnlocked()
        {
            return unlocked;
        }

        [ShowIf("unlockType", UnlockType.Count), Button()] public void SetCounter(ulong newValue)
        {
            counter = newValue;
        }

        [ShowIf("unlockType", UnlockType.Bitfield), Button()] public void SetBitfield(string newValue)
        {
            bitField = newValue;
        }
    }

    public enum UnlockType
    { 
        Simple,
        Count,
        Bitfield
    }
}

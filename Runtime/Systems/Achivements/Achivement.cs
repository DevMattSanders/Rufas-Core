using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rufas.Achivements
{
    [CreateAssetMenu(fileName = "New Achivement", menuName = "Rufas/Achivement System/Achivement")]
    public class Achivement : ScriptableObject
    {
        public string apiName;
        public bool unlocked = false;
    }
}

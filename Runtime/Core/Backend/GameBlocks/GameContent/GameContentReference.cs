using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rufas
{
    [CreateAssetMenu(menuName = "Rufas/GameContent/GameContentReference")]
    public class GameContentReference : ScriptableWithUniqueID
    {
        public GameObject reference;
    }
}

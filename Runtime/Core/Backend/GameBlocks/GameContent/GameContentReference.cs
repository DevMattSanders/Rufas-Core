using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rufas
{
    [CreateAssetMenu(menuName = "Rufas/GameContent/GameContentReference")]
    public class GameContentReference : ScriptableWithUniqueID
    {
        [HideLabel,PreviewField(alignment: ObjectFieldAlignment.Left,height: 100)]
        public GameObject reference;    
    }
}

using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rufas
{
    [CreateAssetMenu(menuName = "Rufas/Variable/GameObject")]
    public class GameObjectVariable : SuperScriptable
    {
        public GameObject value;
    }
}

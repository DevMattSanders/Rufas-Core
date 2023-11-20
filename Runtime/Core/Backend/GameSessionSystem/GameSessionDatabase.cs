using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace Rufas
{
    [CreateAssetMenu]
    public class GameSessionDatabase : ScriptableWithCallbacks
    {
        public GameSystemParentClass[] gameSystems;

#if UNITY_EDITOR
        [PropertySpace(spaceBefore: 20, SpaceAfter = 20)]
        [PropertyOrder(0)]
        [Button(Stretch = true, ButtonAlignment = 0.5f, ButtonHeight = 30)]
        public void Refresh()
        {

            gameSystems = RufasStatic.GetAllScriptables_ToArray<GameSystemParentClass>();

            //allGlobalScriptables = ExtensionMethods.GetAllInstances<GlobalScriptable>();
            // Debug.Log("Finished Refreshing Global Scriptable Objects");
        }
#endif
    }
}

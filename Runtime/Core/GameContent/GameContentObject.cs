using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rufas
{
    [CreateAssetMenu(menuName = "Rufas/GameContent/GameContendObject")]
    public class GameContentObject : ScriptableObject
    {
#if UNITY_EDITOR
        [ShowIf("AlwaysShow")]
#endif
        [ReadOnly]
        //[DisableIf("DisableIDField")]//,InlineButton("Refresh")]
        public string UniqueID;

        private bool DisableIDField()
        {
            if(string.IsNullOrEmpty(UniqueID)) return false;

            return true;
        }

#if UNITY_EDITOR
       // [Button]
        public void Refresh()
        {
            Database.Instance.RefreshReplicationKey(this);
        }

        private int counter = 0;
        private bool AlwaysShow() { counter++; if (counter >= 10) { counter = 0; Debug.Log(name + " Selected"); Refresh(); } return true; }

#endif

        /*
        private void OnValidate()
        {
            Debug.Log("Validate: " + name);

            Refresh();
        }

        private void OnEnable()
        {
            Debug.Log("OnEnable: " + name);

            Refresh();
        }
        */
        //[dontva]
        /*
        private void Awake()
        {
            Debug.Log("Awake: " + name);

            Refresh();
        }
        */
    }
}

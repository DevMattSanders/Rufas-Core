using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rufas
{
    public class SuperScriptableMonobehaviourEvents : MonoBehaviour
    {
        public SuperScriptableDatabase[] database;

        private void Awake()
        {
            if (database == null) database = Resources.FindObjectsOfTypeAll<SuperScriptableDatabase>();

            if (database.Length > 1) { Debug.LogError("Multiple super scriptable databases found!"); }

            foreach (SuperScriptableDatabase nextDatabase in database)
            {
                nextDatabase.TriggerAll_SoOnAwake();
            }

            foreach(SuperScriptableDatabase nextDatabase in database)
            {
                nextDatabase.TriggerAll_SoOnLoad();
            }
        }
        private void Start()
        {
            if (database == null) database = Resources.FindObjectsOfTypeAll<SuperScriptableDatabase>();
            
            if (database.Length > 1) { Debug.LogError("Multiple super scriptable databases found!"); }

            foreach (SuperScriptableDatabase nextDatabase in database)
            {
                nextDatabase.TriggerAll_SoOnStart();
            }
        }

        private void OnDestroy()
        {
            if (database == null) database = Resources.FindObjectsOfTypeAll<SuperScriptableDatabase>();
            
            if (database.Length > 1) { Debug.LogError("Multiple super scriptable databases found!"); }

            foreach (SuperScriptableDatabase nextDatabase in database)
            {
                nextDatabase.TriggerAll_SoOnEnd();
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rufas
{
    public class SuperScriptableMonobehaviourEvents : MonoBehaviour
    {
        public SuperScriptableDatabase database;

        public bool callSoOnStartAtEndOfAwake = false;

        private void Awake()
        {
           // if (database == null) database = SuperScriptableDatabase.superScriptableDatabaseInstance;// Resources.FindObjectsOfTypeAll<SuperScriptableDatabase>();

            //  if (database.Length > 1) { Debug.LogError("Multiple super scriptable databases found!"); }

            //  foreach (SuperScriptableDatabase nextDatabase in database)
            //{
            SuperScriptableDatabase.superScriptableDatabaseInstance.TriggerAll_SoOnAwake();
           // }

            if (callSoOnStartAtEndOfAwake)
            {
                SuperScriptableDatabase.superScriptableDatabaseInstance.TriggerAll_SoOnStart();
                /*
                if (database == null) database = Resources.FindObjectsOfTypeAll<SuperScriptableDatabase>();

                if (database.Length > 1) { Debug.LogError("Multiple super scriptable databases found!"); }

                foreach (SuperScriptableDatabase nextDatabase in database)
                {
                    nextDatabase.TriggerAll_SoOnStart();
                }
                */
            }
        }
        private void Start()
        {
            if (!callSoOnStartAtEndOfAwake)
            {
                SuperScriptableDatabase.superScriptableDatabaseInstance.TriggerAll_SoOnStart();
                /*
                if (database == null) database = Resources.FindObjectsOfTypeAll<SuperScriptableDatabase>();

                if (database.Length > 1) { Debug.LogError("Multiple super scriptable databases found!"); }

                foreach (SuperScriptableDatabase nextDatabase in database)
                {
                   
                }
                */
            }
        }

        private void OnDestroy()
        {
            SuperScriptableDatabase.superScriptableDatabaseInstance.TriggerAll_SoOnEnd();
            /*
            if (database == null) database = Resources.FindObjectsOfTypeAll<SuperScriptableDatabase>();
            
            if (database.Length > 1) { Debug.LogError("Multiple super scriptable databases found!"); }

            foreach (SuperScriptableDatabase nextDatabase in database)
            {
                nextDatabase.TriggerAll_SoOnEnd();
            }
            */
        }
    }
}

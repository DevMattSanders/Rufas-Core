using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rufas
{
    public class SuperScriptableMonobehaviourEvents : MonoBehaviour
    {
        public SuperScriptableDatabase database;

        public bool callSoOnStartAtEndOfAwake = false;

        public void Awake()
        {
           // base.Awake_AfterInitialisation();
        
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

        //start

        public void Start()
        {
           // base.Start_AfterInitialisation();
        
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

        public void OnDestroy()
        {
           // base.OnDestroy();
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

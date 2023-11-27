using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rufas
{
    public class GameSystem<T> : GameSystemParentClass where T : GameSystemParentClass
    {
        public static List<GameSystemParentClass> gameSystems = new List<GameSystemParentClass>();

           
        public override void BehaviourToRunBeforeAwake()
        {
            base.BehaviourToRunBeforeAwake();

            gameSystems.Add(this);
            TriggerInstance();
        }

        
        public override void TriggerInstance() { base.TriggerInstance(); bool tempTrigger = Instance; }

        private static T instance;
        public static T Instance
        {
            get
            {
                if(instance == null)
                {
                    if (Application.isPlaying == false)
                    {
#if UNITY_EDITOR
                        T[] editorFound = RufasStatic.GetAllScriptables_ToArray<T>();
                        if(editorFound.Length > 0) instance = editorFound[0];
#endif
                    }
                    else
                    {
                        foreach (GameSystemParentClass next in gameSystems)
                        {
                            if (next is T)
                            {
                                instance = next as T;
                            }
                        }
                    }
                }

                //Debug.Log(instance + " " + gameSystems.Count);
                return instance;
            }
        }


    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rufas
{
    public class GameSystem<T> : GameSystemParentClass where T : GameSystemParentClass
    {
        public static List<GameSystemParentClass> gameSystems = new List<GameSystemParentClass>();

        /// <summary>
        /// Forces any GameSystem managers to not use OnEnable! OnEnable is used when processing game system managers and can be called accidentally for generated instances!
        /// </summary>
        public sealed override void OnEnable()
        {
            base.OnEnable();
        }

        public override void BehaviourToRunBeforeAwake()
        {
            base.BehaviourToRunBeforeAwake();

            gameSystems.Add(this);
            TriggerInstance();
        }

        
        public void TriggerInstance() { bool tempTrigger = Instance; }

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

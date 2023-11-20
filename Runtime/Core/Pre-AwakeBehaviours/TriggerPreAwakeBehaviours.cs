using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Rufas
{
    public static class TriggerPreAwakeBehaviours
    {

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void FindAndLoadAllPreSceneLoaders()
        {
            PreAwakeBehaviour[] loadBeforeStartObjects = Resources.LoadAll<PreAwakeBehaviour>("");
            List<PreAwakeBehaviour> loadBeforeStartList = new List<PreAwakeBehaviour>(loadBeforeStartObjects);
            //Loop through each 'LoadBeforeStart' object and check it's priority.
            //If equal to the priority value, run it's 'BehaviourToRunBeforeStart' method them remove from the array.            

            int priority = 0;

            while (priority < 11)
            {
                foreach(PreAwakeBehaviour next in loadBeforeStartList)
                {
                    if (next.priority == priority) next.BehaviourToRunBeforeStart();
                }
                priority++;
            }

        }

    }
}

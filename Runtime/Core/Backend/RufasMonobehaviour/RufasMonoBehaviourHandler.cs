using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rufas
{
    public class RufasMonoBehaviourHandler : GameSystem<RufasMonoBehaviourHandler>
    {

        public static List<RufasMonoBehaviour> waitingForAwake = new List<RufasMonoBehaviour>();
        public static List<RufasMonoBehaviour> waitingForStart = new List<RufasMonoBehaviour>();
        public static List<RufasMonoBehaviourWithUpdate> updateBehaviours = new List<RufasMonoBehaviourWithUpdate>(); 
        public static bool awakeCalled;
        public static bool startCalled;

       

        //public void
        [ShowInInspector, ReadOnly]
        public List<RufasMonoBehaviour> debugWaitingForAwake
        {
            get { return waitingForAwake; }
        }
        [ShowInInspector, ReadOnly]
        public List<RufasMonoBehaviour> debugWaitingForStart
        {
            get { return waitingForStart; }
        }
                
        [ShowInInspector, ReadOnly]
        public List<RufasMonoBehaviourWithUpdate> debugUpdateBehaviours
        {
            get { return updateBehaviours; }
        }

        [ShowInInspector, ReadOnly]
        private bool debugAwakeCalled
        {
            get { return awakeCalled; }
        }

        [ShowInInspector, ReadOnly]
        private bool debugStartCalled
        {
            get { return startCalled; }
        }              

#if UNITY_EDITOR
        public override SdfIconType EditorIcon()
        {
            return SdfIconType.CodeSlash;
        }
#endif
        public override string DesiredPath()
        {
            return "Rufas/Framework/" + "R.MonoBehaviours";// this.name;
        }

        public override bool IsRufasSystem()
        {
            return true;
        }

        public override void PreInitialisationBehaviour()
        {
            base.PreInitialisationBehaviour();

            //Debug.Log(RufasMonoBehaviourHandler.Instance);

            awakeCalled = false;

            startCalled = false;

         //   GameSystemManager.OnAllGameSystemsInitialized.AddListener(OnAllSystemsInitialised);
        }

        public override void OnAwakeBehaviour()
        {
            base.OnAwakeBehaviour();
            // foreach (RufasMonoBehaviour next in waitingForAwake)

            while (waitingForAwake.Count > 0)
            {
                for (int i = 0; i < waitingForAwake.Count; i++)
                {
                    RufasMonoBehaviour next = waitingForAwake[i];

                    if(next == null)
                    {
                        waitingForAwake.RemoveAt(i);
                        i--;
                        continue;
                    }

                   // try
                   // {
                        next.Awake_AfterInitialisation();
                   // }
                    //catch
                    //{

                    //}

                    waitingForAwake.Remove(next);
                    i--;
                }
            }

         

            RufasMonoBehaviour.callingAwakeAfterInit.Raise();

            awakeCalled = true;
        }

        public override void OnStartBehaviour()
        {
            base.OnStartBehaviour();
            foreach (RufasMonoBehaviour next in waitingForStart)
            {
                if (!next.enabled) continue;

                try
                {
                    next.Start_AfterInitialisation();
                }
                catch
                {

                }
            }           

            RufasMonoBehaviour.callingStartAfterInit.Raise();

            startCalled = true;
        }

        public override void OnUpdateBehaviour()
        {
            base.OnUpdateBehaviour();
            foreach (RufasMonoBehaviourWithUpdate next in updateBehaviours)
            {
                if (!next.enabled) continue;

                try
                {
                    next.Update_AfterInitialisation();
                }
                catch
                {
                    Debug.LogError("Caught Error: " + next.gameObject.name + " | " + next.name);
                }
            }
        }
    }
}

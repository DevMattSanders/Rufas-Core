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
        public static bool awakeCalled;
        public static bool startCalled;

        //[ShowInInspector]
        [SerializeField, ReadOnly]
        private bool debugAwakeCalled
        {
            get { return awakeCalled; }
        }

        //[ShowInInspector]
        [SerializeField, ReadOnly]
        private bool debugStartCalled
        {
            get { return startCalled; }
        }

        public override bool AutogenerateGameSystem()
        {
            return true;
        }

        public override bool IsRufasSystem()
        {
            return true;
        }

        public override void BehaviourToRunBeforeAwake()
        {
            base.BehaviourToRunBeforeAwake();

            //Debug.Log(RufasMonoBehaviourHandler.Instance);

            awakeCalled = false;

            startCalled = false;

         //   GameSystemManager.OnAllGameSystemsInitialized.AddListener(OnAllSystemsInitialised);
        }

        public override void OnAwakeBehaviour()
        {
            base.OnAwakeBehaviour();
            foreach (RufasMonoBehaviour next in waitingForAwake)
            {
                next.Awake_AfterInitialisation();
            }
            awakeCalled = true;
        }

        public override void OnStartBehaviour()
        {
            base.OnStartBehaviour();
            foreach (RufasMonoBehaviour next in waitingForStart)
            {
                next.Start_AfterInitialisation();
            }
            startCalled = true;
        }
        /*
        private void OnAllSystemsInitialised()
        {
            GameObject rufasMonoBehaviourLink = new GameObject("rufasMonoBehaviourLink");
            rufasMonoBehaviourLink.AddComponent<RufasMonoBehaviourLink>();
        }
        */
        /*
        public static void Register(RufasMonobehaviour mono)
        {
            rufasMonobehaviours.Add(mono);

            if (awakeCalled)
            {
                mono.Awake_AfterInitialisation();
            }

            if (startCalled)
            {
                mono.Start_AfterInitialisation();
            }
        }

        public void Unregister(RufasMonobehaviour mono)
        {
            rufasMonobehaviours.Remove(mono);
        }
        */
       // public void AwakeCalled()
       // {
       
       // }

       // public void StartCalled()
      //  {
       
      //  }
    }
}

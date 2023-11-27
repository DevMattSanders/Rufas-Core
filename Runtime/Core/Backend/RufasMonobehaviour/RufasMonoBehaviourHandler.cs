using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rufas
{
    public class RufasMonoBehaviourHandler : GameSystem<RufasMonoBehaviourHandler>
    {

        [SerializeField, ReadOnly] private List<RufasMonobehaviour> rufasMonobehaviours = new List<RufasMonobehaviour>();
        [SerializeField, ReadOnly] private bool awakeCalled;
        [SerializeField,ReadOnly] private bool startCalled;
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

            awakeCalled = false;

            startCalled = false;

            GameSystemManager.OnAllGameSystemsInitialized.AddListener(OnAllSystemsInitialised);
        }

        private void OnAllSystemsInitialised()
        {
            GameObject rufasMonoBehaviourLink = new GameObject("rufasMonoBehaviourLink");
            rufasMonoBehaviourLink.AddComponent<RufasMonoBehaviourLink>();
        }

        public void Register(RufasMonobehaviour mono)
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

        public void AwakeCalled()
        {
            foreach(RufasMonobehaviour next in rufasMonobehaviours)
            {
                next.Awake_AfterInitialisation();
            }
            awakeCalled = true;
        }

        public void StartCalled()
        {
            foreach (RufasMonobehaviour next in rufasMonobehaviours)
            {
                next.Start_AfterInitialisation();
            }
            startCalled = true;
        }
    }
}

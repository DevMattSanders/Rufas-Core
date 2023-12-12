using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;
using Unity.Services.Analytics;
using Sirenix.OdinInspector;

namespace Rufas.UnitySystems
{
    public class UnityAnalytics : GameSystem<UnityAnalytics>
    {
        [SerializeField] private bool unityAnalyticsEnabled = true;
        [ReadOnly, SerializeField] private bool analyticsStartedDataCollection;

        public override SdfIconType EditorIcon() { return SdfIconType.BarChartSteps; }
        public override string DesiredPath() { return "Platform & Third Party/Unity/Analytics"; }

        public override void PreInitialisationBehaviour()
        {
            base.PreInitialisationBehaviour();            
            UnityAuthenticationSystem.UnityAuthenticationCompleted.AddListener(OnUnityLoggedIn);
        }

        public override void EndOfApplicaitonBehaviour()
        {
            base.EndOfApplicaitonBehaviour();
            if (analyticsStartedDataCollection == false) AnalyticsService.Instance.StopDataCollection();
        }

        public void OnUnityLoggedIn()
        {
            Debug.Log("Ask Player For Consent To Send Analytics Here!");
            TriggerUnityAnalytics();
        }

        private void TriggerUnityAnalytics()
        {
            AnalyticsService.Instance.StartDataCollection();
            analyticsStartedDataCollection = true;
        }


    }
}

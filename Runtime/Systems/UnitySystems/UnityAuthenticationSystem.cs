using System.Collections;
using System.Collections.Generic;
using Unity.Services.Core;
using Unity.Services.Authentication;
using UnityEngine;
using Rufas;
using JetBrains.Annotations;
using Sirenix.OdinInspector;
using Unity.Services.Core.Environments;

namespace Rufas.UnitySystems
{
    public class UnityAuthenticationSystem : GameSystem<UnityAuthenticationSystem>
    {
        public static CodeEvent OnUnityAuthenticationComplete;
        public static bool isUnityAuthenticationCompleted;
        public string PlayerID;

        //[Header("Void Event that triggers Unitys Authentication!")]
        //[Required]
        //public VoidEvent onPlatformCompletedLogin;


        //[TitleGroup("Oculus Auth", horizontalLine: true)]
        //[HorizontalGroup("Oculus Auth/H")] public ULongVariable oculusLogin;
        //[HorizontalGroup("Oculus Auth/H")] public StringVariable oculusProof;


//#if UNITY_EDITOR
        public override SdfIconType EditorIcon() { return SdfIconType.Globe2; }
//#endif

        public override string DesiredPath()
        {
            return "Platform & Third Party/Unity/Unity Authentication";
        }

        public override void OnAwakeBehaviour()
        {
            base.OnAwakeBehaviour();
            PlayerID = "No-ID-Found";
        }

        public override void EndOfApplicaitonBehaviour()
        {
            base.EndOfApplicaitonBehaviour();
            PlayerID = "No-ID-Found";
        }

        public override void PreInitialisationBehaviour()
        {
            base.PreInitialisationBehaviour();
           // onPlatformCompletedLogin.AddListener(InitAndSignIn);            
        }

        private void PostSignIn()
        {
            PlayerID = AuthenticationService.Instance.PlayerId;
            isUnityAuthenticationCompleted = true;
            OnUnityAuthenticationComplete.Raise();
        }

        
        //Gets called by the oculusAppManager
        public async void SignInWithOculus(ulong oculusUserID,string oculusProofCode)
        {
            await UnityServices.InitializeAsync();
            await AuthenticationService.Instance.SignInWithOculusAsync(oculusProofCode, oculusUserID.ToString());
            PostSignIn();
        }

        //Quick and easy way.
        public async void SignInAnonymously()
        {
            await UnityServices.InitializeAsync();
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
            PostSignIn();
        }

    }
}
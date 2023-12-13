using System.Collections;
using System.Collections.Generic;
using Unity.Services.Core;
using Unity.Services.Authentication;
using UnityEngine;
using Rufas;
using JetBrains.Annotations;
using Sirenix.OdinInspector;

namespace Rufas.UnitySystems
{
    public class UnityAuthenticationSystem : GameSystem<UnityAuthenticationSystem>
    {
        public static CodeEvent UnityAuthenticationCompleted;

        //[Header("Void Event that triggers Unitys Authentication!")]
        [Required]
        public VoidEvent onPlatformCompletedLogin;


        [TitleGroup("Oculus Auth", horizontalLine: true)]
        [HorizontalGroup("Oculus Auth/H")] public ULongVariable oculusLogin;
        [HorizontalGroup("Oculus Auth/H")] public StringVariable oculusProof;

       

        public override SdfIconType EditorIcon() { return SdfIconType.Globe2; }

        public override string DesiredPath()
        {
            return "Platform & Third Party/Unity/Unity Authentication";
        }

        public override void PreInitialisationBehaviour()
        {
            base.PreInitialisationBehaviour();
            onPlatformCompletedLogin.AddListener(InitAndSignIn);
        }

        private async void InitAndSignIn()
        {
            await UnityServices.InitializeAsync();

            await AuthenticationService.Instance.SignInWithOculusAsync(oculusProof.Value, oculusLogin.Value.ToString());


            Debug.Log("Unity Authentification Completed! PlayerID: " + AuthenticationService.Instance.PlayerId);
            UnityAuthenticationCompleted.Raise();
        }
    }
}
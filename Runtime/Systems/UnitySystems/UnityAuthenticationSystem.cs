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
        public static CodeEvent UnityAuthenticationCompleted;

        //[Header("Void Event that triggers Unitys Authentication!")]
        [Required]
        public VoidEvent onPlatformCompletedLogin;


        [TitleGroup("Oculus Auth", horizontalLine: true)]
        [HorizontalGroup("Oculus Auth/H")] public ULongVariable oculusLogin;
        [HorizontalGroup("Oculus Auth/H")] public StringVariable oculusProof;


//#if UNITY_EDITOR
        public override SdfIconType EditorIcon() { return SdfIconType.Globe2; }
//#endif

        public override string DesiredPath()
        {
            return "Platform & Third Party/Unity/Unity Authentication";
        }

        public override void PreInitialisationBehaviour()
        {
            base.PreInitialisationBehaviour();
           // Debug.Log("UNITY INIT: 1");
            onPlatformCompletedLogin.AddListener(InitAndSignIn);

            Debug.Log("ID HERE: " + onPlatformCompletedLogin.GetInstanceID());
        }

        private async void InitAndSignIn()
        {
            //#if !UNITY_EDITOR
            //        var options = new InitializationOptions();
            //        options.SetEnvironmentName("production");
            //#endif
            //Debug.Log("UNITY INIT: 2");
            await UnityServices.InitializeAsync();
            // Debug.Log("UNITY INIT: 3");
            //Debug.Log(oculusProof.Value + " " + oculusLogin.Value.ToString());

            await AuthenticationService.Instance.SignInWithOculusAsync(oculusProof.Value, oculusLogin.Value.ToString());
           

            Debug.Log("Unity Authentification Completed! PlayerID: " + AuthenticationService.Instance.PlayerId);
            UnityAuthenticationCompleted.Raise();


        }
    }
}
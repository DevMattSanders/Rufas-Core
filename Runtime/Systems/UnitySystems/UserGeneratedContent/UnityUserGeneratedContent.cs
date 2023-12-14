using Rufas;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Unity.Services.Ugc;
using UnityEngine;

namespace Rufas.UnitySystems
{
    public class UnityUserGeneratedContent : GameSystem<UnityUserGeneratedContent>
    {

        [SerializeField] private bool ugcSystemReadyToGo;


        public override string DesiredPath()
        {
            return "Platform & Third Party/Unity/User Generated Content";
        }

        public override SdfIconType EditorIcon()
        {
            return SdfIconType.Boxes;
        }

        public override void PreInitialisationBehaviour()
        {
            base.PreInitialisationBehaviour();
            ugcSystemReadyToGo = false;
            jsonFile = null;
            uploadName = "";
            uploadDescription = "";
            thumbnail = null;
            UnityAuthenticationSystem.UnityAuthenticationCompleted.AddListener(OnUnityLoggedIn);
        }

        public override void EndOfApplicaitonBehaviour()
        {
            base.EndOfApplicaitonBehaviour();
            ugcSystemReadyToGo = false;
            jsonFile = null;
            uploadName = "";
            uploadDescription = "";
            thumbnail = null;
        }

        private void ResetVals()
        {
            jsonFile = null;
            uploadName = "";
            uploadDescription = "";
            thumbnail = null;
        }

        private void OnUnityLoggedIn()
        {
            ugcSystemReadyToGo = true;
        }

        [Space]
        [EnableIf("ugcSystemReadyToGo")]
        public TextAsset jsonFile;

        [ShowIf("ShowJsonConfirm")]
        [SerializeField] private string uploadName;
        [ShowIf("ShowJsonConfirm"), TextArea]
        [SerializeField] private string uploadDescription;
        [ShowIf("ShowJsonConfirm")]
        [SerializeField] private Texture2D thumbnail;



        private bool ShowJsonConfirm()
        {
            if (ugcSystemReadyToGo == false)
            {
                return false;
            }

            if (jsonFile != null)
            {
                return true;
            }

            return false;
        }


        [Button]
        [ShowIf("ShowJsonConfirm")]
        private async void CreateContent()
        {
            if (ugcSystemReadyToGo == false)
            {
                Debug.Log("UGC System not ready!");
                return;
            }

            //   using FileStream contentFileStream = File.Open(Application.dataPath + "/UGC/" + jsonFile.name, FileMode.Open, FileAccess.Read, FileShare.Read)

            string jsonContent = jsonFile.text;

            byte[] jsonBytes = Encoding.UTF8.GetBytes(jsonContent);

            using MemoryStream contentFileStream = new MemoryStream(jsonBytes);

            byte[] thumbnailBytes = null;
            if (thumbnail != null)
            {
                thumbnailBytes = thumbnail.EncodeToPNG();
            }

            // (thumbnailBytes);

            try
            {
                if (thumbnailBytes != null)
                {
                    using MemoryStream thumbnailStream = new MemoryStream(thumbnailBytes);

                    Content content = await UgcService.Instance.CreateContentAsync(new CreateContentArgs(uploadName, uploadDescription, contentFileStream)
                    {
                        IsPublic = true,
                        Thumbnail = thumbnailStream
                    });
                    Debug.Log("Created Content with Thumbnail: " + content.Id);
                    ResetVals();
                }
                else
                {
                    Content content = await UgcService.Instance.CreateContentAsync(new CreateContentArgs(uploadName, uploadDescription, contentFileStream)
                    {
                        IsPublic = true
                    });
                    Debug.Log("Created Content: " + content.Id);
                    ResetVals();
                }


            }
            catch (UgcException e)
            {
                Debug.LogError(e);
            }
        }

        [PropertySpace(spaceBefore: 10)]
        [EnableIf("ugcSystemReadyToGo")]
        [Button]
        private async void DeleteOnlineContent(string contentID)
        {
            await UgcService.Instance.DeleteContentAsync(contentID);
        }


      
    }
}
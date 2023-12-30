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
        public BoolWithCallback ugcSystemReadyToGo;

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
            ResetVals();
            UnityAuthenticationSystem.UnityAuthenticationCompleted.AddListener(OnUnityLoggedIn);
        }

        public override void EndOfApplicaitonBehaviour()
        {
            base.EndOfApplicaitonBehaviour();
            ResetVals();
        }

        private void ResetVals()
        {
            contentIds.Clear();
            ugcSystemReadyToGo.Value = false;
            jsonFile = null;
            uploadName = "";
            uploadDescription = "";
            thumbnail = null;
        }

        private void OnUnityLoggedIn()
        {
            ugcSystemReadyToGo.Value = true;

            RefreshFromServer();
        }

        public List<ContentReference> contentIds = new List<ContentReference>();

      

        [Button,EnableIf("ugcSystemReadyToGo")]
        public async void RefreshFromServer()
        {            
            try
            {
                contentIds.Clear();

                PagedResults<Content> contentPagedResults = await UgcService.Instance.GetContentsAsync();

                foreach (Content content in contentPagedResults.Results)
                {
                    contentIds.Add(new ContentReference(content.Id, content.Name, content.Description));
                }
            }
            catch (UgcException e)
            {
                Debug.Log(e);
            }
        }

        #region DebugCreateAndUpload
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

            string jsonContent = jsonFile.text;

            byte[] jsonBytes = Encoding.UTF8.GetBytes(jsonContent);

            using MemoryStream contentFileStream = new MemoryStream(jsonBytes);

            byte[] thumbnailBytes = null;
            if (thumbnail != null)
            {
                thumbnailBytes = thumbnail.EncodeToPNG();
            }

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
                }
                else
                {
                    Content content = await UgcService.Instance.CreateContentAsync(new CreateContentArgs(uploadName, uploadDescription, contentFileStream)
                    {
                        IsPublic = true
                    });
                    Debug.Log("Created Content: " + content.Id);
                }


            }
            catch (UgcException e)
            {
                Debug.LogError(e);
            }
        }

        #endregion

        [PropertySpace(spaceBefore: 10)]
        [EnableIf("ugcSystemReadyToGo")]
        [Button]
        private async void DeleteOnlineContent(string contentID)
        {
            await UgcService.Instance.DeleteContentAsync(contentID);
        }

        [Serializable,InlineEditor(InlineEditorObjectFieldModes.Hidden)]
        public class ContentReference
        {
            public ContentReference(string _id, string _title, string _description)
            {
                id = _id;
                title = _title;
                description = _description;
            }


            [HideInInspector] public string id;
            [HideLabel, ShowIf("thumbnail"), HorizontalGroup("H"),PreviewField]            public Texture2D thumbnail;
            [ReadOnly, HideLabel, HorizontalGroup("H")] public string title;
            [ReadOnly, HideLabel, HorizontalGroup("H")] public string description;
            
            public Content downloadedContent;
            //public 


            [ReadOnly]
            public bool IsThumbnailDownloaded;
            [Button]
            [HideIf("thumbnail")]
            public async void DownloadThumbnail()
            {
                try
                {
                    if (IsThumbnailDownloaded || string.IsNullOrEmpty(id)) return;

                    Content content = await UgcService.Instance.GetContentAsync(new GetContentArgs(id));
                    await UgcService.Instance.DownloadContentDataAsync(content, false, true);
                    // byte[] downloadedThumbnailData = ;

                    if (thumbnail == null) thumbnail = new Texture2D(1,1);//ScreenshotUtility.Instance.pixelWidth, ScreenshotUtility.Instance.pixelHeight);
                    thumbnail.LoadImage(content.DownloadedThumbnail);
                    IsThumbnailDownloaded = true;
                }
                catch (Exception ex)
                {
                    Debug.LogError(ex);                    
                }
            }
            [Button]
            [ShowIf("thumbnail")]
            public void DeleteDownloadedThumbnail()
            {
                if (!IsThumbnailDownloaded) return;              
                //thumbnail = null;
                Destroy(thumbnail);
                thumbnail = null;
                //thumbnail.
                IsThumbnailDownloaded = false;
            }

            [ReadOnly]
            public bool IsContentDownloaded;
            bool downloading = false;
            
            [Button]
            [HideIf("downloadedContent")]
            public async void DownloadContent()
            {
                if (downloading) return;

                downloading = true;
                try
                {
                    if (IsContentDownloaded || string.IsNullOrEmpty(id)) return;

                    Content content = await UgcService.Instance.GetContentAsync(new GetContentArgs(id));
                    await UgcService.Instance.DownloadContentDataAsync(content, true, false);
                    //Example here
                    downloadedContent = content;
                    string modText = System.Text.Encoding.UTF8.GetString(content.DownloadedContent);
                    Debug.Log(modText);

                    IsContentDownloaded = true;
                    downloading = false;
                }
                catch(Exception ex) 
                {
                    Debug.LogError(ex);
                    downloading = false;
                }
            }

            [Button]
            [ShowIf("downloadedContent")]
            public void DeleteDownloadedContent()
            {
                if (!IsContentDownloaded) return;
                downloadedContent = null;
                IsContentDownloaded = false;
            }
        }

    }
}
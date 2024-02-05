using Rufas;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Unity.Services.Ugc;
using Unity.Services.Ugc.Generated.Models;
using UnityEngine;

namespace Rufas.UnitySystems
{
    public class UnityUserGeneratedContent : GameSystem<UnityUserGeneratedContent>
    {
       // public bool playerIDFound = false;
       // public string playerID = "";

        #region GameSystemThings
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
            UnityAuthenticationSystem.OnUnityAuthenticationComplete.AddListener(OnUnityLoggedIn);
        }

        public override void EndOfApplicaitonBehaviour()
        {
            base.EndOfApplicaitonBehaviour();
            ResetVals();
        }
        #endregion

        public BoolWithCallback ugcSystemReadyToGo;
        [HideInInspector]
        public CodeEvent<List<ContentReference>> ugcServerRefreshed;
        public List<ContentReference> allContent = new List<ContentReference>();

        private void ResetVals()
        {
            //playerID = "";
            //playerIDFound = false;
            allContent.Clear();
            ugcSystemReadyToGo.Value = false;
        }

        private void OnUnityLoggedIn()
        {
            ugcSystemReadyToGo.Value = true;

            RefreshFromServer();
        }
              
        //Produces a list of contentReferences 
        [Button,EnableIf("ugcSystemReadyToGo")]
        public async void RefreshFromServer()
        {            
            try
            {
                allContent.Clear();

                /*
                if (playerIDFound == false)
                {
                    PagedResults<Content> playersContent = await UgcService.Instance.GetPlayerContentsAsync();

                    if (playersContent.Results.Count > 0)
                    {
                        Debug.Log("LOCAL-TRACKS_" + playersContent.Results.Count);

                        Debug.Log(playersContent.Results[0].Name);

                        playerIDFound = true;
                        playerID = playersContent.Results[0].CreatorAccountId;
                    }
                    else
                    {
                        playerIDFound = false;
                        playerID = "";
                    }
                }
                */

                PagedResults<Content> contentPagedResults = await UgcService.Instance.GetContentsAsync();
               // IsLocalPlayersContent
                foreach (Content content in contentPagedResults.Results)
                {
                    bool isLocalContent = false;

                    if(content.CreatorAccountId ==  UnityAuthenticationSystem.Instance.PlayerID) { isLocalContent = true; }
                    //content.
                    allContent.Add(new ContentReference(content.Id, content.Name, content.Description,content.Metadata,isLocalContent));

                   //if(content.CreatorAccountId == UgcService.Instance.get)
                }

                ugcServerRefreshed.Raise(allContent);
            }
            catch (UgcException e)
            {
                Debug.Log(e);
            }
        }

        [PropertySpace(spaceBefore: 10)]
        [EnableIf("ugcSystemReadyToGo")]
        [Button]
        public async void UploadNewContent(byte[] toUpload, string uploadName, string uploadDescription, string trackFileMetadata, byte[] thumnailBytes = null,List<string> tags = null)
        {
            if (ugcSystemReadyToGo == false)
            {
                Debug.Log("Cannot upload content (not able to access User Generated Content server)");
                return;
            }

            try
            {
                using MemoryStream contentFileStream = new MemoryStream(toUpload);
                CreateContentArgs args = new CreateContentArgs(uploadName, uploadDescription, contentFileStream);                
                args.IsPublic = true;
                args.Metadata = trackFileMetadata;

                if (thumnailBytes != null)
                {
                    using MemoryStream thumbnailStream = new MemoryStream(thumnailBytes);
                    args.Thumbnail = thumbnailStream;
                }
             //   Debug.Log("Here");
                if(tags != null)
                {
                    //tags
                  //  args.TagsId = new List<string>(tags);
                }
              //  Debug.Log("Here2");


                Content content = await UgcService.Instance.CreateContentAsync(args);
            }
            catch (UgcException e)
            {
                Debug.LogError("UGC New Upload Error: " + e);
            }
        }

        [PropertySpace(spaceBefore: 10)]
        [EnableIf("ugcSystemReadyToGo")]
        [Button]
        public async void DeleteOnlineContent(string contentID)
        {
            await UgcService.Instance.DeleteContentAsync(contentID);
        }

        [PropertySpace(spaceBefore: 10)]
        [EnableIf("ugcSystemReadyToGo")]
        [Button]
        private async void ReportContent(string contentID,Reason reason = Reason.NonFunctional)
        {            
            var result = await UgcService.Instance.ReportContentAsync(new ReportContentArgs(contentID, reason));

            Debug.Log($"The following content was reported: {result.Id}.");
        }

        [PropertySpace(spaceBefore: 10)]
        [EnableIf("ugcSystemReadyToGo")]
        [Button]
        private async void RejectReportedContent(string contentID)
        {
            await UgcService.Instance.RejectContentAsync(contentID);
        }

        [Serializable]
        public class ContentReference
        {
            public ContentReference(string _id, string _title, string _description, string _metadata, bool _isLocalPlayerContent)
            {
                id = _id;
                title = _title;
                description = _description;
                metadata = _metadata;
                IsLocalPlayersContent = _isLocalPlayerContent;
            }

            public bool IsLocalPlayersContent;

            public CodeEvent onContentDownloaded;

            
            [HideLabel, ShowIf("thumbnail"), HorizontalGroup("H"),PreviewField]            public Texture2D thumbnail;
            [ReadOnly, HideLabel, HorizontalGroup("H")] public string title;
          
            [ReadOnly, HideLabel, HorizontalGroup("H")] public string description;            

            [ReadOnly, HideLabel, HorizontalGroup("H2")] public string id;
            
            [ReadOnly, HideLabel, HorizontalGroup("H2")] public List<string> tags = new List<string>();

            [ReadOnly,HideLabel]
            public string metadata;

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
                    Debug.Log("DOWNLOAD: " + downloadedContent.DownloadedContent.Length.ToString());
                    //content.id
                    string contentAsText = System.Text.Encoding.UTF8.GetString(content.DownloadedContent);
                    Debug.Log(content.Metadata + "\n" + contentAsText);
                    tags.Clear();
                    foreach (Tag next in content.Tags)
                    {
                       // Debug.Log("TagID: " + next.Id + " | TagName: " + next.Name);
                       //tags.Add()
                    }
                    // Debug.Log("Call event here to return this to save file!");
                    onContentDownloaded.Raise();
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
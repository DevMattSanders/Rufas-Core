using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Rufas
{
    public class DragBuildPreview : MonoBehaviour
    {
        private DragBuildTool trackDragBuildTool;
        private MeshRenderer meshRenderer;
        private MeshFilter meshFilter;
        private MeshCollider meshCollider;

        [SerializeField] private Material[] canBuildMaterials;
        [SerializeField] private Material[] cannotBuildMaterials;
        

        private void Awake()
        {
            trackDragBuildTool = transform.parent.GetComponent<DragBuildTool>();
            meshRenderer = GetComponent<MeshRenderer>();
            meshFilter = GetComponent<MeshFilter>();
            meshCollider = GetComponentInChildren<MeshCollider>();
        }

        private void OnTriggerEnter(Collider other)
        {
            //Check and add build tool points
          //  while (true)
          //  {
                Collider[] excludedColliders = trackDragBuildTool.currentDragPointColliderParent.GetComponentsInChildren<Collider>();
                if (excludedColliders.Contains(other)) { return; }
                if (other.gameObject.layer == 29 || other.gameObject.layer == 6) { return; }
                if (other.isTrigger) { return; }
                if (trackDragBuildTool.overlappingColliders.Contains(other)) { return; }

                trackDragBuildTool.overlappingColliders.Add(other);

               // break;
          //  }

            //Check and add loose connections points


        }

        private void OnTriggerExit(Collider other)
        {
            if (!trackDragBuildTool.overlappingColliders.Contains(other)) { return; }

            trackDragBuildTool.overlappingColliders.Remove(other);
        }

        public void UpdateBuildPreview(bool canBuild, Mesh previewMesh, Mesh collisionMesh, bool showPreviewMesh, bool mirror)
        {
            // Update and enable / disable mesh
            meshCollider.sharedMesh = collisionMesh;
            meshFilter.sharedMesh = previewMesh;
            meshRenderer.enabled = showPreviewMesh;

            // Set scale
            if (mirror) {
                transform.localScale = new Vector3(-1f, 1f, 1f);
            }
            else {
                transform.localScale = Vector3.one;
            }

            // Update materials
            if (canBuild) {
                meshRenderer.materials = canBuildMaterials;
            } else {
                meshRenderer.materials = cannotBuildMaterials;
            }
        }
    }
}

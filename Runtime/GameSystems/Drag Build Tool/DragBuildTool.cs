
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Rufas
{
    public class DragBuildTool : MonoBehaviour
    {
        public static DragBuildTool Instance;
        private DragBuildPreview previewObject;
        [SerializeField] private Transform canDragPreview;
        [SerializeField] private float canActivateRadius;
        [SerializeField] private float deactivateRadius;

        public enum DragBuildToolMode { Active, Inactive };
        public DragBuildToolMode mode;
        
        [Space, Header("Builder")]
        [SerializeField] private Transform builderTransform;
        [SerializeField] private float maxCutOffPoint;

        [Space, Header("Target Points")]
        [SerializeField] private DragBuildTargetPoint closestTargetPoint;
        [SerializeField] private Transform[] targetPoints;

        [Space, Header("Current Connection & Collision")]
        public DragBuildTriggerPoint currentConnection;
        public bool previewColliding;
        public List<Collider> overlappingColliders;

        [FoldoutGroup("Events")] public UnityEvent<GameObject> OnObjectSpawnedEvent;

        private void Awake()
        {
            if (Instance != null) { Debug.LogError("Two Drag Build Toos!", gameObject);  return; }
            Instance = this;

            previewObject = GetComponentInChildren<DragBuildPreview>();
        }

        private void Start()
        {
            DeactivateBuildTool();
        }

        private void Update()
        {
            if (mode == DragBuildToolMode.Inactive && !previewObject.gameObject.activeSelf) 
            { 
                float builderDistanceToMe = Vector3.Distance(builderTransform.position, transform.position);
                if (builderDistanceToMe < canActivateRadius)
                {
                    canDragPreview.gameObject.SetActive(true);
                    canDragPreview.transform.position = transform.position;
                    canDragPreview.transform.rotation = Quaternion.Inverse(currentConnection.transform.rotation);
                }
                else
                {
                    canDragPreview.gameObject.SetActive(false);
                
                    if (builderDistanceToMe > deactivateRadius)
                    {
                        DeactivateBuildTool();
                    }
                }
            }

            UpdateClosestEndPoint();
            
            if (closestTargetPoint == null) { 
                previewObject.UpdateBuildPreview(false, null, null, false, false); 
                return; 
            } else {
                Debug.DrawLine(closestTargetPoint.transform.position, builderTransform.position, Color.blue);

                if (overlappingColliders.Count <= 0) {
                    previewObject.UpdateBuildPreview(true, closestTargetPoint.previewMesh, closestTargetPoint.previewColliderMesh, true, closestTargetPoint.mirror);
                }
                else {
                    previewObject.UpdateBuildPreview(false, closestTargetPoint.previewMesh, closestTargetPoint.previewColliderMesh, true, closestTargetPoint.mirror);
                }
            }
        }

        private void UpdateClosestEndPoint()
        {
            DragBuildTargetPoint newEndPoint = null;
            float closestDistance = float.MaxValue;

            for (int i = 0; i < targetPoints.Length; i++)
            {
                float distance = Vector3.Distance(builderTransform.position, targetPoints[i].position);

                if (distance < closestDistance && distance < maxCutOffPoint)
                {
                    closestDistance = distance;
                    newEndPoint = targetPoints[i].GetComponent<DragBuildTargetPoint>();
                }
            }

            if (newEndPoint != closestTargetPoint)
            {
                closestTargetPoint = newEndPoint;
                overlappingColliders.Clear();

                if (newEndPoint != null) {
                    previewObject.UpdateBuildPreview(true, newEndPoint.previewMesh, newEndPoint.previewColliderMesh, true, newEndPoint.mirror);
                } else {
                    previewObject.UpdateBuildPreview(false, null, null, false, false);
                }
            }
        }

        [Button()]
        private void SpawnNewObject()
        {
            if (closestTargetPoint == null  || previewColliding == true || mode == DragBuildToolMode.Inactive) { return; }

            currentConnection.canBuild = false;

            GameObject newObject = Instantiate(closestTargetPoint.targetPrefab);
         
            newObject.transform.position = this.transform.position;
            newObject.transform.rotation = this.transform.rotation;
            newObject.transform.localScale = previewObject.transform.localScale;

            DragBuildTriggerPoint[] triggerPoints = newObject.GetComponentsInChildren<DragBuildTriggerPoint>();
            DragBuildTriggerPoint closestTriggerPoint = null;
            float closestDistance = float.MaxValue;

            foreach (DragBuildTriggerPoint triggerPoint in triggerPoints)
            {
                float distance = Vector3.Distance(triggerPoint.transform.position, currentConnection.transform.position);
                if (distance < closestDistance)
                {
                    closestTriggerPoint = triggerPoint;
                    closestDistance = distance;
                }
            }
            closestTriggerPoint.canBuild = false;

            //UpdateToolTransformAndConnection(currentConnection);
            OnObjectSpawnedEvent.Invoke(newObject);

            DeactivateBuildTool();
        }

        [Button()] public void ActivateBuildTool()
        {
            mode = DragBuildToolMode.Active;
            previewObject.gameObject.SetActive(true);
            canDragPreview.gameObject.SetActive(false);
        }
        [Button()] public void DeactivateBuildTool()
        {
            mode = DragBuildToolMode.Inactive;
            previewObject.gameObject.SetActive(false);
        }

        public void UpdateToolTransformAndConnection(DragBuildTriggerPoint newConnection)
        {
            if (mode == DragBuildToolMode.Active) { return; }

            currentConnection = newConnection;

            transform.position = newConnection.transform.position;
            transform.rotation = newConnection.transform.rotation;
            transform.Rotate(0, 180, 0, Space.Self);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(builderTransform.position, maxCutOffPoint);

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(builderTransform.position, canActivateRadius);
            Gizmos.DrawWireSphere(builderTransform.position, deactivateRadius);
        }
    }
}

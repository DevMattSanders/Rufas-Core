using DG.Tweening;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rufas
{
    public class FollowGameobject : MonoBehaviour
    {
        [SerializeField,HideInInspector]
        private bool useScriptable;

        [HorizontalGroup("H")]
        [ShowIf("useScriptable")]
        [HideLabel]
        [Required,SerializeField]
        private GameObjectVariable scriptable;

        [HorizontalGroup("H")]
        [HideIf("useScriptable")]
        [HideLabel]
        [Required,SerializeField]
        private GameObject sceneRef;
        private Transform trueTarget;

        [HorizontalGroup("H")]
        [HideIf("useScriptable")]
        [Button]
        private void UseScriptable()
        {
            useScriptable = true;
        }

        [HorizontalGroup("H")]
        [ShowIf("useScriptable")]
        [Button]
        private void UseSceneRef()
        {
            useScriptable = false;
        }

        public void InjectSceneRef(GameObject _gameObject)
        {
            useScriptable = false;

            sceneRef = _gameObject;
        }

        public void InjectScriptable(GameObjectVariable _gameObjectVariable)
        {
            useScriptable = true;

            scriptable = _gameObjectVariable;
        }


        //Smooth Position

        [BoxGroup("Position"), HorizontalGroup("Position/Pos")]
        public bool positionX;
        [BoxGroup("Position"), HorizontalGroup("Position/Pos")]
        public bool positionY;
        [BoxGroup("Position"), HorizontalGroup("Position/Pos")]
        public bool positionZ;

        [ShowIf("ShowAnyPosFields")]
        [BoxGroup("Position")]
        public bool smoothPosition= true;

        [ShowIf("ShowAnyPosFields")]
        [BoxGroup("Position")]
        public Vector3 positionOffset;

        [ShowIf("ShowAnyRotFields")]
        [BoxGroup("Rotation")]
        public Vector3 positionThresholdBeforeMoving;

        [ShowIf("ShowPosFollowSpeed")]
        [BoxGroup("Position")]
        public float positionFollowSpeed = 3;

        [ShowIf("ShowPosFollowSpeed")]
        [BoxGroup("Position")]
        public bool multiplyByDistance = false;

        //Odin fields
        private bool ShowAnyPosFields() { if (!positionX && !positionY && !positionZ) return false; return true; }
        private bool ShowPosFollowSpeed() { if (ShowAnyPosFields() && smoothPosition) return true; return false; }


        //Smooth Rotation
        [BoxGroup("Rotation"), HorizontalGroup("Rotation/Rot")]
        public bool rotationX;
        [BoxGroup("Rotation"), HorizontalGroup("Rotation/Rot")]
        public bool rotationY;
        [BoxGroup("Rotation"), HorizontalGroup("Rotation/Rot")]
        public bool rotationZ;

        [ShowIf("ShowAnyRotFields")]
        [BoxGroup("Rotation")]
        public bool smoothRotation = true;

        [ShowIf("ShowAnyRotFields")]
        [BoxGroup("Rotation")]
        public Vector3 rotationOffset;

        [ShowIf("ShowAnyRotFields")]
        [BoxGroup("Rotation")]        
        public Vector3 eulerThresholdBeforeRotating;

        [ShowIf("ShowRotFollowSpeed")]
        [BoxGroup("Rotation")]
        public float rotationFollowSpeed = 5;

        //Odin Fields
        private bool ShowAnyRotFields() { if (!rotationX && !rotationY && !rotationZ) return false; return true; }
        private bool ShowRotFollowSpeed() { if (ShowAnyRotFields() && smoothRotation) return true; return false; }

        public bool useLateUpdate = false;

        void Update()
        {
            if (useLateUpdate == false)
            {
                //Debug.Log(name + " " + useScriptable + " " + scriptable.name);
                //Null checks
                if (useScriptable && scriptable != null)
                {
                    if (scriptable.value != null)
                    {
                        trueTarget = scriptable.value.transform;
                    }
                }
                else if (sceneRef != null)
                {
                    trueTarget = sceneRef.transform;
                }
                else
                {
                    trueTarget = null;
                }

                if (trueTarget == null)
                {
                    // Debug.LogError("Target to follow is null!");
                    return;
                }

                UpdatePositon();
                UpdateRotation();
            }
        }

        private void LateUpdate()
        {
            if (useLateUpdate)
            {
                //Debug.Log(name + " " + useScriptable + " " + scriptable.name);
                //Null checks
                if (useScriptable && scriptable != null)
                {
                    if (scriptable.value != null)
                    {
                        trueTarget = scriptable.value.transform;
                    }
                }
                else if (sceneRef != null)
                {
                    trueTarget = sceneRef.transform;
                }
                else
                {
                    trueTarget = null;
                }

                if (trueTarget == null)
                {
                    // Debug.LogError("Target to follow is null!");
                    return;
                }

                UpdatePositon();
                UpdateRotation();
            }
        }

        private void UpdatePositon()
        {
            if(!positionX && !positionY && !positionZ) return;            

            Vector3 targetPos = trueTarget.position + positionOffset;
            
            if (!positionX) targetPos.x = transform.position.x;

            if(!positionY) targetPos.y = transform.position.y;

            if(!positionZ) targetPos.z = transform.position.z;


            if (!smoothPosition) { transform.position = targetPos; return; }

            if (multiplyByDistance)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPos, Vector3.Distance(transform.position, targetPos) * positionFollowSpeed * Vector3.Distance(transform.position,targetPos) * Time.deltaTime);
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPos, Vector3.Distance(transform.position, targetPos) * positionFollowSpeed * Time.deltaTime);
            }        
        }

        Vector3 targetRot;

        private void UpdateRotation()
        {
            if (!rotationX && !rotationY && !rotationZ) return;

            Vector3 targetEulerAngles = Quaternion.Euler(trueTarget.rotation.eulerAngles + rotationOffset).eulerAngles;
            Vector3 currentEulerAngles = transform.rotation.eulerAngles;

            if (rotationX)
            {
                if (ShouldRotateAxis(currentEulerAngles.x, targetEulerAngles.x, eulerThresholdBeforeRotating.x))
                {
                    targetRot.x = targetEulerAngles.x;
                }
            }
            else
            {
                targetRot.x = currentEulerAngles.x;
            }

            if (rotationY)
            {
                if (ShouldRotateAxis(currentEulerAngles.y, targetEulerAngles.y, eulerThresholdBeforeRotating.y))
                {
                    targetRot.y = targetEulerAngles.y;
                }
            }
            else
            {
                targetRot.y = currentEulerAngles.y;
            }

            if (rotationZ)
            {
                if (ShouldRotateAxis(currentEulerAngles.z, targetEulerAngles.z, eulerThresholdBeforeRotating.z))
                {
                    targetRot.z = targetEulerAngles.z;
                }
            }
            else
            {
                targetRot.z = currentEulerAngles.z;
            }            

            Quaternion targetRotationAdjusted = Quaternion.Euler(targetRot);

            if (!smoothRotation)
            {
                transform.rotation = targetRotationAdjusted;
                return;
            }

            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotationAdjusted, rotationFollowSpeed * Time.deltaTime);          
        }

        private bool ShouldRotateAxis(float currentAngle, float targetAngle, float threshold)
        {
            float angleDifference = Mathf.DeltaAngle(currentAngle, targetAngle);
            return Mathf.Abs(angleDifference) > threshold;
        }
    }
}

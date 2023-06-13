using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Sirenix.OdinInspector;

namespace Rufas
{
    public class FollowCamera : MonoBehaviour
    {
        public Transform target; // The object the camera will follow
        [Space]
        public float smoothTime = 0.3f; // The time it takes for the camera to catch up to the target
        public float distance = 5f; // The initial distance between the camera and the target
        public float height = 2f; // The initial height offset between the camera and the target

        [SerializeField] private FollowCameraProfile[] profiles;

        private Vector3 velocity = Vector3.zero;

        void LateUpdate()
        {
            // Calculate the target position, including the offset
            Vector3 targetPosition = target.position - target.forward * distance + Vector3.up * height;

            // Rotate the offset based on the target's forward rotation
            Vector3 rotatedOffset = Quaternion.AngleAxis(target.rotation.eulerAngles.y, Vector3.up) * (targetPosition - target.position);

            // Smoothly move the camera to the target position
            transform.position = Vector3.SmoothDamp(transform.position, target.position + rotatedOffset, ref velocity, smoothTime);

            // Make the camera look at the target
            transform.LookAt(target);
        }

        private void OnValidate()
        {
            foreach (var profile in profiles)
            {
                profile.followCamera = this;
            }

            // Calculate the target position, including the offset
            Vector3 targetPosition = target.position - target.forward * distance + Vector3.up * height;

            // Rotate the offset based on the target's forward rotation
            Vector3 rotatedOffset = Quaternion.AngleAxis(target.rotation.eulerAngles.y, Vector3.up) * (targetPosition - target.position);

            // Smoothly move the camera to the target position
            transform.position = Vector3.SmoothDamp(transform.position, target.position + rotatedOffset, ref velocity, smoothTime);

            // Make the camera look at the target
            transform.LookAt(target);
        }
    }


    [System.Serializable]
    public class FollowCameraProfile
    {
        [HideInInspector] public FollowCamera followCamera;
        
        [Space]
        public string profileName;
        public float smoothTime;
        public float distance;
        public float height;

        [Button]
        public void ActivateThisProfile()
        {
            followCamera.smoothTime = smoothTime;
            followCamera.distance = distance;
            followCamera.height = height;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rufas
{
    public class RotateAroundTarget : MonoBehaviour
    {
        [SerializeField] private Transform target;

        private void LateUpdate()
        {
            if (target != null)
            {
                // Calculate the direction from the canvas to the player
                Vector3 directionToPlayer = target.position - transform.position;

                // Rotate the canvas to face the player
                transform.rotation = Quaternion.LookRotation(-directionToPlayer, Vector3.up);
            }
        }
    }
}
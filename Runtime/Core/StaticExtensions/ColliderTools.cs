using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rufas
{
    public static class ColliderTools
    {
        public static Collider GetClosestFromArray(Collider[] array, Transform transform)
        {
            if (array.Length > 0)
            {
                Collider closestCollider = null;
                float closestDistance = float.MaxValue;

                foreach (Collider collider in array)
                {
                    float distance = Vector3.Distance(transform.position, collider.transform.position);

                    // Check if this collider is closer than the previously found closest collider
                    if (distance < closestDistance)
                    {
                        closestCollider = collider;
                        closestDistance = distance;
                    }
                }

                return closestCollider;
            }
            else
            { 
                return null; 
            }
        }
    }
}

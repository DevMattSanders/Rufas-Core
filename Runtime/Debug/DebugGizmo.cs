using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rufas
{
    public class DebugGizmo : MonoBehaviour
    {
        private enum Shape { Sphere, Cube };
        [SerializeField] private Shape shape;

        [Space]
        [SerializeField] private Color gizmoColour;
        [SerializeField] private float gizmoSize;

        private void OnDrawGizmos()
        {
            Gizmos.color = gizmoColour;

            if (shape == Shape.Sphere)
            {
                Gizmos.DrawSphere(transform.position, gizmoSize);
            }
            else //(shape == Shape.Cube)
            {
                Gizmos.color = gizmoColour;
                Gizmos.DrawCube(transform.position, new Vector3(gizmoSize, gizmoSize, gizmoSize));
            }
        }
    }
}

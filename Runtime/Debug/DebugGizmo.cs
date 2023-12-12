using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Rufas
{
    public class DebugGizmo : MonoBehaviour
    {
        private enum Shape { Sphere, Cube };
        [SerializeField] private Shape shape;

        [SerializeField] private bool showGizmo = true;
        [Space]
        [SerializeField] private Color gizmoColour;
        [SerializeField] private float gizmoSize;

        private void OnDrawGizmos()
        {
            if (!showGizmo) { return; } 

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

            //UnityEditor.EditorSettings

        }
    }
}
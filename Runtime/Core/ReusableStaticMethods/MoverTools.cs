using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rufas
{
    public static class MoverTools
    {
        public static Transform GetMover()
        {
            return (GetMover("Mover"));
        }

        public static Transform GetMover(string moverName)
        {
            return new GameObject(moverName).transform;
        }

        public static void HandleMove(Transform mover, Transform transformToMove, Transform startPosition, Transform targetPosRot)
        {
            HandleMove(mover, transformToMove, startPosition, targetPosRot, Vector3.zero);
        }

        public static void HandleMove(Transform mover, Transform transformToMove, Transform startPosition, Transform targetPosRot, Vector3 applyLocalRotation)
        {
            // Mover moves to connection pos & rot on last segment
            mover.transform.position = startPosition.position;
            mover.transform.rotation = startPosition.rotation;

            // Get Old Parent
            Transform oldParent = transformToMove.parent;

            // last segment parents to mover
            transformToMove.parent = mover;

            // mover moves to target pos & rot
            mover.transform.position = targetPosRot.position;
            mover.transform.rotation = targetPosRot.rotation;

            mover.Rotate(applyLocalRotation, Space.Self);


            // mover unparents last segment
            transformToMove.parent = oldParent;
        }
    }
}

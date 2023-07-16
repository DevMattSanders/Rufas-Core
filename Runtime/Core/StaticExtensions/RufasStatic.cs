using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Rufas
{
    public static class RufasStatic
    {
        public static bool CompareVector3WithTolerance(Vector3 a, Vector3 b, float tolerance)
        {
            return (Mathf.Abs(a.x - b.x) < tolerance) && (Mathf.Abs(a.y - b.y) < tolerance) && (Mathf.Abs(a.z - b.z) < tolerance);            
        }

        //Main camera getter and stored reference
        private static Camera _mainCamera;
        public static Camera mainCamera
        {
            get
            {


                if (_mainCamera == null)
                {
                    _mainCamera = Camera.main;
                }

                return _mainCamera;
            }
        }

#if UNITY_EDITOR

        public static T[] GetAllScriptables_ToArray<T>() where T : ScriptableObject
        {
            string[] guids = AssetDatabase.FindAssets("t:" + typeof(T).Name);  //FindAssets uses tags check documentation for more info
            T[] a = new T[guids.Length];
            for (int i = 0; i < guids.Length; i++)         //probably could get optimized
            {
                string path = AssetDatabase.GUIDToAssetPath(guids[i]);
                a[i] = AssetDatabase.LoadAssetAtPath<T>(path);
            }

            return a;
        }

        public static List<T> GetAllScriptables_ToList<T>() where T : ScriptableObject
        {
            string[] guids = AssetDatabase.FindAssets("t:" + typeof(T).Name);

            List<T> scriptableObjects = new List<T>();

            for (int i = 0; i < guids.Length; i++)
            {
                string path = AssetDatabase.GUIDToAssetPath(guids[i]);

                T scriptableObject = AssetDatabase.LoadAssetAtPath<T>(path);

                if (scriptableObject != null)
                {
                    scriptableObjects.Add(scriptableObject);
                }
            }

            return scriptableObjects;
        }

#endif
    }

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

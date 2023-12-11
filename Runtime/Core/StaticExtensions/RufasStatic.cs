using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
#if UNITY_EDITOR
using UnityEditor;
#endif
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

        static System.Random random = new System.Random();

        public static string ShortGUID_FromRandomLong()
        {                     
            return random.Next().ToString("x");
        }

        public static void Let<T>(this T obj, Action<T> action, Action ifNull) where T : class
        {
            if (obj != null)
            {
                action(obj);
            }
            else
            {
                ifNull();
            }
        }

#if UNITY_EDITOR

        public static IEnumerable<Type> FindDerivedTypes(Type baseType, bool inThisAsseblyOnly = false)
        {
            if (inThisAsseblyOnly)
            {
                return baseType.Assembly.GetTypes().Where(t => baseType.IsAssignableFrom(t));
            }
            else
            {
                var derivedTypes = new List<Type>();

                foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
                {
                    try
                    {
                        var typesInAssembly = assembly.GetTypes().Where(t => baseType.IsAssignableFrom(t) && t != baseType);
                        derivedTypes.AddRange(typesInAssembly);
                    }
                    catch (ReflectionTypeLoadException)
                    {
                        // Handle exceptions related to loading types from the assembly
                        // You can log the exception or handle it in any other way based on your requirements
                    }
                }

                return derivedTypes;
            }
        }

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

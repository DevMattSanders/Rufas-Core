using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rufas
{
    public static class StringConvert
    {
      //  private const string trueBool = "t";
      //  private const string falseBool = "f";

        //BOOLS
        public static bool ToBool(string val)
        {
            return JsonUtility.FromJson<bool>(val);
            /*
            if (string.Compare(val, trueBool) == 0) return true;

            if (string.Compare(val, falseBool) == 0) return false;

            Debug.LogError("No bool found from string!");
            return false;
            */
        }

        public static string FromBool(bool val)
        {
            return JsonUtility.ToJson(val);
            /*
            if (val)
            {
                return trueBool;
            }
            else
            {
                return falseBool;
            }
            */
        }

        // INT
        public static int ToInt(string val)
        {
            return JsonUtility.FromJson<int>(val);
            /*
            int result;
            
            if (int.TryParse(val, out result))
            {
                return result;
            }
            else
            {
                Debug.LogError("Failed to convert string to int!");
                return 0;
            }
            */
        }

        public static string FromInt(int val)
        {
            return JsonUtility.ToJson(val);
            //return val.ToString();
        }

        // FLOAT
        public static float ToFloat(string val)
        {
            return JsonUtility.FromJson<float>(val);
            /*
            float result;
            if (float.TryParse(val, out result))
            {
                return result;
            }
            else
            {
                Debug.LogError("Failed to convert string to float!");
                return 0.0f;
            }
            */
        }

        public static string FromFloat(float val)
        {
            return JsonUtility.ToJson(val);
            //return val.ToString("R"); // "R" format ensures that the string representation is culture-invariant
        }

        // VECTOR2
        public static Vector2 ToVector2(string val)
        {
            return JsonUtility.FromJson<Vector2>(val);
            /*
            string[] components = val.Split(',');
            if (components.Length == 2 && float.TryParse(components[0], out float x) && float.TryParse(components[1], out float y))
            {
                return new Vector2(x, y);
            }
            else
            {
                Debug.LogError("Failed to convert string to Vector2!");
                return Vector2.zero;
            }
            */
        }

        public static string FromVector2(Vector2 val)
        {
            return JsonUtility.ToJson(val);
            //return $"{val.x},{val.y}";
        }

        // VECTOR3
        public static Vector3 ToVector3(string val)
        {
            return JsonUtility.FromJson<Vector3>(val);
            /*
            string[] components = val.Split(',');
            if (components.Length == 3 && float.TryParse(components[0], out float x) && float.TryParse(components[1], out float y) && float.TryParse(components[2], out float z))
            {
                return new Vector3(x, y, z);
            }
            else
            {
                Debug.LogError("Failed to convert string to Vector3!");
                return Vector3.zero;
            }
            */
        }

        public static string FromVector3(Vector3 val)
        {
            return JsonUtility.ToJson(val);
            //return $"{val.x},{val.y},{val.z}";
        }
    }
}

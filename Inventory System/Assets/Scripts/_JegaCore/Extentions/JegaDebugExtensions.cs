using System.Diagnostics;
using UnityEngine;

namespace JegaCore
{
    public static class JegaDebugExtensions
    {
        //These are shortcut methods to Debug log calls; they immediatly send the 'this' reference as context as well.
        [Conditional(JegaDebug.ConditionString)]
        public static void LogVerbose(this UnityEngine.Object obj, object message) => JegaDebug.LogVerbose(message, obj);
        [Conditional(JegaDebug.ConditionString)]
        public static void Log(this UnityEngine.Object obj, object message) => JegaDebug.Log(message, obj);
        [Conditional(JegaDebug.ConditionString)]
        public static void LogWarning(this UnityEngine.Object obj, object message) => JegaDebug.LogWarning(message, obj);
        [Conditional(JegaDebug.ConditionString)]
        public static void LogError(this UnityEngine.Object obj, object message) => JegaDebug.LogError(message, obj);
    }
    
    public static class JegaLoggingLevelExtensions
    {
        public static bool HasFlagFast(this JegaLoggingLevel value, JegaLoggingLevel flag) => (value & flag) != 0;
    }

    public static class JegaVector3Extensions
    {
        /// <summary>
        /// Limits Vector3 per range made between min and max parameters.
        /// </summary>
        /// <param name="vector3"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static Vector3 Clamp(this Vector3 vector3, Vector3 min, Vector3 max)
        {
            Vector3 result;
            result.x = Mathf.Clamp(vector3.x, min.x, max.x);
            result.y = Mathf.Clamp(vector3.y, min.y, max.y);
            result.z = Mathf.Clamp(vector3.z, min.z, max.z);
            return result;
        }

        /// <summary>
        /// Clamps a Vector3's X axis only.
        /// </summary>
        /// <param name="vector"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static Vector3 ClampOnXAxis(this Vector3 vector, float min, float max)
        {
            vector.x = Mathf.Clamp(vector.x, min, max);
            return vector;
        }

        /// <summary>
        /// Clamps a Vector3's Y axis only.
        /// </summary>
        /// <param name="vector"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static Vector3 ClampOnYAxis(this Vector3 vector, float min, float max)
        {
            vector.y = Mathf.Clamp(vector.y, min, max);
            return vector;
        }

        /// <summary>
        /// Clamps a Vector3's Z axis only.
        /// </summary>
        /// <param name="vector"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static Vector3 ClampOnZAxis(this Vector3 vector, float min, float max)
        {
            vector.z = Mathf.Clamp(vector.z, min, max);
            return vector;
        }

        /// <summary>
        /// Returns a Vector3 with random values between vector3 min and max.
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static Vector3 RandomVector3(Vector3 min, Vector3 max)
        {
            Vector3 result;
            result.x = Random.Range(min.x, max.x);
            result.y = Random.Range(min.y, max.y);
            result.z = Random.Range(min.z, max.z);
            return result;
        }

        /// <summary>
        /// Returns a Vector2 with x and z values
        /// </summary>
        /// <param name="vector3"></param>
        /// <returns></returns>
        public static Vector2 ToVector2XZ(this Vector3 vector3) => new Vector2(vector3.x, vector3.z);

    }
    
    public static class JegaVector2Extensions
    {
        /// <summary>
        /// Returns a random float value between X and Y.
        /// </summary>
        /// <param name="minMax"></param>
        /// <returns></returns>
        public static float RandomFloatMinMax(this Vector2 minMax)
        {
            return Random.Range(minMax.x, minMax.y);
        }

        public static Vector3 FromPlaneToVector3(this Vector2 vector2) => new Vector3(vector2.x, 0, vector2.y);

        /// <summary>
        /// Returns a Vector2 with result of Quadratic Function.
        /// </summary>
        /// <param name="equation"></param>
        /// <returns>
        /// X = Positive
        /// Y = Negative
        /// </returns>
        public static Vector2 QuadraticFormula(this Vector3 equation)
        {
            Vector2 result = Vector2.zero;
            var a = equation.x;
            var b = equation.y;
            var c = equation.z;
            var delta = Mathf.Sqrt(b * b - 4 * a * c);
            result.x = (-b + delta) / (2 * a);
            result.y = (-b - delta) / (2 * a);
            return result;
        }
	
        /// <summary>
        /// Returns the smaller of a Vector2's values.
        /// </summary>
        /// <param name="vector2"></param>
        /// <returns></returns>
        public static float MinXY(this Vector2 vector2) => Mathf.Min(vector2.x, vector2.y);
        /// <summary>
        /// Returns the greater of a Vector2's values.
        /// </summary>
        /// <param name="vector2"></param>
        /// <returns></returns>
        public static float MaxXY(this Vector2 vector2) => Mathf.Max(vector2.x, vector2.y);
    }

}
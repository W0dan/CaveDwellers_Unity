using System;
using UnityEngine;

namespace Assets.Code.Entities.Player
{
    public static class MathHelpers
    {
        public static Vector3 GetForwardVector(double alpha, float distance)
        {
            var x = -(float)Math.Sin(alpha * Mathf.Deg2Rad) * distance;
            var y = (float)Math.Cos(alpha * Mathf.Deg2Rad) * distance;

            return new Vector3(x, y);
        }

        public static Vector3 GetBackwardVector(double alpha, float distance)
        {
            var x = (float)Math.Sin(alpha * Mathf.Deg2Rad) * distance;
            var y = -(float)Math.Cos(alpha * Mathf.Deg2Rad) * distance;

            return new Vector3(x, y);
        }
    }
}
using UnityEngine;

namespace Assets.Code.Common
{
    internal static class Colors
    {
        public static Color FromArgb(byte alpha, byte red, byte green, byte blue)
        {
            return new Color((float)red / 255, (float)green / 255, (float)blue / 255, (float)alpha / 255);
        }

        public static bool IsEqualTo(this Color color1, Color color2)
        {
            if ((byte)(color1.a * 255) != (byte)(color2.a * 255)) return false;
            if ((byte)(color1.r * 255) != (byte)(color2.r * 255)) return false;
            if ((byte)(color1.g * 255) != (byte)(color2.g * 255)) return false;
            if ((byte)(color1.b * 255) != (byte)(color2.b * 255)) return false;

            return true;
        }
    }
}

/* File Description
 * Original Works/Author: Thomas Slusny
 * Other Contributors: None
 * Author Website: http://indiearmory.com
 * License: MIT
*/

using System;

namespace SFGL.Utils
{
    public static class FloatMath
    {
        private const int LookupSize = 1024*16; //has to be power of 2
		private static readonly float[] getSin, getCos;

        static FloatMath()
        {
            getSin = new float[LookupSize];
            getCos = new float[LookupSize];

            for (var i = 0; i < LookupSize; i++)
            {
                getSin[i] = (float)Math.Sin(i * Math.PI / LookupSize * 2);
                getCos[i] = (float)Math.Cos(i * Math.PI / LookupSize * 2);
            }

            var max = 0f;
            for (int i = 1; i < LookupSize; i++)
            {
                var er = Math.Abs(getSin[i] - getSin[i - 1]);
                if (er > max) max = er;
            }
            Console.WriteLine("Max fast sin error is: " + max / 2);
        }

        /// <summary>
        /// Fast innacurate sinus
        /// </summary>
        public static float Sin(float degrees)
        {
            var rot = GetIndex(degrees);
            return getSin[rot];
        }

        /// <summary>
        /// Fast innacurate cosinus
        /// </summary>
        public static float Cos(float degrees)
        {
            var rot = GetIndex(degrees);
            return getCos[rot];
        }

        static int GetIndex(float degrees)
        {
            return (int)(degrees * (LookupSize / 360f) + 0.5f) & (LookupSize - 1);
        }
        public static void SinCos(float degrees, out float sin, out float cos)
        {
            var rot = GetIndex(degrees);

            sin = getSin[rot];
            cos = getCos[rot];
            
            //sin = get[rot*2];
            //cos = get[rot*2+1];
        }

        public const float Pi = (float) Math.PI;
        public const float TwoPi = Pi * 2;

        public static float Normalize(float radians)
        {
            return radians - TwoPi * (int)((radians+Pi) / TwoPi);
        }

        public static float ToDegrees(float radians)
        {
            return radians/Pi*180;
        }
        public static float ToRadians(float degrees)
        {
            return degrees/180*Pi;
        }
    }
}

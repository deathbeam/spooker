//-----------------------------------------------------------------------------
// MathHelper.cs
//
// Copyright (C) 2006 The Mono.Xna Team. All rights reserved.
// Website: http://monogame.com
// Other Contributors: deathbeam @ http://indiearmory.com
// License: MIT
//-----------------------------------------------------------------------------

using System;

namespace Spooker
{
    public static class MathHelper
	{
		private static readonly Random random = new Random();

        public const double E = Math.E;
        public const double Log10E = 0.4342945f;
        public const double Log2E = 1.442695f;
        public const double Pi = Math.PI;
        public const double PiOver2 = Math.PI / 2.0;
        public const double PiOver4 = Math.PI / 4.0;
        public const double TwoPi = Math.PI * 2.0;

        public static double Barycentric(double value1, double value2, double value3, double amount1, double amount2)
		{
            return value1 + (value2 - value1) * amount1 + (value3 - value1) * amount2;
        }

        public static double CatmullRom(double value1, double value2, double value3, double value4, double amount)
		{
			var amountSquared = amount * amount;
			var amountCubed = amountSquared * amount;
			return 0.5 * (2.0 * value2 +
				(value3 - value1) * amount +
				(2.0 * value1 - 5.0 * value2 + 4.0 * value3 - value4) * amountSquared +
				(3.0 * value2 - value1 - 3.0 * value3 + value4) * amountCubed);
        }

        public static double Clamp(double value, double min, double max)
		{
            value = (value > max) ? max : value;
            value = (value < min) ? min : value;
            return value;
        }

		public static float ScaleClamp(float value, float min, float max, float min2, float max2)
		{
			value = min2 + ((value - min) / (max - min)) * (max2 - min2);
			if (max2 > min2)
			{
				value = value < max2 ? value : max2;
				return value > min2 ? value : min2;
			}
			value = value < min2 ? value : min2;
			return value > max2 ? value : max2;
		}

        public static double Distance(double value1, double value2)
		{
            return Math.Abs(value1 - value2);
        }

        public static double Hermite(double value1, double tangent1, double value2, double tangent2, double amount)
		{
            double v1 = value1, v2 = value2, t1 = tangent1, t2 = tangent2, s = amount, result;
			var sCubed = s * s * s;
			var sSquared = s * s;

			if (amount == 0f)
				result = value1;
			else if (amount == 1f)
				result = value2;
			else
				result = (2 * v1 - 2 * v2 + t2 + t1) * sCubed + (3 * v2 - 3 * v1 - 2 * t1 - t2) * sSquared + t1 * s + v1;
            return result;
        }


        public static double Lerp(double value1, double value2, double amount)
		{
            return value1 + (value2 - value1) * amount;
        }

		public static double Min(params double[] values)
		{
			var min = values[0];
			for (var i = 1; i < values.Length; i++)
				min = Math.Min(values[i], min);
			return min;
		}

		public static double Max(params double[] values)
		{
			var max = values[0];
			for (var i = 1; i < values.Length; i++)
				max = Math.Max(values[i], max);
			return max;
		}

        public static double SmoothStep(double value1, double value2, double amount)
		{
			var result = Clamp(amount, 0f, 1f);
            result = Hermite(value1, 0f, value2, 0f, result);
            return result;
        }

        public static double ToDegrees(double radians)
		{
            return radians * 57.295779513082320876798154814105;
        }

        public static double ToRadians(double degrees)
		{
            return degrees * 0.017453292519943295769236907684886;
        }

        public static double WrapAngle(double angle)
		{
            angle = Math.IEEERemainder(angle, 6.2831854820251465);

			if (angle <= -3.14159274f)
				angle += 6.28318548f;
			else if (angle > 3.14159274f)
				angle -= 6.28318548f;
			return angle;
        }

        public static bool IsPowerOfTwo(int value)
		{
            return (value > 0) && ((value & (value - 1)) == 0);
		}
		
		public static bool RandomChance(double percent)
		{
			return percent >= Random(100);
		}

		public static double Random(double max)
		{
			return Random(0, max);
		}
		
		public static double Random(double min, double max)
		{
			if (min == max) return min;
			if (min > max)
			{
				min = max;
				max = min;
			}
			return min + random.NextDouble() * (max - min);
		}

		public static double Random()
		{
			return random.NextDouble();
		}
    }
}

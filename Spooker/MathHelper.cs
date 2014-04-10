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

        public static double Barycentric(double value1, double value2, double value3, double amount1, double amount2) {
            return value1 + (value2 - value1) * amount1 + (value3 - value1) * amount2;
        }

        public static double CatmullRom(double value1, double value2, double value3, double value4, double amount) {
            // Using formula from http://www.mvps.org/directx/articles/catmull/
            // Internally using doubles not to lose precission
            double amountSquared = amount * amount;
            double amountCubed = amountSquared * amount;
            return 0.5 * (2.0 * value2 +
                          (value3 - value1) * amount +
                          (2.0 * value1 - 5.0 * value2 + 4.0 * value3 - value4) * amountSquared +
                          (3.0 * value2 - value1 - 3.0 * value3 + value4) * amountCubed);
        }

		/// <summary>
		/// Clamps a value inside a range.
		/// </summary>
		/// <param name="value">The value to clamp.</param>
		/// <param name="min">Min clamp.</param>
		/// <param name="max">Max clamp.</param>
		/// <returns>The new value between min and max.</returns>
        public static double Clamp(double value, double min, double max) {
            // First we check to see if we're greater than the max
            value = (value > max) ? max : value;

            // Then we check to see if we're less than the min.
            value = (value < min) ? min : value;

            // There's no check to see if min > max.
            return value;
        }

		public static float ScaleClamp(float value, float min, float max, float min2, float max2) {
			value = min2 + ((value - min) / (max - min)) * (max2 - min2);
			if (max2 > min2) {
				value = value < max2 ? value : max2;
				return value > min2 ? value : min2;
			}
			value = value < min2 ? value : min2;
			return value > max2 ? value : max2;
		}

        public static double Distance(double value1, double value2) {
            return Math.Abs(value1 - value2);
        }

        public static double Hermite(double value1, double tangent1, double value2, double tangent2, double amount) {
            // All transformed to double not to lose precission
            // Otherwise, for high numbers of param:amount the result is NaN instead of Infinity
            double v1 = value1, v2 = value2, t1 = tangent1, t2 = tangent2, s = amount, result;
            double sCubed = s * s * s;
            double sSquared = s * s;

            if (amount == 0f)
                result = value1;
            else if (amount == 1f)
                result = value2;
            else
                result = (2 * v1 - 2 * v2 + t2 + t1) * sCubed +
                    (3 * v2 - 3 * v1 - 2 * t1 - t2) * sSquared +
                    t1 * s +
                    v1;
            return result;
        }


        public static double Lerp(double value1, double value2, double amount) {
            return value1 + (value2 - value1) * amount;
        }

		public static double Min(params double[] values) {
			var min = values[0];
			for (var i = 1; i < values.Length; i++)
				min = Math.Min(values[i], min);
			return min;
		}

		public static double Max(params double[] values) {
			var max = values[0];
			for (var i = 1; i < values.Length; i++)
				max = Math.Max(values[i], max);
			return max;
		}

        public static double SmoothStep(double value1, double value2, double amount) {
            // It is expected that 0 < amount < 1
            // If amount < 0, return value1
            // If amount > 1, return value2
            double result = Clamp(amount, 0f, 1f);
            result = Hermite(value1, 0f, value2, 0f, result);
            return result;
        }

        public static double ToDegrees(double radians) {
            // This method uses double precission internally,
            // though it returns single double
            // Factor = 180 / pi
            return radians * 57.295779513082320876798154814105;
        }

        public static double ToRadians(double degrees) {
            // This method uses double precission internally,
            // though it returns single double
            // Factor = pi / 180
            return degrees * 0.017453292519943295769236907684886;
        }

        public static double WrapAngle(double angle) {
            angle = Math.IEEERemainder(angle, 6.2831854820251465);
            if (angle <= -3.14159274f) {
                angle += 6.28318548f;
            }
            else {
                if (angle > 3.14159274f) {
                    angle -= 6.28318548f;
                }
            }
            return angle;
        }

        public static bool IsPowerOfTwo(int value) {
            return (value > 0) && ((value & (value - 1)) == 0);
		}

		/// <summary>
		/// A random chance of true or false with controlled odds.
		/// </summary>
		/// <param name="percent">How likely to return true.</param>
		/// <returns>True or false based on the random percent.</returns>
		public static bool RandomChance(double percent) {
			return percent >= RandomRange(100);
		}

		/// <summary>
		/// Returns a number inside a range.
		/// </summary>
		/// <param name="min"></param>
		/// <param name="max"></param>
		/// <returns></returns>
		public static double RandomRange(double min, double max) {
			if (min == max) return min;
			if (min > max) {
				min = max;
				max = min;
			}
			return min + random.NextDouble() * (max - min);
		}

		/// <summary>
		/// Returns a number inside a range.
		/// </summary>
		/// <param name="max"></param>
		/// <returns></returns>
		public static double RandomRange(double max) {
			return RandomRange(0, max);
		}

		/// <summary>
		/// Produces a random number.
		/// </summary>
		/// <returns></returns>
		public static double Random() {
			return random.NextDouble();
		}
    }
}

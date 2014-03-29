//-----------------------------------------------------------------------------
// Vector2.cs
//
// Copyright (C) 2006 The Mono.Xna Team. All rights reserved.
// Website: http://monogame.com
// Other Contributors: deathbeam @ http://indiearmory.com
// License: MIT
//-----------------------------------------------------------------------------

using System;
using System.Globalization;

namespace Spooker
{
    [Serializable]
    public struct Vector2 : IEquatable<Vector2>
	{
        #region Public Fields

        public float X;
        public float Y;

        #endregion Public Fields


        #region Properties

        public static Vector2 Zero
		{
			get { return new Vector2(0f, 0f); }
        }

        public static Vector2 One
		{
			get { return new Vector2(1f, 1f); }
        }

        public static Vector2 UnitX
		{
			get { return new Vector2(1f, 0f); }
        }

        public static Vector2 UnitY
		{
			get { return new Vector2(0f, 1f); }
        }

        #endregion Properties

		#region SFML Helpers

		internal Vector2(SFML.Window.Vector2f copy)
		{
			X = copy.X;
			Y = copy.Y;
		}

		internal SFML.Window.Vector2f ToSfml()
		{
			return new SFML.Window.Vector2f(X, Y);
		}

		#endregion


        #region Constructors

        public Vector2(float x, float y)
		{
            X = x;
            Y = y;
        }

        public Vector2(float value)
		{
            X = value;
            Y = value;
        }

        #endregion Constructors


        #region Public Methods

        public static Vector2 Add(Vector2 value1, Vector2 value2)
		{
            value1.X += value2.X;
            value1.Y += value2.Y;
            return value1;
        }

        public static void Add(ref Vector2 value1, ref Vector2 value2, out Vector2 result)
		{
            result.X = value1.X + value2.X;
            result.Y = value1.Y + value2.Y;
        }

        public static Vector2 Barycentric(Vector2 value1, Vector2 value2, Vector2 value3, float amount1, float amount2)
		{
            return new Vector2(
				(float)MathHelper.Barycentric(value1.X, value2.X, value3.X, amount1, amount2),
				(float)MathHelper.Barycentric(value1.Y, value2.Y, value3.Y, amount1, amount2));
        }

        public static void Barycentric(ref Vector2 value1, ref Vector2 value2, ref Vector2 value3, float amount1, float amount2, out Vector2 result)
		{
            result = new Vector2(
				(float)MathHelper.Barycentric(value1.X, value2.X, value3.X, amount1, amount2),
				(float)MathHelper.Barycentric(value1.Y, value2.Y, value3.Y, amount1, amount2));
        }

        public static Vector2 CatmullRom(Vector2 value1, Vector2 value2, Vector2 value3, Vector2 value4, float amount)
		{
            return new Vector2(
				(float)MathHelper.CatmullRom(value1.X, value2.X, value3.X, value4.X, amount),
				(float)MathHelper.CatmullRom(value1.Y, value2.Y, value3.Y, value4.Y, amount));
        }

        public static void CatmullRom(ref Vector2 value1, ref Vector2 value2, ref Vector2 value3, ref Vector2 value4, float amount, out Vector2 result)
		{
            result = new Vector2(
				(float)MathHelper.CatmullRom(value1.X, value2.X, value3.X, value4.X, amount),
				(float)MathHelper.CatmullRom(value1.Y, value2.Y, value3.Y, value4.Y, amount));
        }

        public static Vector2 Clamp(Vector2 value1, Vector2 min, Vector2 max)
		{
            return new Vector2(
				(float)MathHelper.Clamp(value1.X, min.X, max.X),
				(float)MathHelper.Clamp(value1.Y, min.Y, max.Y));
        }

        public static void Clamp(ref Vector2 value1, ref Vector2 min, ref Vector2 max, out Vector2 result)
		{
            result = new Vector2(
				(float)MathHelper.Clamp(value1.X, min.X, max.X),
				(float)MathHelper.Clamp(value1.Y, min.Y, max.Y));
        }

		////////////////////////////////////////////////////////////
		/// <summary>
		/// Calculates the direction between two vectors.
		/// </summary>
		/// <param name="value1">Source vector.</param>
		/// <param name="value2">Source vector.</param>
		////////////////////////////////////////////////////////////
		public static float Direction(Vector2 value1, Vector2 value2)
		{
			var num2 = value2.X - value1.X;
			var num = value1.Y - value2.Y;
			var num3 = (float)Math.Atan2(num, num2);
			return num3 < 0 ? num3 + (2 * (float)Math.PI) : num3;
		}

		////////////////////////////////////////////////////////////
		/// <summary>
		/// Creates vector in specified direction with specified length.
		/// </summary>
		/// <param name="dir">Direction.</param>
		/// <param name="len">Length.</param>
		////////////////////////////////////////////////////////////
		public static Vector2 LengthDir(float dir, float len)
		{
			var num2 = (float)-Math.Sin (dir);
			var num1 = (float)Math.Cos (dir);
			var num3 = num1 * len;
			var num4 = num2 * len;
			return new Vector2(num3, num4);
		}

        public static float Distance(Vector2 value1, Vector2 value2)
		{
            float v1 = value1.X - value2.X, v2 = value1.Y - value2.Y;
			return (float)Math.Sqrt((v1 * v1) + (v2 * v2));
        }

        public static void Distance(ref Vector2 value1, ref Vector2 value2, out float result)
		{
            float v1 = value1.X - value2.X, v2 = value1.Y - value2.Y;
			result = (float)Math.Sqrt((v1 * v1) + (v2 * v2));
        }

        public static float DistanceSquared(Vector2 value1, Vector2 value2)
		{
            float v1 = value1.X - value2.X, v2 = value1.Y - value2.Y;
            return (v1 * v1) + (v2 * v2);
        }

        public static void DistanceSquared(ref Vector2 value1, ref Vector2 value2, out float result)
		{
            float v1 = value1.X - value2.X, v2 = value1.Y - value2.Y;
            result = (v1 * v1) + (v2 * v2);
        }

        public static Vector2 Divide(Vector2 value1, Vector2 value2)
		{
            value1.X /= value2.X;
            value1.Y /= value2.Y;
            return value1;
        }

        public static void Divide(ref Vector2 value1, ref Vector2 value2, out Vector2 result)
		{
            result.X = value1.X / value2.X;
            result.Y = value1.Y / value2.Y;
        }

        public static Vector2 Divide(Vector2 value1, float divider)
		{
            float factor = 1 / divider;
            value1.X *= factor;
            value1.Y *= factor;
            return value1;
        }

        public static void Divide(ref Vector2 value1, float divider, out Vector2 result)
		{
            float factor = 1 / divider;
            result.X = value1.X * factor;
            result.Y = value1.Y * factor;
        }

        public static float Dot(Vector2 value1, Vector2 value2)
		{
            return (value1.X * value2.X) + (value1.Y * value2.Y);
        }

        public static void Dot(ref Vector2 value1, ref Vector2 value2, out float result)
		{
            result = (value1.X * value2.X) + (value1.Y * value2.Y);
        }

        public override bool Equals(object obj)
		{
            if (obj is Vector2)
                return Equals(this);

            return false;
        }

        public bool Equals(Vector2 other)
		{
            return (X == other.X) && (Y == other.Y);
        }

        public static Vector2 Reflect(Vector2 vector, Vector2 normal)
		{
            Vector2 result;
			var val = 2.0f * ((vector.X * normal.X) + (vector.Y * normal.Y));
            result.X = vector.X - (normal.X * val);
            result.Y = vector.Y - (normal.Y * val);
            return result;
        }

        public static void Reflect(ref Vector2 vector, ref Vector2 normal, out Vector2 result)
		{
			var val = 2.0f * ((vector.X * normal.X) + (vector.Y * normal.Y));
            result.X = vector.X - (normal.X * val);
            result.Y = vector.Y - (normal.Y * val);
        }

        public override int GetHashCode()
		{
            return X.GetHashCode() + Y.GetHashCode();
        }

        public static Vector2 Hermite(Vector2 value1, Vector2 tangent1, Vector2 value2, Vector2 tangent2, float amount)
		{
            Vector2 result;
            Hermite(ref value1, ref tangent1, ref value2, ref tangent2, amount, out result);
            return result;
        }

        public static void Hermite(ref Vector2 value1, ref Vector2 tangent1, ref Vector2 value2, ref Vector2 tangent2, float amount, out Vector2 result)
		{
			result.X = (float)MathHelper.Hermite(value1.X, tangent1.X, value2.X, tangent2.X, amount);
			result.Y = (float)MathHelper.Hermite(value1.Y, tangent1.Y, value2.Y, tangent2.Y, amount);
        }

		public double Length()
		{
			return (float)Math.Sqrt((X * X) + (Y * Y));
		}

		public double LengthSquared()
		{
			return (X * X) + (Y * Y);
		}

        public static Vector2 Lerp(Vector2 value1, Vector2 value2, float amount)
		{
            return new Vector2(
				(float)MathHelper.Lerp(value1.X, value2.X, amount),
				(float)MathHelper.Lerp(value1.Y, value2.Y, amount));
        }

        public static void Lerp(ref Vector2 value1, ref Vector2 value2, float amount, out Vector2 result)
		{
            result = new Vector2(
				(float)MathHelper.Lerp(value1.X, value2.X, amount),
				(float)MathHelper.Lerp(value1.Y, value2.Y, amount));
        }

        public static Vector2 Max(Vector2 value1, Vector2 value2)
		{
            return new Vector2(value1.X > value2.X ? value1.X : value2.X,
                               value1.Y > value2.Y ? value1.Y : value2.Y);
        }

        public static void Max(ref Vector2 value1, ref Vector2 value2, out Vector2 result)
		{
            result.X = value1.X > value2.X ? value1.X : value2.X;
            result.Y = value1.Y > value2.Y ? value1.Y : value2.Y;
        }

        public static Vector2 Min(Vector2 value1, Vector2 value2)
		{
            return new Vector2(value1.X < value2.X ? value1.X : value2.X,
                               value1.Y < value2.Y ? value1.Y : value2.Y);
        }

        public static void Min(ref Vector2 value1, ref Vector2 value2, out Vector2 result)
		{
            result.X = value1.X < value2.X ? value1.X : value2.X;
            result.Y = value1.Y < value2.Y ? value1.Y : value2.Y;
        }

        public static Vector2 Multiply(Vector2 value1, Vector2 value2)
		{
            value1.X *= value2.X;
            value1.Y *= value2.Y;
            return value1;
        }

        public static Vector2 Multiply(Vector2 value1, float scaleFactor)
		{
            value1.X *= scaleFactor;
            value1.Y *= scaleFactor;
            return value1;
        }

        public static void Multiply(ref Vector2 value1, float scaleFactor, out Vector2 result)
		{
            result.X = value1.X * scaleFactor;
            result.Y = value1.Y * scaleFactor;
        }

        public static void Multiply(ref Vector2 value1, ref Vector2 value2, out Vector2 result)
		{
            result.X = value1.X * value2.X;
            result.Y = value1.Y * value2.Y;
        }

        public static Vector2 Negate(Vector2 value)
		{
            value.X = -value.X;
            value.Y = -value.Y;
            return value;
        }

        public static void Negate(ref Vector2 value, out Vector2 result)
		{
            result.X = -value.X;
            result.Y = -value.Y;
        }

        public void Normalize()
		{
			float val = 1.0f / (float)Math.Sqrt((X * X) + (Y * Y));
            X *= val;
            Y *= val;
        }

        public void Normalize(float value)
		{
			float val = 1.0f / (float)Math.Sqrt((X * X) + (Y * Y));
            X *= val * value;
            Y *= val * value;
        }

        public static Vector2 Normalize(Vector2 value)
		{
			float val = 1.0f / (float)Math.Sqrt((value.X * value.X) + (value.Y * value.Y));
            value.X *= val;
            value.Y *= val;
            return value;
        }

        public static void Normalize(ref Vector2 value, out Vector2 result)
		{
			float val = 1.0f / (float)Math.Sqrt((value.X * value.X) + (value.Y * value.Y));
            result.X = value.X * val;
            result.Y = value.Y * val;
        }

        public static Vector2 SmoothStep(Vector2 value1, Vector2 value2, float amount)
		{
            return new Vector2(
				(float)MathHelper.SmoothStep(value1.X, value2.X, amount),
				(float)MathHelper.SmoothStep(value1.Y, value2.Y, amount));
        }

        public static void SmoothStep(ref Vector2 value1, ref Vector2 value2, float amount, out Vector2 result)
		{
            result = new Vector2(
				(float)MathHelper.SmoothStep(value1.X, value2.X, amount),
				(float)MathHelper.SmoothStep(value1.Y, value2.Y, amount));
        }

        public static Vector2 Subtract(Vector2 value1, Vector2 value2)
		{
            value1.X -= value2.X;
            value1.Y -= value2.Y;
            return value1;
        }

        public static void Subtract(ref Vector2 value1, ref Vector2 value2, out Vector2 result)
		{
            result.X = value1.X - value2.X;
            result.Y = value1.Y - value2.Y;
        }

        public override string ToString()
		{
            CultureInfo currentCulture = CultureInfo.CurrentCulture;
            return string.Format(currentCulture, "{{X:{0} Y:{1}}}", new object[] { 
				X.ToString(currentCulture), Y.ToString(currentCulture) });
        }

        #endregion Public Methods


        #region Operators

        public static Vector2 operator -(Vector2 value)
		{
            value.X = -value.X;
            value.Y = -value.Y;
            return value;
        }


        public static bool operator ==(Vector2 value1, Vector2 value2)
		{
            return value1.X == value2.X && value1.Y == value2.Y;
        }


        public static bool operator !=(Vector2 value1, Vector2 value2)
		{
            return value1.X != value2.X || value1.Y != value2.Y;
        }


        public static Vector2 operator +(Vector2 value1, Vector2 value2)
		{
            value1.X += value2.X;
            value1.Y += value2.Y;
            return value1;
        }


        public static Vector2 operator -(Vector2 value1, Vector2 value2)
		{
            value1.X -= value2.X;
            value1.Y -= value2.Y;
            return value1;
        }


        public static Vector2 operator *(Vector2 value1, Vector2 value2)
		{
            value1.X *= value2.X;
            value1.Y *= value2.Y;
            return value1;
        }


        public static Vector2 operator *(Vector2 value, float scaleFactor)
		{
            value.X *= scaleFactor;
            value.Y *= scaleFactor;
            return value;
        }


        public static Vector2 operator *(float scaleFactor, Vector2 value)
		{
            value.X *= scaleFactor;
            value.Y *= scaleFactor;
            return value;
        }


        public static Vector2 operator /(Vector2 value1, Vector2 value2)
		{
            value1.X /= value2.X;
            value1.Y /= value2.Y;
            return value1;
        }


        public static Vector2 operator /(Vector2 value1, float divider)
		{
            float factor = 1 / divider;
            value1.X *= factor;
            value1.Y *= factor;
            return value1;
        }

        #endregion Operators
    }
}

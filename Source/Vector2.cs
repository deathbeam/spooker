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

		#endregion SFML Helpers


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

        #endregion Constructors


		#region Static Methods

        public static Vector2 Add(Vector2 value1, Vector2 value2)
		{
            value1.X += value2.X;
            value1.Y += value2.Y;
            return value1;
        }

        public static Vector2 Barycentric(Vector2 value1, Vector2 value2, Vector2 value3, float amount1, float amount2)
		{
            return new Vector2(
				(float)MathHelper.Barycentric(value1.X, value2.X, value3.X, amount1, amount2),
				(float)MathHelper.Barycentric(value1.Y, value2.Y, value3.Y, amount1, amount2));
        }

        public static Vector2 CatmullRom(Vector2 value1, Vector2 value2, Vector2 value3, Vector2 value4, float amount)
		{
            return new Vector2(
				(float)MathHelper.CatmullRom(value1.X, value2.X, value3.X, value4.X, amount),
				(float)MathHelper.CatmullRom(value1.Y, value2.Y, value3.Y, value4.Y, amount));
        }

        public static Vector2 Clamp(Vector2 value1, Vector2 min, Vector2 max)
		{
            return new Vector2(
				(float)MathHelper.Clamp(value1.X, min.X, max.X),
				(float)MathHelper.Clamp(value1.Y, min.Y, max.Y));
        }
		
		public static float Direction(Vector2 value1, Vector2 value2)
		{
			var num2 = value2.X - value1.X;
			var num = value1.Y - value2.Y;
			var num3 = (float)Math.Atan2(num, num2);
			return num3 < 0 ? num3 + (2 * (float)Math.PI) : num3;
		}
		
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

        public static float DistanceSquared(Vector2 value1, Vector2 value2)
		{
            float v1 = value1.X - value2.X, v2 = value1.Y - value2.Y;
            return (v1 * v1) + (v2 * v2);
        }

        public static Vector2 Divide(Vector2 value1, Vector2 value2)
		{
            value1.X /= value2.X;
            value1.Y /= value2.Y;
            return value1;
        }

        public static Vector2 Divide(Vector2 value1, float divider)
		{
            float factor = 1 / divider;
            value1.X *= factor;
            value1.Y *= factor;
            return value1;
        }
		
        public static float Dot(Vector2 value1, Vector2 value2)
		{
            return (value1.X * value2.X) + (value1.Y * value2.Y);
        }
		
        public static Vector2 Reflect(Vector2 vector, Vector2 normal)
		{
            Vector2 result;
			var val = 2.0f * ((vector.X * normal.X) + (vector.Y * normal.Y));
            result.X = vector.X - (normal.X * val);
            result.Y = vector.Y - (normal.Y * val);
            return result;
        }

        public static Vector2 Hermite(Vector2 value1, Vector2 tangent1, Vector2 value2, Vector2 tangent2, float amount)
		{
			return new Vector2 ((float)MathHelper.Hermite (value1.X, tangent1.X, value2.X, tangent2.X, amount),
				(float)MathHelper.Hermite (value1.Y, tangent1.Y, value2.Y, tangent2.Y, amount));
        }

        public static Vector2 Lerp(Vector2 value1, Vector2 value2, float amount)
		{
            return new Vector2(
				(float)MathHelper.Lerp(value1.X, value2.X, amount),
				(float)MathHelper.Lerp(value1.Y, value2.Y, amount));
        }

        public static Vector2 Max(Vector2 value1, Vector2 value2)
		{
            return new Vector2(value1.X > value2.X ? value1.X : value2.X,
                               value1.Y > value2.Y ? value1.Y : value2.Y);
        }
		
        public static Vector2 Min(Vector2 value1, Vector2 value2)
		{
            return new Vector2(value1.X < value2.X ? value1.X : value2.X,
                               value1.Y < value2.Y ? value1.Y : value2.Y);
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

        public static Vector2 Negate(Vector2 value)
		{
            value.X = -value.X;
            value.Y = -value.Y;
            return value;
        }

        public static Vector2 Normalize(Vector2 value)
		{
			var val = 1f / (float)Math.Sqrt((value.X * value.X) + (value.Y * value.Y));
            value.X *= val;
            value.Y *= val;
            return value;
        }

        public static Vector2 SmoothStep(Vector2 value1, Vector2 value2, float amount)
		{
            return new Vector2(
				(float)MathHelper.SmoothStep(value1.X, value2.X, amount),
				(float)MathHelper.SmoothStep(value1.Y, value2.Y, amount));
        }

        public static Vector2 Subtract(Vector2 value1, Vector2 value2)
		{
            value1.X -= value2.X;
            value1.Y -= value2.Y;
            return value1;
        }

		#endregion Static Methods


		#region Public Methods

		public override bool Equals(object obj)
		{
			return (obj is Vector2) && Equals((Vector2)obj);
		}

		public bool Equals(Vector2 other)
		{
			return (X == other.X) && (Y == other.Y);
		}

		public override int GetHashCode()
		{
			return X.GetHashCode() ^ Y.GetHashCode();
		}

		public double Length()
		{
			return (float)Math.Sqrt ((X * X) + (Y * Y));
		}

		public double LengthSquared()
		{
			return (X * X) + (Y * Y);
		}

		public void Normalize()
		{
			Normalize (1f);
		}

		public void Normalize(float value)
		{
			float val = 1f / (float)Math.Sqrt((X * X) + (Y * Y));
			X *= val * value;
			Y *= val * value;
		}

		public override string ToString()
		{
			var currentCulture = CultureInfo.CurrentCulture;
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

		public static Vector2 operator +(Vector2 value, float addFactor)
		{
			value.X += addFactor;
			value.Y += addFactor;
			return value;
		}

        public static Vector2 operator -(Vector2 value1, Vector2 value2)
		{
            value1.X -= value2.X;
            value1.Y -= value2.Y;
            return value1;
        }

		public static Vector2 operator -(Vector2 value, float substractFactor)
		{
			value.X -= substractFactor;
			value.Y -= substractFactor;
			return value;
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
			var factor = 1f / divider;
            value1.X *= factor;
            value1.Y *= factor;
            return value1;
        }

        #endregion Operators
    }
}

/* File Description
 * Original Works/Author: Laurent Gomilla
 * Other Contributors: Thomas Slusny
 * Author Website: http://sfml-dev.org
 * License: 
*/

using System;
using System.Runtime.InteropServices;
using SFML.Window;

namespace SFGL.Graphics
{
	////////////////////////////////////////////////////////////
	/// <summary>
	/// An implementation of SFML.Window.Vector2f with extended
    /// functionality.
	/// </summary>
	////////////////////////////////////////////////////////////
	[StructLayout(LayoutKind.Sequential)]
	[Serializable]
	public struct Vector2 : IEquatable<Vector2>
	{
		#region Static members
		////////////////////////////////////////////////////////////
		/// <summary>
		/// Returns a Vector2 with all of its components set to zero.
		/// </summary>
		////////////////////////////////////////////////////////////
		public static Vector2 Zero
		{
			get { return new Vector2(0,0); }
		}

		////////////////////////////////////////////////////////////
		/// <summary>
		/// Returns a Vector2 with both of its components set to one.
		/// </summary>
		////////////////////////////////////////////////////////////
		public static Vector2 One
		{
			get { return new Vector2(1,1); }
		}

		////////////////////////////////////////////////////////////
		/// <summary>
		/// Calculates the distance between two vectors.
		/// </summary>
		/// <param name="value1">Source vector.</param>
		/// <param name="value2">Source vector.</param>
		////////////////////////////////////////////////////////////
		public static float Distance(Vector2 value1, Vector2 value2)
		{
			var num2 = value1.X - value2.X;
			var num = value1.Y - value2.Y;
			var num3 = (num2 * num2) + (num * num);
			return (float)Math.Sqrt(num3);
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

		////////////////////////////////////////////////////////////
		/// <summary>
		/// Performs a linear interpolation between two vectors.
		/// </summary>
		/// <param name="value1">Source vector.</param>
		/// <param name="value2">Source vector.</param>
		/// <param name="amount">Value between 0 and 1 indicating
		/// the weight of value2.</param>
		////////////////////////////////////////////////////////////
		public static Vector2 Lerp(Vector2 value1, Vector2 value2, float amount)
		{
			Vector2 vector;
			vector.X = value1.X + ((value2.X - value1.X) * amount);
			vector.Y = value1.Y + ((value2.Y - value1.Y) * amount);
			return vector;
		}
		#endregion

		#region Properties
		/// <summary>X (horizontal) component of the vector</summary>
		public float X;

		/// <summary>Y (vertical) component of the vector</summary>
		public float Y;
		#endregion

		#region Constructors
		////////////////////////////////////////////////////////////
		/// <summary>
		/// Construct the vector from its coordinates
		/// </summary>
		/// <param name="x">X coordinate</param>
		/// <param name="y">Y coordinate</param>
		////////////////////////////////////////////////////////////
		public Vector2(float x, float y)
		{
			X = x;
			Y = y;
		}

		////////////////////////////////////////////////////////////
		/// <summary>
		/// Construct the vector from its coordinates
		/// </summary>
		/// <param name="x">X coordinate</param>
		/// <param name="y">Y coordinate</param>
		////////////////////////////////////////////////////////////
		public Vector2(int x, int y)
		{
			X = (float)x;
			Y = (float)y;
		}

		////////////////////////////////////////////////////////////
		/// <summary>
		/// Construct the vector from its coordinates
		/// </summary>
		/// <param name="x">X coordinate</param>
		/// <param name="y">Y coordinate</param>
		////////////////////////////////////////////////////////////
		public Vector2(uint x, uint y)
		{
			X = (float)x;
			Y = (float)y;
		}

		////////////////////////////////////////////////////////////
		/// <summary>
		/// Construct the vector from one value set to both coordinates
		/// </summary>
		/// <param name="value">Value to initialize both
		/// components to.</param>
		////////////////////////////////////////////////////////////
		public Vector2(float value)
		{
			X = Y = value;
		}

		////////////////////////////////////////////////////////////
		/// <summary>
		/// Construct the vector from one value set to both coordinates
		/// </summary>
		/// <param name="value">Value to initialize both
		/// components to.</param>
		////////////////////////////////////////////////////////////
		public Vector2(int value)
		{
			X = Y = value;
		}

		////////////////////////////////////////////////////////////
		/// <summary>
		/// Construct the vector from one value set to both coordinates
		/// </summary>
		/// <param name="value">Value to initialize both
		/// components to.</param>
		////////////////////////////////////////////////////////////
		public Vector2(uint value)
		{
			X = Y = value;
		}

		////////////////////////////////////////////////////////////
		/// <summary>
		/// Construct the vector from another vector
		/// </summary>
		/// <param name="copy">Vector from what will this vector 
		/// constructs</param>
		////////////////////////////////////////////////////////////
		public Vector2(Vector2 copy)
		{
			X = copy.X;
			Y = copy.Y;
		}

		////////////////////////////////////////////////////////////
		/// <summary>
		/// Construct the vector from integer vector
		/// </summary>
		/// <param name="copy">Integer vector from what will this vector 
		/// constructs</param>
		////////////////////////////////////////////////////////////
		public Vector2(Vector2i copy)
		{
			X = (float)copy.X;
			Y = (float)copy.Y;
		}

		////////////////////////////////////////////////////////////
		/// <summary>
		/// Construct the vector from float vector
		/// </summary>
		/// <param name="copy">Float vector from what will this vector 
		/// constructs</param>
		////////////////////////////////////////////////////////////
		public Vector2(Vector2f copy)
		{
			X = copy.X;
			Y = copy.Y;
		}

		////////////////////////////////////////////////////////////
		/// <summary>
		/// Construct the vector from unsinged integer vector
		/// </summary>
		/// <param name="copy">Unsinged integer vector from what will
		/// this vector constructs</param>
		////////////////////////////////////////////////////////////
		public Vector2(Vector2u copy)
		{
			X = (float)copy.X;
			Y = (float)copy.Y;
		}
		#endregion

		#region Functions
		////////////////////////////////////////////////////////////
		/// <summary>
		/// Determines whether the specified Object is equal to
		/// the Vector2.
		/// </summary>
		/// <param name="other">The Object to compare with the
		/// current Vector2.</param>
		////////////////////////////////////////////////////////////
		public bool Equals(Vector2 other)
		{
			return ((X == other.X) && (Y == other.Y));
		}

		////////////////////////////////////////////////////////////
		/// <summary>
		/// Returns a value that indicates whether the current
		/// instance is equal to a specified object.
		/// </summary>
		/// <param name="obj">Object to make the comparison with.</param>
		////////////////////////////////////////////////////////////
		public override bool Equals(object obj)
		{
			var flag = false;
			if (obj is Vector2)
				flag = Equals((Vector2)obj);
			return flag;
		}

		////////////////////////////////////////////////////////////
		/// <summary>
		/// Gets the hash code of the vector object.
		/// </summary>
		/// <returns>Hash code of the object</returns>
		////////////////////////////////////////////////////////////
		public override int GetHashCode()
		{
			return (X.GetHashCode() + Y.GetHashCode());
		}

		////////////////////////////////////////////////////////////
		/// <summary>
		/// Provide a string describing the object
		/// </summary>
		/// <returns>String description of the object</returns>
		////////////////////////////////////////////////////////////
		public override string ToString()
		{
			return "[Vector2]" +
				" X(" + X + ")" +
				" Y(" + Y + ")";
		}
		#endregion

		#region Operators
		////////////////////////////////////////////////////////////
		/// <summary>
		/// Operator - overload ; returns the opposite of a vector
		/// </summary>
		/// <param name="v">Vector to negate</param>
		/// <returns>-v</returns>
		////////////////////////////////////////////////////////////
		public static Vector2 operator -(Vector2 v)
		{
			return new Vector2(-v.X, -v.Y);
		}

		////////////////////////////////////////////////////////////
		/// <summary>
		/// Operator - overload ; subtracts two vectors
		/// </summary>
		/// <param name="v1">First vector</param>
		/// <param name="v2">Second vector</param>
		/// <returns>v1 - v2</returns>
		////////////////////////////////////////////////////////////
		public static Vector2 operator -(Vector2 v1, Vector2 v2)
		{
			return new Vector2(v1.X - v2.X, v1.Y - v2.Y);
		}

		////////////////////////////////////////////////////////////
		/// <summary>
		/// Operator + overload ; add two vectors
		/// </summary>
		/// <param name="v1">First vector</param>
		/// <param name="v2">Second vector</param>
		/// <returns>v1 + v2</returns>
		////////////////////////////////////////////////////////////
		public static Vector2 operator +(Vector2 v1, Vector2 v2)
		{
			return new Vector2(v1.X + v2.X, v1.Y + v2.Y);
		}

		////////////////////////////////////////////////////////////
		/// <summary>
		/// Operator == overload; tests vectors for equality.
		/// </summary>
		/// <param name="value1">Source vector.</param>
		/// <param name="value2">Source vector.</param>
		////////////////////////////////////////////////////////////
		public static bool operator ==(Vector2 value1, Vector2 value2)
		{
			return ((value1.X == value2.X) && (value1.Y == value2.Y));
		}

		////////////////////////////////////////////////////////////
		/// <summary>
		/// Operator != overload; tests vectors for inequality.
		/// </summary>
		/// <param name="value1">Vector to compare.</param>
		/// <param name="value2">Vector to compare.</param>
		////////////////////////////////////////////////////////////
		public static bool operator !=(Vector2 value1, Vector2 value2)
		{
			if (value1.X == value2.X)
				return (value1.Y != value2.Y);
			return true;
		}

		////////////////////////////////////////////////////////////
		/// <summary>
		/// Operator * overload ; multiply a vector by another vector
		/// </summary>
		/// <param name="value1">Vector</param>
		/// <param name="value2">Another vector</param>
		/// <returns>value1 * value2</returns>
		////////////////////////////////////////////////////////////
		public static Vector2 operator *(Vector2 value1, Vector2 value2)
		{
			return new Vector2(value1.X * value2.X, value1.Y * value2.Y);
		}

		////////////////////////////////////////////////////////////
		/// <summary>
		/// Operator * overload ; multiply a vector by a scalar value
		/// </summary>
		/// <param name="v">Vector</param>
		/// <param name="x">Scalar value</param>
		/// <returns>v * x</returns>
		////////////////////////////////////////////////////////////
		public static Vector2 operator *(Vector2 v, float x)
		{
			return new Vector2(v.X * x, v.Y * x);
		}

		////////////////////////////////////////////////////////////
		/// <summary>
		/// Operator * overload ; multiply a scalar value by a vector
		/// </summary>
		/// <param name="x">Scalar value</param>
		/// <param name="v">Vector</param>
		/// <returns>x* v</returns>
		////////////////////////////////////////////////////////////
		public static Vector2 operator *(float x, Vector2 v)
		{
			return new Vector2(v.X * x, v.Y * x);
		}

		////////////////////////////////////////////////////////////
		/// <summary>
		/// Operator / overload ; divide a vector by another vector
		/// </summary>
		/// <param name="value1">Vector</param>
		/// <param name="value2">Another vector</param>
		/// <returns>value1 / value2</returns>
		////////////////////////////////////////////////////////////
		public static Vector2 operator /(Vector2 value1, Vector2 value2)
		{
			return new Vector2(value1.X / value2.X, value1.Y / value2.Y);
		}

		////////////////////////////////////////////////////////////
		/// <summary>
		/// Operator / overload ; divide a vector by a scalar value
		/// </summary>
		/// <param name="v">Vector</param>
		/// <param name="x">Scalar value</param>
		/// <returns>v / x</returns>
		////////////////////////////////////////////////////////////
		public static Vector2 operator /(Vector2 v, float x)
		{
			return new Vector2(v.X / x, v.Y / x);
		}
		#endregion
	}
}
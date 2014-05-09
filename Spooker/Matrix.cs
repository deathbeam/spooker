//-----------------------------------------------------------------------------
// Matrix.cs
//
// Spooker Open Source Game Framework
// Copyright (C) Indie Armory. All rights reserved.
// Website: http://indiearmory.com
// Other Contributors: Mono.Xna Team @ https://monogame.com, Laurent Gomila @ http://sfml-dev.org
// License: MIT
//-----------------------------------------------------------------------------

using System;

namespace Spooker
{
	public struct Matrix
	{
		#region Public fields
		
		public float M11, M12, M13, M14,
					 M21, M22, M23, M24,
			  		 M31, M32, M33, M34,
			  		 M41, M42, M43, M44;

		#endregion

		#region SFML Helpers

		internal SFML.Graphics.Transform ToSfml()
		{
			return new SFML.Graphics.Transform (
				M11, M12, M14,
				M21, M22, M24,
				M41, M42, M44);
		}

		#endregion

		#region Constructors

		public static Matrix Identity
		{
			get { return new Matrix (
				1f, 0f, 0f, 0f, 
				0f, 1f, 0f, 0f, 
				0f, 0f, 1f, 0f, 
				0f, 0f, 0f, 1f); }
		}
		
		public Matrix (
			float m11, float m12, float m13,
			float m21, float m22, float m23,
			float m31, float m32, float m33)
		{
			M11 = m11; M12 = m12; M13 = 0; M14 = m13;
			M21 = m21; M22 = m22; M23 = 0; M24 = m23;
			M31 = 0;   M32 = 0;   M33 = 1; M34 = 0;
			M41 = m31; M42 = m32; M43 = 0; M44 = m33;
		}

		public Matrix(float m11, float m12, float m13, float m14,
			float m21, float m22, float m23, float m24,
			float m31, float m32, float m33, float m34,
			float m41, float m42, float m43, float m44)
		{
			M11 = m11; M12 = m12; M13 = m13; M14 = m14;
			M21 = m21; M22 = m22; M23 = m23; M24 = m24;
			M31 = m31; M32 = m32; M33 = m33; M34 = m34;
			M41 = m41; M42 = m42; M43 = m43; M44 = m44;
		}

		#endregion

		#region Public methods

		public Vector2 TransformPoint(Vector2 point)
		{
			return new Vector2 (
				M11 * point.X + M12 * point.Y + M14,
				M21 * point.X + M22 * point.Y + M24);
		}

		#endregion

		#region Static methods

		public static Matrix CreateRotation(float angle)
		{
			var rad = angle * 3.141592654f / 180f;
			var cos = (float)Math.Cos (rad);
			var sin = (float)Math.Sin (rad);

			return new Matrix (
				cos, -sin, 0,
				sin, cos,  0,
				0,   0,    1);
		}

		public static Matrix CreateRotation(float angle, Vector2 center)
		{
			var rad = angle * 3.141592654f / 180f;
			var cos = (float)Math.Cos (rad);
			var sin = (float)Math.Sin (rad);

			return new Matrix (
				cos, -sin, center.X * (1 - cos) + center.Y * sin,
				sin,  cos, center.Y * (1 - cos) - center.X * sin,
				0,    0,   1);
		}

		public static Matrix CreateScale(float scale)
		{
			return new Matrix (
				scale, 0,     0,
				0,     scale, 0,
				0,     0,     1);
		}

		public static Matrix CreateScale(float scale, Vector2 center)
		{
			return new Matrix (
				scale, 0,     center.X * (1 - scale),
				0,     scale, center.Y * (1 - scale),
				0,     0,     1);
		}

		public static Matrix CreateScale(Vector2 scale)
		{
			return new Matrix (
				scale.X, 0,       0,
				0,       scale.Y, 0,
				0,       0,       1);
		}

		public static Matrix CreateScale(Vector2 scale, Vector2 center)
		{
			return new Matrix (
				scale.X, 0,       center.X * (1 - scale.X),
				0,       scale.Y, center.Y * (1 - scale.Y),
				0,       0,       1);
		}

		public static Matrix CreateTranslation(float x, float y)
		{
			return new Matrix (
				1, 0, x,
				0, 1, y,
				0, 0, 1);
		}

		public static Matrix CreateTranslation(Vector2 offset)
		{
			return new Matrix (
				1, 0, offset.X,
				0, 1, offset.Y,
				0, 0, 1);
		}

		public static Matrix Invert(Matrix matrix)
		{
			Invert(ref matrix, out matrix);
			return matrix;
		}

		public static void Invert(ref Matrix matrix, out Matrix result)
		{
			var num1 = matrix.M11;
			var num2 = matrix.M12;
			var num3 = matrix.M13;
			var num4 = matrix.M14;
			var num5 = matrix.M21;
			var num6 = matrix.M22;
			var num7 = matrix.M23;
			var num8 = matrix.M24;
			var num9 = matrix.M31;
			var num10 = matrix.M32;
			var num11 = matrix.M33;
			var num12 = matrix.M34;
			var num13 = matrix.M41;
			var num14 = matrix.M42;
			var num15 = matrix.M43;
			var num16 = matrix.M44;
			var num17 = (float) (num11 * (double) num16 - num12 * (double) num15);
			var num18 = (float) (num10 * (double) num16 - num12 * (double) num14);
			var num19 = (float) (num10 * (double) num15 - num11 * (double) num14);
			var num20 = (float) (num9 * (double) num16 - num12 * (double) num13);
			var num21 = (float) (num9 * (double) num15 - num11 * (double) num13);
			var num22 = (float) (num9 * (double) num14 - num10 * (double) num13);
			var num23 = (float) (num6 * (double) num17 - num7 * (double) num18 + num8 * (double) num19);
			var num24 = (float) -(num5 * (double) num17 - num7 * (double) num20 + num8 * (double) num21);
			var num25 = (float) (num5 * (double) num18 - num6 * (double) num20 + num8 * (double) num22);
			var num26 = (float) -(num5 * (double) num19 - num6 * (double) num21 + num7 * (double) num22);
			var num27 = (float) (1.0 / (num1 * (double) num23 + num2 * (double) num24 + num3 * (double) num25 + num4 * (double) num26));

			result.M11 = num23 * num27;
			result.M21 = num24 * num27;
			result.M31 = num25 * num27;
			result.M41 = num26 * num27;
			result.M12 = (float) -(num2 * (double) num17 - num3 * (double) num18 + num4 * (double) num19) * num27;
			result.M22 = (float) (num1 * (double) num17 - num3 * (double) num20 + num4 * (double) num21) * num27;
			result.M32 = (float) -(num1 * (double) num18 - num2 * (double) num20 + num4 * (double) num22) * num27;
			result.M42 = (float) (num1 * (double) num19 - num2 * (double) num21 + num3 * (double) num22) * num27;
			var num28 = (float) (num7 * (double) num16 - num8 * (double) num15);
			var num29 = (float) (num6 * (double) num16 - num8 * (double) num14);
			var num30 = (float) (num6 * (double) num15 - num7 * (double) num14);
			var num31 = (float) (num5 * (double) num16 - num8 * (double) num13);
			var num32 = (float) (num5 * (double) num15 - num7 * (double) num13);
			var num33 = (float) (num5 * (double) num14 - num6 * (double) num13);
			result.M13 = (float) (num2 * (double) num28 - num3 * (double) num29 + num4 * (double) num30) * num27;
			result.M23 = (float) -(num1 * (double) num28 - num3 * (double) num31 + num4 * (double) num32) * num27;
			result.M33 = (float) (num1 * (double) num29 - num2 * (double) num31 + num4 * (double) num33) * num27;
			result.M43 = (float) -(num1 * (double) num30 - num2 * (double) num32 + num3 * (double) num33) * num27;
			var num34 = (float) (num7 * (double) num12 - num8 * (double) num11);
			var num35 = (float) (num6 * (double) num12 - num8 * (double) num10);
			var num36 = (float) (num6 * (double) num11 - num7 * (double) num10);
			var num37 = (float) (num5 * (double) num12 - num8 * (double) num9);
			var num38 = (float) (num5 * (double) num11 - num7 * (double) num9);
			var num39 = (float) (num5 * (double) num10 - num6 * (double) num9);
			result.M14 = (float) -(num2 * (double) num34 - num3 * (double) num35 + num4 * (double) num36) * num27;
			result.M24 = (float) (num1 * (double) num34 - num3 * (double) num37 + num4 * (double) num38) * num27;
			result.M34 = (float) -(num1 * (double) num35 - num2 * (double) num37 + num4 * (double) num39) * num27;
			result.M44 = (float) (num1 * (double) num36 - num2 * (double) num38 + num3 * (double) num39) * num27;
		}

		public static Matrix operator *(Matrix matrix1, Matrix matrix2)
		{
			var m11 = (((matrix1.M11 * matrix2.M11) + (matrix1.M12 * matrix2.M21)) + (matrix1.M13 * matrix2.M31)) + (matrix1.M14 * matrix2.M41);
			var m12 = (((matrix1.M11 * matrix2.M12) + (matrix1.M12 * matrix2.M22)) + (matrix1.M13 * matrix2.M32)) + (matrix1.M14 * matrix2.M42);
			var m13 = (((matrix1.M11 * matrix2.M13) + (matrix1.M12 * matrix2.M23)) + (matrix1.M13 * matrix2.M33)) + (matrix1.M14 * matrix2.M43);
			var m14 = (((matrix1.M11 * matrix2.M14) + (matrix1.M12 * matrix2.M24)) + (matrix1.M13 * matrix2.M34)) + (matrix1.M14 * matrix2.M44);
			var m21 = (((matrix1.M21 * matrix2.M11) + (matrix1.M22 * matrix2.M21)) + (matrix1.M23 * matrix2.M31)) + (matrix1.M24 * matrix2.M41);
			var m22 = (((matrix1.M21 * matrix2.M12) + (matrix1.M22 * matrix2.M22)) + (matrix1.M23 * matrix2.M32)) + (matrix1.M24 * matrix2.M42);
			var m23 = (((matrix1.M21 * matrix2.M13) + (matrix1.M22 * matrix2.M23)) + (matrix1.M23 * matrix2.M33)) + (matrix1.M24 * matrix2.M43);
			var m24 = (((matrix1.M21 * matrix2.M14) + (matrix1.M22 * matrix2.M24)) + (matrix1.M23 * matrix2.M34)) + (matrix1.M24 * matrix2.M44);
			var m31 = (((matrix1.M31 * matrix2.M11) + (matrix1.M32 * matrix2.M21)) + (matrix1.M33 * matrix2.M31)) + (matrix1.M34 * matrix2.M41);
			var m32 = (((matrix1.M31 * matrix2.M12) + (matrix1.M32 * matrix2.M22)) + (matrix1.M33 * matrix2.M32)) + (matrix1.M34 * matrix2.M42);
			var m33 = (((matrix1.M31 * matrix2.M13) + (matrix1.M32 * matrix2.M23)) + (matrix1.M33 * matrix2.M33)) + (matrix1.M34 * matrix2.M43);
			var m34 = (((matrix1.M31 * matrix2.M14) + (matrix1.M32 * matrix2.M24)) + (matrix1.M33 * matrix2.M34)) + (matrix1.M34 * matrix2.M44);
			var m41 = (((matrix1.M41 * matrix2.M11) + (matrix1.M42 * matrix2.M21)) + (matrix1.M43 * matrix2.M31)) + (matrix1.M44 * matrix2.M41);
			var m42 = (((matrix1.M41 * matrix2.M12) + (matrix1.M42 * matrix2.M22)) + (matrix1.M43 * matrix2.M32)) + (matrix1.M44 * matrix2.M42);
			var m43 = (((matrix1.M41 * matrix2.M13) + (matrix1.M42 * matrix2.M23)) + (matrix1.M43 * matrix2.M33)) + (matrix1.M44 * matrix2.M43);
			var m44 = (((matrix1.M41 * matrix2.M14) + (matrix1.M42 * matrix2.M24)) + (matrix1.M43 * matrix2.M34)) + (matrix1.M44 * matrix2.M44);
			matrix1.M11 = m11;
			matrix1.M12 = m12;
			matrix1.M13 = m13;
			matrix1.M14 = m14;
			matrix1.M21 = m21;
			matrix1.M22 = m22;
			matrix1.M23 = m23;
			matrix1.M24 = m24;
			matrix1.M31 = m31;
			matrix1.M32 = m32;
			matrix1.M33 = m33;
			matrix1.M34 = m34;
			matrix1.M41 = m41;
			matrix1.M42 = m42;
			matrix1.M43 = m43;
			matrix1.M44 = m44;
			return matrix1;
		}

		#endregion
	}
}


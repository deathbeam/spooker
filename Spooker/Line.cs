//-----------------------------------------------------------------------------
// Line.cs
//
// Spooker Open Source Game Framework
// Copyright (C) Indie Armory. All rights reserved.
// Website: http://indiearmory.com
// Other Contributors: None
// License: MIT
//-----------------------------------------------------------------------------

using System;
using Spooker.Physics;

namespace Spooker
{
	[Serializable]
	public struct Line : IEquatable<Line>, ICollidable
	{
		#region Public fields

		public float X1, X2, Y1, Y2;

		#endregion

		#region Constructors/Destructors

		public Line(float x1, float y1, float x2, float y2)
		{
			X1 = x1;
			X2 = x2;
			Y1 = y1;
			Y2 = y2;
		}

		#endregion

		#region Public methods

		public bool Intersects(Line line)
		{
			return LineCollider.Intersects (this, line);
		}

		public bool Intersects(Rectangle rectangle)
		{
			return LineCollider.Intersects (this, rectangle);
		}

		public bool Intersects(Circle circle)
		{
			return LineCollider.Intersects (this, circle);
		}

		public bool Intersects(Polygon polygon)
		{
			return LineCollider.Intersects (this, polygon);
		}

		public float Distance(Vector2 point)
		{
			var a = new Vector2 (X1, Y1);
			var b = new Vector2 (X2, Y2);
			var pointVector = point - a;
			var lineVector = (b - a);
			lineVector.Normalize();
			var lineLength = (float)(b - a).Length();
			var intersectDistanceFromA = Vector2.Dot(lineVector, pointVector);

			if (intersectDistanceFromA < 0f)
				return -1f;
			if (intersectDistanceFromA > lineLength)
				return -1f;

			lineVector *= intersectDistanceFromA;
			var intersectPoint = a + lineVector;

			return (float)(point - intersectPoint).Length();
		}

		public bool Equals(Line other)
		{
			return ((X1 == other.X1) && (X2 == other.X2) && (Y1 == other.Y1) && (Y2 == other.Y2));
		}

		public override bool Equals(object obj)
		{
			return (obj is Line) && Equals((Line)obj);
		}

		public override int GetHashCode()
		{
			return X1.GetHashCode() + X2.GetHashCode() + Y1.GetHashCode() + Y2.GetHashCode();
		}

		#endregion Public methods
	}
}
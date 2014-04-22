//-----------------------------------------------------------------------------
// Circle.cs
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
	public struct Circle : IEquatable<Circle>, ICollidable
	{
		#region Public fields

		/// <summary>Center point of circle</summary>
		public Vector2 Position;

		/// <summary>Distance from center to circle bounds</summary>
		public float Radius;

		#endregion Public fields

		#region Constructors

		public Circle(Vector2 position, float radius)
		{
			Position = position;
			Radius = radius;
		}

		#endregion Constructors

		#region Public methods

		public bool Intersects(Line line)
		{
			return CircleCollider.Intersects (this, line);
		}

		public bool Intersects(Rectangle rectangle)
		{
			return CircleCollider.Intersects (this, rectangle);
		}

		public bool Intersects(Circle circle)
		{
			return CircleCollider.Intersects (this, circle);
		}

		public bool Intersects(Polygon polygon)
		{
			return CircleCollider.Intersects (this, polygon);
		}

		public bool Equals(Circle other)
		{
			return ((Position == other.Position) && (Radius == other.Radius));
		}

		public override bool Equals(object obj)
		{
			return (obj is Circle) && Equals((Circle)obj);
		}

		public override int GetHashCode()
		{
			return Position.GetHashCode() + Radius.GetHashCode();
		}

		#endregion Public methods
	}
}
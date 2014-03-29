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

namespace Spooker
{
	[Serializable]
	public struct Circle : IEquatable<Circle>
	{
		#region Public fields

		public Vector2 Position;
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
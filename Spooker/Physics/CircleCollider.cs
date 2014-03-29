//-----------------------------------------------------------------------------
// CircleCollider.cs
//
// Spooker Open Source Game Framework
// Copyright (C) Indie Armory. All rights reserved.
// Website: http://indiearmory.com
// Other Contributors: None
// License: MIT
//-----------------------------------------------------------------------------

namespace Spooker.Physics
{
	public static class CircleCollider
	{
		public static bool Intersects(Circle circle1, Circle circle2)
		{
			if (circle1.Equals(circle2))
				return true;

			return Vector2.Distance(circle1.Position, circle2.Position) <=
				circle1.Radius + circle2.Radius;
		}
	}
}
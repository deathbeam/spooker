//-----------------------------------------------------------------------------
// RectCollider.cs
//
// Spooker Open Source Game Framework
// Copyright (C) Indie Armory. All rights reserved.
// Website: http://indiearmory.com
// Other Contributors: None
// License: MIT
//-----------------------------------------------------------------------------

namespace Spooker.Physics
{
	public static class RectCollider
	{
		public static bool Intersects(Rectangle rectangle1, Rectangle rectangle2)
		{
			if (rectangle1.Equals(rectangle2))
				return true;

			return rectangle1.Intersects(rectangle2);
		}

		public static bool Intersects(Rectangle rectangle, Line line)
		{
			return LineCollider.Intersects (line, rectangle);
		}

		public static bool Intersects(Rectangle rectangle, Polygon polygon)
		{
			return PolygonCollider.Intersects (polygon, rectangle);
		}
	}
}


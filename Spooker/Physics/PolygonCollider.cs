//-----------------------------------------------------------------------------
// PolygonCollider.cs
//
// Spooker Open Source Game Framework
// Copyright (C) Indie Armory. All rights reserved.
// Website: http://indiearmory.com
// Other Contributors: None
// License: MIT
//-----------------------------------------------------------------------------

using System.Linq;

namespace Spooker.Physics
{
	public static class PolygonCollider
	{
		public static bool Intersects(Polygon polygon1, Polygon polygon2)
		{
			if (polygon1.Equals(polygon2))
				return true;

			return polygon1.Lines.Any(
				t => polygon2.Lines.Any(
					u => LineCollider.Intersects(t,u)));
		}

		public static bool Intersects(Polygon polygon, Line line)
		{
			return LineCollider.Intersects (line, polygon);
		}

		public static bool Intersects(Polygon polygon, Rectangle rectangle)
		{
			return polygon.Lines.Any(t => LineCollider.Intersects(t, rectangle));
		}

		public static bool Intersects(Polygon polygon, Circle circle)
		{
			return CircleCollider.Intersects (circle, polygon);
		}
	}
}


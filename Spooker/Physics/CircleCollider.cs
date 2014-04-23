//-----------------------------------------------------------------------------
// CircleCollider.cs
//
// Spooker Open Source Game Framework
// Copyright (C) Indie Armory. All rights reserved.
// Website: http://indiearmory.com
// Other Contributors: None
// License: MIT
//-----------------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;

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
		
		public static bool Intersects(Circle circle, Line line)
		{
			var distance = line.Distance (circle.Position);
			if (distance < circle.Radius && distance != -1f)
				return true;
			return false;
		}

		public static bool Intersects(Circle circle, Polygon polygon)
		{
			return polygon.Lines.Any(t => Intersects(circle, t));
		}

		public static bool Intersects(Circle circle, Rectangle rectangle)
		{
			var lines = new List<Line>
			{
				new Line(rectangle.X, rectangle.Y, rectangle.X + rectangle.Width, rectangle.Y),
				new Line(rectangle.X, rectangle.Y, rectangle.X, rectangle.Y + rectangle.Height),
				new Line(rectangle.X + rectangle.Width, rectangle.Y, rectangle.X + rectangle.Width,
					rectangle.Y + rectangle.Height),
				new Line(rectangle.X, rectangle.Y + rectangle.Height, rectangle.X + rectangle.Width,
					rectangle.Y + rectangle.Height)
			};
			return lines.Any (t => Intersects (circle, t));
		}
	}
}
//-----------------------------------------------------------------------------
// RectCollider.cs
//
// Spooker Open Source Game Framework
// Copyright (C) Indie Armory. All rights reserved.
// Website: http://indiearmory.com
// Other Contributors: None
// License: MIT
//-----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;

namespace Spooker.Physics
{
	public static class RectCollider
	{
		public static void Intersect(ref Rectangle value1, ref Rectangle value2, out Rectangle result)
		{
			if (value1.Intersects(value2))
			{
				var rightSide = Math.Min(value1.X + value1.Width, value2.X + value2.Width);
				var leftSide = Math.Max(value1.X, value2.X);
				var topSide = Math.Max(value1.Y, value2.Y);
				var bottomSide = Math.Min(value1.Y + value1.Height, value2.Y + value2.Height);
				result = new Rectangle(leftSide, topSide, rightSide - leftSide, bottomSide - topSide);
			}
			else
			{
				result = new Rectangle(0, 0, 0, 0);
			}
		}

		public static bool Intersects(Rectangle rectangle1, Rectangle rectangle2)
		{
			if (rectangle1.Equals(rectangle2))
				return true;

			return rectangle2.Left < rectangle1.Right &&
				rectangle1.Left < rectangle2.Right &&
				rectangle2.Top < rectangle1.Bottom &&
				rectangle1.Top < rectangle2.Bottom;
		}

		public static bool Intersects(Rectangle rectangle, Line line)
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
			return lines.Any(t => LineCollider.Intersects(line,t));
		}

		public static bool Intersects(Rectangle rectangle, Polygon polygon)
		{
			return PolygonCollider.Intersects (polygon, rectangle);
		}

		public static bool Intersects(Rectangle rectangle, Circle circle)
		{
			return CircleCollider.Intersects (circle, rectangle);
		}
	}
}
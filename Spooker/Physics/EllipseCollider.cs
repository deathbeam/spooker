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
	public static class EllipseCollider
	{
		public static bool Intersects(Ellipse ellipse1, Ellipse ellipse2)
		{
			if (ellipse1.Equals(ellipse2))
				return true;

			var radiusSum = (ellipse1.Size / 2) + (ellipse2.Size / 2);
			var radiusSquare = radiusSum * radiusSum;
			return ((ellipse1.Position.X - ellipse2.Position.X) * (ellipse1.Position.X - ellipse2.Position.X) * radiusSquare.Y +
				(ellipse1.Position.Y - ellipse2.Position.Y) * (ellipse1.Position.Y - ellipse2.Position.Y) * radiusSquare.X) <
				(radiusSquare.X * radiusSquare.Y);
		}

		public static bool Intersects(Ellipse ellipse, Line line)
		{
			var pt1 = new Vector2 (line.X1, line.Y1);
			var pt2 = new Vector2 (line.X2, line.Y2);
			var rect = new Rectangle ((int)ellipse.Position.X, (int)ellipse.Position.Y, (int)ellipse.Size.X, (int)ellipse.Size.Y);

			if ((rect.Width == 0) || (rect.Height == 0) ||
				((pt1.X == pt2.X) && (pt1.Y == pt2.Y)))
				return false;

			if (rect.Width < 0)
			{
				rect.X = rect.Right;
				rect.Width = -rect.Width;
			}
			if (rect.Height < 0)
			{
				rect.Y = rect.Bottom;
				rect.Height = -rect.Height;
			}

			var cx = rect.Left + rect.Width / 2f;
			var cy = rect.Top + rect.Height / 2f;
			rect.X -= (int)cx;
			rect.Y -= (int)cy;
			pt1.X -= cx;
			pt1.Y -= cy;
			pt2.X -= cx;
			pt2.Y -= cy;

			var a = rect.Width / 2f;
			var b = rect.Height / 2f;

			var A = (pt2.X - pt1.X) * (pt2.X - pt1.X) / a / a +
			        (pt2.Y - pt1.Y) * (pt2.Y - pt1.Y) / b / b;
			var B = 2 * pt1.X * (pt2.X - pt1.X) / a / a +
			        2 * pt1.Y * (pt2.Y - pt1.Y) / b / b;
			var C = pt1.X * pt1.X / a / a + pt1.Y * pt1.Y / b / b - 1;

			var discriminant = B * B - 4 * A * C;

			if (discriminant >= 0)
				return true;

			return false;
		}

		public static bool Intersects(Ellipse ellipse, Polygon polygon)
		{
			return polygon.Lines.Any(t => Intersects(ellipse, t));
		}

		public static bool Intersects(Ellipse ellipse, Rectangle rectangle)
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
			return lines.Any (t => Intersects (ellipse, t));
		}
	}
}
//-----------------------------------------------------------------------------
// LineCollider.cs
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
	public static class LineCollider
	{
		/// <summary>
		/// Checks if line is colliding with polygon.
		/// </summary>
		/// <param name="line">Line to check for collision</param> 
		/// <param name="polygon">Polygon to collide with</param>
		/// <returns></returns>
		public static bool Intersects(Line line, Polygon polygon)
		{
			return polygon.Lines.Any(t => Intersects(line, t));
		}

		public static bool Intersects(Line line, Rectangle rectangle)
		{
			return RectCollider.Intersects (rectangle, line);
		}

		public static bool Intersects(Line line1, Line line2)
		{
			if (line1.Equals(line2))
				return true;

			var denominator = ((line1.X2 - line1.X1) * (line2.Y2 - line2.Y1)) - ((line1.Y2 - line1.Y1) * (line2.X2 - line2.X1));
			var numerator1 = ((line1.Y1 - line2.Y1) * (line2.X2 - line2.X1)) - ((line1.X1 - line2.X1) * (line2.Y2 - line2.Y1));
			var numerator2 = ((line1.Y1 - line2.Y1) * (line1.X2 - line1.X1)) - ((line1.X1 - line2.X1) * (line1.Y2 - line1.Y1));

			var r = numerator1 / denominator;
			var s = numerator2 / denominator;

			return (r >= 0 && r <= 1) && (s >= 0 && s <= 1);
		}

		public static bool Intersects(Line line, Circle circle)
		{
			return CircleCollider.Intersects (circle, line);
		}
	}
}
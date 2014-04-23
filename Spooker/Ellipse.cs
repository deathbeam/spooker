using System;
using Spooker.Physics;

namespace Spooker
{
	public class Ellipse
	{
		/// <summary>Center point of ellipse</summary>
		public Vector2 Position;

		/// <summary>Size of ellipse</summary>
		public Vector2 Size;

		public Ellipse (int x, int y, int width, int height)
		{
			Position = new Vector2 (x, y);
			Size = new Vector2 (width, height);
		}

		public bool Intersects(Line line)
		{
			return EllipseCollider.Intersects (this, line);
		}

		public bool Intersects(Rectangle rectangle)
		{
			return EllipseCollider.Intersects (this, rectangle);
		}

		public bool Intersects(Ellipse ellipse)
		{
			return EllipseCollider.Intersects (this, ellipse);
		}

		public bool Intersects(Polygon polygon)
		{
			return EllipseCollider.Intersects (this, polygon);
		}
	}
}
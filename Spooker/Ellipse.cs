using System;
using Spooker.Physics;

namespace Spooker
{
	public class Ellipse : ICollidable
	{
		private Circle _circle;
		private Rectangle _rect;

		public Ellipse (Vector2 position, Vector2 size)
		{
			_circle = new Circle (position + (size / 2), size.X / 2);
			_rect = new Rectangle ((int)position.X, (int)position.Y, (int)size.X, (int)size.Y);
		}

		public bool Intersects(Line line)
		{
			return _circle.Intersects (line) && _rect.Intersects(line);
		}

		public bool Intersects(Rectangle rectangle)
		{
			return _circle.Intersects (rectangle) && _rect.Intersects(rectangle);
		}

		public bool Intersects(Circle circle)
		{
			return _circle.Intersects (circle) && _rect.Intersects(circle);
		}

		public bool Intersects(Polygon polygon)
		{
			return _circle.Intersects (polygon) && _rect.Intersects(polygon);
		}
	}
}
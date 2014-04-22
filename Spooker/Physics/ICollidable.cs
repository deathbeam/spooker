using System;

namespace Spooker.Physics
{
	public interface ICollidable
	{
		bool Intersects (Line line);
		bool Intersects (Rectangle rectangle);
		bool Intersects (Circle circle);
		bool Intersects (Polygon polygon);
	}
}
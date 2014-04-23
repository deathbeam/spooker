using System;

namespace Spooker.Physics
{
	////////////////////////////////////////////////////////////
	/// <summary>
	/// Interface used for collidable objects
	/// </summary>
	////////////////////////////////////////////////////////////
	public interface ICollidable
	{
		/// <summary>
		/// Intersects the specified line.
		/// </summary>
		/// <param name="line">Line.</param>
		bool Intersects (Line line);

		/// <summary>
		/// Intersects the specified rectangle.
		/// </summary>
		/// <param name="rectangle">Rectangle.</param>
		bool Intersects (Rectangle rectangle);

		/// <summary>
		/// Intersects the specified circle.
		/// </summary>
		/// <param name="circle">Circle.</param>
		bool Intersects (Circle circle);

		/// <summary>
		/// Intersects the specified polygon.
		/// </summary>
		/// <param name="polygon">Polygon.</param>
		bool Intersects (Polygon polygon);
	}
}
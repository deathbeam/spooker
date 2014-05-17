using System;

namespace Spooker
{
	/// <summary>
	/// Interface used for targetable objects.
	/// </summary>
	public interface ITargetable
	{
		/// <summary>
		/// Targets the position.
		/// </summary>
		/// <returns>The position.</returns>
		Vector2 TargetPosition();
	}
}
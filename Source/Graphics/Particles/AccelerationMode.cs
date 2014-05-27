using System;

namespace Spooker.Graphics.Particles
{
	/// <summary>
	/// Used to specify the method of acceleration for a particle system.
	/// </summary>
	public enum AccelerationMode
	{
		/// <summary>
		/// The particle system does not use acceleration.
		/// </summary>
		None,

		/// <summary>
		/// The particle system computes the acceleration by using the
		/// MinAccelerationScale and MaxAccelerationScale values to compute a random
		/// scalar value which is then multiplied by the direction of the particles.
		/// </summary>
		Scalar,

		/// <summary>
		/// The particle system computes the acceleration by using the EndVelocity
		/// value and solving the equation vt = v0 + (a0 * t) for a0. See
		/// ParticleSystem.cs for more details.
		/// </summary>
		EndVelocity,

		/// <summary>
		/// The particle system computes the acceleration by using the
		/// MinAccelerationVector and MaxAccelerationVector values to compute a random
		/// vector value which is used as the acceleration of the particles.
		/// </summary>
		Vector
	}
}


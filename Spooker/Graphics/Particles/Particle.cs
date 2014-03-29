//-----------------------------------------------------------------------------
// Particle.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
// Website: http://xbox.create.msdn.com/en-US/education/catalog/sample/particle
// Other Contributors: Thomas Slusny @ http://indiearmory.com
// License: Microsoft Permissive License (Ms-PL)
//-----------------------------------------------------------------------------

namespace Spooker.Graphics.Particles
{
	/// <summary>
	/// particles are the little bits that will make up an effect. each effect will
	/// be comprised of many of these particles. They have basic physical properties,
	/// such as position, velocity, acceleration, and rotation. They'll be drawn as
	/// sprites, all layered on top of one another, and will be very pretty.
	/// </summary>
	public class Particle
	{
		/// <summary>Position of particle</summary>
		public Vector2 Position;

        /// <summary>Velocity of particle</summary>
		public Vector2 Velocity;

        /// <summary>Acceleration of particle</summary>
		public Vector2 Acceleration;

        /// <summary>How long this particle will "live"</summary>
	    public float Lifetime;

        /// <summary>How long it has been since initialize was called</summary>
	    public float TimeSinceStart;

        /// <summary>the scale of this particle</summary>
		public float Scale;

	    /// <summary>its rotation, in radians</summary>
	    public float Rotation;

        /// <summary>how fast does it rotate?</summary>
	    public float RotationSpeed;

        /// <summary>
		/// is this particle still alive? once TimeSinceStart becomes greater than
		/// Lifetime, the particle should no longer be drawn or updated.
        /// </summary>
		public bool Active
		{
			get { return TimeSinceStart < Lifetime; }
		}

        /// <summary>
		/// initialize is called by ParticleSystem to set up the particle, and prepares
		/// the particle for use.
        /// </summary>
		public void Initialize(Vector2 position, Vector2 velocity, Vector2 acceleration,
			float lifetime, float scale, float rotationSpeed)
		{
			// set the values to the requested values
			Position = position;
			Velocity = velocity;
			Acceleration = acceleration;
			Lifetime = lifetime;
			Scale = scale;
			RotationSpeed = rotationSpeed;

			// reset TimeSinceStart - we have to do this because particles will be
			// reused.
			TimeSinceStart = 0.0f;

			// set rotation to some random value between 0 and 360 degrees.
			Rotation = (float)MathHelper.RandomRange(0, (float)MathHelper.TwoPi);
		}

        /// <summary>
        /// Update is called by the ParticleSystem on every frame. This is where the
        /// particle's position and that kind of thing get updated.
        /// </summary>
        /// <param name="dt"></param>
		public void Update(float dt)
		{
			Velocity += Acceleration * dt;
			Position += Velocity * dt;
			Rotation += RotationSpeed * dt;

			TimeSinceStart += dt;
		}
	}
}

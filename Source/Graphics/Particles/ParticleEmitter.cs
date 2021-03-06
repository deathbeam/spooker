//-----------------------------------------------------------------------------
// Emitter.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
// Website: http://xbox.create.msdn.com/en-US/education/catalog/sample/particle
// Other Contributors: Thomas Slusny @ http://indiearmory.com
// License: Microsoft Permissive License (Ms-PL)
//-----------------------------------------------------------------------------

using Spooker.Time;

namespace Spooker.Graphics.Particles
{
	/// <summary>
	/// Helper for objects that want to leave particles behind them as they
	/// move around the world. This emitter implementation solves two related
	/// problems:
	/// 
	/// If an object wants to create particles very slowly, less than once per
	/// frame, it can be a pain to keep track of which updates ought to create
	/// a new particle versus which should not.
	/// 
	/// If an object is moving quickly and is creating many particles per frame,
	/// it will look ugly if these particles are all bunched up together. Much
	/// better if they can be spread out along a line between where the object
	/// is now and where it was on the previous frame. This is particularly
	/// important for leaving trails behind fast moving objects such as rockets.
	/// 
	/// This emitter class keeps track of a moving object, remembering its
	/// previous position so it can calculate the velocity of the object. It
	/// works out the perfect locations for creating particles at any frequency
	/// you specify, regardless of whether this is faster or slower than the
	/// game update rate.
	/// </summary>
	public class ParticleEmitter
	{
		#region Fields
	    private readonly ParticleSystem _particleSystem;
        private readonly float _timeBetweenParticles;
        private Vector2 _position;
        private float _timeLeftOver;
		#endregion

		/// <summary>
		/// Gets the location of the emitter. To change locations, pass a new value
		/// in to Update.
		/// </summary>
		public Vector2 Position
		{
			get { return _position; }
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Spooker.Graphics.Particles.ParticleEmitter"/> class.
		/// </summary>
		/// <param name="particleSystem">Particle system.</param>
		/// <param name="particlesPerSecond">Particles per second.</param>
		/// <param name="initialPosition">Initial position.</param>
		public ParticleEmitter(
			ParticleSystem particleSystem,
			float particlesPerSecond,
			Vector2 initialPosition)
		{
			_particleSystem = particleSystem;
			_timeBetweenParticles = 1.0f / particlesPerSecond;
			_position = initialPosition;
		}

		/// <summary>
		/// Updates the emitter, creating the appropriate number of particles
		/// in the appropriate positions.
		/// </summary>
		/// <param name="gameTime">Game time.</param>
		/// <param name="newPosition">New position.</param>
		public void Update(GameTime gameTime, Vector2 newPosition)
		{
			Update (gameTime, newPosition, Matrix.Identity);
		}


		/// <summary>
		/// Updates the emitter, creating the appropriate number of particles
		/// in the appropriate positions.
		/// </summary>
		/// <param name="gameTime">Game time.</param>
		/// <param name="newPosition">New position.</param>
		/// <param name="transform">Transform.</param>
		public void Update(GameTime gameTime, Vector2 newPosition, Matrix transform)
		{
			// Work out how much time has passed since the previous update.
			var elapsedTime = (float)gameTime.ElapsedGameTime.Seconds;

			if (elapsedTime > 0)
			{
				var pos = transform.TransformPoint(_position);
				var newPos = transform.TransformPoint(newPosition);

				// Work out how fast we are moving.
				var velocity = (newPos - pos) / elapsedTime;

				// If we had any time left over that we didn't use during the
				// previous update, add that to the current elapsed time.
				float timeToSpend = _timeLeftOver + elapsedTime;

				// Counter for looping over the time interval.
				float currentTime = -_timeLeftOver;

				// Create particles as long as we have a big enough time interval.
				while (timeToSpend > _timeBetweenParticles)
				{
					currentTime += _timeBetweenParticles;
					timeToSpend -= _timeBetweenParticles;

					// Work out the optimal position for this particle. This will produce
					// evenly spaced particles regardless of the object speed, particle
					// creation frequency, or game update rate.
					float mu = currentTime / elapsedTime;

					var particlePosition = Vector2.Lerp(pos, newPos, mu);

					// Create the particle.
					_particleSystem.AddParticles(particlePosition, velocity);
				}

				// Store any time we didn't use, so it can be part of the next update.
				_timeLeftOver = timeToSpend;
			}

			_position = newPosition;
		}
	}
}

//-----------------------------------------------------------------------------
// ParticleSystem.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
// Website: http://xbox.create.msdn.com/en-US/education/catalog/sample/particle
// Other Contributors: Thomas Slusny @ http://indiearmory.com
// License: Microsoft Permissive License (Ms-PL)
//-----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using Spooker.Time;
using Spooker.Core;

namespace Spooker.Graphics.Particles
{
	/// <summary>
	/// Particle system.
	/// </summary>
	public class ParticleSystem : GameComponent, IDrawable, IUpdateable
	{
		#region Private fields

		// the graphic this particle system will use.
		private readonly Texture _texture;
		private readonly Rectangle _sourceRect;
		private readonly Vector2 _origin;

		// the array of particles used by this system. these are reused, so that calling
		// AddParticles will only cause allocations if we're trying to create more particles
		// than we have available
		private readonly List<Particle> _particles;

		// the queue of free particles keeps track of particles that are not curently
		// being used by an effect. when a new effect is requested, particles are taken
		// from this queue. when particles are finished they are put onto this queue.
		private readonly Queue<Particle> _freeParticles;

		// The settings used for this particle system
		private readonly ParticleSettings _settings;

		#endregion

		/// <summary>
		/// returns the number of particles that are available for a new effect.
		/// </summary>
		public int FreeParticleCount
		{
			get { return _freeParticles.Count; }
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Spooker.Graphics.Particles.ParticleSystem"/> class.
		/// </summary>
		/// <param name="game">The host for this particle system.</param>
		/// <param name="settings">Settings used for this particle system.</param>
		public ParticleSystem(GameWindow game, ParticleSettings settings)
			: this(game, settings, 10)
		{ }


		/// <summary>
		/// Initializes a new instance of the <see cref="Spooker.Graphics.Particles.ParticleSystem"/> class.
		/// </summary>
		/// <param name="game">The host for this particle system.</param>
		/// <param name="settings">Settings used for this particle system.</param>
		/// <param name="initialParticleCount">The initial number of particles this
		/// system expects to use. The system will grow as needed, however setting
		/// this value to be as close as possible will reduce allocations.</param>
		public ParticleSystem(GameWindow game, ParticleSettings settings, int initialParticleCount)
			: base(game)
		{
			_settings = settings;

			// we create the particle list and queue with our initial count and create that
			// many particles. If we picked a reasonable value, our system will not allocate
			// any more objects after this point, however the AddParticles method will allocate
			// more particles as needed.
			_particles = new List<Particle>(initialParticleCount);
			_freeParticles = new Queue<Particle>(initialParticleCount);
			for (int i = 0; i < initialParticleCount; i++)
			{
				_particles.Add(new Particle());
				_freeParticles.Enqueue(_particles[i]);
			}

			// load the graphic....
			_texture = new Texture(_settings.TextureFilename);
			_sourceRect = new Rectangle (0, 0, (int)_texture.Size.X, (int)_texture.Size.Y);
			_origin = _texture.Size / 2;
		}

		/// <summary>
		/// AddParticles's job is to add an effect somewhere on the screen. If there 
		/// aren't enough particles in the freeParticles queue, it will use as many as 
		/// it can. This means that if there not enough particles available, calling
		/// AddParticles will have no effect.
		/// </summary>
		/// <param name="where">Where the particle effect should be created</param>
		/// <param name="velocity">A base velocity for all particles. This is weighted 
		/// by the EmitterVelocitySensitivity specified in the settings for the 
		/// particle system.</param>
		public void AddParticles(Vector2 where, Vector2 velocity)
		{
			// the number of particles we want for this effect is a random number
			// somewhere between the two constants specified by the settings.
			var numParticles = (int)MathHelper.Random(_settings.MinNumParticles, _settings.MaxNumParticles);

			// create that many particles, if you can.
			for (int i = 0; i < numParticles; i++)
			{
				// if we're out of free particles, we allocate another ten particles
				// which should keep us going.
				if (_freeParticles.Count == 0)
				{
					for (int j = 0; j < 10; j++)
					{
						var newParticle = new Particle();
						_particles.Add(newParticle);
						_freeParticles.Enqueue(newParticle);
					}
				}

				// grab a particle from the freeParticles queue, and Initialize it.
				var p = _freeParticles.Dequeue();
				InitializeParticle(p, where, velocity);
			}
		}

		/// <summary>
		/// InitializeParticle randomizes some properties for a particle, then
		/// calls initialize on it. It can be overriden by subclasses if they 
		/// want to modify the way particles are created. For example, 
		/// SmokePlumeParticleSystem overrides this function make all particles
		/// accelerate to the right, simulating wind.
		/// </summary>
		/// <param name="p">the particle to initialize</param>
		/// <param name="where">the position on the screen that the particle should be
		/// </param>
		/// <param name="velocity">The base velocity that the particle should have</param>
		private void InitializeParticle(Particle p, Vector2 where, Vector2 velocity)
		{
			// Adjust the input velocity based on how much
			// this particle system wants to be affected by it.
			velocity *= _settings.EmitterVelocitySensitivity;

			// Adjust the velocity based on our random values
			// our settings angles are in degrees, so we must convert to radians
			var angle = (float)MathHelper.ToRadians (MathHelper.Random (_settings.MinDirectionAngle, _settings.MaxDirectionAngle));
			var direction = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));

			var speed = (float)MathHelper.Random(_settings.MinInitialSpeed, _settings.MaxInitialSpeed);
			velocity += direction * speed;

			// pick some random values for our particle
			var lifetime =(float) MathHelper.Random(_settings.MinLifetime, _settings.MaxLifetime);
			var scale = (float)MathHelper.Random(_settings.MinSize, _settings.MaxSize);
			var rotationSpeed =  (float)MathHelper.Random(_settings.MinRotationSpeed, _settings.MaxRotationSpeed);

			// our settings angles are in degrees, so we must convert to radians
			rotationSpeed = (float)MathHelper.ToRadians(rotationSpeed);

			// figure out our acceleration base on our AccelerationMode
			var acceleration = Vector2.Zero;
			switch (_settings.AccelerationMode)
			{
			case AccelerationMode.Scalar:
				// randomly pick our acceleration using our direction and 
				// the MinAcceleration/MaxAcceleration values
				var accelerationScale = (float)MathHelper.Random(
					_settings.MinAccelerationScale, _settings.MaxAccelerationScale);
				acceleration = direction * accelerationScale;
				break;
			case AccelerationMode.EndVelocity:
				// Compute our acceleration based on our ending velocity from the settings.
				// We'll use the equation vt = v0 + (a0 * t). (If you're not familar with
				// this, it's one of the basic kinematics equations for constant
				// acceleration, and basically says:
				// velocity at time t = initial velocity + acceleration * t)
				// We're solving for a0 by substituting t for our lifetime, v0 for our
				// velocity, and vt as velocity * settings.EndVelocity.
				acceleration = (velocity * (_settings.EndVelocity - 1)) / lifetime;
				break;
			case AccelerationMode.Vector:
				acceleration = new Vector2(
					(float)MathHelper.Random(_settings.MinAccelerationVector.X, _settings.MaxAccelerationVector.X),
					(float)MathHelper.Random(_settings.MinAccelerationVector.Y, _settings.MaxAccelerationVector.Y));
				break;
			}

			// then initialize it with those random values. initialize will save those,
			// and make sure it is marked as active.
			p.Initialize(where, velocity, acceleration, lifetime, scale, rotationSpeed);
		}

		/// <summary>
		/// Allows the game component to update itself.
		/// </summary>
		/// <param name="gameTime">Provides snapshot of timing values.</param>
		public void Update(GameTime gameTime)
		{
			// calculate dt, the change in the since the last frame. the particle
			// updates will use this value.
			var dt = (float)gameTime.ElapsedGameTime.Seconds;

			// go through all of the particles...
			foreach (Particle p in _particles)
			{
				if (p.Active)
				{
					// ... and if they're active, update them.
					p.Acceleration += _settings.Gravity * dt;
					p.Update(dt);
					// if that update finishes them, put them onto the free particles
					// queue.
					if (!p.Active) _freeParticles.Enqueue(p);
				}   
			}
		}

		/// <summary>
		/// overriden from DrawableGameComponent, Draw will use ParticleSampleGame's 
		/// sprite batch to render all of the active particles.
		/// </summary>
		public void Draw(SpriteBatch spriteBatch, SpriteEffects effects = SpriteEffects.None)
		{
			// tell sprite batch to begin
			spriteBatch.Begin (_settings.BlendMode);

			foreach (Particle p in _particles)
			{
				// skip inactive particles
				if (!p.Active)
					continue;

				// normalized lifetime is a value from 0 to 1 and represents how far
				// a particle is through its life. 0 means it just started, .5 is half
				// way through, and 1.0 means it's just about to be finished.
				// this value will be used to calculate alpha and scale, to avoid 
				// having particles suddenly appear or disappear.
				var normalizedLifetime = p.TimeSinceStart / p.Lifetime;

				// we want particles to fade in and fade out, so we'll calculate alpha
				// to be (normalizedLifetime) * (1-normalizedLifetime). this way, when
				// normalizedLifetime is 0 or 1, alpha is 0. the maximum value is at
				// normalizedLifetime = .5, and is
				// (normalizedLifetime) * (1-normalizedLifetime)
				// (.5)                 * (1-.5)
				// .25
				// since we want the maximum alpha to be 1, not .25, we'll scale the 
				// entire equation by 4.
				var alpha = 4f * normalizedLifetime * (1f - normalizedLifetime);

				// change color alpha based on lifetime
				var color = _settings.Color;
				color.A = alpha;

				// make particles grow as they age. they'll start at 75% of their size,
				// and increase to 100% once they're finished.
				var scale = new Vector2 (p.Scale * (.75f + .25f * normalizedLifetime));

				spriteBatch.Draw(_texture, p.Position, _sourceRect, color, scale, _origin, p.Rotation, effects);
			}

			// tell sprite batch to end, using the spriteBlendMode specified in
			// particle settings
			spriteBatch.End ();
		}
	}
}
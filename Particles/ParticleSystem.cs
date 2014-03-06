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
using System.Text;
using SFML.Graphics;
using SFML.Window;
using SFGL.Graphics;
using SFGL.Time;
using SFGL.Utils;
using SFGL.Window;

namespace SFGL.Particles
{
	public class ParticleSystem : GameComponent, Drawable, IUpdateable, ILoadable
	{
		// the sprite this particle system will use.
		private Sprite sprite;

		// the texture this particle system will use.
		private Texture texture;

		// the array of particles used by this system. these are reused, so that calling
		// AddParticles will only cause allocations if we're trying to create more particles
		// than we have available
		private List<Particle> particles;

		// the queue of free particles keeps track of particles that are not curently
		// being used by an effect. when a new effect is requested, particles are taken
		// from this queue. when particles are finished they are put onto this queue.
		private Queue<Particle> freeParticles;

		// The settings used for this particle system
		private ParticleSettings settings;

		// The asset name used to load our settings from a file.
		private string filepath;

		// the BlendState used when rendering the particles.
		private RenderStates blendState;

		/// <summary>
		/// returns the number of particles that are available for a new effect.
		/// </summary>
		public int FreeParticleCount
		{
			get { return freeParticles.Count; }
		}

		/// <summary>
		/// Constructs a new ParticleSystem.
		/// </summary>
		/// <param name="game">The host for this particle system.</param>
		/// <param name="filepath">The name of the settings file to load 
		/// used when creating and updating particles in the system.</param>
		public ParticleSystem(GameWindow game, string filepath)
			: this(game, filepath, 10)
		{ }


		/// <summary>
		/// Constructs a new ParticleSystem.
		/// </summary>
		/// <param name="game">The host for this particle system.</param>
		/// <param name="filepath">The name of the settings file to load 
		/// used when creating and updating particles in the system.</param>
		/// <param name="initialParticleCount">The initial number of particles this
		/// system expects to use. The system will grow as needed, however setting
		/// this value to be as close as possible will reduce allocations.</param>
		public ParticleSystem(GameWindow game, string filepath, int initialParticleCount)
			: base(game)
		{
			this.filepath = filepath;

			// we create the particle list and queue with our initial count and create that
			// many particles. If we picked a reasonable value, our system will not allocate
			// any more objects after this point, however the AddParticles method will allocate
			// more particles as needed.
			particles = new List<Particle>(initialParticleCount);
			freeParticles = new Queue<Particle>(initialParticleCount);
			for (int i = 0; i < initialParticleCount; i++)
			{
				particles.Add(new Particle());
				freeParticles.Enqueue(particles[i]);
			}
		}

		/// <summary>
		/// Override the base class LoadContent to load the texture. once it's
		/// loaded, calculate the origin.
		/// </summary>
		public void LoadContent()
		{
			// Load our settings
			settings = new ParticleSettings(filepath);

			// load the texture....
			texture = new Texture(settings.TextureFilename);

			// create the blend state using the values from our settings
			blendState = new RenderStates (settings.BlendMode);

			// create rendering sprite from texture
			sprite = new Sprite (texture);
		}

		/// <summary>
		/// PickRandomDirection is used by AddParticle to decide which direction
		/// particles will move. 
		/// </summary>
		private Vector2 PickRandomDirection()
		{
			float angle = FloatMath.RandomBetween(settings.MinDirectionAngle, settings.MaxDirectionAngle);

			// our settings angles are in degrees, so we must convert to radians
			angle = FloatMath.ToRadians(angle);

			return new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
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
			int numParticles =
				FloatMath.random.Next(settings.MinNumParticles, settings.MaxNumParticles);

			// create that many particles, if you can.
			for (int i = 0; i < numParticles; i++)
			{
				// if we're out of free particles, we allocate another ten particles
				// which should keep us going.
				if (freeParticles.Count == 0)
				{
					for (int j = 0; j < 10; j++)
					{
						Particle newParticle = new Particle();
						particles.Add(newParticle);
						freeParticles.Enqueue(newParticle);
					}
				}

				// grab a particle from the freeParticles queue, and Initialize it.
				Particle p = freeParticles.Dequeue();
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
			velocity *= settings.EmitterVelocitySensitivity;

			// Adjust the velocity based on our random values
			Vector2 direction = PickRandomDirection();
			float speed = FloatMath.RandomBetween(settings.MinInitialSpeed, settings.MaxInitialSpeed);
			velocity += direction * speed;

			// pick some random values for our particle
			float lifetime =
				FloatMath.RandomBetween(settings.MinLifetime, settings.MaxLifetime);
			float scale =
				FloatMath.RandomBetween(settings.MinSize, settings.MaxSize);
			float rotationSpeed = 
				FloatMath.RandomBetween(settings.MinRotationSpeed, settings.MaxRotationSpeed);

			// our settings angles are in degrees, so we must convert to radians
			rotationSpeed = FloatMath.ToRadians(rotationSpeed);

			// figure out our acceleration base on our AccelerationMode
			Vector2 acceleration = Vector2.Zero;
			switch (settings.AccelerationMode)
			{
			case AccelerationMode.Scalar:
				// randomly pick our acceleration using our direction and 
				// the MinAcceleration/MaxAcceleration values
				float accelerationScale = FloatMath.RandomBetween(
					settings.MinAccelerationScale, settings.MaxAccelerationScale);
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
				acceleration = (velocity * (settings.EndVelocity - 1)) / lifetime;
				break;
			case AccelerationMode.Vector:
				acceleration = new Vector2(
					FloatMath.RandomBetween(settings.MinAccelerationVector.X, settings.MaxAccelerationVector.X),
					FloatMath.RandomBetween(settings.MinAccelerationVector.Y, settings.MaxAccelerationVector.Y));
				break;
			default:
				break;
			}

			// then initialize it with those random values. initialize will save those,
			// and make sure it is marked as active.
			p.Initialize(
				where, 
				velocity, 
				acceleration, 
				lifetime, 
				scale, 
				rotationSpeed);
		}

		/// <summary>
		/// overriden from DrawableGameComponent, Update will update all of the active
		/// particles.
		/// </summary>
		public void Update(GameTime gameTime)
		{
			// calculate dt, the change in the since the last frame. the particle
			// updates will use this value.
			float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

			// go through all of the particles...
			foreach (Particle p in particles)
			{

				if (p.Active)
				{
					// ... and if they're active, update them.
					p.Acceleration += settings.Gravity * dt;
					p.Update(dt);
					// if that update finishes them, put them onto the free particles
					// queue.
					if (!p.Active)
					{
						freeParticles.Enqueue(p);
					}
				}   
			}
		}

		/// <summary>
		/// overriden from DrawableGameComponent, Draw will use ParticleSampleGame's 
		/// sprite batch to render all of the active particles.
		/// </summary>
		public void Draw(RenderTarget graphicsDevice, RenderStates states)
		{
			// tell sprite batch to begin, using the spriteBlendMode specified in
			// initializeConstants
			SpriteBatch.Begin();

			foreach (Particle p in particles)
			{
				// skip inactive particles
				if (!p.Active)
					continue;

				// normalized lifetime is a value from 0 to 1 and represents how far
				// a particle is through its life. 0 means it just started, .5 is half
				// way through, and 1.0 means it's just about to be finished.
				// this value will be used to calculate alpha and scale, to avoid 
				// having particles suddenly appear or disappear.
				float normalizedLifetime = p.TimeSinceStart / p.Lifetime;

				// we want particles to fade in and fade out, so we'll calculate alpha
				// to be (normalizedLifetime) * (1-normalizedLifetime). this way, when
				// normalizedLifetime is 0 or 1, alpha is 0. the maximum value is at
				// normalizedLifetime = .5, and is
				// (normalizedLifetime) * (1-normalizedLifetime)
				// (.5)                 * (1-.5)
				// .25
				// since we want the maximum alpha to be 1, not .25, we'll scale the 
				// entire equation by 4.
				float alpha = 4 * normalizedLifetime * (1 - normalizedLifetime);
				Color color = new Color (255, 255, 255, (byte)Math.Round(255 * alpha));

				// make particles grow as they age. they'll start at 75% of their size,
				// and increase to 100% once they're finished.
				float scale = p.Scale * (.75f + .25f * normalizedLifetime);

				sprite.Position = new Vector2f (p.Position.X, p.Position.Y);
				sprite.Color = color;
				sprite.Scale = new Vector2f (scale, scale);
				sprite.Rotation = p.Rotation;

				SpriteBatch.Draw(sprite);
			}

			SpriteBatch.End();
			graphicsDevice.Draw (SpriteBatch, blendState);
		}
	}
}
using System;
using System.Collections.Generic;
using SFML.Graphics;
using SFML.Window;
using SFGL.Graphics;
using SFGL.Time;
using SFGL.Window;

namespace SFGL.Particles
{
	/// <summary>
	/// Define a configuration for an emitter.
	/// </summary>
	public struct EmitterConfiguration
	{
		public int MaxParticles;
		public Vector2 Position;
		public Vector2 Direction;
		public int Intensity;
		public float Rotation;
		public int ParticlesPerEmission;
		public bool Repeat;
	}

	/// <summary>
	/// An particle emitter.
	/// </summary>
	public class Emitter : GameComponent, Drawable, IUpdateable
	{
		private Vector2 _position;
		private Vector2 _direction;
		private int _maxParticles;
		private int _intensity;
		private int _nbParticlePerEmission;
		private float _rotation;
		private bool _repeat;

		private List<Particle> _particles;
		private long _elapsedTime = 0;
		private int _activeParticleIndex = 0;
		private bool _canRestart = false;
		private bool _active = false;

		public bool Active
		{
			get { return _active; }
			set { _active = value; }
		}

		public Emitter(GameWindow game, EmitterConfiguration configuration)
			: base(game)
		{
			_particles = new List<Particle>(configuration.MaxParticles);
			_maxParticles = configuration.MaxParticles;
			_position = configuration.Position;
			_direction = configuration.Direction;
			_intensity = configuration.Intensity;
			_rotation = configuration.Rotation;
			_nbParticlePerEmission = configuration.ParticlesPerEmission;
			_repeat = configuration.Repeat;
		}

		public void Initialize(ParticleConfiguration configuration)
		{
			Particle particle = null;
			var direction = Vector2.Zero;

			for (int i = 0; i < _maxParticles; i++)
			{
				var random = new Random ();
				direction = new Vector2 (
					(float)(random.NextDouble() * (
						(_direction.X + _rotation / 2) -
						(_direction.X - _rotation / 2)) +
						(_direction.X - _rotation / 2)),
					(float)(random.NextDouble() * (
						(_direction.Y + _rotation / 2) -
						(_direction.Y - _rotation / 2))
						+ (_direction.Y - _rotation / 2))
				);

				particle = new Particle(configuration);
				particle.SetDirection (_direction);
				_particles.Add(particle);
			}
		}

		/// <summary>
		/// Start emitter.
		/// </summary>
		public void Start()
		{
			_active = false;
			_elapsedTime = 0;
			_canRestart = false;

			foreach (Particle particle in _particles)
			{
				particle.Revive();
				particle.SetPosition(_position);
				particle.Active = false;
			}

			_activeParticleIndex = 0;
			_particles[_activeParticleIndex].Active = true;
			_active = true;
		}

		/// <summary>
		/// Stop emission.
		/// </summary>
		public void Stop()
		{
			_active = false;
		}

		/// <summary>
		/// Sets the emitter position.
		/// </summary>
		/// <param name="x">X coordinate on screen.</param>
		/// <param name="y">Y coordinate on screen.</param>
		public void Move(float x, float y)
		{
			int rx = (int)(x - _position.X);
			int ry = (int)(y - _position.Y);

			_position.X = x;
			_position.Y = y;

			foreach (Particle particle in _particles)
				particle.AddPosition(new Vector2(rx, ry));
		}

		private int GetNextParticleIndex()
		{
			_activeParticleIndex++;

			if (_activeParticleIndex >= _maxParticles)
			{
				_canRestart = true;
				return 0;
			}

			return _activeParticleIndex;
		}

		public void Update(GameTime gameTime)
		{
			if (!_active)
				return;

			_elapsedTime += GameTime.ElapsedGameTime.Milliseconds;

			if (_elapsedTime >= _intensity)
			{
				_elapsedTime = 0;
				if (_nbParticlePerEmission > 1)
				{
					for (int i = 0; i < _nbParticlePerEmission; i++)
						_particles[GetNextParticleIndex()].Active = true;
				}
				else
				{
					_particles[GetNextParticleIndex()].Active = true;
				}
			}

			if (_repeat && _canRestart)
			{
				Start();
			}

			foreach (Particle particle in _particles)
				particle.Update(gameTime);
		}

		public void Draw(RenderTarget graphicsDevice, RenderStates states)
		{
			if (!_active)
				return;

			vertexBatch.Begin ();
			foreach (Particle particle in _particles)
				particle.Draw(vertexBatch);
			vertexBatch.End ();
			graphicsDevice.Draw (vertexBatch);
		}
	}
}
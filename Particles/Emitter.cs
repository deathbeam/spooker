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
	/// An particle emitter.
	/// </summary>
	public class Emitter : GameComponent, Drawable, IUpdateable
	{
		private Vector2 _position;
		private Vector2 _direction;
		private List<Particle> _particles;
		private int _maxParticles;
		private int _intensity;
		private long _elapsedTime;
		private int _nbParticlePerEmission;
		private int _activeParticleIndex;
		private float _rotation;
		private bool _repeat;
		private bool _canRestart;
		private ParticleConfiguration _particleConfiguration;
		private bool _active;

		public bool Active
		{
			get { return _active; }
			set { _active = value; }
		}

		/// <summary>
		/// Gets or sets the direction of the emitter.
		/// </summary>
		public Vector2 Direction
		{
			get { return _direction; }
			set { _direction = value; }
		}

		/// <summary>
		/// Gets or sets the maximum particles can be emitted.
		/// </summary>
		public int MaxParticles
		{
			get { return _maxParticles; }
			set
			{
				Stop();
				_maxParticles = value;
				Initialize();
			}
		}

		/// <summary>
		/// Gets or sets the intensity of emission.
		/// </summary>
		public int Intensity
		{
			get { return _intensity; }
			set { _intensity = value; }
		}

		/// <summary>
		/// Gets or sets the number of particles emitted in one emission.
		/// </summary>
		public int NumberOfParticlePerEmission
		{
			get { return _nbParticlePerEmission; }
			set { _nbParticlePerEmission = value; }
		}

		/// <summary>
		/// Enable or disable the loop
		/// </summary>
		public bool Repeat
		{
			get { return _repeat; }
			set { _repeat = value; }
		}

		public Emitter(GameWindow game, Vector2 position, Vector2 direction, float angle, int maxParticles)
			: base(game)
		{
			_maxParticles = maxParticles;
			_particles = new List<Particle>(_maxParticles);
			_rotation = angle;
			_direction = direction;
			_position = position;
			_elapsedTime = 0;
			_intensity = 100;
			_activeParticleIndex = 0;
			_nbParticlePerEmission = 3;
			_repeat = true;
			_canRestart = false;
			_active = false;

			_particleConfiguration = new ParticleConfiguration()
			{
				EnabledRotation = false,
				LifeTime = 400,
				Speed = 8.5f,
				RotationIncrement = 0
			};
		}

		public void Initialize(ParticleConfiguration configuration)
		{
			Particle particle = null;
			Vector2 direction = Vector2.Zero;

			for (int i = 0; i < _maxParticles; i++)
			{
				var random = new Random ();
				direction = new Vector2 (
					(float)(random.NextDouble() * (
						(_particleConfiguration.Direction.X + _particleConfiguration.RotationIncrement / 2) -
						(_particleConfiguration.Direction.X - _particleConfiguration.RotationIncrement / 2)) +
						(_particleConfiguration.Direction.X - _particleConfiguration.RotationIncrement / 2)),
					(float)(random.NextDouble() * (
						(_particleConfiguration.Direction.Y + _particleConfiguration.RotationIncrement / 2) -
						(_particleConfiguration.Direction.Y - _particleConfiguration.RotationIncrement / 2))
						+ (_particleConfiguration.Direction.Y - _particleConfiguration.RotationIncrement / 2))
				);

				_particleConfiguration.Direction = direction;
				particle = new Particle(_particleConfiguration);
				_particles.Add(particle);
			}
		}

		/// <summary>
		/// Initialize the emitter with a texture for particles.
		/// </summary>
		/// <param name="particleSprite">Texture used for particles.</param>
		public void Initialize(Sprite particleSprite)
		{
			_particleConfiguration.Sprite = particleSprite;

			Particle particle = null;
			Vector2 direction = Vector2.Zero;

			for (int i = 0; i < _maxParticles; i++)
			{
				var random = new Random ();
				direction = new Vector2 (
					(float)(random.NextDouble() * (
					    (_direction.X + _rotation / 2) - (_direction.X - _rotation / 2))
						+ (_direction.X - _rotation / 2)),
					(float)(random.NextDouble() * (
					    (_direction.Y + _rotation / 2) - (_direction.Y - _rotation / 2))
						+ (_direction.Y - _rotation / 2)));


				_particleConfiguration.Direction = direction;
				_particleConfiguration.Sprite.Position = new Vector2f (
					_position.X,
					_position.Y);
				particle = new Particle(_particleConfiguration);
				_particles.Add(particle);
			}
		}

		/// <summary>
		/// Initialize the emitter and setup the particles.
		/// </summary>
		/// <param name="particleColor">Particle color</param>
		/// <param name="particleWidth">Particle width</param>
		/// <param name="particleHeight">Particle height</param>
		public void Initialize(Color particleColor, int particleWidth, int particleHeight)
		{
			var image = new Image ((uint)particleWidth, (uint)particleHeight, particleColor);
			var texture = new Texture (image);
			var sprite = new Sprite (texture);
			Initialize(sprite);
		}

		/// <summary>
		/// Initialize the emitter with a default particle collection. 
		/// Created particles are a size of 4x4 and a random color.
		/// </summary>
		public void Initialize()
		{
			var image = new Image (4, 4, Color.Blue);
			var texture = new Texture (image);
			var sprite = new Sprite (texture);
			Initialize(sprite);
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
			if (_activeParticleIndex >= _maxParticles)
			{
				_canRestart = true;
				return 0;
			}
			return _activeParticleIndex++;
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
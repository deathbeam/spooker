using SFGL.Graphics;
using SFML.Graphics;
using SFML.Window;
using SFGL.Time;
using SFGL.Window;

namespace SFGL.Particles
{
	/// <summary>
	/// Define a configuration for a particle.
	/// </summary>
	public struct ParticleConfiguration
	{
		public Sprite Sprite;
		public float Speed;
		public bool EnabledRotation;
		public float RotationIncrement;
		public int LifeTime;
	}

	public class Particle : IUpdateable
	{
		private Sprite _sprite;
		private Vector2 _direction;
		private float _rotationIncrement;
		private bool _enableRotation;
		private float _speed;
		private float _lifeTime;
		private long _elapsedTime;
		private bool _active;

		public bool Active
		{
			get { return _active; }
			set { _active = value; }
		}

		public Particle(ParticleConfiguration configuration)
		{
			_sprite = configuration.Sprite;
			_speed = configuration.Speed;
			_enableRotation = configuration.EnabledRotation;
			_rotationIncrement = configuration.RotationIncrement;
			_lifeTime = configuration.LifeTime;
			_elapsedTime = 0;
			_active = false;
		}
		
		public void AddPosition(Vector2 position)
		{
			var pos = _sprite.Position;
			_sprite.Position = new Vector2f (
				pos.X + position.X,
				pos.Y + position.Y);
		}

		public void SetPosition(Vector2 position)
		{
			_sprite.Position = new Vector2f (
				position.X,
				position.Y);
		}

		public void SetDirection(Vector2 direction)
		{
			_direction = new Vector2(direction);
		}

		/// <summary>
		/// Revive the particle.
		/// </summary>
		public void Revive()
		{
			_active = true;
			_elapsedTime = 0;
		}

		/// <summary>
		/// Update particle position if active.
		/// </summary>
		/// <param name="gameTime"></param>
		public void Update(GameTime gameTime)
		{
			if (!_active)
				return;
				
			_elapsedTime += GameTime.ElapsedGameTime.Milliseconds;

			if (_elapsedTime >= _lifeTime)
				_active = false;
			else
			{
				var pos = _sprite.Position;
				_sprite.Position = new Vector2f (
					pos.X + _direction.X * _speed,
					pos.Y + _direction.Y * _speed);

				if (_enableRotation)
					_sprite.Rotation += _rotationIncrement;
			}
		}

		/// <summary>
		/// Draw particle on screen if active.
		/// </summary>
		/// <param name="vertexBatch"></param>
		public void Draw(VertexBatch vertexBatch)
		{
			if (!_active)
				return;
			vertexBatch.Draw (_sprite);
		}
	}
}

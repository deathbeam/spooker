//-----------------------------------------------------------------------------
// Sprite.cs
//
// Spooker Open Source Game Framework
// Copyright (C) Indie Armory. All rights reserved.
// Website: http://indiearmory.com
// Other Contributors: None
// License: MIT
//-----------------------------------------------------------------------------

namespace Spooker.Graphics
{
	public class Sprite : IDrawable
	{
		#region Private fields

		private readonly SFML.Graphics.Transformable _transformable;

		#endregion

		#region Public fields

		public Texture Texture;
		public Color Color;
		public Rectangle SourceRect;

		#endregion

		#region Properties

		public Vector2 Position
		{
			get { return new Vector2 (_transformable.Position); }
			set { _transformable.Position = value.ToSfml(); }
		}

		public Vector2 Scale
		{
			get { return new Vector2 (_transformable.Scale); }
			set { _transformable.Scale = value.ToSfml(); }
		}

		public Vector2 Size
		{
			get { return Texture.Size; }
		}

		public Vector2 Origin
		{
			get { return new Vector2 (_transformable.Origin); }
			set { _transformable.Origin = value.ToSfml(); }
		}

		public float Rotation
		{
			get { return _transformable.Rotation; }
			set { _transformable.Rotation = value; }
		}

		public Transform Transform
		{
			get { return new Transform (_transformable.Transform); }
		}

		public Rectangle DestRect
		{
			get { return new Rectangle (
				(int)Position.X,
				(int)Position.Y,
				(int)Size.X,
				(int)Size.Y); }
		}

		#endregion

		#region Constructors

		public Sprite (Sprite copy)
		{
			_transformable = new SFML.Graphics.Transformable(copy._transformable);
			Texture = new Texture(copy.Texture);
			Color = new Color(copy.Color);
			SourceRect = copy.SourceRect;
		}

		public Sprite (Texture texture)
		{
			_transformable = new SFML.Graphics.Transformable ();
			Texture = new Texture (texture);
			Color = Color.White;
			SourceRect = new Rectangle (0, 0, (int)Texture.Size.X, (int)Texture.Size.Y);
		}

		#endregion

		#region Public methods

		public void Draw(SpriteBatch spriteBatch, SpriteEffects effects = SpriteEffects.None)
		{
			spriteBatch.Draw (
				Texture,
				Position,
				SourceRect,
				Color,
				Scale,
				Origin,
				Rotation,
				effects);
		}

		#endregion
	}
}
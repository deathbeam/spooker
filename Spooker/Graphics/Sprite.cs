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
	public class Sprite : Transformable, IDrawable
	{
		#region Public fields

		public Texture Texture;
		public Color Color;
		public Rectangle SourceRect;

		#endregion

		#region Properties

		public Vector2 Size
		{
			get { return Texture.Size; }
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

		public Sprite (Sprite copy) : base(copy)
		{
			Texture = new Texture(copy.Texture);
			Color = new Color(copy.Color);
			SourceRect = copy.SourceRect;
		}

		public Sprite (Texture texture)
		{
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
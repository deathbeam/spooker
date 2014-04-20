//-----------------------------------------------------------------------------
// Object.cs
//
// Spooker Open Source Game Framework
// Copyright (C) Indie Armory. All rights reserved.
// Website: http://indiearmory.com
// Other Contributors: None
// License: MIT
//-----------------------------------------------------------------------------

using System.Collections.Generic;

namespace Spooker.Graphics.TiledMap
{
	////////////////////////////////////////////////////////////
	/// <summary>
	/// Human-understandable implementation of objects loaded with
	/// TiledSharp.
	/// </summary>
	////////////////////////////////////////////////////////////
	public class Object : IDrawable
	{
		private readonly Camera _camera;

		public string Name;
		public string Type;
		public Vector2 Position;
		public Vector2 Size;
		public Dictionary<string, string> Properties;
		public Polygon Collision;
		public Texture Texture;
		public Rectangle SourceRect;

		public Object(Camera camera)
		{
			_camera = camera;
		}

		public void Draw(SpriteBatch spriteBatch, SpriteEffects effects = SpriteEffects.None)
		{
			if (Texture == null)
				return;

			spriteBatch.Draw(
				Texture,
				_camera.Transform(Position),
				SourceRect,
				Color.White,
				Vector2.One,
				Vector2.Zero,
				0f,
				effects);
		}
	}
}
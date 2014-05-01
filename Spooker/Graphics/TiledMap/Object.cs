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
using Spooker.Physics;

namespace Spooker.Graphics.TiledMap
{
	////////////////////////////////////////////////////////////
	/// <summary>
	/// Human-understandable implementation of objects loaded with
	/// TiledSharp.
	/// </summary>
	////////////////////////////////////////////////////////////
	public class Object : IDrawable, ICollidable
	{
		public string Name;
		public string Type;
		public ObjectType ObjectType;
		public Vector2 Position;
		public Vector2 Size;
		public Dictionary<string, string> Properties;
		public ICollidable Shape;
		public Texture Texture;
		public Rectangle SourceRect;

		public void Draw(SpriteBatch spriteBatch, SpriteEffects effects = SpriteEffects.None)
		{
			if (ObjectType == ObjectType.Graphic)
				spriteBatch.Draw(Texture, Position, SourceRect,
					Color.White, Vector2.One, Vector2.Zero, 0f, effects);
		}

		public bool Intersects(Line line)
		{
			return Shape.Intersects (line);
		}

		public bool Intersects(Rectangle rectangle)
		{
			return Shape.Intersects (rectangle);
		}

		public bool Intersects(Circle circle)
		{
			return Shape.Intersects (circle);
		}

		public bool Intersects(Polygon polygon)
		{
			return Shape.Intersects (polygon);
		}
	}
}
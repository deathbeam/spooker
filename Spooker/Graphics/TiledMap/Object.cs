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
using FarseerPhysics.Dynamics;
using FarseerPhysics;
using Spooker.Time;

namespace Spooker.Graphics.TiledMap
{
	////////////////////////////////////////////////////////////
	/// <summary>
	/// Human-understandable implementation of objects loaded with
	/// TiledSharp.
	/// </summary>
	////////////////////////////////////////////////////////////
	public class Object : IDrawable, IUpdateable
	{
		public string Name;
		public string Type;
		public ObjectType ObjectType;
		public Vector2 Position;
		public Vector2 Size;
		public Body Shape;
		public PropertyDict Properties;

		/// <summary>
		/// Component uses this for drawing itself
		/// </summary>
		/// <param name="spriteBatch">Sprite batch.</param>
		/// <param name="effects">Effects.</param>
		public void Draw(SpriteBatch spriteBatch, SpriteEffects effects = SpriteEffects.None)
		{
			if (!(Shape.UserData is Sprite))
				return;

			var sprite = Shape.UserData as Sprite;
			spriteBatch.Draw(sprite, effects);
		}

		/// <summary>
		/// Component uses this for updating itself.
		/// </summary>
		/// <param name="gameTime">Provides snapshot of timing values.</param>
		public void Update(GameTime gameTime)
		{
			if (Shape.IsStatic || !(Shape.UserData is Sprite))
				return;

			var sprite = Shape.UserData as Sprite;
			sprite.Position = new Vector2 (
				ConvertUnits.ToDisplayUnits (Shape.Position.X),
				ConvertUnits.ToDisplayUnits (Shape.Position.Y));
			Shape.UserData = sprite;
		}
	}
}
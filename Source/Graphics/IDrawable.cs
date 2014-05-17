//-----------------------------------------------------------------------------
// IDrawable.cs
//
// Spooker Open Source Game Framework
// Copyright (C) Indie Armory. All rights reserved.
// Website: http://indiearmory.com
// Other Contributors: None
// License: MIT
//-----------------------------------------------------------------------------

namespace Spooker.Graphics
{
	/// <summary>
	/// Class with this interface implemented can be drawn with
	/// spritebatch.
	/// </summary>
	public interface IDrawable
	{
		/// <summary>
		/// Component uses this for drawing itself
		/// </summary>
		/// <param name="spriteBatch">Sprite batch.</param>
		/// <param name="effects">Effects.</param>
		void Draw(SpriteBatch spriteBatch, SpriteEffects effects = SpriteEffects.None);
	}
}
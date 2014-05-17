//-----------------------------------------------------------------------------
// IUpdateable.cs
//
// Spooker Open Source Game Framework
// Copyright (C) Indie Armory. All rights reserved.
// Website: http://indiearmory.com
// Other Contributors: None
// License: MIT
//-----------------------------------------------------------------------------

namespace Spooker.Time
{
	////////////////////////////////////////////////////////////
	/// <summary>
	/// Interface used for updateable objects
	/// </summary>
	////////////////////////////////////////////////////////////
    public interface IUpdateable
    {
		/// <summary>
		/// Allows the game component to update itself.
		/// </summary>
		/// <param name="gameTime">Provides snapshot of timing values.</param>
		void Update(GameTime gameTime);
    }
}
//-----------------------------------------------------------------------------
// IUpdateable.cs
//
// Spooker Open Source Game Framework
// Copyright (C) Indie Armory. All rights reserved.
// Website: http://indiearmory.com
// Other Contributors: None
// License: MIT
//-----------------------------------------------------------------------------

using Spooker.Time;

namespace Spooker
{
	////////////////////////////////////////////////////////////
	/// <summary>
	/// Impements Update void, what is used for example
	/// in EntityList class.
	/// </summary>
	////////////////////////////////////////////////////////////
    public interface IUpdateable
    {
		////////////////////////////////////////////////////////////
		/// <summary>
		/// Component uses this for updating itself
		/// </summary>
		////////////////////////////////////////////////////////////
		void Update(GameTime gameTime);
    }
}
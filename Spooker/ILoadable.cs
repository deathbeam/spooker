//-----------------------------------------------------------------------------
// ILoadable.cs
//
// Spooker Open Source Game Framework
// Copyright (C) Indie Armory. All rights reserved.
// Website: http://indiearmory.com
// Other Contributors: None
// License: MIT
//-----------------------------------------------------------------------------

namespace Spooker
{
	////////////////////////////////////////////////////////////
	/// <summary>
	/// Impements LoadContent void, what is used for example
	/// in EntityList class.
	/// </summary>
	////////////////////////////////////////////////////////////
    public interface ILoadable
    {
		////////////////////////////////////////////////////////////
		/// <summary>
		/// Component uses this for loading itself
		/// </summary>
		////////////////////////////////////////////////////////////
        void LoadContent();
    }
}
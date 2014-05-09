//-----------------------------------------------------------------------------
// SpriteBlendMode.cs
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
	/// Blending options to use when rendering.
	/// </summary>
	public enum SpriteBlendMode : byte
	{
		None = 0,
		Alpha = 1,
		Additive = 2,
		Multiply = 3
	}
}
//-----------------------------------------------------------------------------
// SpriteEffects.cs
//
// Spooker Open Source Game Framework
// Copyright (C) Indie Armory. All rights reserved.
// Website: http://indiearmory.com
// Other Contributors: Spodi @ http://netgore.com
// License: MIT
//-----------------------------------------------------------------------------

using System;

namespace Spooker.Graphics
{
	////////////////////////////////////////////////////////////
	/// <summary>
	/// Enum of flags containing the effects that can be applied
	/// to a sprite when rendering it.
	/// </summary>
	////////////////////////////////////////////////////////////
	[Flags]
	public enum SpriteEffects : byte
	{
		/// <summary>No effects specified.</summary>
		None = 0,

		/// <summary>Flips the sprite horizontally before rendering.</summary>
		FlipHorizontally = 1 << 0,

		/// <summary>Flips the sprite vertically before rendering.</summary>
		FlipVertically = 1 << 1,

		/// <summary>Flips the both vertically and horizontally before rendering.</summary>
		FlipVerticalHorizontal = FlipVertically | FlipHorizontally,
	}

	/// <summary>
	/// 
	/// </summary>
	public static class ScaleEffectMultiplier
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="effects"></param>
		/// <returns></returns>
		public static Vector2 Get(SpriteEffects effects)
		{
			return new Vector2(
				((effects & SpriteEffects.FlipHorizontally) != 0) ? -1 : 1,
				((effects & SpriteEffects.FlipVertically) != 0) ? -1 : 1
			);
		}
	}
}
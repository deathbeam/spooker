//-----------------------------------------------------------------------------
// AnimType.cs
//
// Spooker Open Source Game Framework
// Copyright (C) Indie Armory. All rights reserved.
// Website: http://indiearmory.com
// Other Contributors: Spodi @ http://netgore.com
// License: MIT
//-----------------------------------------------------------------------------

namespace Spooker.Graphics.Animations
{
	/// <summary>
	/// Defines how an <see cref="Spooker.Graphics.Animations.Animation"/> animates.
	/// </summary>
	public enum AnimType : byte
	{
		/// <summary>
		/// <see cref="Spooker.Graphics.Animations.Animation"/> that will not animate.
		/// </summary>
		None,

		/// <summary>
		/// <see cref="Spooker.Graphics.Animations.Animation"/> will loop once then change to <see cref="AnimType.None"/> back on the first frame.
		/// </summary>
		LoopOnce,

		/// <summary>
		/// <see cref="Spooker.Graphics.Animations.Animation"/> will loop forever.
		/// </summary>
		Loop
	}
}

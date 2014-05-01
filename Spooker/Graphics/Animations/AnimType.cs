//-----------------------------------------------------------------------------
// AnimType.cs
//
// Spooker Open Source Game Framework
// Copyright (C) Indie Armory. All rights reserved.
// Website: http://indiearmory.com
// Other Contributors: Spodi @ http://netgore.com
// License: MIT
//-----------------------------------------------------------------------------

using System;

namespace Spooker.Graphics.Animations
{
	/// <summary>
	/// Defines how an <see cref="Animation"/> animates.
	/// </summary>
	public enum AnimType : byte
	{
		/// <summary>
		/// <see cref="Animation"/> that will not animate.
		/// </summary>
		None,

		/// <summary>
		/// <see cref="Animation"/> will loop once then change to <see cref="AnimType.None"/> back on the first frame.
		/// </summary>
		LoopOnce,

		/// <summary>
		/// <see cref="Animation"/> will loop forever.
		/// </summary>
		Loop
	}
}

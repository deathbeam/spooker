//-----------------------------------------------------------------------------
// Animation.cs
//
// Spooker Open Source Game Framework
// Copyright (C) Indie Armory. All rights reserved.
// Website: http://indiearmory.com
// Other Contributors: None
// License: MIT
//-----------------------------------------------------------------------------

using System.Collections.Generic;
using Spooker.Time;

namespace Spooker.Graphics.Animations
{
	/// <summary>
	/// Animation.
	/// </summary>
	public class Animation
	{
		private readonly List<Rectangle> _frames;
		private AnimType _animType;
        private int _currentFrame;
		internal string Name;

		/// <summary>Defines for how long will be one frame drawn</summary>
		public GameSpan Duration;

		/// <summary>
		/// Initializes a new instance of the <see cref="Spooker.Graphics.Animations.Animation"/> class.
		/// </summary>
		/// <param name="name">Name.</param>
		/// <param name="animType">Animation type.</param>
		public Animation (string name, AnimType animType)
		{
			Name = name;
			_animType = animType;
			_frames = new List<Rectangle> ();
		}

		/// <summary>
		/// Add the specified frame.
		/// </summary>
		/// <param name="frame">Frame.</param>
		public void Add(Rectangle frame)
		{
			_frames.Add (frame);
		}

		/// <summary>
		/// Remove the specified frame.
		/// </summary>
		/// <param name="frame">Frame.</param>
		public void Remove(Rectangle frame)
		{
			_frames.Remove (frame);
		}

		internal Rectangle GetNextFrame()
		{
			if (_animType == AnimType.None)
				return _frames [0];

			_currentFrame++;

			if (_currentFrame == _frames.Count)
			{
				if (_animType == AnimType.LoopOnce) _animType = AnimType.None;
				_currentFrame = 0;
			}

			return _frames [_currentFrame];
		}
	}
}


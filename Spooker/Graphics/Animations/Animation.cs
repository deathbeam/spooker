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
using System;

namespace Spooker.Graphics.Animations
{
	public class Animation
	{
		private readonly List<Rectangle> _frames;
		private readonly bool _repeat;
        private int _currentFrame;
		internal string Name;

		/// <summary>Defines for how long will be one frame drawn</summary>
		public TimeSpan Duration;

		public Animation (string name, bool repeat)
		{
			Name = name;
			_repeat = repeat;
			_frames = new List<Rectangle> ();
		}

		public void Add(Rectangle frame)
		{
			_frames.Add (frame);
		}

		public void Remove(Rectangle frame)
		{
			_frames.Remove (frame);
		}

		public Rectangle GetNextFrame()
		{
			_currentFrame++;

			if (_currentFrame == _frames.Count)
			{
				if (!_repeat)
					return Rectangle.Empty;

				_currentFrame = 0;
			}

			// Get the current frame
			return _frames [_currentFrame];
		}
	}
}


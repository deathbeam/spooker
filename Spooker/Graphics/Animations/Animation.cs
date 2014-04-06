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
		private int _currentFrame;
		private List<Rectangle> _frames;

		/// <summary>String used to identify this animation</summary>
		public string Name;

		/// <summary>Defines for how long will be one frame drawn</summary>
		public TimeSpan Duration;

		public Animation (string name)
		{
			Name = name;
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
				_currentFrame = 0;

			// Get the current frame
			return _frames [_currentFrame];
		}
	}
}


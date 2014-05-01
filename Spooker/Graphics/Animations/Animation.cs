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
using Spooker.Time;

namespace Spooker.Graphics.Animations
{
	public class Animation
	{
		private readonly List<Rectangle> _frames;
		private AnimType _animType;
        private int _currentFrame;
		internal string Name;

		/// <summary>Defines for how long will be one frame drawn</summary>
		public GameSpan Duration;

		public Animation (string name, AnimType animType)
		{
			Name = name;
			_animType = animType;
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


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

namespace Spooker.Graphics.Animations
{
	public class Animation
	{
		private int _currentFrame;

		/// <summary>List with the frames of the animation</summary>
		public List<Rectangle> Frames;

		/// <summary>Defines for how long will be one frame drawn</summary>
		public float Duration;

		public Animation (string name, AnimatedSprite animator)
		{
			Frames = new List<Rectangle> ();
			animator.AddAnim (name, this);
		}

		public Rectangle GetNextFrame()
		{
			_currentFrame++;

			if (_currentFrame == Frames.Count)
				_currentFrame = 0;

			// Get the current frame
			return Frames [_currentFrame];
		}
	}
}


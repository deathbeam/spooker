//-----------------------------------------------------------------------------
// AnimatedSprite.cs
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
	////////////////////////////////////////////////////////////
	/// <summary>
	/// Simple method of drawing and updating animated sprite
	/// using frame animations.
	/// </summary>
	////////////////////////////////////////////////////////////
	public class AnimatedSprite : Sprite, IUpdateable
	{
		#region Variables

		private readonly List<Animation> _animations;
		private Animation _currentAnim;
		private float _timeSinceStart;
		private bool _pause;

		#endregion

		#region Properties

		public Animation this[string name]
		{
			get
			{
				return _animations.Find(a=> a.Name == name); 
			}
		}

		#endregion

		#region Constructors/Destructors

		public AnimatedSprite (Texture texture) : base(texture)
		{
			_animations = new List<Animation> ();
		}

		#endregion

		#region Functions

		public void Add(string name, bool repeat = true)
		{
			_animations.Add (new Animation(name, repeat));
		}

		public void Play(string name)
		{
			var anim = this [name];

			if (_currentAnim == anim)
				return;

			_pause = false;
			_currentAnim = anim;
			_timeSinceStart = 0;
		}

		public void Pause()
		{
			_pause = true;
		}

		public void Resume()
		{
			_pause = false;
		}

		public void Stop()
		{
			_currentAnim = null;
		}

		////////////////////////////////////////////////////////////
		/// <summary>
		/// Allows the game component to update itself.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.
		/// </param>
		////////////////////////////////////////////////////////////
		public void Update(GameTime gameTime)
		{
			// do not update if animation is paused
			if (_currentAnim == null || _pause) return;

			// calculate dt, the change in the since the last frame.
			var dt = (float)gameTime.ElapsedGameTime.TotalMilliseconds;

			// increment time since starting playing this animation
			_timeSinceStart += dt;

			// get duration of current animation
			var duration = (float)_currentAnim.Duration.TotalMilliseconds;

			// it's time to a next frame?
			if (_timeSinceStart > duration)
			{
				_timeSinceStart = 0;
				SourceRect = _currentAnim.GetNextFrame ();

				if (SourceRect == Rectangle.Empty)
					_currentAnim = null;
			}
		}

		#endregion
	}
}
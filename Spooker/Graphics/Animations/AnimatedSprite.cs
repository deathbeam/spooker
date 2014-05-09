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
	/// <summary>
	/// Simple method of drawing and updating animated sprite
	/// using frame animations.
	/// </summary>
	public class AnimatedSprite : Sprite, IUpdateable
	{
		#region Variables

		private readonly List<Animation> _animations;
		private Animation _currentAnim;
		private float _timeSinceStart;
		private bool _pause;

		#endregion

		#region Properties

		/// <summary>
		/// Gets the <see cref="Spooker.Graphics.Animations.Animation"/> with the specified name.
		/// </summary>
		/// <param name="name">Name.</param>
		public Animation this[string name]
		{
			get
			{
				return _animations.Find(a=> a.Name == name); 
			}
		}

		#endregion

		#region Constructors/Destructors

		/// <summary>
		/// Initializes a new instance of the <see cref="Spooker.Graphics.Animations.AnimatedSprite"/> class.
		/// </summary>
		/// <param name="texture">Texture.</param>
		public AnimatedSprite (Texture texture) : base(texture)
		{
			_animations = new List<Animation> ();
		}

		#endregion

		#region Functions

		/// <summary>
		/// Add animation with the specified name and animType.
		/// </summary>
		/// <param name="name">Name.</param>
		/// <param name="animType">Animation type.</param>
		public void Add(string name, AnimType animType)
		{
			_animations.Add (new Animation(name, animType));
		}

		/// <summary>
		/// Play the specified animation.
		/// </summary>
		/// <param name="name">Name.</param>
		public void Play(string name)
		{
			var anim = this [name];

			if (_currentAnim == anim)
				return;

			_pause = false;
			_currentAnim = anim;
			_timeSinceStart = 0;
		}

		/// <summary>
		/// Pause this instance.
		/// </summary>
		public void Pause()
		{
			_pause = true;
		}

		/// <summary>
		/// Resume this instance.
		/// </summary>
		public void Resume()
		{
			_pause = false;
		}

		/// <summary>
		/// Stop this instance.
		/// </summary>
		public void Stop()
		{
			_currentAnim = null;
		}

		/// <summary>
		/// Allows the game component to update itself.
		/// </summary>
		/// <param name="gameTime">Provides snapshot of timing values.</param>
		public void Update(GameTime gameTime)
		{
			// do not update if animation is paused
			if (_currentAnim == null || _pause) return;

			// calculate dt, the change in the since the last frame.
			var dt = (float)gameTime.ElapsedGameTime.Milliseconds;

			// increment time since starting playing this animation
			_timeSinceStart += dt;

			// get duration of current animation
			var duration = (float)_currentAnim.Duration.Milliseconds;

			// it's time to a next frame?
			if (_timeSinceStart > duration)
			{
				_timeSinceStart = 0;
				SourceRect = _currentAnim.GetNextFrame ();
			}
		}

		#endregion
	}
}
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
using System.Linq;
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
		private string _currentAnim;
		private float _timeSinceStart;
		private bool _pause;

		#endregion

		#region Properties

		public Animation this[string name]
		{
			get
			{
				return _animations.Find((Animation a)=>{return a.Name == name;}); 
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

		public void AddAnim(string name)
		{
			_animations.Add (new Animation(name));
		}

		public void PlayAnim(string name)
		{
			_pause = false;
			_currentAnim = name;
			_timeSinceStart = 0;
		}

		public void PauseAnim()
		{
			_pause = true;
		}

		public void ResumeAnim()
		{
			_pause = false;
		}

		public void StopAnim()
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
			// calculate dt, the change in the since the last frame.
			var dt = (float)gameTime.ElapsedGameTime.TotalMilliseconds;

			// increment time since starting playing this animation
			_timeSinceStart += dt;

			// get duration of current animation
			var duration = (float)this [_currentAnim].Duration.TotalMilliseconds;

			// it's time to a next frame?
			if (!_pause && _currentAnim != null &&_timeSinceStart > duration)
			{
				_timeSinceStart -= duration;

				SourceRect = this[_currentAnim].GetNextFrame ();
			}
		}

		#endregion
	}
}
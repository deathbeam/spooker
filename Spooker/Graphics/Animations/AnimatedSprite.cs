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

		private readonly Dictionary<string, Animation> _animations;
		private string _currentAnim;
		private float _timeSinceStart;
		private readonly Rectangle _resetRect;

		#endregion

		#region Constructors/Destructors

		public AnimatedSprite (Texture texture, Rectangle rectangle) : base(texture)
		{
			_resetRect = rectangle;
			_animations = new Dictionary<string, Animation> ();
		}

		#endregion

		#region Functions

		internal void AddAnim(string name, Animation anim)
		{
			_animations.Add (name, anim);
		}

		public void PlayAnim(string name)
		{
			_currentAnim = name;
			_timeSinceStart = 0;
		}

		public void Reset()
		{
			_currentAnim = null;
			SourceRect = _resetRect;
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

			_timeSinceStart += dt;

			// it's time to a next frame?
			if (_currentAnim != null &&_timeSinceStart > _animations[_currentAnim].Duration.TotalMilliseconds)
			{
				_timeSinceStart -= (float)_animations[_currentAnim].Duration.TotalMilliseconds;

				SourceRect = _animations [_currentAnim].GetNextFrame ();
			}
		}

		#endregion
	}
}
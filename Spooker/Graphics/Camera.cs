//-----------------------------------------------------------------------------
// Camera.cs
//
// Spooker Open Source Game Framework
// Copyright (C) Indie Armory. All rights reserved.
// Website: http://indiearmory.com
// Other Contributors: None
// License: MIT
//-----------------------------------------------------------------------------

using Spooker.Time;

namespace Spooker.Graphics
{
	////////////////////////////////////////////////////////////
	/// <summary>
	/// Camera can be used for controlling drawbale or/and
	/// updateable area of game screen
	/// </summary>
	////////////////////////////////////////////////////////////
	public class Camera : IUpdateable
	{
		private Vector2 _actualPosition;

		////////////////////////////////////////////////////////////
		/// <summary>
        /// Toggle for smooth camera transition
        /// </summary>
		////////////////////////////////////////////////////////////
		public bool Smooth;

		////////////////////////////////////////////////////////////
		/// <summary>
		/// Smoothness determines how quickly the transition will
		/// take place. Higher smoothness will reach the target
		/// position faster.
		/// </summary>
		////////////////////////////////////////////////////////////
		public float Smoothness;

		////////////////////////////////////////////////////////////
		/// <summary>
		/// Center point of the camera
		/// </summary>
		////////////////////////////////////////////////////////////
		public Vector2 Position;

		////////////////////////////////////////////////////////////
		/// <summary>
		/// Size of camera visible area
		/// </summary>
		////////////////////////////////////////////////////////////
		public Vector2 Size;

		////////////////////////////////////////////////////////////
		/// <summary>
		/// Transforms position based on current camera position
		/// </summary>
		////////////////////////////////////////////////////////////
		public Vector2 Transform(Vector2 point)
		{
			return new Vector2 (
				point.X - Bounds.X,
				point.Y - Bounds.Y);
		}

		////////////////////////////////////////////////////////////
		/// <summary>
		/// Calculates the area the camera should display
		/// </summary>
		////////////////////////////////////////////////////////////
		public Rectangle Bounds
		{
			get {
				return new Rectangle(
					(int)(_actualPosition.X - (Size.X / 2)),
					(int)(_actualPosition.Y - (Size.Y / 2)),
					(int)Size.X,
					(int)Size.Y);
			}
		}

		////////////////////////////////////////////////////////////
		/// <summary>
		/// Returns new instance of Camera class with default settings
		/// </summary>
		////////////////////////////////////////////////////////////
		public static Camera Default
		{
			get
			{
				return new Camera(new Rectangle(400, 300, 800, 600))
                {
                    Smoothness = 0.33f,
                    Smooth = false,
                };
			}
		}

		////////////////////////////////////////////////////////////
		/// <summary>
		/// Creates new instance of Camera class.
		/// </summary>
		////////////////////////////////////////////////////////////
		public Camera(Rectangle rectangle)
		{
			Size = new Vector2 (rectangle.Width, rectangle.Height);
			Position = new Vector2 (rectangle.X, rectangle.Y);
			_actualPosition = Position;
		}

		////////////////////////////////////////////////////////////
		/// <summary>
		/// Updates camera position based on camera smooth settings.
		/// </summary>
		////////////////////////////////////////////////////////////
		public void Update(GameTime gameTime)
		{
			if (_actualPosition == Position)
				return;

			if (Smooth)
			{
				var dir = Vector2.Direction(_actualPosition, Position);
				var len = Vector2.Distance(_actualPosition, Position);
				_actualPosition += Vector2.LengthDir(dir, len * Smoothness);
			}
			else
			{
				_actualPosition = Position;
			}
		}
	}
}
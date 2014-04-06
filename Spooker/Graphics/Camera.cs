//-----------------------------------------------------------------------------
// Camera.cs
//
// Spooker Open Source Game Framework
// Copyright (C) Indie Armory. All rights reserved.
// Website: http://indiearmory.com
// Other Contributors: None
// License: MIT
//-----------------------------------------------------------------------------

using System;
using SFML.Graphics;
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
		private Vector2 ActualPosition;

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
				point.X - (ActualPosition.X - (Size.X / 2)),
				point.Y - (ActualPosition.Y - (Size.Y / 2)));
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
					(int)(ActualPosition.X - (Size.X / 2)),
					(int)(ActualPosition.Y - (Size.Y / 2)),
					(int)Size.X,
					(int)Size.Y);
			}
		}

		////////////////////////////////////////////////////////////
		/// <summary>
		/// Checks if object is visible in current camera area
		/// </summary>
		////////////////////////////////////////////////////////////
		public bool ObjectIsVisible(Vector2 position, Vector2 size)
		{
			return (Bounds.Intersects(new Rectangle(
				(int)position.X,
				(int)position.Y,
				(int)size.X,
				(int)size.Y)));
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
			Size = new Vector2 ((float)rectangle.Width, (float)rectangle.Height);
			Position = new Vector2 ((float)rectangle.X, (float)rectangle.Y);
			ActualPosition = Position;
		}

		////////////////////////////////////////////////////////////
		/// <summary>
		/// Updates camera position based on camera smooth settings.
		/// </summary>
		////////////////////////////////////////////////////////////
		public void Update(GameTime gameTime)
		{
			if (ActualPosition == Position)
				return;

			if (Smooth)
			{
				var dir = Vector2.Direction(ActualPosition, Position);
				var len = Vector2.Distance(ActualPosition, Position);
				ActualPosition += Vector2.LengthDir(dir, len * Smoothness);
			}
			else
			{
				ActualPosition = Position;
			}
		}
	}
}
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
		internal View View;
		internal Vector2 ActualPosition;

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
		/// Toggle for automatic position rounding. Useful if pixel
		/// sizes become inconsistent or font blurring occurs.
		/// </summary>
		////////////////////////////////////////////////////////////
		public bool RoundPosition;

		////////////////////////////////////////////////////////////
		/// <summary>
		/// Center point of the camera
		/// </summary>
		////////////////////////////////////////////////////////////
		public Vector2 Position;

		////////////////////////////////////////////////////////////
		/// <summary>
		/// Transforms position based on current camera position
		/// </summary>
		////////////////////////////////////////////////////////////
		public Vector2 Transform(Vector2 point)
		{
			return point - new Vector2(Bounds.X, Bounds.Y);
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
					(int)(View.Center.X - (View.Size.X / 2)),
					(int)(View.Center.Y - (View.Size.Y / 2)),
					(int)View.Size.X,
					(int)View.Size.Y);
			}
		}

		////////////////////////////////////////////////////////////
		/// <summary>
		/// Checks if object is visible in current camera area
		/// </summary>
		////////////////////////////////////////////////////////////
		public bool ObjectIsVisible(Rectangle bounds)
		{
			return (Bounds.Intersects(bounds));
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
                return new Camera(new Rectangle(0, 0, 800, 600))
                {
                    Smoothness = 0.33f,
                    Smooth = false,
                    RoundPosition = true
                };
			}
		}

		////////////////////////////////////////////////////////////
		/// <summary>
		/// Creates new instance of Camera class using Rectangle
		/// </summary>
		////////////////////////////////////////////////////////////
		public Camera(Rectangle rect)
			: this(new View (new FloatRect(rect.X, rect.Y, rect.Width, rect.Height)))
		{
		}

		////////////////////////////////////////////////////////////
		/// <summary>
		/// Creates new instance of Camera class using View
		/// </summary>
		////////////////////////////////////////////////////////////
		public Camera(View view)
		{
			View = new View(view);
			View.Center = View.Size / 2;
			Position = new Vector2 (View.Center);
			ActualPosition = Position;
		}

		////////////////////////////////////////////////////////////
		/// <summary>
		/// Applies camera position to view.
		/// </summary>
		////////////////////////////////////////////////////////////
		public void Apply()
		{
			var center = ActualPosition;

            if (RoundPosition)
            {
				center.X = (float)Math.Round(ActualPosition.X, 1);
				center.Y = (float)Math.Round(ActualPosition.Y, 1);
            }

			View.Center = center.ToSfml();
		}

		////////////////////////////////////////////////////////////
		/// <summary>
		/// Updates camera position based on camera smooth settings.
		/// </summary>
		////////////////////////////////////////////////////////////
		public void Update(GameTime gameTime)
		{
			if (Smooth)
			{
				var dir = Vector2.Direction(ActualPosition, Position);
				var len = Vector2.Distance(ActualPosition, Position);
				ActualPosition += Vector2.LengthDir(dir, len * Smoothness);
			}
			else
				ActualPosition = Position;
		}
	}
}
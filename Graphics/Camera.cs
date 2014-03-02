/* File Description
 * Original Works/Author: Thomas Slusny
 * Other Contributors: None
 * Author Website: http://indiearmory.com
 * License: MIT
*/

using System;
using SFML.Graphics;
using SFML.Window;
using SFGL.Window;
using SFGL.Time;
using SFGL.Utils;

namespace SFGL.Graphics
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
		public bool Smooth { get; set; }

		////////////////////////////////////////////////////////////
		/// <summary>
		/// Smoothness determines how quickly the transition will
		/// take place. Higher smoothness will reach the target
		/// position faster.
		/// </summary>
		////////////////////////////////////////////////////////////
		public float Smoothness { get; set; }

		////////////////////////////////////////////////////////////
		/// <summary>
		/// Toggle for automatic position rounding. Useful if pixel
		/// sizes become inconsistent or font blurring occurs.
		/// </summary>
		////////////////////////////////////////////////////////////
		public bool RoundPosition { get; set; }

		////////////////////////////////////////////////////////////
		/// <summary>
		/// Center point of the camera
		/// </summary>
		////////////////////////////////////////////////////////////
		public Vector2 Position { get; set; }

		////////////////////////////////////////////////////////////
		/// <summary>
		/// Transforms position based on current camera position
		/// </summary>
		////////////////////////////////////////////////////////////
		public Vector2 Transform(Vector2 point)
		{
			return point - ActualPosition;
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
					View.Center.X - (View.Size.X / 2),
					View.Center.Y - (View.Size.Y / 2),
					View.Size.X,
					View.Size.Y);
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
		/// Creates new instance of Camera class using Rectangle
		/// </summary>
		////////////////////////////////////////////////////////////
		public Camera(Rectangle rect)
		{
			View = new View (new FloatRect (rect.X, rect.Y, rect.Width, rect.Height));
			View.Center = View.Size / 2;
			Position = new Vector2 (View.Center);
			ActualPosition = Position;
			Smoothness = 0.33f;
			Smooth = false;
			RoundPosition = true;
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
			Smoothness = 0.33f;
			Smooth = false;
			RoundPosition = true;
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
				center.X = FloatMath.RoundToNearest(ActualPosition.X, 1);
				center.Y = FloatMath.RoundToNearest(ActualPosition.Y, 1);
            }

			View.Center = new Vector2f(center.X, center.Y);
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
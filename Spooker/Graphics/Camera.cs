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
	public class Camera : Transformable, IUpdateable
	{
		public interface IFollowable
		{
			/// <summary>
			/// Follows the position.
			/// </summary>
			/// <returns>The position.</returns>
			Vector2 FollowPosition();
		}

		/// <summary>
		/// This is used for following specified followable object.
		/// </summary>
		public IFollowable Follow;

		/// <summary>
		/// Size of camera visible area
		/// </summary>
		public Vector2 Size;

		/// <summary>
		/// Gets or sets the zoom of this camera.
		/// </summary>
		/// <value>The zoom.</value>
		public float Zoom
		{
			get { return Scale.X; }
			set { Scale = new Vector2 ((float)MathHelper.Clamp (value, 0.0001f, 10f)); }
		}

		/// <summary>
		/// Gets or sets the position.
		/// </summary>
		/// <value>The position.</value>
		public new Vector2 Position
		{
			get { return Origin; }
			set { Origin = value; }
		}

		private new Vector2 Origin
		{
			get { return base.Origin; }
			set { base.Origin = value; }
		}

		private new Vector2 Scale
		{
			get { return base.Scale; }
			set { base.Scale = value; }
		}

		/// <summary>
		/// Returns new instance of Camera class with default settings.
		/// </summary>
		/// <value>The default.</value>
		public static Camera Default
		{
			get
			{
				return new Camera (new Rectangle (400, 300, 800, 600));
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Spooker.Graphics.Camera"/> class.
		/// </summary>
		/// <param name="rectangle">Rectangle.</param>
		public Camera(Rectangle rectangle)
		{
			Size = new Vector2 (rectangle.Width, rectangle.Height);
			base.Origin = new Vector2 (rectangle.X, rectangle.Y);
			base.Position = Size / 2;
		}

		/// <summary>
		/// Component uses this for updating itself.
		/// </summary>
		/// <param name="gameTime">Provides snapshot of timing values.</param>
		public void Update(GameTime gameTime)
		{
			Scale = new Vector2 ((float)MathHelper.Clamp (Scale.X, 0.01, 10), (float)MathHelper.Clamp (Scale.Y, 0.01, 10));

			if (Follow != null)
				Position = Follow.FollowPosition ();
		}
	}
}
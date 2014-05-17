//-----------------------------------------------------------------------------
// Light.cs
//
// Spooker Open Source Game Framework
// Copyright (C) Indie Armory. All rights reserved.
// Website: http://indiearmory.com
// Other Contributors: None
// License: MIT
//-----------------------------------------------------------------------------

namespace Spooker.Graphics.Lights
{
	public class Light
	{
		/// <summary>
		/// The position.
		/// </summary>
		public Vector2 Position;

		/// <summary>
		/// The color.
		/// </summary>
		public Color Color;

		/// <summary>
		/// The ratio.
		/// </summary>
		public float Ratio;

		/// <summary>
		/// The use camera.
		/// </summary>
		public bool UseCamera;

		/// <summary>
		/// Initializes a new instance of the <see cref="Spooker.Graphics.Lights.Light"/> class.
		/// </summary>
		/// <param name="pos">Position.</param>
		/// <param name="ratio">Ratio.</param>
		/// <param name="color">Color.</param>
		/// <param name="useCamera">If set to <c>true</c> use camera.</param>
		public Light ( Vector2 pos, float ratio, Color color, bool useCamera = true)
		{
			Position = pos;
			Ratio = ratio;
			Color = color;
			UseCamera = useCamera;
		}
	}
}
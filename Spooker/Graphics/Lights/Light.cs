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
		public Vector2 Position;
		public Color Color;
		public float Ratio;

		public Light ( Vector2 pos, float ratio) : this(pos, ratio, Color.White)
		{
		}

		public Light ( Vector2 pos, float ratio, Color color)
		{
			Position = pos;
			Ratio = ratio;
			Color = color;
		}
	}
}
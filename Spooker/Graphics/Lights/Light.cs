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
		//TODO: Finish light sytem

		public Vector2 Position;
		public Color Color = Color.White;
		public float Ratio;

		public Light ( Vector2 pos, float ratio)
		{
			Position = pos;
			Ratio = ratio;
		}
	}
}
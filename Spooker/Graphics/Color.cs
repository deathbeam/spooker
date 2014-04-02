//-----------------------------------------------------------------------------
// Color.cs
//
// Spooker Open Source Game Framework
// Copyright (C) Indie Armory. All rights reserved.
// Website: http://indiearmory.com
// Other Contributors: Kyle Pulver @ http://otter2d.com
// License: MIT
//-----------------------------------------------------------------------------

using System;

namespace Spooker.Graphics
{
	////////////////////////////////////////////////////////////
	/// <summary>
	/// Class that represents a color with red, green, blue, and 
	/// alpha channels.
	/// </summary>
	////////////////////////////////////////////////////////////
	[Serializable]
	public class Color
	{
		#region Private fields

		private float _r;
		private float _g;
		private float _b;
		private float _a;

		#endregion

		#region Properties

		/// <summary>Red</summary>
		public float R
		{
			get { return _r; }
			set { _r = (float)MathHelper.Clamp(value, 0, 1); }
		}

		/// <summary>Green</summary>
		public float G
		{
			get { return _g; }
			set { _g = (float)MathHelper.Clamp(value, 0, 1);}
		}

		/// <summary>Blue</summary>
		public float B
		{
			get { return _b; }
			set { _b = (float)MathHelper.Clamp(value, 0, 1); }
		}

		/// <summary>Alpha</summary>
		public float A
		{
			get { return _a; }
			set { _a = (float)MathHelper.Clamp(value, 0, 1); }
		}

		/// <summary>The bytes for Red.</summary>
		public byte ByteR
		{
			set { R = value / 255f; }
			get { return (byte)(R * 255); }
		}

		/// <summary>The bytes for Green.</summary>
		public byte ByteG
		{
			set { G = value / 255f; }
			get { return (byte)(G * 255); }
		}

		/// <summary>The bytes for Blue.</summary>
		public byte ByteB
		{
			set { B = value / 255f; }
			get { return (byte)(B * 255); }
		}

		/// <summary>The bytes for Alpha.</summary>
		public byte ByteA
		{
			set { A = value / 255f; }
			get { return (byte)(A * 255); }
		}

		#endregion

		#region SFML Helpers

		internal Color(SFML.Graphics.Color copy)
		{
			ByteR = copy.R;
			ByteG = copy.G;
			ByteB = copy.B;
			ByteA = copy.A;
		}

		internal SFML.Graphics.Color ToSfml()
		{
			return new SFML.Graphics.Color(ByteR, ByteG, ByteB, ByteA);
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Create a empty color.
		/// </summary>
		public Color()
		{
		}

		/// <summary>
		/// Create a new color.
		/// </summary>
		/// <param name="r">Red, 0 to 1.</param>
		/// <param name="g">Green, 0 to 1.</param>
		/// <param name="b">Blue, 0 to 1.</param>
		/// <param name="a">Alpha, 0 to 1.</param>
		public Color(float r, float g, float b, float a)
		{
			R = r;
			G = g;
			B = b;
			A = a;
		}

        /// <summary>
        /// Create a new color.
        /// </summary>
        /// <param name="r">Red, 0 to 1.</param>
        /// <param name="g">Green, 0 to 1.</param>
        /// <param name="b">Blue, 0 to 1.</param>
        public Color(float r, float g, float b)
        {
            R = r;
            G = g;
            B = b;
			A = 1f;
        }

		/// <summary>
		/// Create a color by copying the RGBA from another color.
		/// </summary>
		/// <param name="copy">The color to copy.</param>
		public Color(Color copy) : this(copy.R, copy.G, copy.B, copy.A) { }

		/// <summary>
		/// Create a new color from a string.  Formats are "RGB", "RGBA", "RRGGBB", and "RRGGBBAA".
		/// </summary>
		/// <param name="hex">A string with a hex representation of each channel.</param>
		public Color(string hex)
		{
			int red = 255, green = 255, blue = 255, alpha = 255;

			if (hex.Length == 3)
			{
				red = Convert.ToInt32(hex.Substring(0, 1) + hex.Substring(0, 1), 16);
				green = Convert.ToInt32(hex.Substring(1, 1) + hex.Substring(1, 1), 16);
				blue = Convert.ToInt32(hex.Substring(2, 1) + hex.Substring(2, 1), 16);
			}
			else if (hex.Length == 4)
			{
				red = Convert.ToInt32(hex.Substring(0, 1) + hex.Substring(0, 1), 16);
				green = Convert.ToInt32(hex.Substring(1, 1) + hex.Substring(1, 1), 16);
				blue = Convert.ToInt32(hex.Substring(2, 1) + hex.Substring(2, 1), 16);
				alpha = Convert.ToInt32(hex.Substring(3, 1) + hex.Substring(2, 1), 16);
			}
			else if (hex.Length == 6)
			{
				red = Convert.ToInt32(hex.Substring(0, 2), 16);
				green = Convert.ToInt32(hex.Substring(2, 2), 16);
				blue = Convert.ToInt32(hex.Substring(4, 2), 16);
			}
			else if (hex.Length == 8)
			{
				red = Convert.ToInt32(hex.Substring(0, 2), 16);
				green = Convert.ToInt32(hex.Substring(2, 2), 16);
				blue = Convert.ToInt32(hex.Substring(4, 2), 16);
				alpha = Convert.ToInt32(hex.Substring(6, 2), 16);
			}

			R = MathHelper.ScaleClamp(red, 0, 255, 0, 1);
			G = MathHelper.ScaleClamp(green, 0, 255, 0, 1);
			B = MathHelper.ScaleClamp(blue, 0, 255, 0, 1);
			A = MathHelper.ScaleClamp(alpha, 0, 255, 0, 1);
		}

		/// <summary>
		/// Create a new color from a hex number.  Formats are 0xRGB, 0xRRGGBB, 0xRGBA, 0xRRGGBBAA.
		/// </summary>
		/// <param name="hex">A hex number representing a color.</param>
		public Color(UInt32 hex) : this(hex.ToString("X")) { }

		/// <summary>
		/// Create a new color using bytes from 0 to 255.
		/// </summary>
		/// <param name="r">Red bytes 0 to 255.</param>
		/// <param name="g">Green bytes 0 to 255.</param>
		/// <param name="b">Blue bytes 0 to 255.</param>
		/// <param name="a">Alpha bytes 0 to 255.</param>
		/// <returns>A new color.</returns>
		public static Color FromBytes(byte r, byte g, byte b, byte a)
		{
			return  new Color(
				r / 255f,
				g / 255f,
				b / 255f,
				a / 255f);
		}

		/// <summary>
		/// Create a new color from HSV values.
		/// </summary>
		/// <param name="h">Hue, 0 to 360.</param>
		/// <param name="s">Saturation, 0 to 1.</param>
		/// <param name="v">Value, 0 to 1.</param>
		/// <param name="a">Alpha, 0 to 1.</param>
		/// <returns>A new RGBA color.</returns>
		public static Color FromHSV(float h, float s, float v, float a)
		{
			h = h < 0 ? 0 : (h > 1 ? 1 : h);
			s = s < 0 ? 0 : (s > 1 ? 1 : s);
			v = v < 0 ? 0 : (v > 1 ? 1 : v);
			h *= 360;

			int hi = (int)(h / 60) % 6;
			float f = (h / 60) - (int)(h / 60);
			float p = (v * (1 - s));
			float q = (v * (1 - f * s));
			float t = (v * (1 - (1 - f) * s));
			float r, g, b;

			switch (hi)
			{
			case 0: r = v; g = t; b = p; break;
			case 1: r = q; g = v; b = p; break;
			case 2: r = p; g = v; b = t; break;
			case 3: r = p; g = q; b = v; break;
			case 4: r = t; g = p; b = v; break;
			case 5: r = v; g = p; b = q; break;
			default: r = g = b = 0; break;
			}

			return new Color(r, g, b, a);
		}

		public static Color White
		{
			get { return new Color(1f, 1f, 1f); }
		}

		public static Color Black
		{
			get { return new Color(0f, 0f, 0f); }
		}

		public static Color Red
		{
			get { return new Color(1f, 0f, 0f); }
		}

		public static Color Green
		{
			get { return new Color(0f, 1f, 0f); }
		}

		public static Color Blue
		{
			get { return new Color(0f, 0f, 1f); }
		}

		public static Color Cyan
		{
			get { return new Color(0f, 1f, 1f); }
		}

		public static Color Magenta
		{
			get { return new Color(1f, 0f, 1f); }
		}

		public static Color Yellow
		{
			get { return new Color(1f, 1f, 0f); }
		}

		public static Color Orange
		{
			get { return new Color(1f, 0.5f, 0); }
		}

		public static Color Gold
		{
			get { return new Color("FFCC00"); }
		}

		public static Color None
		{
			get { return new Color(0f, 0f, 0f, 0f); }
		}

		public static Color Grey
		{
			get { return new Color("999999"); }
		}

		public static Color Gray
		{
			get { return Grey; }
		}

		public static Color Random
		{
			get { return new Color(MathHelper.Random(), MathHelper.Random(), MathHelper.Random()); }
		}

		public static Color RandomAlpha
		{
			get { return new Color(MathHelper.Random(), MathHelper.Random(), MathHelper.Random(), MathHelper.Random()); }
		}

		#endregion

		#region Public methods

		public override string ToString()
		{
			return "Color [R: " + R.ToString("0.00") +
				" G: " + G.ToString("0.00") +
				" B: " + B.ToString("0.00") +
				" A: " + A.ToString("0.00") + "]";
		}

		public static Color operator *(Color value1, Color value2)
		{
			value1.R *= value2.R;
			value1.G *= value2.G;
			value1.B *= value2.B;
			value1.A *= value2.A;
			return value1;
		}

		public static Color operator *(Color value1, float value2)
		{
			value1.R *= value2;
			value1.G *= value2;
			value1.B *= value2;
			return value1;
		}

		public static Color operator +(Color value1, Color value2)
		{
			value1.R += value2.R;
			value1.G += value2.G;
			value1.B += value2.B;
			value1.A += value2.A;
			return value1;
		}

		public static Color operator -(Color value1, Color value2)
		{
			value1.R -= value2.R;
			value1.G -= value2.G;
			value1.B -= value2.B;
			value1.A -= value2.A;
			return value1;
		}

		public static Color operator /(Color value1, Color value2)
		{
			value1.R /= value2.R;
			value1.G /= value2.G;
			value1.B /= value2.B;
			value1.A /= value2.A;
			return value1;
		}

		#endregion
	}
}

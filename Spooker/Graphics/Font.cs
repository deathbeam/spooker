//-----------------------------------------------------------------------------
// Font.cs
//
// Spooker Open Source Game Framework
// Copyright (C) Indie Armory. All rights reserved.
// Website: http://indiearmory.com
// Other Contributors: None
// License: MIT
//-----------------------------------------------------------------------------

using System.IO;

namespace Spooker.Graphics
{
	public class Font
	{
		private SFML.Graphics.Font _font;

		#region Public Properties

		public SFML.Graphics.Glyph Glyph(int characterSize, int codePoint, bool bold)
		{
			return _font.GetGlyph ((uint)codePoint, (uint)characterSize, bold);
		}

		public int Kerning(int characterSize, int first, int second)
		{
			return _font.GetKerning ((uint)first, (uint)second,(uint)characterSize);
		}

		public int LineSpacing(int characterSize)
		{
			return _font.GetLineSpacing ((uint)characterSize);
		}

		public Texture Texture(int characterSize)
		{
			return new Texture (_font.GetTexture ((uint)characterSize));
		}

		#endregion Public Properties

		#region SFML Helpers

		internal Font (SFML.Graphics.Font copy)
		{
			_font = new SFML.Graphics.Font (copy);
		}

		internal SFML.Graphics.Font ToSfml()
		{
			return _font;
		}

		#endregion SFML Helpers

		#region Constructors

		public Font (Font copy)
		{
			_font = new SFML.Graphics.Font (copy._font);
		}

		public Font (string filename)
			: this(new FileStream(filename, FileMode.Open))
		{
		}

		public Font (Stream stream)
		{
			_font = new SFML.Graphics.Font (stream);
		}

		#endregion Constructors
	}
}
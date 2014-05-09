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
	/// <summary>
	/// Font.
	/// </summary>
	public class Font
	{
		private readonly SFML.Graphics.Font _font;

		#region Public Properties

		/// <summary>
		/// Glyph with the specified characterSize, codePoint and bold.
		/// </summary>
		/// <param name="characterSize">Character size.</param>
		/// <param name="codePoint">Code point.</param>
		/// <param name="bold">If set to <c>true</c> bold.</param>
		public SFML.Graphics.Glyph Glyph(int characterSize, int codePoint, bool bold)
		{
			return _font.GetGlyph ((uint)codePoint, (uint)characterSize, bold);
		}

		/// <summary>
		/// Kerning of the specified characterSize, first and second.
		/// </summary>
		/// <param name="characterSize">Character size.</param>
		/// <param name="first">First.</param>
		/// <param name="second">Second.</param>
		public int Kerning(int characterSize, int first, int second)
		{
			return _font.GetKerning ((uint)first, (uint)second,(uint)characterSize);
		}

		/// <summary>
		/// Spacing of lines.
		/// </summary>
		/// <returns>The spacing.</returns>
		/// <param name="characterSize">Character size.</param>
		public int LineSpacing(int characterSize)
		{
			return _font.GetLineSpacing ((uint)characterSize);
		}

		/// <summary>
		/// Texture of this font.
		/// </summary>
		/// <param name="characterSize">Character size.</param>
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

		/// <summary>
		/// Initializes a new instance of the <see cref="Spooker.Graphics.Font"/> class.
		/// </summary>
		/// <param name="copy">Copy.</param>
		public Font (Font copy)
		{
			_font = new SFML.Graphics.Font (copy._font);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Spooker.Graphics.Font"/> class.
		/// </summary>
		/// <param name="filename">Filename.</param>
		public Font (string filename)
			: this(new FileStream(filename, FileMode.Open))
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Spooker.Graphics.Font"/> class.
		/// </summary>
		/// <param name="stream">Stream.</param>
		public Font (Stream stream)
		{
			_font = new SFML.Graphics.Font (stream);
		}

		#endregion Constructors
	}
}
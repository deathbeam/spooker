//-----------------------------------------------------------------------------
// Text.cs
//
// Spooker Open Source Game Framework
// Copyright (C) Indie Armory. All rights reserved.
// Website: http://indiearmory.com
// Other Contributors: Laurent Gomila @ http://sfml-dev.org, omeg
// License: MIT
//-----------------------------------------------------------------------------

using System;

namespace Spooker.Graphics
{
	public class Text : Transformable, IDrawable
	{
		[Flags]
		public enum Styles : byte
		{
			/// <summary>Regular characters, no style</summary>
			Regular = 0,

			/// <summary> Characters are bold</summary>
			Bold = 1 << 0,

			/// <summary>Characters are in italic</summary>
			Italic = 1 << 1,

			/// <summary>Characters are underlined</summary>
			Underlined = 1 << 2
		}

		#region Private fields

		private bool _needsUpdate;
		private string _displayedString;
		private int _characterSize;
		private Vector2 _size;
		private Font _font;

		#endregion

		#region Public fields
		
		public Color Color;
		public Styles Style;

		#endregion

		#region Properties

		public string DisplayedString
		{
			get { return _displayedString; }
			set { _displayedString = value; _needsUpdate = true; }
		}

		public int CharacterSize
		{
			get { return _characterSize; }
			set { _characterSize = value; _needsUpdate = true; }
		}

		public Font Font
		{
			get { return _font; }
			set { _font = value; _needsUpdate = true; }
		}

		public Vector2 Size
		{
			get
			{
				if (_needsUpdate)
				{
					if (Environment.OSVersion.Platform != PlatformID.Win32NT)
						if (_displayedString[_displayedString.Length - 1] != '\0')
							_displayedString += '\0';

					var extents = new Point (0, Font.LineSpacing (CharacterSize));
					var prev = '\0';

					foreach (var cur in DisplayedString)
					{
						prev = cur;
						if (cur == '\n' || cur == '\v') continue;
						extents.X += Font.Glyph(CharacterSize, cur, false).Advance;
					}

					_size = new Vector2 (extents.X, extents.Y);
					_needsUpdate = false;
				}

				return _size;
			}
		}

		public Rectangle DestRect
		{
			get { return new Rectangle (
				(int)Position.X,
				(int)Position.Y,
				(int)Size.X,
				(int)Size.Y); }
		}

		#endregion

		#region Constructors

		public Text (Text copy) : base(copy)
		{
			DisplayedString = copy.DisplayedString;
			CharacterSize = copy.CharacterSize;
			Font = new Font(copy.Font);
			Color = new Color(copy.Color);
			Style = copy.Style;
		}

		public Text (Font font)
		{
			Font = new Font (font);
			Color = Color.White;
			Style = Styles.Regular;
		}

		#endregion

		#region Public methods

		public void Draw(SpriteBatch spriteBatch, SpriteEffects effects = SpriteEffects.None)
		{
			spriteBatch.Draw (
				Font,
				DisplayedString,
				CharacterSize,
				Position,
				Color,
				Scale,
				Origin,
				Rotation,
				Style,
				effects);
		}

		#endregion
	}
}


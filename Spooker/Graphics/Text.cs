using System;

namespace Spooker.Graphics
{
	public class Text : IDrawable
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

		private readonly SFML.Graphics.Transformable _transformable;
		private bool _needsUpdate;
		private string _displayedString;
		private Vector2 _size;
		#endregion

		#region Public fields
		
		public int CharacterSize;
		public Font Font;
		public Color Color;
		public Styles Style;

		#endregion

		#region Properties

		public string DisplayedString
		{
			get { return _displayedString; }
			set { _displayedString = value; _needsUpdate = true; }
		}

		public Vector2 Position
		{
			get { return new Vector2 (_transformable.Position); }
			set { _transformable.Position = value.ToSfml(); }
		}

		public Vector2 Scale
		{
			get { return new Vector2 (_transformable.Scale); }
			set { _transformable.Scale = value.ToSfml(); }
		}

		public Vector2 Origin
		{
			get { return new Vector2 (_transformable.Origin); }
			set { _transformable.Origin = value.ToSfml(); }
		}

		public float Rotation
		{
			get { return _transformable.Rotation; }
			set { _transformable.Rotation = value; }
		}

		public Vector2 Size
		{
			get
			{
				if (_needsUpdate)
				{
					if (Environment.OSVersion.Platform != PlatformID.Win32NT)
					{
						if (_displayedString[_displayedString.Length - 1] != '\0')
							_displayedString += '\0';
					}

					var extents = new Point (0, Font.LineSpacing (CharacterSize));
					var prev = '\0';

					foreach (var cur in DisplayedString)
					{
						prev = cur;
						if (cur == '\n' || cur == '\v')
							continue;
						extents.X += Font.Glyph(CharacterSize, cur, false).Advance;
					}

					_size = new Vector2 (extents.X, extents.Y);

					_needsUpdate = false;
				}

				return _size;
			}
		}

		public Transform Transform
		{
			get { return new Transform (_transformable.Transform); }
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

		public Text (Text copy)
		{
			_transformable = new SFML.Graphics.Transformable(copy._transformable);
			DisplayedString = copy.DisplayedString;
			CharacterSize = copy.CharacterSize;
			Font = new Font(copy.Font);
			Color = new Color(copy.Color);
			Style = copy.Style;
		}

		public Text (Font font)
		{
			_transformable = new SFML.Graphics.Transformable ();
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


/* File Description
 * Original Works/Author: Spodi
 * Other Contributors: Thomas Slusny
 * Author Website: http://netgore.com
 * License: 
*/

using System;
using SFGL.Utils;
using SFML.Graphics;
using SFML.Window;
using System.Text;
using SFGL.Window;

namespace SFGL.Graphics
{
	////////////////////////////////////////////////////////////
	/// <summary>
	/// An implementation of SpriteBatch using the RenderWindow.
	/// Warning: this spritebatch do not provides optimized 
	/// drawing of sprites. It just provides most of functionality
	///  of XNA spritebatch.
	/// </summary>
	////////////////////////////////////////////////////////////
	public class SpriteBatch : IDisposable
	{
		#region Variables

		private Sprite _sprite = new Sprite();
		private Text _str = new Text();
		private View _view = new View();
		private RenderStates _rs = RenderStates.Default;
		private RenderTarget _rt = null;

		private bool _isDisposed;
		private bool _isStarted;

		#endregion

		#region Properties

		public BlendMode BlendMode
		{
			get { return _rs.BlendMode; }
			set { _rs.BlendMode = value; }
		}

		public bool IsDisposed
		{
			get { return _isDisposed; }
		}

		public bool IsStarted
		{
			get { return _isStarted; }
		}

		#endregion

		#region Constructors

		public SpriteBatch(RenderTarget graphicsDevice){ _rt = graphicsDevice; }

		#endregion

		#region General functions

		public void ChangeTarget(RenderTarget graphicsDevice)
		{
			_rt.Clear ();
			_rt = graphicsDevice;
		}

		public void Begin(BlendMode blendMode, Vector2 position, Vector2 size, float rotation)
		{
			_view.Reset(new FloatRect(position.X, position.Y, size.X, size.Y));
			_view.Rotate(rotation);
			_rt.SetView(_view);

			_rs.BlendMode = blendMode;

			_isStarted = true;
		}

		public void Begin(BlendMode blendMode)
		{
			Begin(blendMode, new Vector2(0f, 0f), new Vector2(_rt.Size.X, _rt.Size.Y), 0f);
		}

		public void Begin()
		{
			Begin(BlendMode.Alpha);
		}

		public void Dispose()
		{
			_isDisposed = true;
		}

		public void End()
		{
			_isStarted = false;
		}

		public void Draw(Drawable drawable, Shader shader = null)
		{
			if (drawable == null)
				return;

			_rs.Shader = shader;
			_rt.Draw(drawable, _rs);
		}

		#endregion

		#region Sprites

		public void Draw(Sprite sprite, Shader shader = null)
		{
			if (sprite == null || sprite.Texture == null)
				return;

			_rs.Shader = shader;
			_rt.Draw(sprite, _rs);
		}

		public void Draw(Texture texture, Rectangle destinationRectangle, Rectangle? sourceRectangle, Color color,
			float rotation, Vector2 origin, SpriteEffects effects = SpriteEffects.None, Shader shader = null)
		{
			if (texture == null)
				return;

			if (sourceRectangle.HasValue)
			{
				_sprite.TextureRect = new IntRect (
					sourceRectangle.Value.X,
					sourceRectangle.Value.Y,
					sourceRectangle.Value.Width,
					sourceRectangle.Value.Height); 
			}
			else
			{
				_sprite.TextureRect = new IntRect(
					0,
					0,
					(int)texture.Size.X,
					(int)texture.Size.Y);
			}

			var spriteTextureRect = _sprite.TextureRect;

			_sprite.Texture = texture;
			_sprite.Position = new Vector2f(destinationRectangle.Left, destinationRectangle.Top);
			_sprite.Color = color;
			_sprite.Rotation = FloatMath.ToDegrees(rotation);
			_sprite.Origin = new Vector2f(origin.X, origin.Y);
			_sprite.Scale = new Vector2f(
				(destinationRectangle.Width / spriteTextureRect.Width)
				* ScaleEffectMultiplier.Get(effects).X,
				(destinationRectangle.Height / spriteTextureRect.Height)
				* ScaleEffectMultiplier.Get(effects).Y);

			_rs.Shader = shader;
			_rt.Draw(_sprite, _rs);
		}

		public void Draw(Texture texture, Rectangle destinationRectangle, Rectangle? sourceRectangle, Color color, Shader shader = null)
		{
			Draw(texture, destinationRectangle, sourceRectangle, color, 0f, new Vector2(), SpriteEffects.None, shader);
		}

		public void Draw(Texture texture, Rectangle destinationRectangle, Color color, Shader shader = null)
		{
			Draw(texture, destinationRectangle, null, color, 0f, new Vector2(), SpriteEffects.None, shader);
		}

		public void Draw(Texture texture, Vector2 position, Rectangle? sourceRectangle, Color color, float rotation,
			Vector2 origin, Vector2 scale, SpriteEffects effects = SpriteEffects.None, Shader shader = null)
		{
			if (texture == null)
				return;

			if (sourceRectangle.HasValue)
			{
				_sprite.TextureRect = new IntRect(
					sourceRectangle.Value.X,
					sourceRectangle.Value.Y,
					sourceRectangle.Value.Width,
					sourceRectangle.Value.Height);
			}
			else
			{
				_sprite.TextureRect = new IntRect(
					0,
					0,
					(int)texture.Size.X,
					(int)texture.Size.Y);
			}

			_sprite.Texture = texture;
			_sprite.Position = new Vector2f(position.X, position.Y);
			_sprite.Color = color;
            _sprite.Rotation = FloatMath.ToDegrees(rotation);
			_sprite.Origin = new Vector2f(origin.X, origin.Y);
			_sprite.Scale = new Vector2f (
				scale.X * ScaleEffectMultiplier.Get (effects).X,
				scale.Y * ScaleEffectMultiplier.Get (effects).Y);

			_rs.Shader = shader;

			_rt.Draw(_sprite, _rs);
		}

		public void Draw(Texture texture, Vector2 position, Rectangle? sourceRectangle, Color color, float rotation,
			Vector2 origin, float scale, SpriteEffects effects = SpriteEffects.None, Shader shader = null)
		{
			Draw(texture, position, sourceRectangle, color, rotation, origin, new Vector2(scale), effects, shader);
		}

		public void Draw(Texture texture, Vector2 position, Rectangle? sourceRectangle, Color color, Shader shader = null)
		{
			Draw(texture, position, sourceRectangle, color, 0, Vector2.Zero, 1.0f, SpriteEffects.None, shader);
		}

		public void Draw(Texture texture, Vector2 position, Color color, Shader shader = null)
		{
			Draw(texture, position, null, color, 0, Vector2.Zero, 1.0f, SpriteEffects.None, shader);
		}

		#endregion

		#region Text
		
		public void DrawString(Font font, string text, Vector2 position, Color color, float rotation, Vector2 origin,
			Vector2 scale, Text.Styles style = Text.Styles.Regular, Shader shader = null)
		{
			if (font == null || string.IsNullOrEmpty(text))
				return;

			_str.Font = font;
			_str.DisplayedString = text;
			_str.Position = new Vector2f(position.X, position.Y);
			_str.Color = color;
			_str.Rotation = rotation;
			_str.Origin = new Vector2f(origin.X, origin.Y);
			_str.Scale = new Vector2f(scale.X, scale.Y);
			_str.Style = style;
			_str.CharacterSize = 12;

			_rs.Shader = shader;

			_rt.Draw(_str, _rs);
		}

		public void DrawString(Font font, StringBuilder text, Vector2 position, Color color, float rotation,
			Vector2 origin, Vector2 scale, Text.Styles style = Text.Styles.Regular, Shader shader = null)
		{
			if (font == null)
				return;

			DrawString(font, text.ToString(), position, color, rotation, origin, scale, style, shader);
		}

		public void DrawString(Font font, StringBuilder text, Vector2 position, Color color, float rotation,
			Vector2 origin, float scale, Text.Styles style = Text.Styles.Regular, Shader shader = null)
		{
			if (font == null)
				return;

			DrawString(font, text.ToString(), position, color, rotation, origin, new Vector2(scale), style, shader);
		}

		public void DrawString(Font font, string text, Vector2 position, Color color, float rotation, Vector2 origin,
			float scale, Text.Styles style = Text.Styles.Regular, Shader shader = null)
		{
			if (font == null)
				return;

			DrawString(font, text, position, color, rotation, origin, new Vector2(scale,scale), style, shader);
		}

		public void DrawString(Font font, StringBuilder text, Vector2 position, Color color)
		{
			if (font == null)
				return;

			DrawString(font, text.ToString(), position, color, 0.0f, Vector2.Zero, 1.0f);
		}

		public void DrawString(Font font, string text, Vector2 position, Color color)
		{
			if (font == null)
				return;

			DrawString(font, text, position, color, 0.0f, Vector2.Zero, 1.0f);
		}

		#endregion
	}
}
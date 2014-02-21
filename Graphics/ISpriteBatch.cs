/* File Description
 * Original Works/Author: Thomas Slusny
 * Other Contributors: None
 * Author Website: http://indiearmory.com
 * License: MIT
*/

using SFGL.Utils;
using SFML.Graphics;
using SFML.Window;
using System.Text;
using System;

namespace SFGL.Graphics
{
	////////////////////////////////////////////////////////////
	/// <summary>
	/// An implementation of SpriteBatch using the RenderWindow.
	/// </summary>
	////////////////////////////////////////////////////////////
	public interface ISpriteBatch : IDisposable
	{
		#region Properties

		BlendMode BlendMode { get; set; }
		bool IsDisposed { get; }
		bool IsStarted { get; }

		#endregion

		#region General functions

		void Begin (BlendMode blendMode, Vector2 position, Vector2 size, float rotation);

		void Begin (BlendMode blendMode);

		void Begin ();

		void End ();

		void Draw (Drawable drawable, Shader shader = null);

		#endregion

		#region Sprites

		void Draw (Sprite sprite, Shader shader = null);

		void Draw (Texture texture, Rectangle destinationRectangle, Rectangle? sourceRectangle, Color color,
			float rotation, Vector2 origin, SpriteEffects effects = SpriteEffects.None, Shader shader = null);

		void Draw (Texture texture, Rectangle destinationRectangle, Rectangle? sourceRectangle, Color color, Shader shader = null);

		void Draw (Texture texture, Rectangle destinationRectangle, Color color, Shader shader = null);

		void Draw (Texture texture, Vector2 position, Rectangle? sourceRectangle, Color color, float rotation,
			Vector2 origin, Vector2 scale, SpriteEffects effects = SpriteEffects.None, Shader shader = null);

		void Draw (Texture texture, Vector2 position, Rectangle? sourceRectangle, Color color, float rotation,
			Vector2 origin, float scale, SpriteEffects effects = SpriteEffects.None, Shader shader = null);

		void Draw (Texture texture, Vector2 position, Rectangle? sourceRectangle, Color color, Shader shader = null);

		void Draw (Texture texture, Vector2 position, Color color, Shader shader = null);

		#endregion

		#region Text
		
		void DrawString (Font font, string text, Vector2 position, Color color, float rotation, Vector2 origin,
			Vector2 scale, Text.Styles style = Text.Styles.Regular, Shader shader = null);

		void DrawString (Font font, StringBuilder text, Vector2 position, Color color, float rotation,
			Vector2 origin, Vector2 scale, Text.Styles style = Text.Styles.Regular, Shader shader = null);
		
		void DrawString (Font font, StringBuilder text, Vector2 position, Color color, float rotation,
			Vector2 origin, float scale, Text.Styles style = Text.Styles.Regular, Shader shader = null);

		void DrawString (Font font, string text, Vector2 position, Color color, float rotation, Vector2 origin,
			float scale, Text.Styles style = Text.Styles.Regular, Shader shader = null);

		void DrawString (Font font, StringBuilder text, Vector2 position, Color color);

		void DrawString (Font font, string text, Vector2 position, Color color);

		#endregion
	}
}
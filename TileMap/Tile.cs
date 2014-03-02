/* File Description
 * Original Works/Author: Thomas Slusny
 * Other Contributors: None
 * Author Website: http://indiearmory.com
 * License: MIT
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SFGL.Graphics;
using SFML.Graphics;
using SFML.Window;
using TiledSharp;

namespace SFGL.TileMap
{
    public class Tile
    {
		private Sprite _sprite;

		public Color Color
		{ 
			get { return _sprite.Color; }
			set { _sprite.Color = value; }
		}

        public Vector2 Position
		{ 
			get { return new Vector2 (_sprite.Position); }
		}

		public void Draw(SpriteBatch spriteBatch, Camera camera)
        {
			var sprite = new Sprite (_sprite) { Position = new Vector2f (
				_sprite.Position.X - camera.Bounds.X,
				_sprite.Position.Y - camera.Bounds.Y)
			};
			spriteBatch.Draw(sprite);
        }

		public Tile(TmxLayerTile tile, Vector2 tileSize, float opacity, Rectangle tileRect, Texture tileSheet)
        {
			_sprite = new Sprite (tileSheet);
			_sprite.Position = new Vector2f (tile.X * tileSize.X, tile.Y * tileSize.Y);
			_sprite.TextureRect = new IntRect (tileRect.X, tileRect.Y, tileRect.Width, tileRect.Height);
			_sprite.Color = new Color (255, 255, 255, (byte)opacity);
		}
    }
}
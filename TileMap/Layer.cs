using System;
/* File Description
 * Original Works/Author: Thomas Slusny
 * Other Contributors: None
 * Author Website: http://indiearmory.com
 * License: MIT
*/

using System.Collections.Generic;
using System.Linq;
using System.Text;
using TiledSharp;
using SFGL.Graphics;
using SFML.Graphics;

namespace SFGL.TileMap
{
    public class Layer
    {
        public string Name { get; set; }
		public byte Opacity { get; set; }
        public bool Visible { get; set; }
        public List<Tile> Tiles { get; set; }
		public Vector2 TileSize { get; set; }

		public void Draw(SpriteBatch spriteBatch, Camera camera)
		{
			foreach (Tile tile in Tiles)
				if ((tile.Position.X >= camera.Bounds.X - TileSize.X) &&
					(tile.Position.X <= camera.Bounds.Width + TileSize.X) &&
					(tile.Position.Y >= camera.Bounds.Y - TileSize.Y) &&
					(tile.Position.Y <= camera.Bounds.Height + TileSize.Y)) 
					tile.Draw(spriteBatch, camera);
		}

        public Layer(TmxLayer layer, Vector2 tileSize, Dictionary<int, Rectangle> tileRect, Dictionary<int, Texture> tileSheet)
        {
            Name = layer.Name;
			Opacity = (byte)Math.Round(255 * layer.Opacity);
            Visible = layer.Visible;
			TileSize = tileSize;

            Tiles = new List<Tile>();
			for (int i = 0; i < layer.Tiles.Count; i++)
            {
				try 
				{
					Tiles.Add(new Tile(
						layer.Tiles[i],
						tileSize,
						Opacity,
						new Rectangle(tileRect[layer.Tiles[i].Gid]),
						tileSheet[layer.Tiles[i].Gid]));
				}
				catch { }
            }
        }
    }
}
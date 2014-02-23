using System;
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
        public float Opacity { get; set; }
        public bool Visible { get; set; }
        public List<Tile> Tiles { get; set; }

        public void Draw(ISpriteBatch spriteBatch)
        {
            foreach (Tile tile in Tiles)
                tile.Draw(spriteBatch);
        }
        public Layer(TmxLayer layer, Vector2 tileSize, Dictionary<int, Rectangle> tileRect, Dictionary<int, Texture> tileSheet)
        {
            Name = layer.Name;
            Opacity = (float)layer.Opacity;
            Visible = layer.Visible;

            Tiles = new List<Tile>();
            for (int i = 0; i < layer.Tiles.Count; i++)
            {
                try {
                    Tiles.Add(new Tile(layer.Tiles[i], tileSize, new Rectangle(tileRect[i]), tileSheet[i]));
                }
                catch {
                    Tiles.Add(new Tile(layer.Tiles[i], tileSize, new Rectangle(), null));
                }
            }
        }
    }
}

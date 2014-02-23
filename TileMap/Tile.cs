using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SFGL.Graphics;
using SFML.Graphics;
using TiledSharp;

namespace SFGL.TileMap
{
    public class Tile
    {
        public Vector2 Position { get; set; }
        public Rectangle SourceRect { get; set; }
        public Texture Tileset { get; set; }

        public void Draw(ISpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Tileset, Position, SourceRect, Color.White);
        }

        public Tile(TmxLayerTile tile, Vector2 tileSize, Rectangle tileRect, Texture tileSheet)
        {
            Position = new Vector2(tile.X * tileSize.X, tile.Y * tileSize.Y);
            SourceRect = tileRect;
            Tileset = tileSheet;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SFGL.Graphics;

namespace SFGL.TileMap
{
    public class Tile
    {
        public Vector2 Position { get; set; }
        public Rectangle SourceRect { get; set; }
        public string Tileset { get; set; }
        public bool Visible { get; set; }
    }
}

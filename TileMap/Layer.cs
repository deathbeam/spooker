using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SFGL.TileMap
{
    public class Layer
    {
        public string Name { get; set; }
        public float Opacity { get; set; }
        public bool Visible { get; set; }
        public List<Tile> Tiles { get; set; }
    }
}

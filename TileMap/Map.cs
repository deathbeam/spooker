using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TiledSharp;
using SFGL.Window;

namespace SFGL.TileMap
{
    public class Map : GameComponent
    {
        public string Name { get; set; }
        public string Version { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int TileWidth { get; set; }
        public int TileHeight { get; set; }
        public List<Layer> Layers { get; set; }

        public Map(GameWindow game, string mapPath) : base(game)
        {
            TmxMap map = new TmxMap(mapPath);
            Name = map.Properties["Name"];
            Version = map.Properties["Version"];
            Width = map.Width;
            Height = map.Height;
            TileWidth = map.TileWidth;
            TileHeight = map.TileHeight;
        }
    }
}

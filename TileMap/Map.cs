using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TiledSharp;
using SFGL.Graphics;
using SFGL.Window;
using SFML.Graphics;

namespace SFGL.TileMap
{
    public class Map : GameComponent, IDrawable
    {
        public string Name { get; set; }
        public string Version { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int TileWidth { get; set; }
        public int TileHeight { get; set; }
        public List<Layer> Layers { get; set; }

        public void Draw()
        {
            SpriteBatch.Begin();
            foreach (Layer layer in Layers)
                layer.Draw(SpriteBatch);
            SpriteBatch.End();
        }

        public Map(GameWindow game, string mapPath) : base(game)
        {
            TmxMap map = new TmxMap(mapPath);
            // Name = map.Properties["Name"];
            // Version = map.Properties["Version"];
            Width = map.Width;
            Height = map.Height;
            TileWidth = map.TileWidth;
            TileHeight = map.TileHeight;

            Dictionary<int, Rectangle> tileRect = new Dictionary<int, Rectangle>();
            Dictionary<int, Texture> tileSheet = new Dictionary<int, Texture>();

            foreach (TmxTileset ts in map.Tilesets)
            {
                var sheet = new Texture(ts.Image.Source);

                // Loop hoisting
                var wStart = ts.Margin;
                var wInc = ts.TileWidth + ts.Spacing;
                var wEnd = ts.Image.Width;

                var hStart = ts.Margin;
                var hInc = ts.TileHeight + ts.Spacing;
                var hEnd = ts.Image.Height;

                // Pre-compute tileset rectangles
                var id = ts.FirstGid;
                for (var h = hStart; h < hEnd; h += hInc)
                {
                    for (var w = wStart; w < wEnd; w += wInc)
                    {
                        var rect = new Rectangle(w, h,
                            ts.TileWidth, ts.TileHeight);
                        tileRect.Add(id, rect);
                        tileSheet.Add(id, sheet);
                        id += 1;
                    }
                }
            }

            // Load layers
            Layers = new List<Layer>();
            foreach (TmxLayer layer in map.Layers)
            {
                Layers.Add(new Layer(layer, new Vector2(map.TileWidth, map.TileHeight), tileRect, tileSheet));
            }
        }
    }
}

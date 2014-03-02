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
using TiledSharp;
using SFGL.Graphics;
using SFGL.Window;
using SFML.Graphics;

namespace SFGL.TileMap
{
	public class Map : GameComponent, Drawable
    {
        public int Width { get; set; }
        public int Height { get; set; }
		public Vector2 TileSize { get; set; }
        public List<Layer> Layers { get; set; }
		
		private Camera camera = null;

		public void Draw(RenderTarget graphicsDevice, RenderStates states)
        {
			SpriteBatch.Begin();
            foreach (Layer layer in Layers)
				layer.Draw(SpriteBatch, camera);
			SpriteBatch.End();
			graphicsDevice.Draw (SpriteBatch);
        }

		public Map(GameWindow game, string filename)
			: this(game, new Camera(game.GetView()), filename)
		{
		}

		public Map(GameWindow game, Camera camera, string filename) : base(game)
        {
			this.camera = camera;

			var map = new TmxMap(filename);
            Width = map.Width;
            Height = map.Height;
			TileSize = new Vector2 (map.TileWidth, map.TileHeight);

			var tileRect = new Dictionary<int, Rectangle>();
			var tileSheet = new Dictionary<int, Texture>();

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
                        var rect = new Rectangle(
							w, h,
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
                Layers.Add(new Layer(
					layer,
					TileSize,
					tileRect,
					tileSheet));
            }
        }
    }
}
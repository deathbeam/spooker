//-----------------------------------------------------------------------------
// Map.cs
//
// Spooker Open Source Game Framework
// Copyright (C) Indie Armory. All rights reserved.
// Website: http://indiearmory.com
// Other Contributors: None
// License: MIT
//-----------------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using TiledSharp;

namespace Spooker.Graphics.TiledMap
{
	////////////////////////////////////////////////////////////
	/// <summary>
	/// Human-understandable implementation of maps loaded with
	/// TiledSharp.
	/// </summary>
	////////////////////////////////////////////////////////////
	public class Map : IDrawable
    {
		// TODO: Make map class more dynamic (not only for loading Tiled tmx maps)

		private readonly List<Layer> _layers;
		private readonly Camera _camera;

	    /// <summary>Width of map in pixels.</summary>
		public int Width;

		/// <summary>Height of map in pixels.</summary>
		public int Height;

		/// <summary>Width and height of one tile (in pixels)</summary>
		public Vector2 TileSize;

		/// <summary>List of polygons what can be used for collisions</summary>
		public List<Polygon> CollisionPolys;

		/// <summary>List of rectangles what can be used for collisions</summary>
		public List<Rectangle> CollisionRects;

		////////////////////////////////////////////////////////////
		/// <summary>
		/// Draws this instance of Map.
		/// </summary>
		////////////////////////////////////////////////////////////
		public void Draw(SpriteBatch spriteBatch, SpriteEffects effects = SpriteEffects.None)
        {
			spriteBatch.Begin ();
			foreach (var layer in _layers)
				layer.Draw(spriteBatch, _camera, effects);
			spriteBatch.End ();
        }

		////////////////////////////////////////////////////////////
		/// <summary>
		/// Draws specified layer of map (you must begin and end
		/// SpriteBatch externally!).
		/// </summary>
		////////////////////////////////////////////////////////////
		public void Draw(int layer, SpriteBatch spriteBatch, SpriteEffects effects = SpriteEffects.None)
		{
			_layers[layer].Draw (spriteBatch, _camera, effects);
		}

		////////////////////////////////////////////////////////////
		/// <summary>
		/// Creates new instance of Map class.
		/// </summary>
		/// <param name="filename">Path to map file</param>
		/// <param name="camera">Camera used for rendering map tiles
		/// </param>
		////////////////////////////////////////////////////////////
		public Map(Camera camera, string filename)
        {
		    _camera = camera;

			var map = new TmxMap(filename);
            Width = map.Width;
            Height = map.Height;
			TileSize = new Vector2 (map.TileWidth, map.TileHeight);

			var gidDict = ConvertGidDict (map.Tilesets);

			foreach(var objectGroup in map.ObjectGroups)
			{
				CollisionPolys = ConvertCollisionPolys(objectGroup.Objects);
				CollisionRects = ConvertCollisionRects (objectGroup.Objects);
			}

            // Load layers
			_layers = new List<Layer>();
			foreach (var layer in map.Layers)
				_layers.Add(new Layer(layer, TileSize, gidDict));
        }

		private Dictionary<int, KeyValuePair<Rectangle, Texture>> ConvertGidDict(IEnumerable<TmxTileset> tilesets)
		{
			var gidDict = new Dictionary<int, KeyValuePair<Rectangle, Texture>>();

			foreach (var ts in tilesets)
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
						var rect = new Rectangle(w, h, ts.TileWidth, ts.TileHeight);
						gidDict.Add(id, new KeyValuePair<Rectangle, Texture>(rect, sheet));
						id += 1;
					}
				}
			}

			return gidDict;
		}

		private List<Polygon> ConvertCollisionPolys(IEnumerable<TmxObjectGroup.TmxObject> objects)
		{
			var polys = new List<Polygon>();

		    foreach (var o in objects)
			{
			    List<Line> lines;
			    Polygon p;
			    if (o.ObjectType == TmxObjectGroup.TmxObjectType.Polyline)
				{
					lines = new List<Line> ();
					p = new Polygon();

					for (int i = 0; i < o.Points.Count - 1; i++)
						lines.Add(new Line(
							o.Points[i].Item1 + o.X,
							o.Points[i].Item2 + o.Y,
							o.Points[i + 1].Item1 + o.X,
							o.Points[i + 1].Item2 + o.Y));

					p.Lines = lines;
					polys.Add(p);
				}

				if (o.ObjectType == TmxObjectGroup.TmxObjectType.Polygon)
				{
					lines = new List<Line> ();
					p = new Polygon();

					for (var i = 0; i < o.Points.Count; i++)
				    {
						if (i == (o.Points.Count - 1))
							lines.Add(new Line(
								o.Points[0].Item1 + o.X,
								o.Points[0].Item2 + o.Y,
								o.Points[i].Item1 + o.X,
								o.Points[i].Item2 + o.Y));
						else
							lines.Add(new Line(
								o.Points[i].Item1 + o.X,
								o.Points[i].Item2 + o.Y,
								o.Points[i + 1].Item1 + o.X,
								o.Points[i + 1].Item2 + o.Y));
				    }

				    p.Lines = lines;
				    polys.Add(p);
				}
			}

			return polys;
		}

		private List<Rectangle> ConvertCollisionRects(IEnumerable<TmxObjectGroup.TmxObject> objects)
		{
			return (from o in objects where o.ObjectType == TmxObjectGroup.TmxObjectType.Basic select new Rectangle(o.X, o.Y, o.Width, o.Height)).ToList();
		}
    }
}
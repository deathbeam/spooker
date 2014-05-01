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
		private readonly Camera _camera;

		/// <summary>Layers of this map.</summary>
		public List<Layer> Layers;

		/// <summary>Width of map (in tiles).</summary>
		public int Width;

		/// <summary>Height of map (in tiles).</summary>
		public int Height;

		/// <summary>Width and height of one tile (in pixels)</summary>
		public Vector2 TileSize;

		/// <summary>List of all objects in this map</summary>
		public List<Object> Objects;

		/// <summary>Properties of this map</summary>
		public Dictionary<string, string> Properties;

		/// <summary>
		/// Gets the bounds of this map (in pixels).
		/// </summary>
		/// <value>The bounds.</value>
		public Rectangle Bounds
		{
			get { return new Rectangle (0, 0, Width * (int)TileSize.X, Height * (int)TileSize.Y); }
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Spooker.Graphics.TiledMap.Map"/> class.
		/// </summary>
		/// <param name="camera">Camera.</param>
		/// <param name="filename">Filename.</param>
		public Map(Camera camera, string filename)
        {
		    _camera = camera;

			var map = new TmxMap(filename);
			Properties = map.Properties;
            Width = map.Width;
            Height = map.Height;
			TileSize = new Vector2 (map.TileWidth, map.TileHeight);

			var gidDict = ConvertGidDict (map.Tilesets);

			// Load objects
			Objects = ConvertObjects(map.ObjectGroups, gidDict);

            // Load layers
			Layers = new List<Layer>();
			foreach (var layer in map.Layers)
				Layers.Add(new Layer(layer, TileSize, gidDict));
        }

		/// <summary>
		/// Component uses this for drawing itself (spriteBatch is started and ended in this sub,
		/// so do not put this into already started spriteBatch.
		/// </summary>
		/// <param name="spriteBatch">Sprite batch.</param>
		/// <param name="effects">Effects.</param>
		public void Draw(SpriteBatch spriteBatch, SpriteEffects effects = SpriteEffects.None)
		{
			spriteBatch.Begin (SpriteBlendMode.Alpha, SpriteSortMode.FrontToBack, _camera.Transform);
			foreach (var layer in Layers)
				layer.Draw(spriteBatch, effects);
			spriteBatch.End ();
		}

		/// <summary>
		/// Draw the layer specified by its name.
		/// </summary>
		/// <param name="spriteBatch">Sprite batch.</param>
		/// <param name="name">Name.</param>
		/// <param name="effects">Effects.</param>
		public void Draw(string name, SpriteBatch spriteBatch, SpriteEffects effects = SpriteEffects.None)
		{
			Layers.Find(l=> l.Name == name).Draw (spriteBatch, effects);
		}

		/// <summary>
		/// Draw the layer specified by its index.
		/// </summary>
		/// <param name="spriteBatch">Sprite batch.</param>
		/// <param name="index">Index.</param>
		/// <param name="effects">Effects.</param>
		public void Draw(int index, SpriteBatch spriteBatch, SpriteEffects effects = SpriteEffects.None)
		{
			Layers[index].Draw (spriteBatch, effects);
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

		private List<Object> ConvertObjects(IEnumerable<TmxObjectGroup> objectGroups, Dictionary<int, KeyValuePair<Rectangle, Texture>> gidDict)
		{
			var objList = new List<Object>();

			foreach (var objectGroup in objectGroups)
			{
				foreach (var o in objectGroup.Objects)
				{
					var obj = new Object ()
					{
					    Name = o.Name,
					    Type = o.Type,
					    Position = new Vector2(o.X, o.Y),
					    Size = new Vector2(o.Width, o.Height),
					    Properties = o.Properties
					};

					if (o.ObjectType == TmxObjectGroup.TmxObjectType.Basic)
					{
						obj.ObjectType = ObjectType.Rectangle;
						obj.Shape = new Rectangle (o.X, o.Y, o.Width, o.Height);
					}

					if (o.ObjectType == TmxObjectGroup.TmxObjectType.Ellipse)
					{
						obj.ObjectType = ObjectType.Ellipse;
						obj.Shape = new Circle (new Vector2 (o.X + o.Width / 2, o.Y + o.Height / 2), o.Width /2);
					}

					if (o.ObjectType == TmxObjectGroup.TmxObjectType.Polyline)
					{
						var lines = new List<Line> ();
						for (int i = 0; i < o.Points.Count - 1; i++)
							lines.Add(new Line(
								o.Points[i].Item1 + o.X,
								o.Points[i].Item2 + o.Y,
								o.Points[i + 1].Item1 + o.X,
								o.Points[i + 1].Item2 + o.Y));

						obj.ObjectType = ObjectType.Polyline;
						obj.Shape = new Polygon (lines);
					}

					if (o.ObjectType == TmxObjectGroup.TmxObjectType.Polygon)
					{
						var lines = new List<Line> ();
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

						obj.ObjectType = ObjectType.Polygon;
						obj.Shape = new Polygon (lines);
					}

					if (o.ObjectType == TmxObjectGroup.TmxObjectType.Tile)
					{
						obj.ObjectType = ObjectType.Graphic;
						obj.Shape = new Rectangle (o.X, o.Y - o.Height, o.Width, o.Height);
						obj.Texture = gidDict [o.Tile.Gid].Value;
						obj.SourceRect = gidDict [o.Tile.Gid].Key;
						obj.Position.Y -= o.Height;
					}

					objList.Add (obj);
				}
			}

			return objList;
		}
    }
}
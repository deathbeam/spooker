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

		////////////////////////////////////////////////////////////
		/// <summary>
		/// Calculates the area the map should display (in pixels)
		/// </summary>
		////////////////////////////////////////////////////////////
		public Rectangle Bounds
		{
			get { return new Rectangle (0, 0, Width * (int)TileSize.X, Height * (int)TileSize.Y); }
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
			Properties = map.Properties;
            Width = map.Width;
            Height = map.Height;
			TileSize = new Vector2 (map.TileWidth, map.TileHeight);

			var gidDict = ConvertGidDict (map.Tilesets);

			foreach(var objectGroup in map.ObjectGroups)
				Objects = ConvertObjects(objectGroup.Objects, gidDict);

            // Load layers
			Layers = new List<Layer>();
			foreach (var layer in map.Layers)
				Layers.Add(new Layer(layer, TileSize, gidDict));
        }

		////////////////////////////////////////////////////////////
		/// <summary>
		/// Draws all layers of this map.
		/// </summary>
		////////////////////////////////////////////////////////////
		public void Draw(SpriteBatch spriteBatch, SpriteEffects effects = SpriteEffects.None)
		{
			spriteBatch.Begin ();
			foreach (var layer in Layers)
				layer.Draw(spriteBatch, _camera, effects);
			spriteBatch.End ();
		}

		////////////////////////////////////////////////////////////
		/// <summary>
		/// Finds and draws layer specified by its name.
		/// </summary>
		////////////////////////////////////////////////////////////
		public void Draw(SpriteBatch spriteBatch, string name, SpriteEffects effects = SpriteEffects.None)
		{
			spriteBatch.Begin ();
			Layers.Find(l=> l.Name == name).Draw (spriteBatch, _camera, effects);
			spriteBatch.End ();
		}

		////////////////////////////////////////////////////////////
		/// <summary>
		/// Finds and draws layer specified by its index.
		/// </summary>
		////////////////////////////////////////////////////////////
		public void Draw(SpriteBatch spriteBatch, int index, SpriteEffects effects = SpriteEffects.None)
		{
			spriteBatch.Begin ();
			Layers[index].Draw (spriteBatch, _camera, effects);
			spriteBatch.End ();
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

		private List<Object> ConvertObjects(IEnumerable<TmxObjectGroup.TmxObject> objects, Dictionary<int, KeyValuePair<Rectangle, Texture>> gidDict)
		{
			var objList = new List<Object>();

		    foreach (var o in objects)
			{
				var obj = new Object (_camera)
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

			return objList;
		}
    }
}
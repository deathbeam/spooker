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
using FarseerPhysics;
using FarseerPhysics.Common;
using FarseerPhysics.Common.Decomposition;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using Spooker.Time;

namespace Spooker.Graphics.TiledMap
{
	////////////////////////////////////////////////////////////
	/// <summary>
	/// Human-understandable implementation of maps loaded with
	/// TiledSharp.
	/// </summary>
	////////////////////////////////////////////////////////////
	public class Map : IDrawable, IUpdateable
    {
		private readonly Camera _camera;

		/// <summary>Handles collisions of this map.</summary>
		public World Physics;

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
			var map = new TmxMap(filename);
			Properties = map.Properties;
            Width = map.Width;
            Height = map.Height;
			TileSize = new Vector2 (map.TileWidth, map.TileHeight);

			// Load camera
			_camera = camera;

			// Initialize grid dictionary
			var gidDict = ConvertGidDict (map.Tilesets);

			// Load layers
			Layers = new List<Layer>();
			foreach (var layer in map.Layers)
				Layers.Add(new Layer(layer, TileSize, gidDict));

			// Load physics
			ConvertUnits.SetDisplayUnitToSimUnitRatio (64f);
			Physics = new World (Microsoft.Xna.Framework.Vector2.Zero); // No gravity

			BodyFactory.CreateEdge (Physics,
				ConvertUnits.ToSimUnits (Microsoft.Xna.Framework.Vector2.Zero),
				ConvertUnits.ToSimUnits (new Microsoft.Xna.Framework.Vector2 (0, (float)Bounds.Height)));
			BodyFactory.CreateEdge (Physics,
				ConvertUnits.ToSimUnits (Microsoft.Xna.Framework.Vector2.Zero),
				ConvertUnits.ToSimUnits (new Microsoft.Xna.Framework.Vector2 ((float)Bounds.Width, 0)));

			BodyFactory.CreateEdge (Physics,
				ConvertUnits.ToSimUnits (new Microsoft.Xna.Framework.Vector2 (0, (float)Bounds.Height)),
				ConvertUnits.ToSimUnits (new Microsoft.Xna.Framework.Vector2 ((float)Bounds.Width, (float)Bounds.Height)));

			BodyFactory.CreateEdge (Physics,
				ConvertUnits.ToSimUnits (new Microsoft.Xna.Framework.Vector2 ((float)Bounds.Width, 0)),
				ConvertUnits.ToSimUnits (new Microsoft.Xna.Framework.Vector2 ((float)Bounds.Width, (float)Bounds.Height)));

			Objects = ConvertObjects(map.ObjectGroups, gidDict);
        }

		/// <summary>
		/// Component uses this for drawing itself
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

		/// <summary>
		/// Component uses this for updating itself.
		/// </summary>
		/// <param name="gameTime">Provides snapshot of timing values.</param>
		public void Update(GameTime gameTime)
		{
			Physics.Step ((float)gameTime.ElapsedGameTime.Seconds);

			foreach (var o in Objects)
				o.Update (gameTime);
		}

		private Dictionary<int, KeyValuePair<Rectangle, Texture>> ConvertGidDict(IEnumerable<TmxTileset> tilesets)
		{
			var gidDict = new Dictionary<int, KeyValuePair<Rectangle, Texture>>();

			foreach (var ts in tilesets)
			{
				var sheet = new Texture(ts.Image.Source);
				sheet.Smooth = false;

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
						if (o.Width == 0 || o.Height == 0)
							continue;

						obj.Shape = BodyFactory.CreateRectangle (Physics,
							ConvertUnits.ToSimUnits(o.Width),
							ConvertUnits.ToSimUnits(o.Height), 1f);
						obj.Shape.Position = new Microsoft.Xna.Framework.Vector2 (
							ConvertUnits.ToSimUnits(o.X),
							ConvertUnits.ToSimUnits(o.Y));

						obj.ObjectType = ObjectType.Rectangle;
					}

					if (o.ObjectType == TmxObjectGroup.TmxObjectType.Tile)
					{
						if (o.Width == 0 || o.Height == 0)
							continue;

						obj.Position.Y -= o.Height;
						var sprite = new Sprite (gidDict [o.Tile.Gid].Value) {
							Position = obj.Position,
							SourceRect = gidDict [o.Tile.Gid].Key,
							Origin = gidDict [o.Tile.Gid].Key.Size /2
						};

						obj.Shape = BodyFactory.CreateRectangle (Physics,
							ConvertUnits.ToSimUnits(o.Width),
							ConvertUnits.ToSimUnits(o.Height), 1f, sprite);
						obj.Shape.Position = new Microsoft.Xna.Framework.Vector2 (
							ConvertUnits.ToSimUnits(o.X), 
							ConvertUnits.ToSimUnits(o.Y - o.Height));

						obj.ObjectType = ObjectType.Graphic;
					}

					if (o.ObjectType == TmxObjectGroup.TmxObjectType.Ellipse)
					{
						obj.Shape = BodyFactory.CreateEllipse (Physics,
							ConvertUnits.ToSimUnits (o.Width / 2),
							ConvertUnits.ToSimUnits (o.Height / 2),
							0, 1f);
						obj.Shape.Position = new Microsoft.Xna.Framework.Vector2 (
							ConvertUnits.ToSimUnits(o.X),
							ConvertUnits.ToSimUnits(o.Y));

						obj.ObjectType = ObjectType.Ellipse;
					}

					if (o.ObjectType == TmxObjectGroup.TmxObjectType.Polyline ||
						o.ObjectType == TmxObjectGroup.TmxObjectType.Polygon)
					{
						var vert = new Vertices ();

						foreach (var point in o.Points)
							vert.Add (new Microsoft.Xna.Framework.Vector2 (
								ConvertUnits.ToSimUnits(point.Item1), 
								ConvertUnits.ToSimUnits(point.Item2)));

						if (o.ObjectType == TmxObjectGroup.TmxObjectType.Polyline) {
							obj.ObjectType = ObjectType.Polyline;
							obj.Shape = BodyFactory.CreateChainShape (Physics, vert, new Microsoft.Xna.Framework.Vector2 (
								ConvertUnits.ToSimUnits (o.X),
								ConvertUnits.ToSimUnits (o.Y)));
						} else {
							obj.ObjectType = ObjectType.Polygon;
							var verts = Triangulate.ConvexPartition (vert, TriangulationAlgorithm.Bayazit);
							obj.Shape = BodyFactory.CreateCompoundPolygon (Physics, verts, 1f, new Microsoft.Xna.Framework.Vector2 (
								ConvertUnits.ToSimUnits (o.X),
								ConvertUnits.ToSimUnits (o.Y)));
						}
					}

					if (o.Type == "Dynamic") {
						if (o.Properties.ContainsKey ("LinearDamping"))
							obj.Shape.LinearDamping = float.Parse (o.Properties ["LinearDamping"]);
						obj.Shape.BodyType = BodyType.Dynamic;
						obj.Shape.IsStatic = false;
					} else {
						obj.Shape.BodyType = BodyType.Static;
						obj.Shape.IsStatic = true;
					}

					objList.Add (obj);
				}
			}

			return objList;
		}
    }
}
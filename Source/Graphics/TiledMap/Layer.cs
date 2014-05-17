//-----------------------------------------------------------------------------
// Layer.cs
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
	/// Human-understandable implementation of layer loaded with
	/// TiledSharp.
	/// </summary>
	////////////////////////////////////////////////////////////
	public class Layer : IDrawable
    {
		private readonly List<Tile> _tiles;

        /// <summary>Name of this layer</summary>
		public string Name;

		/// <summary>Color of this layer</summary>
		public Color Color;
        
		/// <summary>Determines if layer will be drawn or not</summary>
		public bool Visible;

		/// <summary>Properties of this layer</summary>
		public Dictionary<string, string> Properties;
		
		/// <summary>
		/// Initializes a new instance of the <see cref="Spooker.Graphics.TiledMap.Layer"/> class.
		/// </summary>
		/// <param name="layer">Layer.</param>
		/// <param name="tileSize">Tile size.</param>
		/// <param name="gidDict">Gid dict.</param>
		public Layer(TmxLayer layer, Vector2 tileSize, Dictionary<int, KeyValuePair<Rectangle, Texture>> gidDict)
        {
			Properties = layer.Properties;
            Name = layer.Name;
            Visible = layer.Visible;
			Color = new Color (1f, 1f, 1f, (float)layer.Opacity);

			_tiles = new List<Tile>();

			foreach (TmxLayerTile t in layer.Tiles)
			{
			    var gid = t.Gid;

			    if (gid > 0 && gidDict[gid].Value != null)
			        _tiles.Add (new Tile (t, tileSize, gidDict[gid].Key, gidDict[gid].Value));
			}
        }

		/// <summary>
		/// Component uses this for drawing itself
		/// </summary>
		/// <param name="spriteBatch">Sprite batch.</param>
		/// <param name="effects">Effects.</param>
		public void Draw(SpriteBatch spriteBatch, SpriteEffects effects = SpriteEffects.None)
		{
			if (!Visible || Color.A == 0)
				return;

			foreach (var tile in _tiles)
				tile.Draw(spriteBatch, Color, effects);
		}

    }
}
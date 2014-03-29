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
using Spooker.Graphics;

namespace Spooker.TileMap
{
	////////////////////////////////////////////////////////////
	/// <summary>
	/// Human-understandable implementation of layer loaded with
	/// TiledSharp.
	/// </summary>
	////////////////////////////////////////////////////////////
    public class Layer
    {
		private readonly List<Tile> _tiles;
		private readonly Vector2 _tileSize;

        /// <summary>Name of this layer</summary>
		public string Name;

		/// <summary>Color of this layer</summary>
		public Color Color;
        
		/// <summary>Determines if layer will be drawn or not</summary>
		public bool Visible;

		////////////////////////////////////////////////////////////
	    /// <summary>
	    /// Draws this instance of Layer class
	    /// </summary>
	    /// <param name="spriteBatch">Spritebatch used for rendering</param>
	    /// <param name="camera">Camera used for rendering tiles</param>
	    /// <param name="effects"></param>
	    ////////////////////////////////////////////////////////////
		public void Draw(SpriteBatch spriteBatch, Camera camera, SpriteEffects effects = SpriteEffects.None)
		{
			if (!Visible || Color.A == 0)
				return;

			foreach (var tile in _tiles)
				if ((tile.Position.X >= camera.Bounds.X - _tileSize.X) &&
					(tile.Position.X <= camera.Bounds.Width + _tileSize.X) &&
					(tile.Position.Y >= camera.Bounds.Y - _tileSize.Y) &&
					(tile.Position.Y <= camera.Bounds.Height + _tileSize.Y)) 
					tile.Draw(spriteBatch, camera, Color, effects);
		}

		////////////////////////////////////////////////////////////
	    /// <summary>
	    /// Creates new instance of Layer class
	    /// </summary>
	    /// <param name="layer">Base layer loaded with TiledSharp</param>
	    /// <param name="tileSize">Size of one tile (in pixels)</param>
	    /// <param name="gidDict">Dictionary of tiles for this layer</param>
	    ////////////////////////////////////////////////////////////
		public Layer(TmxLayer layer, Vector2 tileSize, Dictionary<int, KeyValuePair<Rectangle, Texture>> gidDict)
        {
            Name = layer.Name;
            Visible = layer.Visible;
			Color = new Color (1f, 1f, 1f, (float)layer.Opacity);

			_tileSize = tileSize;
			_tiles = new List<Tile>();

			foreach (TmxLayerTile t in layer.Tiles)
			{
			    var gid = t.Gid;

			    if (gid > 0 && gidDict[gid].Value != null)
			        _tiles.Add (new Tile (t, tileSize, gidDict[gid].Key, gidDict[gid].Value));
			}
        }
    }
}
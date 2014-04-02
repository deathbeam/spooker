//-----------------------------------------------------------------------------
// Tile.cs
//
// Spooker Open Source Game Framework
// Copyright (C) Indie Armory. All rights reserved.
// Website: http://indiearmory.com
// Other Contributors: None
// License: MIT
//-----------------------------------------------------------------------------

using Spooker.Graphics;
using TiledSharp;

namespace Spooker.TiledMap
{
	////////////////////////////////////////////////////////////
	/// <summary>
	/// Human-understandable implementation of tile loaded with
	/// TiledSharp.
	/// </summary>
	////////////////////////////////////////////////////////////
    public class Tile
    {
		private readonly Texture _texture;
		private readonly Vector2 _position;
		private readonly Rectangle _sourceRect;
		
		public Vector2 Position
		{
			get { return _position; }
		}

		////////////////////////////////////////////////////////////
	    /// <summary>
	    /// Draws this instance of Tile class
	    /// </summary>
	    /// <param name="spriteBatch">Spritebatch used for rendering</param>
	    /// <param name="camera">Camera used for rendering tile</param>
	    /// <param name="color"></param>
	    /// <param name="effects"></param>
	    ////////////////////////////////////////////////////////////
		public void Draw(SpriteBatch spriteBatch, Camera camera, Color color, SpriteEffects effects = SpriteEffects.None)
        {
			spriteBatch.Draw(
				_texture,
				camera.Transform(_position),
				_sourceRect,
				color,
				Vector2.One,
				Vector2.Zero,
				0f,
				effects);
        }

		////////////////////////////////////////////////////////////
		/// <summary>
		/// Creates new instance of Tile class
		/// </summary>
		/// <param name="tile">Base tile loaded with TiledSharp</param>
		/// <param name="tileSize">Size of one tile (in pixels)</param>
		/// <param name="tileRect">Positions of tiles on tileSheet</param>
		/// <param name="tileSheet">Tilesheet textures</param>
		////////////////////////////////////////////////////////////
		public Tile(TmxLayerTile tile, Vector2 tileSize, Rectangle tileRect, Texture tileSheet)
        {
			_texture = tileSheet;
			_position = new Vector2 (tile.X * tileSize.X, tile.Y * tileSize.Y);
			_sourceRect = tileRect;
        }
    }
}
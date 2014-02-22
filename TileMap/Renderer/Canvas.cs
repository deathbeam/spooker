/* File Description
 * Original Works/Author: Marshall Ward
 * Other Contributors: Thomas Slusny
 * Author Website: 
 * License: 
*/

using SFML.Graphics;
using SFML.Window;
using SFGL.Graphics;
using System;
using TiledSharp;

namespace SFGL.TileMap.Renderer
{
	// Canvas tracks three coordinate systems:
	// 1. pWidth/Height: Canvas pixel count
	// 2. tWidth/Height: Canvas tile count
	public class TmxCanvas
	{
		// User-defined (or derived) fields
		public int tMaxWidth = 29;
		public int tMaxHeight = 19;

		// Canvas size
		public int pWidth;          // Canvas pixel width
		public int pHeight;         // Canvas pixel height
		public int pWindowWidth;    // Game window pixel width
		public int pWindowHeight;   // Game window pixel height
		public float tileScale;     // Canvas-to-window scale
		
		public int tWidth;      // # of (full) tiles per width
		public int tHeight;     // # of (full) tiles per height
		public int tHalo = 2;   // # of rendered tiles outside viewport

		// Camera position (focal tile)
		public int pX;      // Camera X position in pixels
		public int pY;      // Camera Y position in pixels
		public int tX;      // Tile.X containing camera
		public int tY;      // Tile.Y containing camera

		// Canvas loop hoisting
		public int tStartX;
		public int tEndX;
		public int tStartY;
		public int tEndY;

		// Mosaic parameters
		// (Note: Testing with default arguments)
		public int pTileWidth = 32;      // Tile width in pixels
		public int pTileHeight = 32;     // Tile height in pixels

		// Necessary?
		public Vector2 camera;
		public Vector2 origin;

		public TmxCanvas(Vector2i gamesize, Vector2i maxsize, Vector2i tileSize)
		{
			pWindowWidth = gamesize.X;
			pWindowHeight = gamesize.Y;
			pTileWidth = tileSize.X;
			pTileHeight = tileSize.Y;

			// Get centre pixel (or left/above centre)
			var pXc = (pWidth - 1) / 2;
			var pYc = (pHeight - 1) / 2;

			// Get tile index containing the pixel
			tX = pXc / pTileWidth;
			tY = pYc / pTileHeight;

			// Readjust pX, pY to the centre of the tile
			pX = tX * pTileWidth + pTileWidth / 2;
			pY = tY * pTileHeight + pTileHeight / 2;

			UpdateViewport ();
		}

		public void UpdateCamera(Vector2f center)
		{
			var pXc = (int)center.X;
			var pYc = (int)center.Y;
			// Get tile index containing the pixel
			tX = pXc / pTileWidth;
			tY = pYc / pTileHeight;

			// Readjust pX, pY to the centre of the tile
			pX = pXc;
			pY = pYc;

			RescaleCamera ();
		}
		
		private void RescaleCanvas()
		{
			// Determine the minimum scaling
			var xScale = (float)pWindowWidth / (pTileWidth * tMaxWidth);
			var yScale = (float)pWindowHeight / (pTileHeight * tMaxHeight);
			tileScale = Math.Max(xScale, yScale);
			tWidth = (int)Math.Round(pWindowWidth / (pTileWidth * tileScale));
			tHeight = (int)Math.Round(pWindowHeight / (pTileHeight * tileScale));

			// Virtual height is prescribed (i.e. window-independent)
			pHeight = (int)Math.Round(pWindowHeight / tileScale);
			pWidth = (int)Math.Round(pWindowWidth / tileScale);
		}

		private void RescaleCamera()
		{
			// Visible tile range (tStart <= t < tEnd)
			tStartX = tX - (tWidth - 1) / 2 - tHalo;
			tEndX = tX + (tWidth - 1) / 2 + 1 + tHalo;
			tStartY = tY - (tHeight - 1) / 2 - tHalo;
			tEndY = tY + (tHeight - 1) / 2 + 1 + tHalo;

			camera = new Vector2(pX, pY);
			origin = camera - new Vector2(pWidth/2, pHeight/2);
		}

		public void UpdateViewport()
		{
			RescaleCanvas();
			RescaleCamera ();
		}
	}
}
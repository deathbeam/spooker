//-----------------------------------------------------------------------------
// Texture.cs
//
// Spooker Open Source Game Framework
// Copyright (C) Indie Armory. All rights reserved.
// Website: http://indiearmory.com
// Other Contributors: None
// License: MIT
//-----------------------------------------------------------------------------

using System.IO;

namespace Spooker.Graphics
{
	public class Texture
	{
		#region Private fields

		private SFML.Graphics.Texture _texture;
		private SFML.Graphics.Image _image;
		private bool _needsUpdate;

		#endregion

		#region Properties

		public bool Smooth
		{
			get { return _texture.Smooth; }
			set { _texture.Smooth = value; }
		}

		public Vector2 Size
		{
			get { return new Vector2(_texture.Size.X, _texture.Size.Y); }
		}

		/// <summary>The array of pixels in the texture in bytes.</summary>
		public byte[] Pixels {
			get { CreateImage(); return _image.Pixels; }
			set { _image = new SFML.Graphics.Image((uint)Size.X, (uint)Size.Y, value); Update(); }
		}

		#endregion

		#region SFML Helpers

		internal Texture (SFML.Graphics.Texture copy)
		{
			_texture = new SFML.Graphics.Texture (copy);
		}

		internal SFML.Graphics.Texture ToSfml()
		{
			return _texture;
		}

		#endregion

		#region Constructors/Destructors

		public Texture (Texture copy)
		{
			_texture = new SFML.Graphics.Texture (copy._texture);

		}

		public Texture (string filename)
			: this(new FileStream(filename, FileMode.Open))
		{
		}

		public Texture (Stream stream)
		{
			_texture = new SFML.Graphics.Texture (stream);
		}

		public Texture (Color[,] pixels)
		{

			var pix = new SFML.Graphics.Color[pixels.GetUpperBound(0),pixels.GetUpperBound(1)];
			for (var x = 0; x < pixels.GetUpperBound(0); x++)
			{
				for (var y = 0; y < pixels.GetUpperBound(1); y++)
				{
					pix [x, y] = pixels [x, y].ToSfml();
				}
			}
			_texture = new SFML.Graphics.Texture (new SFML.Graphics.Image (pix));
		}

		public Texture (Vector2 size)
		{
			_texture = new SFML.Graphics.Texture (
					(uint)size.X,
					(uint)size.Y);
		}

		public Texture (Vector2 size, Color color)
		{
			_texture = new SFML.Graphics.Texture (
				new SFML.Graphics.Image (
					(uint)size.X,
					(uint)size.Y,
					color.ToSfml()));
		}

		public Texture (Vector2 size, byte[] pixels)
		{
			_texture = new SFML.Graphics.Texture (
				new SFML.Graphics.Image (
					(uint)size.X,
					(uint)size.Y,
					pixels));
		}

		#endregion

		#region Functions

		public Color GetPixel(int x, int y)
		{
			CreateImage();

			return new Color(_image.GetPixel((uint)x, (uint)y));
		}

		public void SetPixel(int x, int y, Color color)
		{
			CreateImage();

			_image.SetPixel((uint)x, (uint)y, color.ToSfml());
			_texture = new SFML.Graphics.Texture(_image);

			_needsUpdate = true;
		}

		public void CopyPixels(Texture from, int fromX, int fromY, int toX, int toY)
		{
			CreateImage();

			_image.Copy(
				from._image,
				(uint)toX,
				(uint)toY,
				new SFML.Graphics.IntRect(
					fromX,
					fromY,
					(int)from.Size.X,
					(int)from.Size.Y));
		}

		public void SaveToFile(string path)
		{
			CreateImage();

			_image.SaveToFile(path);
		}

		public void CreateImage()
		{
			if (_image == null)
				_image = _texture.CopyToImage();
		}

		public void Update()
		{
			if (_needsUpdate)
			{
				_texture.Update(_image);
				_needsUpdate = false;
			}
		}

		#endregion
	}
}
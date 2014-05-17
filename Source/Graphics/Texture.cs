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
	/// <summary>
	/// Texture.
	/// </summary>
	public class Texture
	{
		#region Private fields

		private SFML.Graphics.Texture _texture;
		private SFML.Graphics.Image _image;
		private bool _needsUpdate;

		#endregion

		#region Properties

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="Spooker.Graphics.Texture"/> is smooth.
		/// </summary>
		/// <value><c>true</c> if smooth; otherwise, <c>false</c>.</value>
		public bool Smooth
		{
			get { return _texture.Smooth; }
			set { _texture.Smooth = value; }
		}

		/// <summary>
		/// Gets the size.
		/// </summary>
		/// <value>The size.</value>
		public Vector2 Size
		{
			get { return new Vector2(_texture.Size.X, _texture.Size.Y); }
		}

		/// <summary>
		/// Gets or sets the array of pixels in the texture in bytes.
		/// </summary>
		/// <value>The pixels.</value>
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

		/// <summary>
		/// Initializes a new instance of the <see cref="Spooker.Graphics.Texture"/> class.
		/// </summary>
		/// <param name="copy">Copy.</param>
		public Texture (Texture copy)
		{
			_texture = new SFML.Graphics.Texture (copy._texture);

		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Spooker.Graphics.Texture"/> class.
		/// </summary>
		/// <param name="filename">Filename.</param>
		public Texture (string filename)
			: this(new FileStream(filename, FileMode.Open))
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Spooker.Graphics.Texture"/> class.
		/// </summary>
		/// <param name="stream">Stream.</param>
		public Texture (Stream stream)
		{
			_texture = new SFML.Graphics.Texture (stream);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Spooker.Graphics.Texture"/> class.
		/// </summary>
		/// <param name="pixels">Pixels.</param>
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

		/// <summary>
		/// Initializes a new instance of the <see cref="Spooker.Graphics.Texture"/> class.
		/// </summary>
		/// <param name="size">Size.</param>
		public Texture (Vector2 size)
		{
			_texture = new SFML.Graphics.Texture (
					(uint)size.X,
					(uint)size.Y);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Spooker.Graphics.Texture"/> class.
		/// </summary>
		/// <param name="size">Size.</param>
		/// <param name="color">Color.</param>
		public Texture (Vector2 size, Color color)
		{
			_texture = new SFML.Graphics.Texture (
				new SFML.Graphics.Image (
					(uint)size.X,
					(uint)size.Y,
					color.ToSfml()));
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Spooker.Graphics.Texture"/> class.
		/// </summary>
		/// <param name="size">Size.</param>
		/// <param name="pixels">Pixels.</param>
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

		/// <summary>
		/// Gets the pixel.
		/// </summary>
		/// <returns>The pixel.</returns>
		/// <param name="x">The x coordinate.</param>
		/// <param name="y">The y coordinate.</param>
		public Color GetPixel(int x, int y)
		{
			CreateImage();

			return new Color(_image.GetPixel((uint)x, (uint)y));
		}

		/// <summary>
		/// Sets the pixel.
		/// </summary>
		/// <param name="x">The x coordinate.</param>
		/// <param name="y">The y coordinate.</param>
		/// <param name="color">Color.</param>
		public void SetPixel(int x, int y, Color color)
		{
			CreateImage();

			_image.SetPixel((uint)x, (uint)y, color.ToSfml());
			_texture = new SFML.Graphics.Texture(_image);

			_needsUpdate = true;
		}

		/// <summary>
		/// Copies the pixels.
		/// </summary>
		/// <param name="from">From.</param>
		/// <param name="fromX">From x.</param>
		/// <param name="fromY">From y.</param>
		/// <param name="toX">To x.</param>
		/// <param name="toY">To y.</param>
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

		/// <summary>
		/// Saves to file.
		/// </summary>
		/// <param name="path">Path.</param>
		public void SaveToFile(string path)
		{
			CreateImage();

			_image.SaveToFile(path);
		}

		/// <summary>
		/// Creates the image.
		/// </summary>
		public void CreateImage()
		{
			if (_image == null)
				_image = _texture.CopyToImage();
		}

		/// <summary>
		/// Update this instance.
		/// </summary>
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
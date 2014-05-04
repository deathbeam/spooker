//-----------------------------------------------------------------------------
// LightEngine.cs
//
// Spooker Open Source Game Framework
// Copyright (C) Indie Armory. All rights reserved.
// Website: http://indiearmory.com
// Other Contributors: None
// License: MIT
//-----------------------------------------------------------------------------

using System.Collections.Generic;
using SFML.Graphics;

namespace Spooker.Graphics.Lights
{
	public class LightEngine : Drawable
	{
		private readonly List<Light> _lights;
        private readonly Camera _camera;
		private readonly RenderStates _drawStates;
		private readonly SpriteBatch _spriteBatch;
		private RenderTexture _lightTexture;
		private SFML.Graphics.Sprite _drawSprite;
		private Sprite _lightSprite;

		/// <summary>
		/// The color of the ambient.
		/// </summary>
		public Color AmbientColor = Color.Black;

		/// <summary>
		/// Initializes a new instance of the <see cref="Spooker.Graphics.Lights.LightEngine"/> class.
		/// </summary>
		/// <param name="camera">Camera.</param>
		/// <param name="texture">Texture.</param>
		public LightEngine (Camera camera, Texture texture)
		{
			_lights = new List<Light>();
			_lightTexture = new RenderTexture (1024, 1024);
			_spriteBatch = new SpriteBatch (_lightTexture);
			_drawSprite = new SFML.Graphics.Sprite ();
			_drawStates = new RenderStates (BlendMode.Multiply);
			_camera = camera;
			SetTexture (texture);
		}

		/// <summary>
		/// Add the specified light.
		/// </summary>
		/// <param name="light">Light.</param>
		public void Add (Light light)
		{
			_lights.Add (light);
		}

		/// <summary>
		/// Remove the specified light.
		/// </summary>
		/// <param name="light">Light.</param>
		public void Remove (Light light)
		{
			_lights.Remove (light);
		}

		/// <summary>
		/// Sets the texture.
		/// </summary>
		/// <param name="texture">Texture.</param>
		public void SetTexture(Texture texture)
		{
			_lightSprite = new Sprite (texture);

			_lightSprite.Origin = new Vector2 (
				_lightSprite.Size.X / 2,
				_lightSprite.Size.Y / 2);
		}

		/// <summary>
		/// Draw to the specified graphicsDevice and states.
		/// </summary>
		/// <param name="graphicsDevice">Graphics device.</param>
		/// <param name="states">States.</param>
		public void Draw(RenderTarget graphicsDevice, RenderStates states)
		{
			_lightTexture.Clear (AmbientColor.ToSfml ());
			_spriteBatch.Begin (SpriteBlendMode.Additive);
			foreach(var light in _lights)
			{
				_lightSprite.Position = light.UseCamera ? _camera.Transform.TransformPoint (light.Position) : light.Position;
				_lightSprite.Color = light.Color;
				_lightSprite.Scale = new Vector2 (light.Ratio * _camera.Scale.X, light.Ratio * _camera.Scale.Y);
				_spriteBatch.Draw (_lightSprite);
			}
			_spriteBatch.End ();
			_lightTexture.Display();

			_drawSprite.Texture = _lightTexture.Texture;
			graphicsDevice.Draw (_drawSprite, _drawStates);
		}
	}
}
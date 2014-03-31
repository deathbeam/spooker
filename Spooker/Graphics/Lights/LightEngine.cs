using System;
using System.Collections.Generic;
using SFML.Graphics;

namespace Spooker.Graphics.Lights
{
	public class LightEngine
	{
		private SFML.Graphics.Sprite _lightSprite;
		private RenderTexture _lightTexture;
		private RenderTarget _device;

		public List<Light> Lights = new List<Light>();

		public LightEngine (RenderTarget graphicsDevice, string filename)
		{
			_lightSprite = new SFML.Graphics.Sprite (
				new SFML.Graphics.Texture (filename));

			_lightSprite.Origin = new SFML.Window.Vector2f (
				_lightSprite.GetLocalBounds().Width / 2,
				_lightSprite.GetLocalBounds().Height / 2);

			_lightTexture = new RenderTexture (1024, 1024);

			_lightTexture.SetView (
				new View (new FloatRect (0f, 0f, 1024, 1024)));

			_device = graphicsDevice;
		}

		public void Draw()
		{
			_lightTexture.Clear (SFML.Graphics.Color.Black);

			foreach(var light in Lights)
			{
				_lightSprite.Position = light.Position.ToSfml();
				_lightSprite.Color = light.Color.ToSfml();
				_lightSprite.Scale = new SFML.Window.Vector2f (light.Ratio, light.Ratio);
				_lightTexture.Draw(_lightSprite, new RenderStates(BlendMode.Add));
			}

			_lightTexture.Display();
			var spr = new SFML.Graphics.Sprite (_lightTexture.Texture);
			_device.Draw (spr, new RenderStates(BlendMode.Multiply));
		}
	}
}


/* File Description
 * Original Works/Author: Thomas Slusny
 * Other Contributors: None
 * Author Website: http://indiearmory.com
 * License: MIT
*/

using System;
using SFML.Graphics;
using Tao.OpenGl;
using SFGL;
using SFGL.GameScenes;
using SFGL.Graphics;
using SFGL.Input;
using SFGL.Resources;
using SFGL.Time;
using SFGL.Audio;

namespace SFGL.Window
{
	public class GameWindow : RenderWindow, GameTarget
	{
		public SpriteBatch spriteBatch { get; set; }
		public SceneManager sceneManager { get; set; }
		public GameSettings settings { get; set; }
		public ContentManager Content { get; set; }
		public AudioManager Audio { get; set; }

		private EntityList Components = null;
		private GameTime gameTime = GameTime.Zero;
		private Color clearColor = Color.Black;

		public void Run()
		{
			while (IsOpen())
			{
				this.Clear(clearColor);
				Gl.glClear(Gl.GL_DEPTH_BUFFER_BIT | Gl.GL_COLOR_BUFFER_BIT);

				Components.Update (gameTime);
				Components.Draw (gameTime);

				this.Display();
				this.DispatchEvents();
			}
		}

		public void AddComponent(object Component)
		{
			this.Components.Add (Component);
		}

		public override void Close()
		{
			Components.Dispose ();
			base.Close ();
		}

		protected void Initialize()
		{
			//Initialize input
			CookieKeyboard keyboardComponent = new CookieKeyboard (this);
			CookieMouse mouseComponent = new CookieMouse (this);

			//Bind input events to components
			base.MouseWheelMoved += (sender, e) => { mouseComponent.window_MouseWheelMoved(this, e); };
			base.MouseWheelMoved += (sender, e) => { sceneManager.window_MouseWheelMoved(this, e); };
			base.TextEntered += (sender, e) => { sceneManager.window_TextEntered(this, e); };
			base.KeyPressed += (sender, e) => { sceneManager.window_KeyPressed(this, e); };
			base.KeyReleased += (sender, e) => { sceneManager.window_KeyReleased(this, e); };
			base.MouseMoved += (sender, e) => { sceneManager.window_MouseMoved(this, e); };
			base.MouseButtonPressed += (sender, e) => { sceneManager.window_MouseButtonPressed(this, e); };
			base.MouseButtonReleased += (sender, e) => { sceneManager.window_MouseButtonReleased(this, e); };

			//Workaround for not recognizing close events is state manager
			this.Closed += (sender, e) => { sceneManager.window_Close(); };

			//Add components to component manager
			this.Components.Add (keyboardComponent);
			this.Components.Add (mouseComponent);
			this.Components.Add (sceneManager);
			this.Components.Add (Audio);
			this.Components.Add (Content);
			this.Components.Add (spriteBatch);

			//Register important components to global holder
			SFCore.Game = this;
			SFCore.SceneManager = this.sceneManager;
			SFCore.Content = this.Content;
			SFCore.Audio = this.Audio;
			SFCore.Keys = keyboardComponent;
			SFCore.Mouse = mouseComponent;
		}

		public GameWindow (GameSettings gameSettings) : base (
			gameSettings.GetVideoMode,
			gameSettings.Title,
			gameSettings.Style, gameSettings.GetContextSettings)
		{
			//Initialize core parts of game
			this.sceneManager = new SceneManager (this);
			this.spriteBatch = new SpriteBatch (this);
			this.Components = new EntityList (this);
			this.Content = new ContentManager (this);
			this.Audio = new AudioManager (this);

			//Load rest of game settings
			base.SetVerticalSyncEnabled(gameSettings.VerticalSync);
			base.SetFramerateLimit(gameSettings.FramerateLimit);
			this.Content.Directory = gameSettings.ContentDirectory;
			this.Audio.SoundDirectory = String.Format("{0}/{1}",
				gameSettings.ContentDirectory,
				gameSettings.SoundDirectory);
			this.Audio.SoundExtension = gameSettings.SoundExtension;
			this.clearColor = gameSettings.ClearColor;
			this.gameTime = gameSettings.GameTime;
			this.settings = gameSettings;

			Initialize ();
		}
	}
}


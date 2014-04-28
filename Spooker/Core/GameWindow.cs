//-----------------------------------------------------------------------------
// GameWindow.cs
//
// Spooker Open Source Game Framework
// Copyright (C) Indie Armory. All rights reserved.
// Website: http://indiearmory.com
// Other Contributors: None
// License: MIT
//-----------------------------------------------------------------------------

using System;
using System.IO;
using Spooker.GameStates;
using Spooker.Graphics;
using Spooker.Input;
using Spooker.Content;
using Spooker.Time;
using Spooker.Audio;

namespace Spooker.Core
{
	////////////////////////////////////////////////////////////
	/// <summary>
	/// Very extended implementation of RenderWindow with many
	/// usefull components like Audio, Spritebatch, Input and so.
	/// </summary>
	////////////////////////////////////////////////////////////
	public abstract class GameWindow : IDrawable, IUpdateable, IDisposable, ILoadable
	{
		/// <summary>Manages various game content (audio, textures, fonts....).</summary>
		internal ContentManager Content;

		/// <summary>Can play various audio files.</summary>
		public AudioManager Audio;

		/// <summary>Handles user input from keyboard and mouse.</summary>
		public GameInput GameInput;

		/// <summary>Manages all present game states.</summary>
		public StateFactory StateFactory;

		/// <summary>Core rendering device what controls everything drawn to screen.</summary>
		public SFML.Graphics.RenderWindow GraphicsDevice;

		/// <summary>Manages components of this game window instance.</summary>
		protected EntityList Components;
		
		private readonly SpriteBatch _spriteBatch;
		private readonly GameTime _gameTime = GameTime.Zero;
		private readonly Color _clearColor = Color.Black;

		////////////////////////////////////////////////////////////
		/// <summary>
		/// Creates new instance of GameWindow class.
		/// </summary>
		/// <param name="gameSettings">Settings from what will game
		/// window constructs</param>
		////////////////////////////////////////////////////////////
		protected GameWindow (GameSettings gameSettings)
		{
            //Load game settings for renderwindow
			GraphicsDevice = new SFML.Graphics.RenderWindow(
				gameSettings.VideoMode,
                gameSettings.Title,
                gameSettings.Style,
                gameSettings.ContextSettings);
			GraphicsDevice.SetVerticalSyncEnabled(gameSettings.VerticalSync);
			GraphicsDevice.SetFramerateLimit(gameSettings.FramerateLimit);

            //Set icon
			if (gameSettings.Icon != "" && File.Exists(gameSettings.Icon))
            {
				var icon = new SFML.Graphics.Image(gameSettings.Icon);
                GraphicsDevice.SetIcon(icon.Size.X, icon.Size.Y, icon.Pixels);   
            }

			//Initialize core parts of game
			_spriteBatch = new SpriteBatch (GraphicsDevice);
			GameInput = new GameInput (GraphicsDevice);
			StateFactory = new StateFactory (GraphicsDevice);
			Content = new ContentManager ();
			Audio = new AudioManager ();
			Components = new EntityList ();

			//Load rest of game settings
			Content.Directory = gameSettings.ContentDirectory;
			Audio.SoundDirectory = String.Format("{0}/{1}",
				gameSettings.ContentDirectory,
				gameSettings.SoundDirectory);
			Audio.SoundExtension = gameSettings.SoundExtension;

			_clearColor = gameSettings.ClearColor;
			_gameTime = GameTime.FromMilliseconds(gameSettings.UpdaterateLimit);

			//Workaround for not closing game window correctly
			GraphicsDevice.Closed += (sender, e) => Dispose();

			LoadContent (Content);
		}

		////////////////////////////////////////////////////////////
		/// <summary>
		/// Starts main loop of game window
		/// </summary>
		////////////////////////////////////////////////////////////
		public void Run()
		{
			var frameClock = new Clock();
			var accumulator = TimeSpan.Zero;

			while (GraphicsDevice.IsOpen())
			{
				var dt = frameClock.RestartFromSpan ();

				if (dt.TotalMilliseconds > 25f)
					dt = new TimeSpan (0, 0, 0, 0, 25);

				_gameTime.ElapsedGameTime = dt;

				accumulator += dt;

				while (accumulator.Ticks >= _gameTime.Ticks)
				{
					accumulator -= new TimeSpan(_gameTime.Ticks);
					Update (_gameTime);
					_gameTime.TotalElapsedGameTime += dt;
				}

				GraphicsDevice.Clear(_clearColor.ToSfml());
				Draw (_spriteBatch);
				GraphicsDevice.Draw (_spriteBatch);
				GraphicsDevice.Display();
				GraphicsDevice.DispatchEvents();
			}
		}

		////////////////////////////////////////////////////////////
		/// <summary>
		/// Called when a state is created.
		/// </summary>
		////////////////////////////////////////////////////////////
		public virtual void LoadContent(ContentManager content)
		{
			Audio.LoadContent (content);
		}

		////////////////////////////////////////////////////////////
		/// <summary>
		/// Draws all drawable members of this GameWindow class.
		/// </summary>
		////////////////////////////////////////////////////////////
		public virtual void Draw(SpriteBatch spriteBatch, SpriteEffects effects = SpriteEffects.None)
		{
			StateFactory.Draw (spriteBatch);
			Components.Draw (spriteBatch);
		}

		////////////////////////////////////////////////////////////
		/// <summary>
		/// Updates this instance of GameWindow class.
		/// </summary>
		////////////////////////////////////////////////////////////
		public virtual void Update(GameTime gameTime)
		{
			Content.Update (gameTime);
			GameInput.Update (gameTime);
			StateFactory.Update (gameTime);
			Components.Update (gameTime);
		}

		////////////////////////////////////////////////////////////
		/// <summary>
		/// Closes game screen and disposes all components.
		/// </summary>
		////////////////////////////////////////////////////////////
		public virtual void Dispose()
		{
			Audio.Dispose ();
			Content.Dispose ();
			StateFactory.Dispose ();
			Components.Dispose ();
			GraphicsDevice.Close ();
		}
	}
}
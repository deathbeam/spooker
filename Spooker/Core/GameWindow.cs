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
		private readonly SpriteBatch _spriteBatch;
		private readonly GameTime _gameTime;
		private readonly GameSpan _timeStep;
		private readonly GameSpan _timeStepCap;
		private readonly Color _clearColor = Color.Black;
		internal ContentManager Content;

		/// <summary>Manages components of this game window instance.</summary>
		protected EntityList Components;

		/// <summary>Can play various audio files.</summary>
		public AudioManager Audio;

		/// <summary>Handles user input from keyboard and mouse.</summary>
		public GameInput GameInput;

		/// <summary>Manages all present game states.</summary>
		public StateFactory StateFactory;

		/// <summary>Core rendering device what controls everything drawn to screen.</summary>
		public SFML.Graphics.RenderWindow GraphicsDevice;

		/// <summary>
		/// Initializes a new instance of the <see cref="Spooker.Core.GameWindow"/> class.
		/// </summary>
		/// <param name="gameSettings">Game settings.</param>
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
			_gameTime = new GameTime ();
			_timeStep = GameSpan.FromMilliseconds(gameSettings.TimeStep);
			_timeStepCap = GameSpan.FromMilliseconds(gameSettings.TimeStepCap);

			//Workaround for not closing game window correctly
			GraphicsDevice.Closed += (sender, e) => Dispose();
		}
		
		/// <summary>
		/// Starts main loop of game window
		/// </summary>
		public void Run()
		{
			LoadContent (Content);

			var frameClock = new Clock();
			var accumulator = GameSpan.Zero;

			while (GraphicsDevice.IsOpen())
			{
				var dt = frameClock.Restart ();

				if (dt > _timeStepCap)
					dt = _timeStepCap;

				_gameTime.ElapsedGameTime = dt;

				accumulator += dt;

				while (accumulator >= _timeStep)
				{
					accumulator -= GameSpan.FromTicks (_timeStep.Ticks);
					Update (_gameTime);
					_gameTime.TotalGameTime += dt;
				}

				GraphicsDevice.Clear(_clearColor.ToSfml());
				Draw (_spriteBatch);
				GraphicsDevice.Display();
				GraphicsDevice.DispatchEvents();
			}
		}

		/// <summary>
		/// Component uses this for loading itself
		/// </summary>
		/// <param name="content">Content.</param>
		public virtual void LoadContent(ContentManager content)
		{
			Audio.LoadContent (content);
		}

		/// <summary>
		/// Component uses this for drawing itself
		/// </summary>
		/// <param name="spriteBatch">Sprite batch.</param>
		/// <param name="effects">Effects.</param>
		public virtual void Draw(SpriteBatch spriteBatch, SpriteEffects effects = SpriteEffects.None)
		{
			StateFactory.Draw (spriteBatch);
			Components.Draw (spriteBatch);
		}

		/// <summary>
		/// Component uses this for updating itself.
		/// </summary>
		/// <param name="gameTime">Provides snapshot of timing values.</param>
		public virtual void Update(GameTime gameTime)
		{
			Content.Update (gameTime);
			GameInput.Update (gameTime);
			StateFactory.Update (gameTime);
			Components.Update (gameTime);
		}

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		/// <filterpriority>2</filterpriority>
		/// <remarks>Call <see cref="Dispose"/> when you are finished using the <see cref="Spooker.Core.GameWindow"/>. The
		/// <see cref="Dispose"/> method leaves the <see cref="Spooker.Core.GameWindow"/> in an unusable state. After calling
		/// <see cref="Dispose"/>, you must release all references to the <see cref="Spooker.Core.GameWindow"/> so the garbage
		/// collector can reclaim the memory that the <see cref="Spooker.Core.GameWindow"/> was occupying.</remarks>
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
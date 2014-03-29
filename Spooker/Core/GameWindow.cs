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
using System.Collections.Generic;
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
	public abstract class GameWindow : IDrawable, IUpdateable, IDisposable
	{
		/// <summary>Manages various game content (audio, textures, fonts....).</summary>
		public ContentManager Content { get; set; }

		/// <summary>Can play various audio files.</summary>
		public AudioManager Audio { get; set; }

		/// <summary>Handles user input from keyboard.</summary>
		public KeyboardManager KeysInput { get; set; }

		/// <summary>Handles user input from mouse.</summary>
		public MouseManager MouseInput { get; set; }

		/// <summary>Core rendering device what controls everything drawn to screen.</summary>
		public SFML.Graphics.RenderWindow GraphicsDevice { get; set; }

		/// <summary>Manages components of this game window instance.</summary>
		protected EntityList Components { get; set; }

		/// <summary>Provides optimized drawing of sprites</summary>
		protected SpriteBatch SpriteBatch { get; set; }

		private readonly List<State> _stateStack = new List<State> ();
		private readonly GameTime _gameTime = GameTime.Zero;
		private readonly Color _clearColor = Color.Black;

		////////////////////////////////////////////////////////////
		/// <summary>
		/// Starts main loop of game window
		/// </summary>
		////////////////////////////////////////////////////////////
		public void Run()
		{
			var elapsedTime = TimeSpan.Zero;
			var frameClock = new Clock();

			while (GraphicsDevice.IsOpen())
			{
				elapsedTime += frameClock.RestartFromSpan();
				_gameTime.Update (elapsedTime);

				while (elapsedTime.Ticks >= _gameTime.Ticks)
				{
					elapsedTime -= new TimeSpan(_gameTime.Ticks);
					Update (_gameTime);
				}

				GraphicsDevice.Clear(_clearColor.ToSfml());
				SpriteBatch.Draw (this);
				GraphicsDevice.Draw (SpriteBatch);
				GraphicsDevice.Display();
				GraphicsDevice.DispatchEvents();
			}
		}

		////////////////////////////////////////////////////////////
		/// <summary>
		/// Pops all states off the stack and pushes one onto it.
		/// </summary>
		////////////////////////////////////////////////////////////
		public void SetState(State state)
		{
			foreach (var s in _stateStack)
			{
				s.Leave();
			}

			_stateStack.Clear();
			PushState(state);
		}

		////////////////////////////////////////////////////////////
		/// <summary>
		/// Pushes a state onto the state stack.
		/// </summary>
		////////////////////////////////////////////////////////////
		public void PushState(State state)
		{
			_stateStack.Add(state);
			state.Enter();
		}

		////////////////////////////////////////////////////////////
		/// <summary>
		/// Pops a state off the state stack.
		/// </summary>
		////////////////////////////////////////////////////////////
		public void PopState()
		{
			var last = _stateStack.Count - 1;
			_stateStack[last].Leave();
			_stateStack.RemoveAt(last);
        }

		////////////////////////////////////////////////////////////
		/// <summary>
		/// Disposes this instance of GameWindow class.
		/// </summary>
		////////////////////////////////////////////////////////////
		public void Dispose()
		{
			Close ();
		}


		////////////////////////////////////////////////////////////
		/// <summary>
		/// Closes game screen and disposes all components.
		/// </summary>
		////////////////////////////////////////////////////////////
		public void Close()
		{
			KeysInput.Dispose ();
			MouseInput.Dispose ();
			Audio.Dispose ();
			Content.Dispose ();
			Components.Dispose ();
			GraphicsDevice.Close ();
		}

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
			GraphicsDevice = new SFML.Graphics.RenderWindow(gameSettings.VideoMode,
                gameSettings.Title,
                gameSettings.Style,
                gameSettings.ContextSettings);
			GraphicsDevice.SetVerticalSyncEnabled(gameSettings.VerticalSync);
			GraphicsDevice.SetFramerateLimit(gameSettings.FramerateLimit);

            //Set icon
            if (gameSettings.Icon != "none")
            {
                if (File.Exists(gameSettings.Icon))
                {
					var icon = new SFML.Graphics.Image(gameSettings.Icon);
                    GraphicsDevice.SetIcon(icon.Size.X, icon.Size.Y, icon.Pixels);
                }
                    
            }

			//Initialize core parts of game
			SpriteBatch = new SpriteBatch (GraphicsDevice);
			Content = new ContentManager ();
			Audio = new AudioManager ();
			KeysInput = new KeyboardManager ();
			MouseInput = new MouseManager ();
			Components = new EntityList ();

			//Load rest of game settings
			Content.Directory = gameSettings.ContentDirectory;
			Audio.SoundDirectory = String.Format("{0}/{1}",
				gameSettings.ContentDirectory,
				gameSettings.SoundDirectory);
			Audio.SoundExtension = gameSettings.SoundExtension;
			Audio.LoadContent ();

			_clearColor = gameSettings.ClearColor;
			_gameTime = gameSettings.UpdaterateLimit;

			//Bind input events to components
			GraphicsDevice.MouseWheelMoved += (sender, e) => MouseInput.MouseWheelMoved(e);

			GraphicsDevice.MouseWheelMoved += (sender, e) =>
			{ 
				var states = _stateStack.FindAll(s => s.IsActive || s.InactiveMode.HasFlag(UpdateMode.Input));
				foreach (var state in states) state.MouseWheelMoved(e);
			};

			GraphicsDevice.TextEntered += (sender, e) =>
			{ 
				var states = _stateStack.FindAll(s => s.IsActive || s.InactiveMode.HasFlag(UpdateMode.Input));
				foreach (var state in states) state.TextEntered(e);
			};

			GraphicsDevice.KeyPressed += (sender, e) =>
			{ 
				var states = _stateStack.FindAll(s => s.IsActive || s.InactiveMode.HasFlag(UpdateMode.Input));
				foreach (var state in states) state.KeyPressed(e);
			};

			GraphicsDevice.KeyReleased += (sender, e) => 
			{ 
				var states = _stateStack.FindAll(s => s.IsActive || s.InactiveMode.HasFlag(UpdateMode.Input));
				foreach (var state in states) state.KeyReleased(e);
			};

			GraphicsDevice.MouseMoved += (sender, e) =>
			{ 
				var states = _stateStack.FindAll(s => s.IsActive || s.InactiveMode.HasFlag(UpdateMode.Input));
				foreach (var state in states) state.MouseMoved(e);
			};

			GraphicsDevice.MouseButtonPressed += (sender, e) =>
			{ 
				var states = _stateStack.FindAll(s => s.IsActive || s.InactiveMode.HasFlag(UpdateMode.Input));
				foreach (var state in states) state.MouseButtonPressed(e);
			};

			GraphicsDevice.MouseButtonReleased += (sender, e) =>
			{ 
				var states = _stateStack.FindAll(s => s.IsActive || s.InactiveMode.HasFlag(UpdateMode.Input));
				foreach (var state in states) state.MouseButtonReleased(e);
			};

			//Workaround for not closing game window correctly
			GraphicsDevice.Closed += (sender, e) => Close();
		}

        internal bool IsActive(State state)
        {
            if (state.IsOverlay)
                return _stateStack.IndexOf(state) == (_stateStack.Count - 1);

			return _stateStack.FindLast(s => !s.IsOverlay) == state;
        }

		////////////////////////////////////////////////////////////
		/// <summary>
		/// Draws all drawable members of this GameWindow class.
		/// </summary>
		////////////////////////////////////////////////////////////
		public virtual void Draw(SpriteBatch spriteBatch, SpriteEffects effects = SpriteEffects.None)
		{
			foreach (var state in _stateStack)
			{
				if (state.IsActive || state.InactiveMode.HasFlag (UpdateMode.Draw))
				{
					state.Draw (spriteBatch, effects);

					var stateUi =  state as StateUi;
					if (stateUi != null)
						stateUi.DrawGui (spriteBatch, effects);
				}
			}
		}

		////////////////////////////////////////////////////////////
		/// <summary>
		/// Updates this instance of GameWindow class.
		/// </summary>
		////////////////////////////////////////////////////////////
		public virtual void Update(GameTime gameTime)
		{
			KeysInput.Update (_gameTime);
			MouseInput.Update (_gameTime);
			Components.Update (_gameTime);

			foreach (var state in _stateStack)
			{
				if (state.IsActive || state.InactiveMode.HasFlag(UpdateMode.Update))
					state.Update (_gameTime);
			}
		}
	}
}
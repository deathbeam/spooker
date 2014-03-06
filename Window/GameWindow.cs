/* File Description
 * Original Works/Author: Thomas Slusny
 * Other Contributors: None
 * Author Website: http://indiearmory.com
 * License: MIT
*/

using System;
using System.Collections.Generic;
using SFML.Graphics;
using SFML.Window;
using SFGL;
using SFGL.GameStates;
using SFGL.Graphics;
using SFGL.Input;
using SFGL.Content;
using SFGL.Time;
using SFGL.Audio;

namespace SFGL.Window
{
	////////////////////////////////////////////////////////////
	/// <summary>
	/// Very extended implementation of RenderWindow with many
	/// usefull components like Audio, Spritebatch, Input and so.
	/// </summary>
	////////////////////////////////////////////////////////////
	public class GameWindow
	{
		internal SpriteBatch SpriteBatch { get; set; }
		internal ContentManager Content { get; set; }
		internal AudioManager Audio { get; set; }
		internal KeyboardManager KeysInput { get; set; }
		internal MouseManager MouseInput { get; set; }
		internal RenderWindow GraphicsDevice { get; set; }

		private List<State> _stateStack = new List<State> ();
		private GameTime _gameTime = GameTime.Zero;
		private Color _clearColor = Color.Black;

		////////////////////////////////////////////////////////////
		/// <summary>
		/// Starts main loop of game window
		/// </summary>
		////////////////////////////////////////////////////////////
		public void Run()
		{
			var _elapsedTime = TimeSpan.Zero;
			var _frameClock = new Clock();

			while (GraphicsDevice.IsOpen())
			{
				_elapsedTime += _frameClock.RestartFromSpan();
				_gameTime.Update (_elapsedTime);

				GraphicsDevice.Clear(_clearColor);

				while (_elapsedTime.Ticks >= _gameTime.Ticks)
				{
					_elapsedTime -= new TimeSpan(_gameTime.Ticks);
					foreach (var state in _stateStack)
					{
						if (state.IsActive || state.InactiveMode.HasFlag(UpdateMode.Update))
							state.Update (_gameTime);
					}
					KeysInput.Update (_gameTime);
					MouseInput.Update (_gameTime);
				}

				foreach (var state in _stateStack)
				{
					if (state.IsActive || state.InactiveMode.HasFlag(UpdateMode.Draw))
						GraphicsDevice.Draw(state);
				}

				GraphicsDevice.Display();
				GraphicsDevice.DispatchEvents();
			}
		}

		////////////////////////////////////////////////////////////
		/// <summary>
		/// Adds new type of content loader to content manager
		/// </summary>
		////////////////////////////////////////////////////////////
		public void AddLoader(ContentProvider Loader)
		{
			this.Content.AddLoader (Loader);
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
		/// Returns current view area of Graphics Device
		/// </summary>
		////////////////////////////////////////////////////////////
		public View GetView()
		{
			return GraphicsDevice.GetView ();
		}

		////////////////////////////////////////////////////////////
		/// <summary>
		/// Changes current view area of Graphics Device
		/// </summary>
		////////////////////////////////////////////////////////////
		public void SetView(View view)
		{
			GraphicsDevice.SetView (view);
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
			GraphicsDevice.Close ();
		}

		////////////////////////////////////////////////////////////
		/// <summary>
		/// Creates new instance of GameWindow class.
		/// </summary>
		/// <param name="gameSettings">Settings from what will game
		/// window constructs</param>
		////////////////////////////////////////////////////////////
		public GameWindow (GameSettings gameSettings)
		{
            //Load game settings for renderwindow
			GraphicsDevice = new RenderWindow(gameSettings.VideoMode,
                gameSettings.Title,
                gameSettings.Style,
                gameSettings.ContextSettings);
			GraphicsDevice.SetVerticalSyncEnabled(gameSettings.VerticalSync);
			GraphicsDevice.SetFramerateLimit(gameSettings.FramerateLimit);

			//Initialize core parts of game
			SpriteBatch = new SpriteBatch ();
			Content = new ContentManager ();
			Audio = new AudioManager ();
			KeysInput = new KeyboardManager ();
			MouseInput = new MouseManager ();

            //Load rest of game settings
			Content.Directory = gameSettings.ContentDirectory;
			Audio.SoundDirectory = String.Format("{0}/{1}",
				gameSettings.ContentDirectory,
				gameSettings.SoundDirectory);
			Audio.SoundExtension = gameSettings.SoundExtension;
			_clearColor = gameSettings.ClearColor;
			_gameTime = gameSettings.GameTime;
			Audio.LoadContent ();

			//Bind input events to components
			GraphicsDevice.MouseWheelMoved += (sender, e) => { MouseInput.MouseWheelMoved(e); };

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
			GraphicsDevice.Closed += (sender, e) => { this.Close(); };
		}

        internal bool IsActive(State state)
        {
            if (state.IsOverlay)
                return _stateStack.IndexOf(state) == (_stateStack.Count - 1);

			return _stateStack.FindLast(s => !s.IsOverlay) == state;
        }
	}
}
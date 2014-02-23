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
        internal RenderWindow Window = null;

		private List<State> _stateStack = new List<State> ();
		private EntityList _components = null;
		private GameTime _gameTime = GameTime.Zero;
		private Color _clearColor = Color.Black;
		private Clock _frameClock = new Clock();
		private GameTime _elapsedTime = GameTime.Zero;

		////////////////////////////////////////////////////////////
		/// <summary>
		/// Starts main loop of game window
		/// </summary>
		////////////////////////////////////////////////////////////
		public void Run()
		{
			while (Window.IsOpen())
			{
				_elapsedTime += _frameClock.Restart();
                Window.Clear(_clearColor);

				while (_elapsedTime >= _gameTime)
				{
					_elapsedTime -= _gameTime;
					foreach (var state in _stateStack)
					{
						if (state.IsActive || state.InactiveMode.HasFlag(UpdateMode.Update))
							state.Update (_gameTime);
					}
					_components.Update (_gameTime);
				}

				foreach (var state in _stateStack)
				{
					if (state.IsActive || state.InactiveMode.HasFlag(UpdateMode.Draw))
						state.Draw();
				}
				_components.Draw ();

				Window.Display();
                Window.DispatchEvents();
			}
		}

		////////////////////////////////////////////////////////////
		/// <summary>
		/// Adds new component to dynamic component holder
		/// </summary>
		////////////////////////////////////////////////////////////
		public void AddComponent(object Component)
		{
			this._components.Add (Component);
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
        /// Draws drawable to rendering window.
        /// </summary>
        ////////////////////////////////////////////////////////////
        public void Draw(Drawable drawable)
        {
            Window.Draw(drawable);
        }

        ////////////////////////////////////////////////////////////
        /// <summary>
        /// Draws drawable with custom render states to rendering
        /// window.
        /// </summary>
        ////////////////////////////////////////////////////////////
        public void Draw(Drawable drawable, RenderStates states)
        {
            Window.Draw(drawable, states);
        }

		////////////////////////////////////////////////////////////
		/// <summary>
		/// Closes game screen and disposes all components.
		/// </summary>
		////////////////////////////////////////////////////////////
		public void Close()
		{
			_components.Dispose ();
			Window.Close ();
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
            Window = new RenderWindow(gameSettings.GetVideoMode,
                gameSettings.Title,
                gameSettings.Style,
                gameSettings.GetContextSettings);
            Window.SetVerticalSyncEnabled(gameSettings.VerticalSync);
            Window.SetFramerateLimit(gameSettings.FramerateLimit);

			//Initialize core parts of game
			this.SpriteBatch = new SpriteBatch (this);
			this._components = new EntityList (this);
			this.Content = new ContentManager (this);
			this.Audio = new AudioManager (this);
			this.KeysInput = new KeyboardManager (this);
			this.MouseInput = new MouseManager (this);

            //Load rest of game settings
			this.Content.Directory = gameSettings.ContentDirectory;
			this.Audio.SoundDirectory = String.Format("{0}/{1}",
				gameSettings.ContentDirectory,
				gameSettings.SoundDirectory);
			this.Audio.SoundExtension = gameSettings.SoundExtension;
			this._clearColor = gameSettings.ClearColor;
			this._gameTime = gameSettings.GameTime;

			//Bind input events to components
            Window.MouseWheelMoved += (sender, e) => { MouseInput.MouseWheelMoved(e); };

            Window.MouseWheelMoved += (sender, e) =>
			{ 
				for (var i = _stateStack.Count - 1; i >= 0; i--)
					if ((_stateStack[i].IsActive || _stateStack[i].InactiveMode.HasFlag(UpdateMode.Input)))
						_stateStack[i].MouseWheelMoved(e);
			};

			Window.TextEntered += (sender, e) =>
			{ 
				for (var i = _stateStack.Count - 1; i >= 0; i--)
					if ((_stateStack[i].IsActive || _stateStack[i].InactiveMode.HasFlag(UpdateMode.Input)))
						_stateStack[i].TextEntered(e);
			};

            Window.KeyPressed += (sender, e) =>
			{ 
				for (var i = _stateStack.Count - 1; i >= 0; i--)
					if ((_stateStack[i].IsActive || _stateStack[i].InactiveMode.HasFlag(UpdateMode.Input)))
						_stateStack[i].KeyPressed(e);
			};

            Window.KeyReleased += (sender, e) => 
			{ 
				for (var i = _stateStack.Count - 1; i >= 0; i--)
					if ((_stateStack[i].IsActive || _stateStack[i].InactiveMode.HasFlag(UpdateMode.Input)))
						_stateStack[i].KeyReleased(e);
			};

            Window.MouseMoved += (sender, e) =>
			{ 
				for (var i = _stateStack.Count - 1; i >= 0; i--)
					if ((_stateStack[i].IsActive || _stateStack[i].InactiveMode.HasFlag(UpdateMode.Input)))
						_stateStack[i].MouseMoved(e);
			};

            Window.MouseButtonPressed += (sender, e) =>
			{ 
				for (var i = _stateStack.Count - 1; i >= 0; i--)
					if ((_stateStack[i].IsActive || _stateStack[i].InactiveMode.HasFlag(UpdateMode.Input)))
						_stateStack[i].MouseButtonPressed(e);
			};

            Window.MouseButtonReleased += (sender, e) =>
			{ 
				for (var i = _stateStack.Count - 1; i >= 0; i--)
					if ((_stateStack[i].IsActive || _stateStack[i].InactiveMode.HasFlag(UpdateMode.Input)))
						_stateStack[i].MouseButtonReleased(e);
			};

			///Workaround for not closing game window correctly
            Window.Closed += (sender, e) => { this.Close(); };

			//Add components to component manager
			this._components.Add (KeysInput);
			this._components.Add (MouseInput);
			this._components.Add (Audio);
			this._components.Add (Content);
			this._components.Add (SpriteBatch);
		}

        internal bool IsActive(State state)
        {
            if (state.IsOverlay)
                return _stateStack.IndexOf(state) == (_stateStack.Count - 1);

            return _stateStack.FindLast(s => !s.IsOverlay) == state;
        }
	}
}


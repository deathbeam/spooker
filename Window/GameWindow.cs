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
	public class GameWindow : RenderWindow
	{
		internal SpriteBatch SpriteBatch { get; set; }
		internal ContentManager Content { get; set; }
		internal AudioManager Audio { get; set; }
		internal KeyboardManager KeysInput { get; set; }
		internal MouseManager MouseInput { get; set; }

		private List<State> StateStack = new List<State> ();
		private EntityList Components = null;
		private GameTime GameTime = GameTime.Zero;
		private Color ClearColor = Color.Black;
		private Clock frameclock = new Clock();
		private GameTime elapsedtime = GameTime.Zero;

		/// <summary>
		/// Starts main loop of game window
		/// </summary>
		public void Run()
		{
			while (IsOpen())
			{
				elapsedtime += frameclock.Restart();
				this.Clear(ClearColor);

				while (elapsedtime >= GameTime)
				{
					elapsedtime -= GameTime;
					foreach (var state in StateStack)
					{
						if (state.IsActive || state.InactiveMode.HasFlag(UpdateMode.Update))
							state.Update (GameTime);
					}
					Components.Update (GameTime);
				}

				foreach (var state in StateStack)
				{
					if (state.IsActive || state.InactiveMode.HasFlag(UpdateMode.Draw))
						state.DrawInternal();
				}
				Components.Draw ();

				this.Display();
				this.DispatchEvents();
			}
		}

		/// <summary>
		/// Adds new component to dynamic component holder
		/// </summary>
		public void AddComponent(object Component)
		{
			this.Components.Add (Component);
		}

		/// <summary>
		/// Adds new type of content loader to content manager
		/// </summary>
		public void AddLoader(ContentProvider Loader)
		{
			this.Content.AddLoader (Loader);
		}

		/// <summary>
		/// Pops all states off the stack and pushes one onto it.
		/// </summary>
		public void SetState(State state)
		{
			foreach (var s in StateStack)
			{
				s.Leave();
			}

			StateStack.Clear();
			PushState(state);
		}

		/// <summary>
		/// Pushes a state onto the state stack.
		/// </summary>
		public void PushState(State state)
		{
			StateStack.Add(state);
			state.Enter();
		}

		/// <summary>
		/// Pops a state off the state stack.
		/// </summary>
		public void PopState()
		{
			var last = StateStack.Count - 1;
			StateStack[last].Leave();
			StateStack.RemoveAt(last);
		}

		/// <summary>
		/// Closes game screen and disposes all components.
		/// </summary>
		public override void Close()
		{
			Components.Dispose ();
			this.Close ();
		}

		internal bool IsActive(State state)
		{
			if (state.IsOverlay)
				return StateStack.IndexOf(state) == (StateStack.Count - 1);

			return StateStack.FindLast(s => !s.IsOverlay) == state;
		}
		
		public GameWindow (GameSettings gameSettings) : base (
			gameSettings.GetVideoMode,
			gameSettings.Title,
			gameSettings.Style, gameSettings.GetContextSettings)
		{
			//Initialize core parts of game
			this.SpriteBatch = new SpriteBatch (this);
			this.Components = new EntityList (this);
			this.Content = new ContentManager (this);
			this.Audio = new AudioManager (this);
			this.KeysInput = new KeyboardManager (this);
			this.MouseInput = new MouseManager (this);

			//Load rest of game settings
			this.SetVerticalSyncEnabled(gameSettings.VerticalSync);
			this.SetFramerateLimit(gameSettings.FramerateLimit);
			this.Content.Directory = gameSettings.ContentDirectory;
			this.Audio.SoundDirectory = String.Format("{0}/{1}",
				gameSettings.ContentDirectory,
				gameSettings.SoundDirectory);
			this.Audio.SoundExtension = gameSettings.SoundExtension;
			this.ClearColor = gameSettings.ClearColor;
			this.GameTime = gameSettings.GameTime;

			//Bind input events to components
			this.MouseWheelMoved += (sender, e) => { MouseInput.window_MouseWheelMoved(e); };

			this.MouseWheelMoved += (sender, e) =>
			{ 
				for (var i = StateStack.Count - 1; i >= 0; i--)
					if ((StateStack[i].IsActive || StateStack[i].InactiveMode.HasFlag(UpdateMode.Input)))
						StateStack[i].window_MouseWheelMoved(e);
			};

			this.TextEntered += (sender, e) =>
			{ 
				for (var i = StateStack.Count - 1; i >= 0; i--)
					if ((StateStack[i].IsActive || StateStack[i].InactiveMode.HasFlag(UpdateMode.Input)))
						StateStack[i].window_TextEntered(e);
			};

			this.KeyPressed += (sender, e) =>
			{ 
				for (var i = StateStack.Count - 1; i >= 0; i--)
					if ((StateStack[i].IsActive || StateStack[i].InactiveMode.HasFlag(UpdateMode.Input)))
						StateStack[i].window_KeyPressed(e);
			};

			this.KeyReleased += (sender, e) => 
			{ 
				for (var i = StateStack.Count - 1; i >= 0; i--)
					if ((StateStack[i].IsActive || StateStack[i].InactiveMode.HasFlag(UpdateMode.Input)))
						StateStack[i].window_KeyReleased(e);
			};

			this.MouseMoved += (sender, e) =>
			{ 
				for (var i = StateStack.Count - 1; i >= 0; i--)
					if ((StateStack[i].IsActive || StateStack[i].InactiveMode.HasFlag(UpdateMode.Input)))
						StateStack[i].window_MouseMoved(e);
			};

			this.MouseButtonPressed += (sender, e) =>
			{ 
				for (var i = StateStack.Count - 1; i >= 0; i--)
					if ((StateStack[i].IsActive || StateStack[i].InactiveMode.HasFlag(UpdateMode.Input)))
						StateStack[i].window_MouseButtonPressed(e);
			};

			this.MouseButtonReleased += (sender, e) =>
			{ 
				for (var i = StateStack.Count - 1; i >= 0; i--)
					if ((StateStack[i].IsActive || StateStack[i].InactiveMode.HasFlag(UpdateMode.Input)))
						StateStack[i].window_MouseButtonReleased(e);
			};

			///Workaround for not closing game window correctly
			this.Closed += (sender, e) => { this.Close(); };

			//Add components to component manager
			this.Components.Add (KeysInput);
			this.Components.Add (MouseInput);
			this.Components.Add (Audio);
			this.Components.Add (Content);
			this.Components.Add (SpriteBatch);
		}
	}
}


using System;
using System.Collections.Generic;
using System.Linq;
using Spooker.Graphics;
using Spooker.Time;

namespace Spooker.GameStates
{
	public class StateFactory : IDrawable, IUpdateable, IDisposable
	{
		private readonly List<State> _stateStack = new List<State> ();

		public StateFactory (SFML.Graphics.RenderWindow graphicsDevice)
		{
			//Bind input events to components
			graphicsDevice.MouseWheelMoved += (sender, e) =>
			{ 
				var states = _stateStack.FindAll(s => s.IsActive || s.InactiveMode.HasFlag(UpdateMode.Input));
				foreach (var state in states) state.MouseWheelMoved(e);
			};

			graphicsDevice.TextEntered += (sender, e) =>
			{ 
				var states = _stateStack.FindAll(s => s.IsActive || s.InactiveMode.HasFlag(UpdateMode.Input));
				foreach (var state in states) state.TextEntered(e);
			};

			graphicsDevice.KeyPressed += (sender, e) =>
			{ 
				var states = _stateStack.FindAll(s => s.IsActive || s.InactiveMode.HasFlag(UpdateMode.Input));
				foreach (var state in states) state.KeyPressed(e);
			};

			graphicsDevice.KeyReleased += (sender, e) => 
			{ 
				var states = _stateStack.FindAll(s => s.IsActive || s.InactiveMode.HasFlag(UpdateMode.Input));
				foreach (var state in states) state.KeyReleased(e);
			};

			graphicsDevice.MouseMoved += (sender, e) =>
			{ 
				var states = _stateStack.FindAll(s => s.IsActive || s.InactiveMode.HasFlag(UpdateMode.Input));
				foreach (var state in states) state.MouseMoved(e);
			};

			graphicsDevice.MouseButtonPressed += (sender, e) =>
			{ 
				var states = _stateStack.FindAll(s => s.IsActive || s.InactiveMode.HasFlag(UpdateMode.Input));
				foreach (var state in states) state.MouseButtonPressed(e);
			};

			graphicsDevice.MouseButtonReleased += (sender, e) =>
			{ 
				var states = _stateStack.FindAll(s => s.IsActive || s.InactiveMode.HasFlag(UpdateMode.Input));
				foreach (var state in states) state.MouseButtonReleased(e);
			};
		}

		public void Draw(SpriteBatch spriteBatch, SpriteEffects effects = SpriteEffects.None)
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

		public void Update(GameTime gameTime)
		{
			foreach (var state in _stateStack)
			{
				if (state.IsActive || state.InactiveMode.HasFlag(UpdateMode.Update))
					state.Update (gameTime);
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

		public bool IsActive(State state)
		{
			if (state.IsOverlay)
				return _stateStack.IndexOf(state) == (_stateStack.Count - 1);

			return _stateStack.FindLast(s => !s.IsOverlay) == state;
		}

		public void Dispose()
		{
			foreach (var state in _stateStack)
				state.Leave ();
			_stateStack.Clear ();
		}
	}
}


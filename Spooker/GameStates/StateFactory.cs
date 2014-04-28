//-----------------------------------------------------------------------------
// StateFactory.cs
//
// Spooker Open Source Game Framework
// Copyright (C) Indie Armory. All rights reserved.
// Website: http://indiearmory.com
// Other Contributors: None
// License: MIT
//-----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using Spooker.Graphics;
using Spooker.Time;

namespace Spooker.GameStates
{
	public class StateFactory : IDrawable, IUpdateable, IDisposable
	{
		private readonly List<State> _stateStack = new List<State> ();

		////////////////////////////////////////////////////////////
		/// <summary>
		/// Creates new instance of StateFactory class.
		/// </summary>
		////////////////////////////////////////////////////////////
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

		////////////////////////////////////////////////////////////
		/// <summary>
		/// Draws this instance of StateFactory class.
		/// </summary>
		////////////////////////////////////////////////////////////
		public void Draw(SpriteBatch spriteBatch, SpriteEffects effects = SpriteEffects.None)
		{
			foreach (var state in _stateStack)
			{
				if (state.IsActive || state.InactiveMode.HasFlag (UpdateMode.Draw))
				{
					state.Draw (spriteBatch, effects);

					var stateGUI =  state as StateGUI;
					if (stateGUI != null)
						stateGUI.DrawGUI (spriteBatch, effects);
				}
			}
		}

		////////////////////////////////////////////////////////////
		/// <summary>
		/// Updates this instance of StateFactory class.
		/// </summary>
		////////////////////////////////////////////////////////////
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

		////////////////////////////////////////////////////////////
		/// <summary>
		/// Checks if specified state is active.
		/// </summary>
		////////////////////////////////////////////////////////////
		public bool IsActive(State state)
		{
			if (state.IsOverlay)
				return _stateStack.IndexOf(state) == (_stateStack.Count - 1);

			return _stateStack.FindLast(s => !s.IsOverlay) == state;
		}

		////////////////////////////////////////////////////////////
		/// <summary>
		/// Disposes this instance of StateFactory class.
		/// </summary>
		////////////////////////////////////////////////////////////
		public void Dispose()
		{
			foreach (var state in _stateStack)
				state.Leave ();
			_stateStack.Clear ();
		}
	}
}


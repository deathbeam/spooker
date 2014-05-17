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
	/// <summary>
	/// State factory.
	/// </summary>
	public class StateFactory : IDrawable, IUpdateable, IDisposable
	{
		private readonly List<State> _stateStack = new List<State> ();

		/// <summary>
		/// Initializes a new instance of the <see cref="Spooker.GameStates.StateFactory"/> class.
		/// </summary>
		/// <param name="graphicsDevice">Graphics device.</param>
		public StateFactory (SFML.Graphics.RenderWindow graphicsDevice)
		{
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

		/// <summary>
		/// Component uses this for drawing itself
		/// </summary>
		/// <param name="spriteBatch">Sprite batch.</param>
		/// <param name="effects">Effects.</param>
		public void Draw(SpriteBatch spriteBatch, SpriteEffects effects = SpriteEffects.None)
		{
			foreach (var state in _stateStack)
			{
				if (state.IsActive || state.InactiveMode.HasFlag (UpdateMode.Draw))
				{
					state.Draw (spriteBatch, effects);

					if (state is StateGUI)
					{
						var stateGUI = state as StateGUI;
						stateGUI.DrawGUI ();
					}
				}
			}
		}

		/// <summary>
		/// Component uses this for updating itself.
		/// </summary>
		/// <param name="gameTime">Provides snapshot of timing values.</param>
		public void Update(GameTime gameTime)
		{
			foreach (var state in _stateStack)
				if (state.IsActive || state.InactiveMode.HasFlag(UpdateMode.Update))
					state.Update (gameTime);
		}

		/// <summary>
		/// Pops all states off the stack and pushes one onto it.
		/// </summary>
		/// <param name="state">State.</param>
		public void SetState(State state)
		{
			foreach (var s in _stateStack)
				s.Leave();

			_stateStack.Clear();
			PushState(state);
		}

		/// <summary>
		/// Pushs the state on the state stack.
		/// </summary>
		/// <param name="state">State.</param>
		public void PushState(State state)
		{
			_stateStack.Add(state);
			state.Enter();
		}

		/// <summary>
		/// Pops the last state off the state stack.
		/// </summary>
		public void PopState()
		{
			var last = _stateStack.Count - 1;
			_stateStack[last].Leave();
			_stateStack.RemoveAt(last);
		}

		/// <summary>
		/// Pops the specified state off the state stack.
		/// </summary>
		public void PopState(State state)
		{
			state.Leave ();
			_stateStack.Remove (state);
		}

		/// <summary>
		/// Determines whether the specified state is active.
		/// </summary>
		/// <returns><c>true</c> if the specified state is active; otherwise, <c>false</c>.</returns>
		/// <param name="state">State.</param>
		public bool IsActive(State state)
		{
			if (state.IsOverlay)
				return _stateStack.IndexOf(state) == (_stateStack.Count - 1);

			return _stateStack.FindLast(s => !s.IsOverlay) == state;
		}

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		/// <filterpriority>2</filterpriority>
		/// <remarks>Call <see cref="Dispose"/> when you are finished using the <see cref="Spooker.GameStates.StateFactory"/>. The
		/// <see cref="Dispose"/> method leaves the <see cref="Spooker.GameStates.StateFactory"/> in an unusable state. After
		/// calling <see cref="Dispose"/>, you must release all references to the
		/// <see cref="Spooker.GameStates.StateFactory"/> so the garbage collector can reclaim the memory that the
		/// <see cref="Spooker.GameStates.StateFactory"/> was occupying.</remarks>
		public void Dispose()
		{
			foreach (var state in _stateStack)
				state.Leave ();

			_stateStack.Clear ();
		}
	}
}


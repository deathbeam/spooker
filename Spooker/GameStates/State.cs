//-----------------------------------------------------------------------------
// State.cs
//
// Spooker Open Source Game Framework
// Copyright (C) Indie Armory. All rights reserved.
// Website: http://indiearmory.com
// Other Contributors: None
// License: MIT
//-----------------------------------------------------------------------------

using SFML.Window;
using Spooker.Graphics;
using Spooker.Core;
using Spooker.Time;

namespace Spooker.GameStates
{
	////////////////////////////////////////////////////////////
	/// <summary>
	/// Abstract class used for handling game input, drawing and
	/// updating for one scene.
	/// </summary>
	////////////////////////////////////////////////////////////
	public abstract class State : GameComponent, IDrawable, IUpdateable
	{
		#region Properties
		////////////////////////////////////////////////////////////
		/// <summary>
		/// Functions to call for this state when it is not the
		/// active state.
		/// </summary>
		////////////////////////////////////////////////////////////
		public UpdateMode InactiveMode { get; protected set; }

		////////////////////////////////////////////////////////////
		/// <summary>
		/// Manages components of this game state.
		/// </summary>
		////////////////////////////////////////////////////////////
		public EntityList Components { get; set; }

		////////////////////////////////////////////////////////////
		/// <summary>
		/// Returns true if this State is at the top of the State stack.
		/// </summary>
		////////////////////////////////////////////////////////////
		public bool IsActive
		{
			get { return Game.StateFactory.IsActive(this); }
		}

		////////////////////////////////////////////////////////////
		/// <summary>
		/// Overlay states are active when on top of the stack.
		/// The following non-overlay state will also be active.
		/// </summary>
		////////////////////////////////////////////////////////////
		public bool IsOverlay { get; protected set; }
		#endregion

		#region Constructors and Destructors
		////////////////////////////////////////////////////////////
		/// <summary>
		/// Creates new instance of game state.
		/// </summary>
		/// ////////////////////////////////////////////////////////////
		protected State(GameWindow game) : base(game)
		{
			InactiveMode = UpdateMode.All;
			IsOverlay = false;
			Components = new EntityList ();
		}
		#endregion

		#region Input bindings
		////////////////////////////////////////////////////////////
		/// <summary>
		/// Called when user tries to enter text.
		/// </summary>
		////////////////////////////////////////////////////////////
		public virtual void TextEntered(TextEventArgs e) { }

		////////////////////////////////////////////////////////////
		/// <summary>
		/// Called when user tries to move with mouse wheel.
		/// </summary>
		////////////////////////////////////////////////////////////
		public virtual void MouseWheelMoved(MouseWheelEventArgs e) { }

		////////////////////////////////////////////////////////////
		/// <summary>
		/// Called when user tries to move with mouse.
		/// </summary>
		////////////////////////////////////////////////////////////
		public virtual void MouseMoved(MouseMoveEventArgs e) { }

		////////////////////////////////////////////////////////////
		/// <summary>
		/// Called when user presses mouse button.
		/// </summary>
		////////////////////////////////////////////////////////////
		public virtual void MouseButtonPressed(MouseButtonEventArgs e) { }

		////////////////////////////////////////////////////////////
		/// <summary>
		/// Called when user releases mouse button.
		/// </summary>
		////////////////////////////////////////////////////////////
		public virtual void MouseButtonReleased(MouseButtonEventArgs e) { }

		////////////////////////////////////////////////////////////
		/// <summary>
		/// Called when user presses keyboard key.
		/// </summary>
		////////////////////////////////////////////////////////////
		public virtual void KeyPressed(KeyEventArgs e) { }

		////////////////////////////////////////////////////////////
		/// <summary>
		/// Called when user releases keyboard key.
		/// </summary>
		////////////////////////////////////////////////////////////
		public virtual void KeyReleased(KeyEventArgs e) { }
		#endregion

		#region Functions
		////////////////////////////////////////////////////////////
		/// <summary>
		/// Called when a state is added to game (pushed to stack).
		/// </summary>
		////////////////////////////////////////////////////////////
		public virtual void Enter() { }

		////////////////////////////////////////////////////////////
		/// <summary>
		/// Called when a state is removed from game (popped from stack).
		/// </summary>
		////////////////////////////////////////////////////////////
		public virtual void Leave()
		{
			Components.Dispose ();
		}

		////////////////////////////////////////////////////////////
		/// <summary>
		/// Update is called once every time step. Game logic should
		/// be handled here (input, movement...)
		/// </summary>
		////////////////////////////////////////////////////////////
		public virtual void Update(GameTime gameTime)
		{
			Components.Update(gameTime);
		}

		////////////////////////////////////////////////////////////
		/// <summary>
		/// Called once per frame. Avoid putting game logic in here.
		/// </summary>
		////////////////////////////////////////////////////////////
		public virtual void Draw(SpriteBatch spriteBatch, SpriteEffects effects = SpriteEffects.None)
		{
			Components.Draw (spriteBatch, effects);
		}
		#endregion
	}
}
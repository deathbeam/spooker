/* File Description
 * Original Works/Author: Thomas Slusny
 * Other Contributors: None
 * Author Website: http://indiearmory.com
 * License: MIT
*/

using System;
using Gwen.Control;
using SFML.Graphics;
using SFML.Window;
using SFGL.Window;
using SFGL.Graphics;
using SFGL.Time;

namespace SFGL.GameStates
{
	/// <summary>
	/// Abstract class used for handling game input, drawing and updating for one scene.
	/// </summary>
	public abstract class State : GameComponent, IDrawable, IUpdateable
	{
		#region Properties
		/// <summary>
		/// Functions to call for this state when it is not the active state.
		/// </summary>
		public UpdateMode InactiveMode { get; protected set; }

		/// <summary>
		/// Returns true if this State is at the top of the State stack.
		/// </summary>
		public bool IsActive
		{
			get { return Game.IsActive(this); }
		}

		/// <summary>
		/// Overlay states are active when on top of the stack. The following non-overlay state will also be active.
		/// </summary>
		public bool IsOverlay { get; protected set; }
		#endregion

		#region Constructors and Destructors
		/// <summary>
		/// Creates new instance of game state.
		/// </summary>
		public State(GameWindow game) : base(game) 
		{
			InactiveMode = UpdateMode.All;
			IsOverlay = false;
		}
		#endregion

		#region Input bindings
		internal virtual void window_TextEntered(TextEventArgs e) { }
		internal virtual void window_MouseWheelMoved(MouseWheelEventArgs e) { }
		internal virtual void window_MouseMoved(MouseMoveEventArgs e) { }
		internal virtual void window_MouseButtonPressed(MouseButtonEventArgs e) { }
		internal virtual void window_MouseButtonReleased(MouseButtonEventArgs e) { }
		internal virtual void window_KeyPressed(KeyEventArgs e) { }
		internal virtual void window_KeyReleased(KeyEventArgs e) { }
		#endregion

		#region Functions
		/// <summary>
		/// Called when a state is added to game (pushed to stack).
		/// </summary>
		public virtual void Enter() { }

		/// <summary>
		/// Called when a state is removed from game (popped from stack).
		/// </summary>
		public virtual void Leave() { }

		/// <summary>
		/// Update is called once every time step. 
		/// Game logic should be handled here (input, movement...)
		/// </summary>
		public virtual void Update(GameTime gameTime) { }

		/// <summary>
		/// Called once per frame. Avoid putting game logic in here.
		/// </summary>
		public virtual void Draw() { }

		internal virtual void DrawInternal()
		{
			Draw ();
		}
		#endregion
	}
}
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
using Spooker.Content;

namespace Spooker.GameStates
{
	////////////////////////////////////////////////////////////
	/// <summary>
	/// Abstract class used for handling game input, drawing and
	/// updating for one scene.
	/// </summary>
	////////////////////////////////////////////////////////////
	public abstract class State : GameComponent, IDrawable, IUpdateable, ILoadable
	{
		#region Properties
		/// <summary>
		/// Gets or sets the functions to call for this state when it is not the
		/// active state.
		/// </summary>
		/// <value>The inactive mode.</value>
		public UpdateMode InactiveMode { get; protected set; }

		/// <summary>
		/// Gets or sets the components of this game state.
		/// </summary>
		/// <value>The components.</value>
		public EntityList Components { get; set; }

		/// <summary>
		/// Gets a value indicating whether this instance is active.
		/// </summary>
		/// <value><c>true</c> if this instance is active; otherwise, <c>false</c>.</value>
		public bool IsActive
		{
			get { return Game.StateFactory.IsActive(this); }
		}

		/// <summary>
		/// Gets or sets a value indicating whether this instance is overlay.
		/// </summary>
		/// <value><c>true</c> if this instance is overlay; otherwise, <c>false</c>.</value>
		public bool IsOverlay { get; protected set; }
		#endregion

		#region Constructors and Destructors
		/// <summary>
		/// Initializes a new instance of the <see cref="Spooker.GameStates.State"/> class.
		/// </summary>
		/// <param name="game">Game.</param>
		protected State(GameWindow game) : base(game)
		{
			InactiveMode = UpdateMode.All;
			IsOverlay = false;
			Components = new EntityList ();
		}
		#endregion

		#region Input bindings
		/// <summary>
		/// Called when user tries to enter text.
		/// </summary>
		/// <param name="e">Text event arguments.</param>
		public virtual void TextEntered(TextEventArgs e) { }

		/// <summary>
		/// Called when user tries to move with mouse wheel.
		/// </summary>
		/// <param name="e">Mouse wheel event arguments.</param>
		public virtual void MouseWheelMoved(MouseWheelEventArgs e) { }

		/// <summary>
		/// Called when user tries to move with mouse.
		/// </summary>
		/// <param name="e">Mouse move event arguments.</param>
		public virtual void MouseMoved(MouseMoveEventArgs e) { }

		/// <summary>
		/// Called when user presses mouse button.
		/// </summary>
		/// <param name="e">Mouse button event arguments.</param>
		public virtual void MouseButtonPressed(MouseButtonEventArgs e) { }
		
		/// <summary>
		/// Called when user releases mouse button.
		/// </summary>
		/// <param name="e">Mouse button event arguments.</param>
		public virtual void MouseButtonReleased(MouseButtonEventArgs e) { }

		/// <summary>
		/// Called when user presses keyboard key.
		/// </summary>
		/// <param name="e">Key event arguments.</param>
		public virtual void KeyPressed(KeyEventArgs e) { }

		/// <summary>
		/// Called when user releases keyboard key.
		/// </summary>
		/// <param name="e">Key event arguments.</param>
		public virtual void KeyReleased(KeyEventArgs e) { }
		#endregion

		#region Functions

		/// <summary>
		/// Called when a state is added to game (pushed to stack).
		/// </summary>
		public virtual void Enter() { }

		/// <summary>
		/// Tells the screen to go away. Unlike StateFactory.Remove, which
		/// instantly kills the screen, this method also unloads all objects.
		/// </summary>
		public virtual void Leave()
		{
			Components.Dispose ();
		}

		/// <summary>
		/// Component uses this for loading itself
		/// </summary>
		/// <param name="content">Content.</param>
		public virtual void LoadContent(ContentManager content)
		{
			Components.LoadContent (content);
		}

		/// <summary>
		/// Component uses this for updating itself. Update is called once every time step. Game logic should
		/// be handled here (input, movement...)
		/// </summary>
		/// <param name="gameTime">Provides snapshot of timing values.</param>
		public virtual void Update(GameTime gameTime)
		{
			Components.Update(gameTime);
		}

		/// <summary>
		/// Component uses this for drawing itself. Avoid putting game logic in here.
		/// </summary>
		/// <param name="spriteBatch">Sprite batch.</param>
		/// <param name="effects">Effects.</param>
		public virtual void Draw(SpriteBatch spriteBatch, SpriteEffects effects = SpriteEffects.None)
		{
			Components.Draw (spriteBatch, effects);
			Components.Draw (GraphicsDevice, SFML.Graphics.RenderStates.Default);
		}
		#endregion
	}
}
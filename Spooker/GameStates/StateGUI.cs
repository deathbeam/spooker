//-----------------------------------------------------------------------------
// StateUI.cs
//
// Spooker Open Source Game Framework
// Copyright (C) Indie Armory. All rights reserved.
// Website: http://indiearmory.com
// Other Contributors: None
// License: MIT
//-----------------------------------------------------------------------------

using Gwen.Control;
using Gwen.Input;
using Gwen.Skin;
using Gwen.Renderer;
using SFML.Window;
using Spooker.Core;

namespace Spooker.GameStates
{
	////////////////////////////////////////////////////////////
	/// <summary>
	/// Abstract class used for handling game input, drawing
	/// and updating for one scene with additional GUI component.
	/// </summary>
	////////////////////////////////////////////////////////////
	public abstract class StateGUI : State
	{
		#region Variables
		private readonly Canvas _gamegui;
		private readonly GuiInput _ginput;
		private bool _isLoaded = false;
		#endregion

		#region Constructors and Destructors
		/// <summary>
		/// Initializes a new instance of the <see cref="Spooker.GameStates.StateGUI"/> class.
		/// </summary>
		/// <param name="game">Game.</param>
		/// <param name="guiImagePath">GUI image path.</param>
		/// <param name="fontName">Font name.</param>
		/// <param name="fontSize">Font size.</param>
		protected StateGUI(GameWindow game, string guiImagePath, string fontName, int fontSize): base(game)
		{
			// create GWEN renderer
			var renderer = new GuiRenderer(GraphicsDevice);

			// Create GWEN skin
			var skin = new TexturedBase(renderer, guiImagePath);

			// set default font
			skin.SetDefaultFont(fontName, fontSize);

			// Create a Canvas (it's root, on which all other GWEN controls are created)
			_gamegui = new Canvas(skin);
			_gamegui.SetSize(
				(int)GraphicsDevice.Size.X,
				(int)GraphicsDevice.Size.Y);
			_gamegui.ShouldDrawBackground = false;
			_gamegui.KeyboardInputEnabled = true;

			// Create GWEN input processor
			_ginput = new GuiInput(GraphicsDevice, _gamegui);

			// Load our GUI
			LoadGUI (_gamegui);
		}
		#endregion

		#region Input bindings
		/// <summary>
		/// Called when user tries to enter text.
		/// </summary>
		/// <param name="e">Text event arguments.</param>
		public override void TextEntered(TextEventArgs e)
		{
			_ginput.ProcessMessage(e);
		}

		/// <summary>
		/// Called when user tries to move with mouse wheel.
		/// </summary>
		/// <param name="e">Mouse wheel event arguments.</param>
		public override void MouseWheelMoved(MouseWheelEventArgs e)
		{
			_ginput.ProcessMessage(e);
		}

		/// <summary>
		/// Called when user tries to move with mouse.
		/// </summary>
		/// <param name="e">Mouse move event arguments.</param>
		public override void MouseMoved(MouseMoveEventArgs e)
		{
			_ginput.ProcessMessage(e);
		}

		/// <summary>
		/// Called when user presses mouse button.
		/// </summary>
		/// <param name="e">Mouse button event arguments.</param>
		public override void MouseButtonPressed(MouseButtonEventArgs e)
		{
			_ginput.ProcessMessage(new SfmlMouseButtonEventArgs(e, true));
		}

		/// <summary>
		/// Called when user releases mouse button.
		/// </summary>
		/// <param name="e">Mouse button event arguments.</param>
		public override void MouseButtonReleased(MouseButtonEventArgs e)
		{
			_ginput.ProcessMessage(new SfmlMouseButtonEventArgs(e, false));
		}

		/// <summary>
		/// Called when user presses keyboard key.
		/// </summary>
		/// <param name="e">Key event arguments.</param>
		public override void KeyPressed(KeyEventArgs e)
		{
			_ginput.ProcessMessage(new SfmlKeyEventArgs(e, true));
		}

		/// <summary>
		/// Called when user releases keyboard key.
		/// </summary>
		/// <param name="e">Key event arguments.</param>
		public override void KeyReleased(KeyEventArgs e)
		{
			_ginput.ProcessMessage(new SfmlKeyEventArgs(e, false));
		}

		/// <summary>
		/// Called when a state is added to game (pushed to stack).
		/// </summary>
		public override void Enter()
		{
			base.Enter ();

			if (!_isLoaded)
			{
				LoadGUI (_gamegui);
				_isLoaded = true;
			}
		}


		/// <summary>
		/// Loads the GUI of this game state.
		/// </summary>
		/// <param name="gameGUI">Game GUI.</param>
		public virtual void LoadGUI(Canvas gameGUI) { }

		#endregion

		#region Functions
		internal void DrawGUI()
		{
			_gamegui.RenderCanvas ();
		}
		#endregion
	}
}
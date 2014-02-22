/* File Description
 * Original Works/Author: Thomas Slusny
 * Other Contributors: None
 * Author Website: http://indiearmory.com
 * License: MIT
*/

using System;
using Gwen.Control;
using Gwen.Input;
using SFML.Graphics;
using SFML.Window;
using SFGL.Window;
using SFGL.Graphics;
using SFGL.Time;

namespace SFGL.GameStates
{
	/// <summary>
	/// Abstract class used for handling game input, drawing and updating for one scene with additional GUI component.
	/// </summary>
	public abstract class StateUI : State
	{
		#region Variables
		private Canvas _gamegui = null;
		private Gwen.Input.SFML _ginput = null;
		#endregion

		#region Properties
		/// <summary>
		/// Returns current instance of GUI component intialized for this state
		/// </summary>
		protected Canvas GUI
		{ 
			get { return _gamegui; }
		}
		#endregion

		#region Constructors and Destructors
		/// <summary>
		/// Creates new instance of game state with GUI component.
		/// </summary>
		public StateUI(GameWindow game, string GuiImagePath, string FontName, int FontSize) : base(game)
		{
			// create GWEN renderer
			Gwen.Renderer.SFML _renderer = new Gwen.Renderer.SFML(game.Window);

			// Create GWEN skin
			Gwen.Skin.TexturedBase _skin = new Gwen.Skin.TexturedBase(
				_renderer, GuiImagePath);

			// set default font
			_skin.SetDefaultFont(FontName, FontSize);

			// Create a Canvas (it's root, on which all other GWEN controls are created)
			_gamegui = new Canvas(_skin);
            _gamegui.SetSize((int)game.Window.Size.X, (int)game.Window.Size.Y);
			_gamegui.ShouldDrawBackground = false;
			_gamegui.KeyboardInputEnabled = true;

			// Create GWEN input processor
			_ginput = new Gwen.Input.SFML();
			_ginput.Initialize(_gamegui, game.Window);
		}
		#endregion

		#region Input bindings
		public override void TextEntered(TextEventArgs e)
		{
			_ginput.ProcessMessage(e);
		}

		public override void MouseWheelMoved(MouseWheelEventArgs e)
		{
			_ginput.ProcessMessage(e);
		}

		public override void MouseMoved(MouseMoveEventArgs e)
		{
			_ginput.ProcessMessage(e);
		}

		public override void MouseButtonPressed(MouseButtonEventArgs e)
		{
			_ginput.ProcessMessage(new SFMLMouseButtonEventArgs(e, true));
		}

		public override void MouseButtonReleased(MouseButtonEventArgs e)
		{
			_ginput.ProcessMessage(new SFMLMouseButtonEventArgs(e, false));
		}

		public override void KeyPressed(KeyEventArgs e)
		{
			_ginput.ProcessMessage(new SFMLKeyEventArgs(e, true));
		}

		public override void KeyReleased(KeyEventArgs e)
		{
			_ginput.ProcessMessage(new SFMLKeyEventArgs(e, false));
		}
		#endregion

		#region Functions
		//General screen functions
		public override void Draw()
		{
			_gamegui.RenderCanvas ();
		}
		#endregion
	}
}
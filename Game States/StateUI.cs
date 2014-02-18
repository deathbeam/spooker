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
	public abstract class StateUI : State
	{
		#region Variables
		private Canvas _gamegui = null;
		private Gwen.Input.SFML _ginput = null;
		#endregion

		#region Properties
		protected Canvas GUI
		{ 
			get { return _gamegui; }
		}
		#endregion

		#region Constructors and Destructors
		public StateUI(GameWindow game, string GuiImagePath) : base(game)
		{
			// create GWEN renderer
			Gwen.Renderer.SFML _renderer = new Gwen.Renderer.SFML(game);

			// Create GWEN skin
			Gwen.Skin.TexturedBase _skin = new Gwen.Skin.TexturedBase(
				_renderer, GuiImagePath);

			// set default font
			Gwen.Font _font = new Gwen.Font(_renderer) { Size = 15, FaceName = "Verdana" };
			_skin.SetDefaultFont("Verdana", 15);
			_font.Dispose ();

			// Create a Canvas (it's root, on which all other GWEN controls are created)
			_gamegui = new Canvas(_skin);
			_gamegui.SetSize((int)game.Size.X, (int)game.Size.Y);
			_gamegui.ShouldDrawBackground = false;
			_gamegui.KeyboardInputEnabled = true;

			// Create GWEN input processor
			_ginput = new Gwen.Input.SFML();
			_ginput.Initialize(_gamegui, Game);
		}
		#endregion

		#region Input bindings
		internal override void window_TextEntered(TextEventArgs e)
		{
			_ginput.ProcessMessage(e);
		}

		internal override void window_MouseWheelMoved(MouseWheelEventArgs e)
		{
			_ginput.ProcessMessage(e);
		}

		internal override void window_MouseMoved(MouseMoveEventArgs e)
		{
			_ginput.ProcessMessage(e);
		}

		internal override void window_MouseButtonPressed(MouseButtonEventArgs e)
		{
			_ginput.ProcessMessage(new Gwen.Input.SFMLMouseButtonEventArgs(e, true));
		}

		internal override void window_MouseButtonReleased(MouseButtonEventArgs e)
		{
			_ginput.ProcessMessage(new Gwen.Input.SFMLMouseButtonEventArgs(e, false));
		}

		internal override void window_KeyPressed(KeyEventArgs e)
		{
			_ginput.ProcessMessage(new Gwen.Input.SFMLKeyEventArgs(e, true));
		}

		internal override void window_KeyReleased(KeyEventArgs e)
		{
			_ginput.ProcessMessage(new Gwen.Input.SFMLKeyEventArgs(e, false));
		}
		#endregion

		#region Functions
		//General screen functions
		internal override void DrawInternal()
		{
			Draw ();
			_gamegui.RenderCanvas ();
		}
		#endregion
	}
}
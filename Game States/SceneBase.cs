/* File Description
 * Original Works/Author: Thomas Slusny
 * Other Contributors: zsbzsb
 * Author Website: 
 * License: MIT
*/

using System;
using Gwen.Control;
using SFML.Graphics;
using SFML.Window;
using SFGL.Window;
using SFGL.Graphics;
using SFGL.Time;

namespace SFGL.GameScenes
{
	public abstract class SceneBase : GameComponent, IDisposable, IDrawable, IUpdateable, ILoadable
	{
		#region Variables
		private Canvas _gamegui = null;
		private Gwen.Input.SFML _ginput = null;
		private string _guipath = null;
		#endregion

		#region Properties

		public Canvas GameGUI
		{
			get { return _gamegui; }
		}

		public string GuiImagePath
		{
			get { return _guipath; }
		}

		#endregion

		#region Events
		public event Action<SceneBase> SwitchScreen;
		public event Action CloseScreen;
		#endregion

		#region Constructors and Destructors
		public SceneBase(GameTarget game, string GuiImagePath) : base(game)
		{
			//sets path of gui image file
			_guipath = GuiImagePath;
			LoadInterface ();
		}

		private void LoadInterface()
		{
			// create GWEN renderer
			Gwen.Renderer.SFML gwenRenderer = new Gwen.Renderer.SFML(Game);

			// Create GWEN skin
			//Skin.Simple skin = new Skin.Simple(GwenRenderer);
			Gwen.Skin.TexturedBase skin = new Gwen.Skin.TexturedBase(gwenRenderer, _guipath);

			// set default font
			Gwen.Font defaultFont = new Gwen.Font(gwenRenderer) { Size = 15, FaceName = "Verdana" };

			skin.SetDefaultFont(defaultFont.FaceName, defaultFont.Size);
			defaultFont.Dispose(); // skin has its own

			// Create a Canvas (it's root, on which all other GWEN controls are created)
			_gamegui = new Canvas(skin);
			_gamegui.SetSize((int)Game.Size.X, (int)Game.Size.Y);
			_gamegui.ShouldDrawBackground = false;
			_gamegui.KeyboardInputEnabled = true;

			// Create GWEN input processor
			_ginput = new Gwen.Input.SFML();
			_ginput.Initialize(_gamegui, Game);
		}
		#endregion

		#region Input bindings
		public void window_TextEntered(GameTarget sender, TextEventArgs e)
		{
			_ginput.ProcessMessage(e);
		}

		public void window_MouseWheelMoved(GameTarget sender, MouseWheelEventArgs e)
		{
			_ginput.ProcessMessage(e);
		}

		public void window_MouseMoved(GameTarget sender, MouseMoveEventArgs e)
		{
			_ginput.ProcessMessage(e);
			MouseMoved(sender, e);
		}

		public void window_MouseButtonPressed(GameTarget sender, MouseButtonEventArgs e)
		{
			_ginput.ProcessMessage(new Gwen.Input.SFMLMouseButtonEventArgs(e, true));
			MouseButtonPressed(sender, e);
		}

		public void window_MouseButtonReleased(GameTarget sender, MouseButtonEventArgs e)
		{
			_ginput.ProcessMessage(new Gwen.Input.SFMLMouseButtonEventArgs(e, false));
			MouseButtonReleased(sender, e);
		}

		public void window_KeyPressed(GameTarget sender, KeyEventArgs e)
		{
			_ginput.ProcessMessage(new Gwen.Input.SFMLKeyEventArgs(e, true));
			KeyPressed(sender,e);
		}

		public void window_KeyReleased(GameTarget sender, KeyEventArgs e)
		{
			_ginput.ProcessMessage(new Gwen.Input.SFMLKeyEventArgs(e, false));
			KeyReleased(sender, e);
		}
		#endregion

		#region Functions
		//Input bindings
		protected virtual void KeyPressed(GameTarget sender, KeyEventArgs EventArgs) { }
		protected virtual void KeyReleased(GameTarget sender, KeyEventArgs EventArgs) { }
		protected virtual void MouseButtonPressed(GameTarget sender, MouseButtonEventArgs EventArgs) { }
		protected virtual void MouseButtonReleased(GameTarget sender, MouseButtonEventArgs EventArgs) { }
		protected virtual void MouseMoved(GameTarget sender, MouseMoveEventArgs EventArgs) { }

		//Internal functions
		protected void OnSwitchScreen(SceneBase NewScreen) { if (SwitchScreen != null) SwitchScreen(NewScreen); }
		protected void OnCloseScreen() { if (CloseScreen != null) CloseScreen(); }

		//General screen functions
		public virtual void LoadContent() { }
		public virtual void Dispose() { }
		public virtual void Update(GameTime gameTime) { }
		public virtual void Draw(GameTime gameTime) { }
		#endregion
	}
}
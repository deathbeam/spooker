/* File Description
 * Original Works/Author: Thomas Slusny
 * Other Contributors: zsbzsb
 * Author Website: 
 * License: MIT
*/

using System;
using System.Collections.Generic;
using Gwen.Control;
using SFML.Graphics;
using SFML.Window;
using SFGL.GUI;
using SFGL.Window;
using SFGL.Graphics;
using SFGL.Time;

namespace SFGL.GameScenes
{
	public class SceneManager : GameComponent, IUpdateable, IDrawable, IDisposable
	{
		#region Variables
		private List<SceneBase> _stack = new List<SceneBase> ();
		private Clock frameclock = new Clock();
		private GameTime elapsedtime = GameTime.Zero;
		#endregion

		#region Properties

		public SceneBase GetCurrentScreen
		{
			get { return _stack[_stack.Count - 1]; }
		}
		
		#endregion

		#region Constructors

		public SceneManager(GameTarget game) : base (game) { }

		#endregion

		#region Input bindings
		public void window_TextEntered(GameTarget sender, TextEventArgs e)
		{
			_stack[_stack.Count - 1].window_TextEntered(Game, e);
		}

		public void window_MouseWheelMoved(GameTarget sender, MouseWheelEventArgs e)
		{
			_stack[_stack.Count - 1].window_MouseWheelMoved(Game, e);
		}

		public void window_MouseMoved(GameTarget sender, MouseMoveEventArgs e)
		{
			_stack[_stack.Count - 1].window_MouseMoved(Game, e);
		}

		public void window_MouseButtonPressed(GameTarget sender, MouseButtonEventArgs e)
		{
			_stack[_stack.Count - 1].window_MouseButtonPressed(Game, e);
		}

		public void window_MouseButtonReleased(GameTarget sender, MouseButtonEventArgs e)
		{
			_stack[_stack.Count - 1].window_MouseButtonReleased(Game, e);
		}

		public void window_KeyPressed(GameTarget sender, KeyEventArgs e)
		{
			_stack[_stack.Count - 1].window_KeyPressed(Game, e);
		}

		public void window_KeyReleased(GameTarget sender, KeyEventArgs e)
		{
			_stack[_stack.Count - 1].window_KeyReleased(Game, e);
		}

		public void window_Close()
		{
			SFCore.Game.Close();
		}

		#endregion

		#region Functions

		public void AddScreen(SceneBase newScreen)
		{
			_stack.Add(newScreen);
			_stack[_stack.Count - 1].SwitchScreen += OnSwitchScreen;
			_stack[_stack.Count - 1].CloseScreen += OnCloseScreen;
			GuiManager.Clear ();
			_stack[_stack.Count - 1].LoadContent();
		}

		public void AddScreen()
		{
			_stack[_stack.Count - 1].SwitchScreen += OnSwitchScreen;
			_stack[_stack.Count - 1].CloseScreen += OnCloseScreen;
			GuiManager.Clear ();
			_stack[_stack.Count - 1].LoadContent();
		}

		public void RemoveScreen()
		{
			_stack[_stack.Count - 1].SwitchScreen -= OnSwitchScreen;
			_stack[_stack.Count - 1].CloseScreen -= OnCloseScreen;
			_stack[_stack.Count - 1].Dispose();
		}

		private void OnSwitchScreen(SceneBase NewScreenManager)
		{
			if (_stack.Contains(NewScreenManager)) return;
			RemoveScreen ();
			AddScreen (NewScreenManager);
		}

		private void OnCloseScreen()
		{
			RemoveScreen ();
			_stack.RemoveAt(_stack.Count - 1);
			if (_stack.Count <= 0) 
				SFCore.Exit();
			else
				AddScreen ();
		}

		public void Draw(GameTime gameTime)
		{
			if (_stack.Count >= 1)
			{
				_stack[_stack.Count - 1].Draw(gameTime); 
				_stack [_stack.Count - 1].GameGUI.RenderCanvas();
			}
		}

		public void Update(GameTime gameTime)
		{
			elapsedtime += frameclock.Restart();
			if (_stack.Count >= 1) 
			{
				while (elapsedtime >= gameTime)
				{
					elapsedtime -= gameTime;
					_stack[_stack.Count - 1].Update(gameTime);
				}
			}
		}

		public void Dispose()
		{
			foreach (var state in _stack)
				state.Dispose ();
		}
		#endregion
	}
}
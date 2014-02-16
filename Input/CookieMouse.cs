/* File Description
 * Original Works/Author: Thomas Slusny
 * Other Contributors: None
 * Author Website: http://indiearmory.com
 * License: MIT
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SFML.Window;
using SFGL.Window;
using SFGL.Time;

namespace SFGL.Input
{
	public class CookieMouse : GameComponent, IUpdateable
	{
		private Dictionary<Mouse.Button, bool> _buttonStates = new Dictionary<Mouse.Button, bool>();
		private Dictionary<Mouse.Button, bool> _previousButtonStates = new Dictionary<Mouse.Button, bool>();
		private IEnumerable<Mouse.Button> buttonEnum = Enum.GetValues(typeof(Mouse.Button)).Cast<Mouse.Button>();

		public event Action<GameTarget, MouseWheelEventArgs> OnWheelScroll;
		public event Action<GameTarget, MouseWheelEventArgs> OnWheelScrollUp;
		public event Action<GameTarget, MouseWheelEventArgs> OnWheelScrollDown;

		/// <summary>
		/// Checks if the key is down(pressed)
		/// </summary>
		/// <param name="button">The desired mouse button</param>
		/// <returns></returns>
		public bool this[Mouse.Button button]
		{
			get { return IsKeyDown(button); }
		}

		public int ScrollWheelDelta { get; set; }
		public bool ScrollWheelMoved
		{
			get { return (ScrollWheelDelta != 0); }
		}

		public Vector2i MousePosition
		{
			get { return Mouse.GetPosition(); }
			set { Mouse.SetPosition(value); }
		}

		public CookieMouse(GameTarget game) : base(game)
		{
			foreach(Mouse.Button button in buttonEnum)
			{
				_buttonStates.Add(button, false);
				_previousButtonStates.Add(button, false);
			}
		}

		public void Update(GameTime gameTime)
		{
			ScrollWheelDelta = 0;

			_previousButtonStates.Clear();
			foreach(KeyValuePair<Mouse.Button, bool> pair in _buttonStates)
			{
				_previousButtonStates.Add(pair.Key, pair.Value);
			}

			_buttonStates.Clear();
			foreach(Mouse.Button button in buttonEnum)
			{
				_buttonStates.Add(button, Mouse.IsButtonPressed(button));
			}
		}

		public void window_MouseWheelMoved(GameTarget sender, MouseWheelEventArgs args)
		{
			ScrollWheelDelta = args.Delta;

			Action<GameTarget, MouseWheelEventArgs> ws = OnWheelScroll;
			Action<GameTarget, MouseWheelEventArgs> wsu = OnWheelScrollUp;
			Action<GameTarget, MouseWheelEventArgs> wsd = OnWheelScrollDown;

			if (ws != null)
				ws(sender, args);

			if (args.Delta > 0 && wsu != null)
				wsu(sender, args);

			else if (args.Delta < 0 && wsd != null)
				wsd(sender, args);
		}

		/// <summary>
		/// Checks if the specified key was down and is now up
		/// </summary>
		/// <param name="button">The desired mouse button</param>
		/// <returns></returns>
		public bool IsKeyReleased(Mouse.Button button)
		{
			return WasKeyDown(button) && IsKeyUp(button);
		}

		/// <summary>
		/// Checks if the specified key was up and is now down
		/// </summary>
		/// <param name="button">The desired mouse button</param>
		/// <returns></returns>
		public bool IsKeyPressed(Mouse.Button button)
		{
			return IsKeyDown(button) && WasKeyUp(button);
		}

		/// <summary>
		/// Checks if the key was up(not pressed) on the last frame
		/// </summary>
		/// <param name="button">The desired mouse button</param>
		/// <returns></returns>
		public bool WasKeyUp(Mouse.Button button)
		{
			return !WasKeyDown(button);
		}

		/// <summary>
		/// Checks if the key was down(pressed) on the last frame
		/// </summary>
		/// <param name="button">The desired mouse button</param>
		/// <returns></returns>
		public bool WasKeyDown(Mouse.Button button)
		{
			return _previousButtonStates[button];
		}

		/// <summary>
		/// Checks if the key is up(not pressed)
		/// </summary>
		/// <param name="button">The desired mouse button</param>
		/// <returns></returns>
		public bool IsKeyUp(Mouse.Button button)
		{
			return !IsKeyDown(button);
		}

		/// <summary>
		/// Checks if the key is down(pressed)
		/// </summary>
		/// <param name="button">The desired mouse button</param>
		/// <returns></returns>
		public bool IsKeyDown(Mouse.Button button)
		{
			return _buttonStates[button];
		}
	}
}

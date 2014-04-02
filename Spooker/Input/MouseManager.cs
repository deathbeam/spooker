//-----------------------------------------------------------------------------
// MouseManager.cs
//
// Spooker Open Source Game Framework
// Copyright (C) Indie Armory. All rights reserved.
// Website: http://indiearmory.com
// Other Contributors: None
// License: MIT
//-----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using SFML.Window;
using Spooker.Time;

namespace Spooker.Input
{
	/// <summary>
	/// 
	/// </summary>
	public class MouseManager : IUpdateable
	{
		private readonly Dictionary<Mouse.Button, bool> _buttonStates = new Dictionary<Mouse.Button, bool>();
		private readonly Dictionary<Mouse.Button, bool> _previousButtonStates = new Dictionary<Mouse.Button, bool>();
		private readonly IEnumerable<Mouse.Button> _buttonEnum = Enum.GetValues(typeof(Mouse.Button)).Cast<Mouse.Button>();
		private SFML.Graphics.RenderWindow _graphicsDevice;
		
		public event Action<MouseWheelEventArgs> OnWheelScroll;
		public event Action<MouseWheelEventArgs> OnWheelScrollUp;
		public event Action<MouseWheelEventArgs> OnWheelScrollDown;
		public int ScrollWheelDelta;

		public bool ScrollWheelMoved
		{
			get { return (ScrollWheelDelta != 0); }
		}
		
		public Vector2 GlobalPosition
		{
			get { return new Vector2((float)Mouse.GetPosition().X, (float)Mouse.GetPosition().Y); }
			set { Mouse.SetPosition(new Vector2i((int)value.X, (int)value.Y)); }
		}
		
		public Vector2 LocalPosition
		{
			get { return new Vector2 ((float)Mouse.GetPosition (_graphicsDevice).X,(float)Mouse.GetPosition (_graphicsDevice).Y); }
			set { Mouse.SetPosition(new Vector2i((int)value.X, (int)value.Y), _graphicsDevice); }
		}

	    /// <summary>
	    /// Creates new instance of MouseManager class.
	    /// </summary>
	    /// <returns></returns>
		public MouseManager(SFML.Graphics.RenderWindow graphicsDevice)
		{

			_graphicsDevice = graphicsDevice;
			_graphicsDevice.MouseWheelMoved += (sender, e) => MouseWheelMoved(e);
			foreach(Mouse.Button button in _buttonEnum)
			{
				_buttonStates.Add(button, false);
				_previousButtonStates.Add(button, false);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="gameTime"></param>
		public void Update(GameTime gameTime)
		{
			ScrollWheelDelta = 0;

			_previousButtonStates.Clear();
			foreach(KeyValuePair<Mouse.Button, bool> pair in _buttonStates)
			{
				_previousButtonStates.Add(pair.Key, pair.Value);
			}

			_buttonStates.Clear();
			foreach(Mouse.Button button in _buttonEnum)
			{
				_buttonStates.Add(button, Mouse.IsButtonPressed(button));
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="args"></param>
		private void MouseWheelMoved(MouseWheelEventArgs args)
		{
			ScrollWheelDelta = args.Delta;

			Action<MouseWheelEventArgs> ws = OnWheelScroll;
			Action<MouseWheelEventArgs> wsu = OnWheelScrollUp;
			Action<MouseWheelEventArgs> wsd = OnWheelScrollDown;

			if (ws != null)
				ws(args);

			if (args.Delta > 0 && wsu != null)
				wsu(args);

			else if (args.Delta < 0 && wsd != null)
				wsd(args);
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

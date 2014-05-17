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
	/// Mouse manager.
	/// </summary>
	public class MouseManager : ITargetable, IUpdateable
	{
		private readonly Dictionary<Mouse.Button, bool> _buttonStates = new Dictionary<Mouse.Button, bool>();
		private readonly Dictionary<Mouse.Button, bool> _previousButtonStates = new Dictionary<Mouse.Button, bool>();
		private readonly IEnumerable<Mouse.Button> _buttonEnum = Enum.GetValues(typeof(Mouse.Button)).Cast<Mouse.Button>();
		private readonly SFML.Graphics.RenderWindow _graphicsDevice;

		/// <summary>
		/// Occurs when mouse wheel scrolls.
		/// </summary>
		public event Action<MouseWheelEventArgs> OnWheelScroll;

		/// <summary>
		/// Occurs when mouse wheel scrolls up.
		/// </summary>
		public event Action<MouseWheelEventArgs> OnWheelScrollUp;

		/// <summary>
		/// Occurs when mouse wheel scrolls down.
		/// </summary>
		public event Action<MouseWheelEventArgs> OnWheelScrollDown;

		/// <summary>
		/// The scroll wheel delta.
		/// </summary>
		public int ScrollWheelDelta;

		/// <summary>
		/// Gets a value indicating whether this <see cref="Spooker.Input.MouseManager"/> scroll wheel moved.
		/// </summary>
		/// <value><c>true</c> if scroll wheel moved; otherwise, <c>false</c>.</value>
		public bool ScrollWheelMoved
		{
			get { return (ScrollWheelDelta != 0); }
		}

		/// <summary>
		/// Gets or sets the global position.
		/// </summary>
		/// <value>The global position.</value>
		public Vector2 GlobalPosition
		{
			get { return new Vector2(Mouse.GetPosition().X, Mouse.GetPosition().Y); }
			set { Mouse.SetPosition(new Vector2i((int)value.X, (int)value.Y)); }
		}

		/// <summary>
		/// Gets or sets the local position.
		/// </summary>
		/// <value>The local position.</value>
		public Vector2 LocalPosition
		{
			get { return new Vector2 (Mouse.GetPosition (_graphicsDevice).X,Mouse.GetPosition (_graphicsDevice).Y); }
			set { Mouse.SetPosition(new Vector2i((int)value.X, (int)value.Y), _graphicsDevice); }
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Spooker.Input.MouseManager"/> class.
		/// </summary>
		/// <param name="graphicsDevice">Graphics device.</param>
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
		/// Targets the position.
		/// </summary>
		/// <returns>The position.</returns>
		public Vector2 TargetPosition()
		{
			return LocalPosition;
		}

		/// <summary>
		/// Component uses this for updating itself.
		/// </summary>
		/// <param name="gameTime">Provides snapshot of timing values.</param>
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

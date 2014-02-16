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
	public class CookieKeyboard : GameComponent, IUpdateable
    {
		private Dictionary<Keyboard.Key, bool> _keyStates = new Dictionary<Keyboard.Key, bool>();
		private Dictionary<Keyboard.Key, bool> _previousKeyStates = new Dictionary<Keyboard.Key, bool>();
		private IEnumerable<Keyboard.Key> keysEnum = Enum.GetValues(typeof(Keyboard.Key)).Cast<Keyboard.Key>();

		/// <summary>
		/// Checks if the key is down(pressed)
		/// </summary>
		/// <param name="key">The desired key</param>
		/// <returns></returns>
		public bool this[Keyboard.Key key]
		{
			get { return IsKeyDown(key); }
		}

		public CookieKeyboard(GameTarget game) : base(game) 
		{
			foreach(Keyboard.Key key in keysEnum)
			{
				_keyStates.Add(key, false);
				_previousKeyStates.Add(key, false);
			}
		}

		public void Update(GameTime gameTime)
        {
			_previousKeyStates.Clear();
			foreach(KeyValuePair<Keyboard.Key, bool> valuePair in _keyStates)
			{
				_previousKeyStates.Add(valuePair.Key, valuePair.Value);
			}

			_keyStates.Clear();
			foreach(Keyboard.Key key in keysEnum)
			{
				_keyStates.Add(key, Keyboard.IsKeyPressed(key));
			}
        }

		/// <summary>
		/// Returns all the keyboard keys that were pressed in the current frame
		/// </summary>
		/// <returns></returns>
		public Keyboard.Key[] GetPressedKeys()
		{
			return (from key in _keyStates where key.Value select key.Key).ToArray();
		}

		/// <summary>
		/// Returns all the keyboard keys that were pressed in the last frame
		/// </summary>
		/// <returns></returns>
		public Keyboard.Key[] GetLastFramePressedKeys()
		{
			return (from key in _previousKeyStates where key.Value select key.Key).ToArray();
		}

		/// <summary>
		/// Checks if the specified key was down and is now up
		/// </summary>
		/// <param name="key">The desired keyboard key</param>
		/// <returns></returns>
		public bool IsKeyReleased(Keyboard.Key key)
		{
			return WasKeyDown(key) && IsKeyUp(key);
		}

		/// <summary>
		/// Checks if the specified key was up and is now down
		/// </summary>
		/// <param name="key">The desired key</param>
		/// <returns></returns>
		public bool IsKeyPressed(Keyboard.Key key)
		{
			return IsKeyDown(key) && WasKeyUp(key);
		}

		/// <summary>
		/// Checks if the key was up(not pressed) on the last frame
		/// </summary>
		/// <param name="key">The desired key</param>
		/// <returns></returns>
		public bool WasKeyUp(Keyboard.Key key)
		{
			return !WasKeyDown(key);
		}

		/// <summary>
		/// Checks if the key was down(pressed) on the last frame
		/// </summary>
		/// <param name="key">The desired key</param>
		/// <returns></returns>
		public bool WasKeyDown(Keyboard.Key key)
		{
			return _previousKeyStates[key];
		}

		/// <summary>
		/// Checks if the key is up(not pressed)
		/// </summary>
		/// <param name="key">The desired key</param>
		/// <returns></returns>
		public bool IsKeyUp(Keyboard.Key key)
		{
			return !IsKeyDown(key);
		}

		/// <summary>
		/// Checks if the key is down(pressed)
		/// </summary>
		/// <param name="key">The desired key</param>
		/// <returns></returns>
		public bool IsKeyDown(Keyboard.Key key)
		{
			return _keyStates[key];
		}
    }
}

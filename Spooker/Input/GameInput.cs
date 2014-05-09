using System.Collections.Generic;
using Spooker.Time;
using SFML.Graphics;

namespace Spooker.Input
{
	/// <summary>
	/// Game input.
	/// </summary>
	public class GameInput : IUpdateable
	{
		private readonly List<InputAction> _actions;

		/// <summary>
		/// The keyboard.
		/// </summary>
		public KeyboardManager Keyboard;

		/// <summary>
		/// The mouse.
		/// </summary>
		public MouseManager Mouse;

		/// <summary>
		/// Gets the <see cref="Spooker.Input.InputAction"/> with the specified name.
		/// </summary>
		/// <param name="name">Name.</param>
		public InputAction this[string name]
		{
			get
			{
				return _actions.Find(a=> a.Name == name); 
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Spooker.Input.GameInput"/> class.
		/// </summary>
		/// <param name="graphicsDevice">Graphics device.</param>
		public GameInput (RenderWindow graphicsDevice)
		{
			_actions = new List<InputAction> ();
			Keyboard = new KeyboardManager ();
			Mouse = new MouseManager (graphicsDevice);
		}

		/// <summary>
		/// Add the action with specified name.
		/// </summary>
		/// <param name="name">Name.</param>
		public void Add(string name)
		{
			_actions.Add (new InputAction(this, name));
		}

		/// <summary>
		/// Remove the action with specified name.
		/// </summary>
		/// <param name="name">Name.</param>
		public void Remove(string name)
		{
			_actions.Remove (this[name]);
		}

		/// <summary>
		/// Allows the game component to update itself.
		/// </summary>
		/// <param name="gameTime">Provides snapshot of timing values.</param>
		public void Update(GameTime gameTime)
		{
			Keyboard.Update (gameTime);
			Mouse.Update (gameTime);

			foreach (var action in _actions)
				action.Trigger ();
		}
	}
}
using System.Collections.Generic;
using Spooker.Time;
using SFML.Graphics;

namespace Spooker.Input
{
	public class GameInput : IUpdateable
	{
		private readonly List<InputAction> _actions;
		public KeyboardManager Keyboard;
		public MouseManager Mouse;

		public InputAction this[string name]
		{
			get
			{
				return _actions.Find(a=> a.Name == name); 
			}
		}

		public GameInput (RenderWindow graphicsDevice)
		{
			_actions = new List<InputAction> ();
			Keyboard = new KeyboardManager ();
			Mouse = new MouseManager (graphicsDevice);
		}

		public void Add(string name)
		{
			_actions.Add (new InputAction(this, name));
		}

		public void Remove(string name)
		{
			_actions.Remove (this[name]);
		}

		public void Update(GameTime gameTime)
		{
			Keyboard.Update (gameTime);
			Mouse.Update (gameTime);

			foreach (var action in _actions)
				action.Trigger ();
		}
	}
}
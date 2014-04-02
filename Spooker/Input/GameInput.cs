using System.Collections.Generic;
using Spooker.Time;
using SFML.Graphics;

namespace Spooker.Input
{
	public class GameInput : IUpdateable
	{
		private readonly Dictionary<string, InputAction> _actions;

		public KeyboardManager Keyboard;
		public MouseManager Mouse;

		public GameInput (RenderWindow graphicsDevice)
		{
			_actions = new Dictionary<string, InputAction> ();
			Keyboard = new KeyboardManager ();
			Mouse = new MouseManager (graphicsDevice);
		}

		internal void AddAction(string name, InputAction action)
		{
			_actions.Add (name, action);
		}

		public void RemoveAction(string name)
		{
			_actions.Remove (name);
		}

		public void Update(GameTime gameTime)
		{
			Keyboard.Update (gameTime);
			Mouse.Update (gameTime);

			foreach (var action in _actions.Values)
			{
				if (action.IsKeyboard)
				{
					if ((action.Type == ActionType.Down && Keyboard.IsKeyDown (action.Key)) ||
						(action.Type == ActionType.Up && Keyboard.IsKeyUp (action.Key)) ||
						(action.Type == ActionType.Pressed && Keyboard.IsKeyPressed (action.Key)) ||
						(action.Type == ActionType.Released && Keyboard.IsKeyReleased (action.Key)))
						action.OnTrigger.Invoke ();
				}
				else
				{
					if ((action.Type == ActionType.Down && Mouse.IsKeyDown (action.Button)) ||
						(action.Type == ActionType.Up && Mouse.IsKeyUp (action.Button)) ||
						(action.Type == ActionType.Pressed && Mouse.IsKeyPressed (action.Button)) ||
						(action.Type == ActionType.Released && Mouse.IsKeyReleased (action.Button)))
						action.OnTrigger.Invoke ();
				}
			}
		}
	}
}
using SFML.Window;
using System.Collections.Generic;

namespace Spooker.Input
{
	public class InputAction
	{
		private GameInput _parent;
		private List<ActionNode> _triggers;

		public string Name;

		public delegate void TriggerEvent();
		public TriggerEvent OnPress;
		public TriggerEvent OnRelease;
		public TriggerEvent OnHold;
		public TriggerEvent OnIdle;
		
		public InputAction (GameInput parent, string name)
		{
			Name = name;
			_triggers = new List<ActionNode> ();
			_parent = parent;
		}

		public void Add(Keyboard.Key key)
		{
			_triggers.Add (
				new KeyNode (key));
		}

		public void Add(Mouse.Button button)
		{
			_triggers.Add (
				new MouseNode (button));
		}

		public bool IsPressed()
		{
			foreach(var trigger in _triggers)
			{
				if (trigger.IsPressed (_parent))
					return true;
			}

			return false;
		}

		public bool IsReleased()
		{
			foreach(var trigger in _triggers)
			{
				if (trigger.IsReleased (_parent))
					return true;
			}

			return false;
		}

		public bool IsDown()
		{
			foreach(var trigger in _triggers)
			{
				if (trigger.IsDown (_parent))
					return true;
			}

			return false;
		}

		public bool IsUp()
		{
			foreach(var trigger in _triggers)
			{
				if (trigger.IsUp (_parent))
					return true;
			}

			return false;
		}
	}
}
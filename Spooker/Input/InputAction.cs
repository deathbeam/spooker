using System.Linq;
using SFML.Window;
using System.Collections.Generic;

namespace Spooker.Input
{
	public class InputAction
	{
		private readonly GameInput _parent;
		private readonly List<ActionNode> _triggers;

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
		    return _triggers.Any(trigger => trigger.IsPressed(_parent));
		}

	    public bool IsReleased()
	    {
	        return _triggers.Any(trigger => trigger.IsReleased(_parent));
	    }

	    public bool IsDown()
	    {
	        return _triggers.Any(trigger => trigger.IsDown(_parent));
	    }

	    public bool IsUp()
	    {
	        return _triggers.Any(trigger => trigger.IsUp(_parent));
	    }
	}
}
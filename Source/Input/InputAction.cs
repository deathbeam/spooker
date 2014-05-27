using System;
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

		/// <summary>
		/// Occurs when action is pressed.
		/// </summary>
		public event Action OnPress;

		/// <summary>
		/// Occurs when action is released.
		/// </summary>
		public event Action OnRelease;

		/// <summary>
		/// Occurs when action is held.
		/// </summary>
		public event Action OnHold;

		/// <summary>
		/// Occurs when action is idle.
		/// </summary>
		public event Action OnIdle;

		/// <summary>
		/// Initializes a new instance of the <see cref="Spooker.Input.InputAction"/> class.
		/// </summary>
		/// <param name="parent">Parent.</param>
		/// <param name="name">Name.</param>
		public InputAction (GameInput parent, string name)
		{
			Name = name;
			_triggers = new List<ActionNode> ();
			_parent = parent;
		}

		/// <summary>
		/// Add the specified key.
		/// </summary>
		/// <param name="key">Key.</param>
		public void Add(Keyboard.Key key)
		{
			_triggers.Add (
				new KeyNode (_parent, key));
		}

		/// <summary>
		/// Add the specified button.
		/// </summary>
		/// <param name="button">Button.</param>
		public void Add(Mouse.Button button)
		{
			_triggers.Add (
				new MouseNode (_parent, button));
		}

		/// <summary>
		/// Trigger this instance.
		/// </summary>
		public void Trigger()
		{
			if (IsDown() && OnHold != null)
				OnHold ();
			else if (IsUp() && OnIdle != null)
				OnIdle ();
			else if (IsPressed() && OnPress != null)
				OnPress ();
			else if (IsReleased() && OnRelease != null)
				OnRelease ();
		}

		/// <summary>
		/// Determines whether this instance is pressed.
		/// </summary>
		/// <returns><c>true</c> if this instance is pressed; otherwise, <c>false</c>.</returns>
		public bool IsPressed()
		{
		    return _triggers.Any(trigger => trigger.IsPressed);
		}

		/// <summary>
		/// Determines whether this instance is released.
		/// </summary>
		/// <returns><c>true</c> if this instance is released; otherwise, <c>false</c>.</returns>
	    public bool IsReleased()
	    {
	        return _triggers.Any(trigger => trigger.IsReleased);
	    }

		/// <summary>
		/// Determines whether this instance is down.
		/// </summary>
		/// <returns><c>true</c> if this instance is down; otherwise, <c>false</c>.</returns>
	    public bool IsDown()
	    {
	        return _triggers.Any(trigger => trigger.IsDown);
	    }

		/// <summary>
		/// Determines whether this instance is up.
		/// </summary>
		/// <returns><c>true</c> if this instance is up; otherwise, <c>false</c>.</returns>
	    public bool IsUp()
	    {
	        return _triggers.Any(trigger => trigger.IsUp);
	    }
	}
}
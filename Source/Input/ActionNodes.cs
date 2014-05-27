using SFML.Window;

namespace Spooker.Input
{
	/// <summary>
	/// Action node.
	/// </summary>
	public abstract class ActionNode
	{
		/// <summary>
		/// The parent.
		/// </summary>
		protected readonly GameInput Parent;

		/// <summary>
		/// Initializes a new instance of the <see cref="Spooker.Input.ActionNode"/> class.
		/// </summary>
		/// <param name="parent">Parent.</param>
		public ActionNode(GameInput parent)
		{
			Parent = parent;
		}

		/// <summary>
		/// Determines whether this instance is pressed.
		/// </summary>
		public abstract bool IsPressed { get; }

		/// <summary>
		/// Determines whether this instance is released.
		/// </summary>
		public abstract bool IsReleased { get; }

		/// <summary>
		/// Determines whether this instance is down.
		/// </summary>
		public abstract bool IsDown { get; }

		/// <summary>
		/// Determines whether this instance is up.
		/// </summary>
		public abstract bool IsUp { get; }
	}

	/// <summary>
	/// Key node.
	/// </summary>
	public class KeyNode : ActionNode
	{
		private readonly Keyboard.Key _key;

		/// <summary>
		/// Initializes a new instance of the <see cref="Spooker.Input.KeyNode"/> class.
		/// </summary>
		/// <param name="parent">Parent.</param>
		/// <param name="key">Key.</param>
		public KeyNode(GameInput parent, Keyboard.Key key) : base(parent)
		{
			_key = key;
		}

		/// <summary>
		/// Determines whether this instance is pressed.
		/// </summary>
		public override bool IsPressed
		{
			get { return Parent.Keyboard.IsKeyPressed (_key); }
		}

		/// <summary>
		/// Determines whether this instance is released.
		/// </summary>
		public override bool IsReleased
		{
			get { return Parent.Keyboard.IsKeyReleased (_key); }
		}

		/// <summary>
		/// Determines whether this instance is down.
		/// </summary>
		public override bool IsDown
		{
			get { return Parent.Keyboard.IsKeyDown (_key); }
		}

		/// <summary>
		/// Determines whether this instance is up.
		/// </summary>
		public override bool IsUp
		{
			get { return Parent.Keyboard.IsKeyUp (_key); }
		}
	}

	public class MouseNode : ActionNode
	{
		private readonly Mouse.Button _button;

		/// <summary>
		/// Initializes a new instance of the <see cref="Spooker.Input.MouseNode"/> class.
		/// </summary>
		/// <param name="parent">Parent.</param>
		/// <param name="button">Button.</param>
		public MouseNode(GameInput parent, Mouse.Button button) : base(parent)
		{
			_button = button;
		}

		/// <summary>
		/// Determines whether this instance is pressed.
		/// </summary>
		public override bool IsPressed
		{
			get { return Parent.Mouse.IsKeyPressed (_button); }
		}

		/// <summary>
		/// Determines whether this instance is released.
		/// </summary>
		public override bool IsReleased
		{
			get { return Parent.Mouse.IsKeyReleased (_button); }
		}

		/// <summary>
		/// Determines whether this instance is down.
		/// </summary>
		public override bool IsDown
		{
			get { return Parent.Mouse.IsKeyDown (_button); }
		}

		/// <summary>
		/// Determines whether this instance is up.
		/// </summary>
		public override bool IsUp
		{
			get { return Parent.Mouse.IsKeyUp (_button); }
		}
	}
}
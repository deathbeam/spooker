using SFML.Window;

namespace Spooker.Input
{
	/// <summary>
	/// Action node.
	/// </summary>
	public abstract class ActionNode
	{
		/// <summary>
		/// Determines whether this instance is pressed.
		/// </summary>
		/// <returns><c>true</c> if this instance is pressed; otherwise, <c>false</c>.</returns>
		/// <param name="input">Input.</param>
		public abstract bool IsPressed(GameInput input);

		/// <summary>
		/// Determines whether this instance is released.
		/// </summary>
		/// <returns><c>true</c> if this instance is released; otherwise, <c>false</c>.</returns>
		/// <param name="input">Input.</param>
		public abstract bool IsReleased(GameInput input);

		/// <summary>
		/// Determines whether this instance is down.
		/// </summary>
		/// <returns><c>true</c> if this instance is down; otherwise, <c>false</c>.</returns>
		/// <param name="input">Input.</param>
		public abstract bool IsDown(GameInput input);

		/// <summary>
		/// Determines whether this instance is up.
		/// </summary>
		/// <returns><c>true</c> if this instance is up; otherwise, <c>false</c>.</returns>
		/// <param name="input">Input.</param>
		public abstract bool IsUp(GameInput input);
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
		/// <param name="key">Key.</param>
		public KeyNode(Keyboard.Key key)
		{
			_key = key;
		}

		/// <summary>
		/// Determines whether this instance is pressed.
		/// </summary>
		/// <returns>true</returns>
		/// <c>false</c>
		/// <param name="input">Input.</param>
		public override bool IsPressed(GameInput input)
		{
			return input.Keyboard.IsKeyPressed (_key);
		}

		/// <summary>
		/// Determines whether this instance is released.
		/// </summary>
		/// <returns>true</returns>
		/// <c>false</c>
		/// <param name="input">Input.</param>
		public override bool IsReleased(GameInput input)
		{
			return input.Keyboard.IsKeyReleased (_key);
		}

		/// <summary>
		/// Determines whether this instance is down.
		/// </summary>
		/// <returns>true</returns>
		/// <c>false</c>
		/// <param name="input">Input.</param>
		public override bool IsDown(GameInput input)
		{
			return input.Keyboard.IsKeyDown (_key);
		}

		/// <summary>
		/// Determines whether this instance is up.
		/// </summary>
		/// <returns>true</returns>
		/// <c>false</c>
		/// <param name="input">Input.</param>
		public override bool IsUp(GameInput input)
		{
			return input.Keyboard.IsKeyUp (_key);
		}
	}

	public class MouseNode : ActionNode
	{
		private readonly Mouse.Button _button;

		/// <summary>
		/// Initializes a new instance of the <see cref="Spooker.Input.MouseNode"/> class.
		/// </summary>
		/// <param name="button">Button.</param>
		public MouseNode(Mouse.Button button)
		{
			_button = button;
		}

		/// <summary>
		/// Determines whether this instance is pressed.
		/// </summary>
		/// <returns>true</returns>
		/// <c>false</c>
		/// <param name="input">Input.</param>
		public override bool IsPressed(GameInput input)
		{
			return input.Mouse.IsKeyPressed (_button);
		}

		/// <summary>
		/// Determines whether this instance is released.
		/// </summary>
		/// <returns>true</returns>
		/// <c>false</c>
		/// <param name="input">Input.</param>
		public override bool IsReleased(GameInput input)
		{
			return input.Mouse.IsKeyReleased (_button);
		}

		/// <summary>
		/// Determines whether this instance is down.
		/// </summary>
		/// <returns>true</returns>
		/// <c>false</c>
		/// <param name="input">Input.</param>
		public override bool IsDown(GameInput input)
		{
			return input.Mouse.IsKeyDown (_button);
		}

		/// <summary>
		/// Determines whether this instance is up.
		/// </summary>
		/// <returns>true</returns>
		/// <c>false</c>
		/// <param name="input">Input.</param>
		public override bool IsUp(GameInput input)
		{
			return input.Mouse.IsKeyUp (_button);
		}
	}
}
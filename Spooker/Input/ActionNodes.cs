using System;
using System.Collections.Generic;
using SFML.Window;

namespace Spooker.Input
{
	public abstract class ActionNode
	{
		public abstract bool IsPressed(GameInput input);
		public abstract bool IsReleased(GameInput input);
		public abstract bool IsDown(GameInput input);
		public abstract bool IsUp(GameInput input);
	}

	public class KeyNode : ActionNode
	{
		private Keyboard.Key _key;

		public KeyNode(Keyboard.Key key)
		{
			_key = key;
		}

		public override bool IsPressed(GameInput input)
		{
			return input.Keyboard.IsKeyPressed (_key);
		}

		public override bool IsReleased(GameInput input)
		{
			return input.Keyboard.IsKeyReleased (_key);
		}

		public override bool IsDown(GameInput input)
		{
			return input.Keyboard.IsKeyDown (_key);
		}

		public override bool IsUp(GameInput input)
		{
			return input.Keyboard.IsKeyUp (_key);
		}
	}

	public class MouseNode : ActionNode
	{
		private Mouse.Button _button;

		public MouseNode(Mouse.Button button)
		{
			_button = button;
		}

		public override bool IsPressed(GameInput input)
		{
			return input.Mouse.IsKeyPressed (_button);
		}

		public override bool IsReleased(GameInput input)
		{
			return input.Mouse.IsKeyReleased (_button);
		}

		public override bool IsDown(GameInput input)
		{
			return input.Mouse.IsKeyDown (_button);
		}

		public override bool IsUp(GameInput input)
		{
			return input.Mouse.IsKeyUp (_button);
		}
	}
}
using SFML.Window;

namespace Spooker.Input
{
	public class InputAction
	{
		internal bool IsKeyboard;
		internal Keyboard.Key Key;
		internal Mouse.Button Button;

		public delegate void TriggerEvent();
		public TriggerEvent OnTrigger;
		public ActionType Type;

		public InputAction (string name, GameInput input)
		{
			input.AddAction (name, this);
		}

		public void SetKey(Keyboard.Key key)
		{
			IsKeyboard = true;
			Key = key;
		}

		public void SetKey(Mouse.Button key)
		{
			IsKeyboard = false;
			Button = key;
		}
	}
}
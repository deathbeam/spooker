using System;

namespace SFGL.GameStates
{
	/// <summary>
	/// Flags that determine the enabled functionality of a state.
	/// </summary>
	[Flags]
	public enum UpdateMode
	{
		/// <summary>
		/// Do not processes inactive state at all.
		/// </summary>
		None = 0,

		/// <summary>
		/// Processes input (keyboard, mouse) for inactive state.
		/// </summary>
		Input = 1,

		/// <summary>
		/// Processes game logic updates for inactive state.
		/// </summary>
		Update = 2,

		/// <summary>
		/// Draws inactive state.
		/// </summary>
		Draw = 4,

		/// <summary>
		/// Draws and processes game logic updates for inactive state.
		/// </summary>
		Background = Update | Draw,

		/// <summary>
		/// Draws and processes game logic updates and input for inactive state.
		/// </summary>
		All = Input | Update | Draw
	}
}


/* File Description
 * Original Works/Author: Thomas Slusny
 * Other Contributors: None
 * Author Website: http://indiearmory.com
 * License: MIT
*/

using SFML.Graphics;
using SFGL.Audio;
using SFGL.Window;
using SFGL.GameScenes;
using SFGL.Input;
using SFGL.Resources;

namespace SFGL
{
	/// <summary>
	/// Static class that contains most important objects like Game, Content, Input, etc...
	/// </summary>
	public static class SFCore
	{
		/// <summary>
		/// Gets or Set the Game instance
		/// </summary>
		public static GameWindow Game { get; set; }

		/// <summary>
		/// Gets or Set the State Manager
		/// </summary>
		public static SceneManager SceneManager { get; set; }

		/// <summary>
		/// Gets or Set the Content Manager
		/// </summary>
		public static ContentManager Content { get; set; }

		/// <summary>
		/// Gets or Set the Audio Manager
		/// </summary>
		public static AudioManager Audio { get; set; }

		/// <summary>
		/// Gets or Set the keyboard states
		/// </summary>
		public static CookieKeyboard Keys { get; set; }

		/// <summary>
		/// Gets or Set the mouse states
		/// </summary>
		public static CookieMouse Mouse { get; set; }

		/// <summary>
		/// Gets the width of the current viewport
		/// </summary>
		public static int Width
		{
			get { return (int)Game.Size.X; }
		}

		/// <summary>
		/// Gets the height of the current viewport
		/// </summary>
		public static int Height
		{
			get { return (int)Game.Size.Y; }
		}

		/// <summary>
		/// Gets the rectangle that represent the screen size
		/// </summary>
		public static IntRect ScreenRectangle
		{
			get { return new IntRect(0,0,(int)Game.Size.X,(int)Game.Size.Y); }
		}

		/// <summary>
		/// Gets the center of the screen on X axis
		/// </summary>
		public static int ScreenCenterX
		{
			get { return Width / 2; }
		}

		/// <summary>
		/// Gets the center of the screen on Y axis
		/// </summary>
		public static int ScreenCenterY
		{
			get { return Height / 2; }
		}

		/// <summary>
		/// Close the game
		/// </summary>
		public static void Exit()
		{
			Game.Close();
		}
	}
}


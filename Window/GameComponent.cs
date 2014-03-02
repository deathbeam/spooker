/* File Description
 * Original Works/Author: Thomas Slusny
 * Other Contributors: None
 * Author Website: http://indiearmory.com
 * License: MIT
*/

using SFGL.Audio;
using SFGL.Graphics;
using SFGL.Content;
using SFGL.Input;
using SFML.Graphics;

namespace SFGL.Window
{
	////////////////////////////////////////////////////////////
	/// <summary>
	/// Abstract class what will enable you to use all Game 
	/// functions in derived classes.
	/// </summary>
	////////////////////////////////////////////////////////////
	public abstract class GameComponent
	{
		/// <summary>Heart of SFGL. All important operations are done here.</summary>
        protected GameWindow Game;
		
		/// <summary>Provides optimized drawing of sprites</summary>
		protected SpriteBatch SpriteBatch
		{ 
			get { return this.Game.SpriteBatch; }
		}

		/// <summary>Manages various game content (audio, textures, fonts....).</summary>
		protected ContentManager Content
		{ 
			get { return this.Game.Content; }
		}

		/// <summary>Can play various audio files.</summary>
		protected AudioManager Audio
		{ 
			get { return this.Game.Audio; }
		}

		/// <summary>Handles user input from keyboard.</summary>
		protected KeyboardManager KeysInput
		{ 
			get { return this.Game.KeysInput; }
		}

		/// <summary>Handles user input from mouse.</summary>
		protected MouseManager MouseInput
		{ 
			get { return this.Game.MouseInput; }
		}

		////////////////////////////////////////////////////////////
		/// <summary>
		/// Creates new instance of GameComponent class.
		/// </summary>
		/// <param name="game">Parent game window.</param>
		////////////////////////////////////////////////////////////
		public GameComponent(GameWindow game)
		{
			this.Game = game;
		}
	}
}


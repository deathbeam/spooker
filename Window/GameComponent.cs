/* File Description
 * Original Works/Author: Thomas Slusny
 * Other Contributors: None
 * Author Website: http://indiearmory.com
 * License: MIT
*/
using SFGL.Graphics;
using SFGL.Resources;

namespace SFGL.Window
{
	public abstract class GameComponent
	{
		protected GameTarget Game;

		protected ISpriteBatch spriteBatch
		{ 
			get { return this.Game.spriteBatch; }
		}

		protected ContentManager Content
		{ 
			get { return this.Game.Content; }
		}

		public GameComponent(GameTarget game)
		{
			this.Game = game;
		}
	}
}


/* File Description
 * Original Works/Author: Thomas Slusny
 * Other Contributors: None
 * Author Website: http://indiearmory.com
 * License: MIT
*/

using SFML.Graphics;
using SFGL.GameScenes;
using SFGL.Graphics;
using SFGL.Resources;
using SFGL.Audio;

namespace SFGL.Window
{
	public interface GameTarget : RenderTarget
	{
		SpriteBatch spriteBatch { get; set; }
		SceneManager sceneManager { get; set; }
		GameSettings settings { get; set; }
		ContentManager Content { get; set; }
		AudioManager Audio { get; set; }
	}
}


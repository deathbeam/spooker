/* File Description
 * Original Works/Author: Thomas Slusny
 * Other Contributors: None
 * Author Website: http://indiearmory.com
 * License: MIT
*/

using SFML.Graphics;
using SFML.Window;
using SFGL.Time;

namespace SFGL.Window
{
    public class GameSettings
    {
		#region Variables
		internal VideoMode VideoMode { get; set; }
		internal ContextSettings ContextSettings { get; set; }
		#endregion

		#region Properties
		public GameTime GameTime { get; set; }

		public Color ClearColor { get; set; }

		public string SoundDirectory { get; set; }

		public string SoundExtension { get; set; }

		public string ContentDirectory { get; set; }
		
		public string Title { get; set; }

		public Styles Style { get; set; }

		public bool VerticalSync { get; set; }

		public uint FramerateLimit { get; set; }

		public uint Width { get; set; }

		public uint Height { get; set; }

		public uint BitsPerPixel { get; set; }

		public uint AntialiasingLevel { get; set; }

		public uint DepthBits { get; set; }

		public uint MajorVersion { get; set; }

		public uint MinorVersion { get; set; }

		public uint StencilBits { get; set; }
		#endregion

		public GameSettings()
		{
			ContextSettings = new ContextSettings(32, 0, 4);
			VideoMode = new VideoMode (800, 600);
			ClearColor = Color.Black;
			ContentDirectory = "Content";
			SoundDirectory = "Sounds";
			SoundExtension = "ogg";
			Title = "Game";
			Style = Styles.Default;
			VerticalSync = true;
			FramerateLimit = 32;
			GameTime = GameTime.FromMilliseconds (1);
		}
    }
}

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
		private GameTime _gameTime = GameTime.Zero;
		private Color _color = Color.Black;
		private string _directory = "Content";
		private string _soundDirectory = "Sounds";
		private string _soundExtension = "ogg";
		private string _title = "Game";
		private Styles _style = Styles.Default;
		private bool _vsync = true;
		private uint _framerate = 32;
		private ContextSettings _context = new ContextSettings(32, 0, 4);
		private VideoMode _videoMode = new VideoMode (800, 600);

		public VideoMode GetVideoMode
		{
			get { return _videoMode; }
		}

		public ContextSettings GetContextSettings
		{
			get { return _context; }
		}

		public GameTime GameTime
		{
			get { return _gameTime; }
			set { _gameTime = value; }
		}

		public Color ClearColor
		{
			get { return _color; }
			set { _color = value; }
		}

		public string SoundDirectory
		{
			get { return _soundDirectory; }
			set { _soundDirectory = value; }
		}

		public string SoundExtension
		{
			get { return _soundExtension; }
			set { _soundExtension = value; }
		}

		public string ContentDirectory
		{
			get { return _directory; }
			set { _directory = value; }
		}
		
		public string Title
		{
			get { return _title; }
			set { _title = value; }
		}

		public Styles Style
		{
			get { return _style; }
			set { _style = value; }
		}

		public bool VerticalSync
		{
			get { return _vsync; }
			set { _vsync = value; }
		}

		public uint FramerateLimit
		{
			get { return _framerate; }
			set { _framerate = value; }
		}

        public uint Width
        {
			get { return _videoMode.Width; }
			set { _videoMode.Width = value; }
        }

        public uint Height
        {
			get { return _videoMode.Height; }
			set { _videoMode.Height = value; }
        }

        public uint BitsPerPixel
        {
			get { return _videoMode.BitsPerPixel; }
			set { _videoMode.BitsPerPixel = value; }
        }

        public uint AntialiasingLevel
        {
			get { return _context.AntialiasingLevel; }
			set { _context.AntialiasingLevel = value; }
        }

        public uint DepthBits
        {
			get { return _context.DepthBits; }
			set { _context.DepthBits = value; }
        }

        public uint MajorVersion
        {
			get { return _context.MajorVersion; }
			set { _context.MajorVersion = value; }
        }

        public uint MinorVersion
        {
			get { return _context.MinorVersion; }
			set { _context.StencilBits = value; }
        }

        public uint StencilBits
        {
			get { return _context.AntialiasingLevel; }
			set { _context.AntialiasingLevel = value; }
        }
    }
}

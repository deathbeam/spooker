/* File Description
 * Original Works/Author: Thomas Slusny
 * Other Contributors: krzat
 * Author Website: http://indiearmory.com
 * License: MIT
*/

using System;
using System.Collections.Generic;
using System.Linq;
using SFGL.Window;

namespace SFGL.Resources
{
	public class ContentManager : GameComponent, IDisposable
    {
		protected readonly List<ContentProvider> _loaders = new List<ContentProvider> ();
		protected string _directory = "Content";

		public string Directory
		{ 
			get { return _directory; }
			set { _directory = value; }
		}
		
		public ContentManager(GameTarget game) : base(game) { }

		public T Load<T>(string path) where T : class
        {
			ContentProvider _loader = _loaders.First(x => x.Type == typeof (T));

			if (_loader.Extension != null)
				path = String.Format(
					"{0}/{1}/{2}.{3}",
					Directory,
					_loader.Folder,
					path,
					_loader.Extension);

			return _loader.Get(path) as T;
        }

		public void AddLoader(ContentProvider loader)
        {
			_loaders.Add(loader);
        }

		public void Dispose()
		{
			foreach (var loader in _loaders)
				loader.Dispose();
			_loaders.Clear();
		}
    }
}
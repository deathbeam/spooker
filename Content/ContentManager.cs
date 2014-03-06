/* File Description
 * Original Works/Author: Thomas Slusny
 * Other Contributors: krzat
 * Author Website: http://indiearmory.com
 * License: MIT
*/

using System;
using System.Collections.Generic;
using System.Linq;

namespace SFGL.Content
{
	////////////////////////////////////////////////////////////
	/// <summary>
	/// Provides simple way of loading game content such as
	/// textures, fonts, shaders and more.
	/// </summary>
	////////////////////////////////////////////////////////////
	public class ContentManager : IDisposable
    {
		private readonly List<ContentProvider> _loaders = new List<ContentProvider> ();
		
		/// <summary>Directory, from what will content providers load data.</summary>
		public string Directory { get; set; }

		////////////////////////////////////////////////////////////
		/// <summary>
		/// Creates new instance of Content Manager.
		/// </summary>
		////////////////////////////////////////////////////////////
		public ContentManager() { }

		////////////////////////////////////////////////////////////
		/// <summary>
		/// Loads file to content manager, or if file is already
		/// loaded, returns file data.
		/// </summary>
		////////////////////////////////////////////////////////////
		public T Get<T>(string path) where T : class
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

		////////////////////////////////////////////////////////////
		/// <summary>
		/// Adds new loader type to content manager loaders stack.
		/// </summary>
		////////////////////////////////////////////////////////////
		public void AddLoader(ContentProvider loader)
        {
			_loaders.Add(loader);
        }

		////////////////////////////////////////////////////////////
		/// <summary>
		/// Adds new loaders to content manager loaders stack.
		/// </summary>
		////////////////////////////////////////////////////////////
		public void AddLoaders(List<ContentProvider> loaders)
		{
			foreach (ContentProvider loader in loaders)
				_loaders.Add(loader);
		}

		////////////////////////////////////////////////////////////
		/// <summary>
		/// Disposes this intance of Content Manager.
		/// </summary>
		////////////////////////////////////////////////////////////
		public void Dispose()
		{
			foreach (var loader in _loaders)
				loader.Dispose();
			_loaders.Clear();
		}
    }
}
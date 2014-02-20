/* File Description
 * Original Works/Author: krzat
 * Other Contributors: Thomas Slusny
 * Author Website: https://bitbucket.org/krzat/sfml.utils
 * License: GPL 3.0
*/

using System;
using System.Collections.Generic;

namespace SFGL.Content
{
	/// <summary>
	/// Used for storing game data such as textures, fonts and shaders. 
	/// Stores only one type of data, depends on settings.
	/// </summary>
    public class ContentProvider : IDisposable
    {
        private readonly Dictionary<string, object> assets = new Dictionary<string, object>();
        
		/// <summary>
		/// File extension of loaded game data.
		/// </summary>
		public string Extension;

		/// <summary>
		/// Folder containing loaded game data.
		/// </summary>
        public string Folder;

		/// <summary>
		/// Function used to load data for this content provider.
		/// </summary>
        public Func<string, object> Load;

		/// <summary>
		/// Value that determines if content provider will store data to cache.
		/// </summary>
        public bool Reuse;

		/// <summary>
		/// Type of game data what content provider manages.
		/// </summary>
        public Type Type;

		/// <summary>
		/// Creates new instance of Content Provider class without passing any arguments.
		/// </summary>
        public ContentProvider() {}

		/// <summary>
		/// Creates new instance of Content Provider class with passing multiple arguments.
		/// </summary>
        public ContentProvider(Type Type, string Folder, string Extension, bool Reuse = true)
        {
            this.Type = Type;
            this.Folder = Folder;
            this.Extension = Extension;
            this.Reuse = Reuse;
        }

		/// <summary>
		/// Disposes this instance of Content Provider class.
		/// </summary>
        public void Dispose()
        {
            if (!Type.IsAssignableFrom(typeof (IDisposable))) return;
            foreach (object o in assets.Values)
            {
                var disposable = o as IDisposable;
                disposable.Dispose();
            }
        }

		/// <summary>
		/// Loads game data to cache or if already loaded, returns data from loader cache.
		/// </summary>
        public virtual object Get(string name)
        {
            object result;
            if (Reuse && assets.TryGetValue(name, out result))
            {
                return result;
            }
            result = Load(name);
            if (Reuse)
                assets.Add(name, result);
            return result;
        }
    }
}
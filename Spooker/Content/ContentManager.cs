//-----------------------------------------------------------------------------
// ContentManager.cs
//
// Spooker Open Source Game Framework
// Copyright (C) Indie Armory. All rights reserved.
// Website: http://indiearmory.com
// Other Contributors: krzat @ https://bitbucket.org/krzat
// License: MIT
//-----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using Spooker.Time;

namespace Spooker.Content
{
	////////////////////////////////////////////////////////////
	/// <summary>
	/// Provides simple way of loading game content such as
	/// textures, fonts, shaders and more.
	/// </summary>
	////////////////////////////////////////////////////////////
	public class ContentManager : IUpdateable, IDisposable
    {
		private readonly List<ContentProvider> _loaders = new List<ContentProvider> ();
		
		/// <summary>Directory, from what will content providers load data.</summary>
		public string Directory;

		////////////////////////////////////////////////////////////

	    ////////////////////////////////////////////////////////////
		/// <summary>
		/// Loads file to content manager, or if file is already
		/// loaded, returns file data.
		/// </summary>
		////////////////////////////////////////////////////////////
		public T Get<T>(string path) where T : class
        {
			ContentProvider loader = _loaders.First(x => x.Type == typeof (T));

			if (loader.Extension != null)
				path = String.Format(
					"{0}/{1}/{2}.{3}",
					Directory,
					loader.Folder,
					path,
					loader.Extension);

			return loader.Get(path) as T;
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
			foreach (var loader in loaders)
				_loaders.Add(loader);
		}

		////////////////////////////////////////////////////////////
		/// <summary>
		/// Component uses this for updating itself
		/// </summary>
		////////////////////////////////////////////////////////////
		public void Update(GameTime gameTime)
		{
			var dt = (float)gameTime.ElapsedGameTime.TotalMilliseconds;

			foreach (var loader in _loaders)
				loader.Update(dt);
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
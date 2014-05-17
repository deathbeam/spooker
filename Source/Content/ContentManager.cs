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

		/// <summary>
		/// Loads file to content manager, or if file is already loaded, returns file data.
		/// </summary>
		/// <param name="path">Path.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
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

		/// <summary>
		/// Adds new loader type to content manager loaders stack.
		/// </summary>
		/// <param name="loader">Loader.</param>
		public void AddLoader(ContentProvider loader)
        {
			_loaders.Add(loader);
        }

		/// <summary>
		/// Adds new loaders to content manager loaders stack.
		/// </summary>
		/// <param name="loaders">Loaders.</param>
		public void AddLoaders(List<ContentProvider> loaders)
		{
			foreach (var loader in loaders)
				_loaders.Add(loader);
		}

		/// <summary>
		/// Component uses this for updating itself.
		/// </summary>
		/// <param name="gameTime">Provides snapshot of timing values.</param>
		public void Update(GameTime gameTime)
		{
			var dt = (float)gameTime.ElapsedGameTime.Milliseconds;

			foreach (var loader in _loaders)
				loader.Update(dt);
		}

		/// <summary>
		/// Loads the content.
		/// </summary>
		/// <param name="loadable">Loadable.</param>
		public void LoadContent(ILoadable loadable)
		{
			loadable.LoadContent (this);
		}

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		/// <filterpriority>2</filterpriority>
		/// <remarks>Call <see cref="Dispose"/> when you are finished using the <see cref="Spooker.Content.ContentManager"/>. The
		/// <see cref="Dispose"/> method leaves the <see cref="Spooker.Content.ContentManager"/> in an unusable state. After
		/// calling <see cref="Dispose"/>, you must release all references to the <see cref="Spooker.Content.ContentManager"/>
		/// so the garbage collector can reclaim the memory that the <see cref="Spooker.Content.ContentManager"/> was occupying.</remarks>
		public void Dispose()
		{
			foreach (var loader in _loaders)
				loader.Dispose();

			_loaders.Clear();
		}
    }
}
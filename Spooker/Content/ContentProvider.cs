//-----------------------------------------------------------------------------
// ContentProvider.cs
//
// Spooker Open Source Game Framework
// Copyright (C) Indie Armory. All rights reserved.
// Website: http://indiearmory.com
// Other Contributors: krzat @ https://bitbucket.org/krzat
// License: MIT
//-----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using Spooker.Graphics;
using Spooker.Graphics.Particles;

namespace Spooker.Content
{
	////////////////////////////////////////////////////////////
	/// <summary>
	/// Used for storing game data such as textures, fonts and
	/// shaders. Stores only one type of data, depends on settings.
	/// </summary>
	////////////////////////////////////////////////////////////
    public class ContentProvider : IDisposable
    {
        private readonly Dictionary<string, object> _assets = new Dictionary<string, object>();
        
		/// <summary>File extension of loaded game data.</summary>
		public string Extension { get; set; }

		/// <summary>Folder containing loaded game data.</summary>
		public string Folder { get; set; }

		/// <summary>Function used to load data for this content provider.</summary>
		public Func<string, object> Load { get; set; }

		/// <summary>Value that determines if content provider will store data to cache.</summary>
		public bool Reuse { get; set; }

		/// <summary>Type of game data what content provider manages.</summary>
		public Type Type { get; set; }

		////////////////////////////////////////////////////////////
		/// <summary>
		/// Returns list of default loaders (texture, font and particle).
		/// </summary>
		////////////////////////////////////////////////////////////
		public static List<ContentProvider> Default
		{ 
			get 
			{
				// Initialize content loaders
				var temploaders = new List<ContentProvider>();

				var loader = new ContentProvider(typeof(Texture), "textures", "png") {Load = str => new Texture(str)};
			    temploaders.Add(loader);

				loader = new ContentProvider(typeof(SFML.Graphics.Font), "fonts", "ttf") {Load = str => new SFML.Graphics.Font(str)};
			    temploaders.Add(loader);

				loader = new ContentProvider(typeof(ParticleSettings), "particles", "sfp") {Load = str => new ParticleSettings(str)};
			    temploaders.Add(loader);

				return temploaders;
			}
		}

		////////////////////////////////////////////////////////////
		/// <summary>
		/// Creates new instance of Content Provider class without
		/// passing any arguments.
		/// </summary>
		////////////////////////////////////////////////////////////
        public ContentProvider() {}

		////////////////////////////////////////////////////////////
		/// <summary>
		/// Creates new instance of Content Provider class with
		/// passing multiple arguments.
		/// </summary>
		////////////////////////////////////////////////////////////
        public ContentProvider(Type type, string folder, string extension, bool reuse = true)
        {
            Type = type;
            Folder = folder;
            Extension = extension;
            Reuse = reuse;
        }

		////////////////////////////////////////////////////////////
		/// <summary>
		/// Disposes this instance of Content Provider class.
		/// </summary>
		////////////////////////////////////////////////////////////
        public void Dispose()
        {
            if (!Type.IsAssignableFrom(typeof (IDisposable))) return;
            foreach (object o in _assets.Values)
            {
                var disposable = o as IDisposable;
                if (disposable != null) disposable.Dispose();
            }
        }

		////////////////////////////////////////////////////////////
		/// <summary>
		/// Loads game data to cache or if already loaded, returns
		/// data from loader cache.
		/// </summary>
		////////////////////////////////////////////////////////////
        public virtual object Get(string name)
        {
            object result;
            if (Reuse && _assets.TryGetValue(name, out result))
            {
                return result;
            }
            result = Load(name);
            if (Reuse)
                _assets.Add(name, result);
            return result;
        }
    }
}
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
using Spooker.Time;

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
		public class Asset
		{
			public string Name;
			public float TimeToLive;
			public object Object;

			public Asset(string name, float timeToLive, object obj)
			{
				Name = name;
				TimeToLive = timeToLive;
				Object = obj;
			}
		}

		private Asset[] _assets = new Asset[10];
        
		/// <summary>File extension of loaded game data.</summary>
		public string Extension;

		/// <summary>Folder containing loaded game data.</summary>
		public string Folder;

		/// <summary>Function used to load data for this content provider.</summary>
		public Func<string, object> Load;

		/// <summary>Value that determines if content provider will store data to cache.</summary>
		public bool Reuse;

		/// <summary>Type of game data what content provider manages.</summary>
		public Type Type;

		/// <summary>Duration, after what will be each not re-used asset cleared from cache.</summary>
		public TimeSpan TTL = TimeSpan.Zero;

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

				temploaders.Add (new ContentProvider (typeof(Texture)) {
					Folder = "textures",
					Extension = "png",
					Load = str => new Texture (str)
				});

				temploaders.Add (new ContentProvider (typeof(SFML.Graphics.Font)) {
					Folder = "fonts",
					Extension = "ttf",
					Load = str => new SFML.Graphics.Font (str)
				});

				temploaders.Add (new ContentProvider (typeof(ParticleSettings)) {
					Folder = "particles",
					Extension = "sfp",
					Load = str => new ParticleSettings (str)
				});

				return temploaders;
			}
		}

		////////////////////////////////////////////////////////////
		/// <summary>
		/// Creates new instance of Content Provider class with
		/// passing multiple arguments.
		/// </summary>
		////////////////////////////////////////////////////////////
		public ContentProvider(Type type, bool reuse = true)
		{
			Type = type;
			Reuse = reuse;
			TTL = TimeSpan.Zero;
		}

		////////////////////////////////////////////////////////////
		/// <summary>
		/// Component uses this for updating itself
		/// </summary>
		////////////////////////////////////////////////////////////
		public void Update(float dt)
		{
			if (TTL == TimeSpan.Zero)
				return;

			for(var i = 0; i < _assets.Length - 1; i++)
			{
				if (_assets[i].TimeToLive > 0)
				{
					_assets[i].TimeToLive -= dt;
				}
				else if (_assets[i].TimeToLive <= 0)
					_assets[i] = null;
			}
		}
		////////////////////////////////////////////////////////////
		/// <summary>
		/// Disposes this instance of Content Provider class.
		/// </summary>
		////////////////////////////////////////////////////////////
        public void Dispose()
        {
            if (!Type.IsAssignableFrom(typeof (IDisposable))) return;
			foreach (var asset in _assets)
            {
				var disposable = asset.Object as IDisposable;
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
			var asset = FindAsset (name);
			if (asset != null)
            {
				asset.TimeToLive = (float)TTL.TotalMilliseconds;
				return asset.Object;
            }
		    asset = new Asset(name, (float)TTL.TotalMilliseconds, Load(name));
		    if (Reuse)
		        AddAsset(asset);
		    return asset.Object;
        }

		private void AddAsset(Asset asset)
		{
			for (var i = 0; i < _assets.Length - 1; i++)
			{
				if (_assets [i] == null)
				{
					_assets [i] = asset;
					return;
				}
			}
			var newSize = _assets.Length;
			Array.Resize (ref _assets, newSize);
			_assets[newSize] = asset;
		}

		private Asset FindAsset(string name)
		{
			for (var i = 0; i < _assets.Length - 1; i++)
			{
				if (_assets [i] != null && _assets [i].Name == name)
				{
					return _assets[i];
				}
			}
			return null;
		}
    }
}
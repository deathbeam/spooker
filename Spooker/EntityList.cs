//-----------------------------------------------------------------------------
// EntityList.cs
//
// Spooker Open Source Game Framework
// Copyright (C) Indie Armory. All rights reserved.
// Website: http://indiearmory.com
// Other Contributors: Theta
// License: MIT
//-----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using Spooker.Graphics;
using Spooker.Time;
using Spooker.Content;

namespace Spooker
{
	////////////////////////////////////////////////////////////
	/// <summary>
	/// Used for automated managing various classes with implemented
	/// drawable, updateable and loadable interfaces.
	/// </summary>
	////////////////////////////////////////////////////////////
	public class EntityList : SFML.Graphics.Drawable, IDrawable, IUpdateable, IDisposable, ILoadable
    {
		private readonly HashSet<IUpdateable> _updateables = new HashSet<IUpdateable>();
		private readonly HashSet<SFML.Graphics.Drawable> _sfdrawables = new HashSet<SFML.Graphics.Drawable>();
		private readonly HashSet<IDrawable> _drawables = new HashSet<IDrawable>();
		private readonly HashSet<ILoadable> _loadables = new HashSet<ILoadable>();

		/// <summary>
		/// Add the specified component to stack.
		/// </summary>
		/// <param name="component">Component.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
        public void Add<T>(T component)
        {
			var updateable = component as IUpdateable;
			var loadable = component as ILoadable;
			var drawable = component as IDrawable;
			var sfdrawable = component as SFML.Graphics.Drawable;

			if (updateable != null) _updateables.Add(updateable);
			if (drawable != null) _drawables.Add(drawable);
			if (sfdrawable != null) _sfdrawables.Add(sfdrawable);
			if (loadable != null) _loadables.Add(loadable);
        }

		/// <summary>
		/// Remove the specified component from stack.
		/// </summary>
		/// <param name="component">Component.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
        public void Remove<T>(T component)
        {
            _updateables.Remove(component as IUpdateable);
			_drawables.Remove(component as IDrawable);
			_sfdrawables.Remove(component as SFML.Graphics.Drawable);
			_loadables.Remove(component as ILoadable);

			var disposable = component as IDisposable;
			if (disposable != null) disposable.Dispose();
        }

		/// <summary>
		/// Component uses this for loading itself
		/// </summary>
		/// <param name="content">Content.</param>
		public void LoadContent(ContentManager content)
		{
			foreach (var loadable in _loadables)
				loadable.LoadContent (content);
		}

		/// <summary>
		/// Component uses this for updating itself.
		/// </summary>
		/// <param name="gameTime">Provides snapshot of timing values.</param>
		public void Update(GameTime gameTime)
        {
			foreach (var updateable in _updateables)
            	updateable.Update(gameTime);
        }

		/// <summary>
		/// Component uses this for drawing itself
		/// </summary>
		/// <param name="spriteBatch">Sprite batch.</param>
		/// <param name="effects">Effects.</param>
		public void Draw(SpriteBatch spriteBatch, SpriteEffects effects = SpriteEffects.None)
        {
			// Begins spriteBatch to avoid errors with drawables without
			// already started spriteBatch
			if (_drawables.Count > 0)
			{
				spriteBatch.Begin ();
				foreach (var drawable in _drawables)
					drawable.Draw (spriteBatch, effects);
				spriteBatch.End ();
			}
        }

		/// <summary>
		/// Component uses this for drawing itself
		/// </summary>
		/// <param name="graphicsDevice">Graphics device.</param>
		/// <param name="states">States.</param>
		public void Draw(SFML.Graphics.RenderTarget graphicsDevice, SFML.Graphics.RenderStates states)
		{
			foreach (var sfdrawable in _sfdrawables)
				sfdrawable.Draw (graphicsDevice, states);
		}


		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		/// <filterpriority>2</filterpriority>
		/// <remarks>Call <see cref="Dispose"/> when you are finished using the <see cref="Spooker.EntityList"/>. The
		/// <see cref="Dispose"/> method leaves the <see cref="Spooker.EntityList"/> in an unusable state. After calling
		/// <see cref="Dispose"/>, you must release all references to the <see cref="Spooker.EntityList"/> so the garbage
		/// collector can reclaim the memory that the <see cref="Spooker.EntityList"/> was occupying.</remarks>
        public void Dispose()
        {
			var toDispose = new HashSet<IDisposable>();

			foreach (var updateable in _updateables)
	            if (updateable is IDisposable) toDispose.Add(updateable as IDisposable);

			foreach (var drawable in _drawables)
	            if (drawable is IDisposable) toDispose.Add(drawable as IDisposable);

			foreach (var disposable in toDispose)
				disposable.Dispose();

            _updateables.Clear();
            _drawables.Clear();
            toDispose.Clear();
        }
    }
}
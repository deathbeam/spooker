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

namespace Spooker
{
	////////////////////////////////////////////////////////////
	/// <summary>
	/// Used for automated managing various classes with implemented
	/// drawable, updateable and loadable interface.
	/// </summary>
	////////////////////////////////////////////////////////////
	public class EntityList : IDrawable, IUpdateable, IDisposable
    {
		private readonly HashSet<IUpdateable> _updateables = new HashSet<IUpdateable>();
		private readonly HashSet<IDrawable> _drawables = new HashSet<IDrawable>();

		////////////////////////////////////////////////////////////
		/// <summary>
		/// Adds new entity to stack.
		/// </summary>
		////////////////////////////////////////////////////////////
        public void Add<T>(T component)
        {
			var updateable = component as IUpdateable;
			var loadable = component as ILoadable;
			var drawable = component as IDrawable;

			if (updateable != null) _updateables.Add(updateable);
			if (drawable != null) _drawables.Add(drawable);
			if (loadable != null) loadable.LoadContent();
        }

		////////////////////////////////////////////////////////////
		/// <summary>
		/// Removes specified entity to stack.
		/// </summary>
		////////////////////////////////////////////////////////////
        public void Remove<T>(T component)
        {
            _updateables.Remove(component as IUpdateable);
			_drawables.Remove(component as IDrawable);

			var disposable = component as IDisposable;
			if (disposable != null) disposable.Dispose();
        }

		////////////////////////////////////////////////////////////
		/// <summary>
		/// Updates all updateable entities in stack.
		/// </summary>
		////////////////////////////////////////////////////////////
		public void Update(GameTime gameTime)
        {
			foreach (var updateable in _updateables)
            	updateable.Update(gameTime);
        }

		////////////////////////////////////////////////////////////
		/// <summary>
		/// Draws all drawable entities in stack.
		/// </summary>
		////////////////////////////////////////////////////////////
		public void Draw(SpriteBatch spriteBatch, SpriteEffects effects = SpriteEffects.None)
        {
			foreach (var drawable in _drawables)
				drawable.Draw (spriteBatch, effects);
        }

		////////////////////////////////////////////////////////////
		/// <summary>
		/// Disposes all disposable entities in stack.
		/// </summary>
		////////////////////////////////////////////////////////////
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
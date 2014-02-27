/* File Description
 * Original Works/Author: Theta Engine
 * Other Contributors: Thomas Slusny
 * Author Website: 
 * License: 
*/

using System;
using System.Collections.Generic;
using SFML.Graphics;
using SFGL.Graphics;
using SFGL.Time;
using SFGL.Window;

namespace SFGL
{
	public class EntityList : Drawable, IUpdateable, IDisposable
    {
		private HashSet<IUpdateable> _updateables = new HashSet<IUpdateable>();
		private HashSet<Drawable> _drawables = new HashSet<Drawable>();

        public void Add<T>(T component)
        {
            IUpdateable updateable = component as IUpdateable;
            ILoadable loadable = component as ILoadable;
			Drawable drawable = component as Drawable;

			if (updateable != null) _updateables.Add(updateable);
			if (drawable != null) _drawables.Add(drawable);
			if (loadable != null) loadable.LoadContent();
        }

        public void Remove<T>(T component)
        {
            _updateables.Remove(component as IUpdateable);
			_drawables.Remove(component as Drawable);

            IDisposable disposable = component as IDisposable;
			if (disposable != null) disposable.Dispose();
        }

		public void Update(GameTime gameTime)
        {
            foreach (IUpdateable updateable in _updateables)
            {
				updateable.Update(gameTime);
            }
        }

		public void Draw(RenderTarget graphicsDevice, RenderStates states)
        {
			foreach (Drawable drawable in _drawables)
            {
				drawable.Draw(graphicsDevice, states);
            }
        }

        public void Dispose()
        {
            HashSet<IDisposable> toDispose = new HashSet<IDisposable>();

            foreach (IUpdateable updateable in _updateables)
            {
				if (updateable is IDisposable) toDispose.Add(updateable as IDisposable);
            }

			foreach (Drawable drawable in _drawables)
            {
				if (drawable is IDisposable) toDispose.Add(drawable as IDisposable);
            }

            foreach (IDisposable disposable in toDispose)
            {
                disposable.Dispose();
            }

            _updateables.Clear();
            _drawables.Clear();
            toDispose.Clear();
        }
    }
}
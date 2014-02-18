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
	public class EntityList : GameComponent, IUpdateable, IDrawable, IDisposable
    {
		private HashSet<IUpdateable> _updateables = new HashSet<IUpdateable>();
		private HashSet<IDrawable> _drawables = new HashSet<IDrawable>();
		private HashSet<Drawable> _sfmlDrawables = new HashSet<Drawable>();

		public EntityList(GameWindow game) : base (game) { }

        public void Add<T>(T component)
        {
            IUpdateable updateable = component as IUpdateable;
            IDrawable drawable = component as IDrawable;
            ILoadable loadable = component as ILoadable;
            Drawable sfmlDrawable = component as Drawable;

			if (sfmlDrawable != null) _sfmlDrawables.Add(sfmlDrawable);
			if (updateable != null) _updateables.Add(updateable);
			if (drawable != null) _drawables.Add(drawable);
			if (loadable != null) loadable.LoadContent();
        }

        public void Remove<T>(T component)
        {
            _updateables.Remove(component as IUpdateable);
            _drawables.Remove(component as IDrawable);
            _sfmlDrawables.Remove(component as Drawable);

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

		public void Draw()
        {
            foreach (IDrawable drawable in _drawables)
            {
				drawable.Draw();
            }

			foreach (Drawable sfmlDrawable in _sfmlDrawables)
            {
				Game.Draw(sfmlDrawable);
            }
        }

        public void Dispose()
        {
            HashSet<IDisposable> toDispose = new HashSet<IDisposable>();

            foreach (IUpdateable updateable in _updateables)
            {
				if (updateable is IDisposable) toDispose.Add(updateable as IDisposable);
            }

            foreach (IDrawable drawable in _drawables)
            {
				if (drawable is IDisposable) toDispose.Add(drawable as IDisposable);
            }

			foreach (Drawable sfmlDrawable in _sfmlDrawables)
            {
				if (sfmlDrawable is IDisposable) toDispose.Add(sfmlDrawable as IDisposable);
            }

            foreach (IDisposable disposable in toDispose)
            {
                disposable.Dispose();
            }

            _updateables.Clear();
            _drawables.Clear();
            _sfmlDrawables.Clear();
            toDispose.Clear();
        }
    }

}

//-----------------------------------------------------------------------------
// Sprite.cs
//
// Spooker Open Source Game Framework
// Copyright (C) Indie Armory. All rights reserved.
// Website: http://indiearmory.com
// Other Contributors: None
// License: MIT
//-----------------------------------------------------------------------------

using System;

namespace Spooker.Graphics
{
	/// <summary>
	/// Sprite.
	/// </summary>
	public class Sprite : Transformable, IDrawable
	{
		#region Public fields

		/// <summary>The texture.</summary>
		public Texture Texture;

		/// <summary>The color.</summary>
		public Color Color;

		/// <summary>The source rect.</summary>
		public Rectangle SourceRect;

		#endregion

		#region Properties

		/// <summary>
		/// Gets the size.
		/// </summary>
		/// <value>The size.</value>
		public Vector2 Size
		{
			get { return Texture.Size; }
		}

		/// <summary>
		/// Gets the destination rect.
		/// </summary>
		/// <value>The destination rect.</value>
		public Rectangle DestRect
		{
			get { return new Rectangle (
				(int)Position.X,
				(int)Position.Y,
				(int)Size.X,
				(int)Size.Y); }
		}

		/// <summary>
		/// Gets the AAB.
		/// </summary>
		/// <value>The AAB.</value>
		public Rectangle AABB
		{
			get
			{
				var pos = Transform.TransformPoint(Vector2.Zero);
				var size = new Vector2(SourceRect.Width, SourceRect.Height);
				var angle = Rotation;

				if (angle == 0.0f)
					return new Rectangle((int)pos.X, (int)pos.Y, (int)size.X, (int)size.Y);

				var b = RotatePoint (new Vector2(size.X, 0), angle);
				var c = RotatePoint (new Vector2(size.X, size.Y), angle);
				var d = RotatePoint (new Vector2(0, size.Y), angle);

				var left = (int)MathHelper.Min (0.0f, b.X, c.X, d.X);
				var top = (int)MathHelper.Min (0.0f, b.Y, c.Y, d.Y);
				var right = (int)MathHelper.Max (0.0f, b.X, c.X, d.X);
				var bottom = (int)MathHelper.Max (0.0f, b.Y, c.Y, d.Y);

				return new Rectangle(left + (int)pos.X, top + (int)pos.Y, right - left, bottom - top);
			}
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="Spooker.Graphics.Sprite"/> class.
		/// </summary>
		/// <param name="copy">Copy.</param>
		public Sprite (Sprite copy) : base(copy)
		{
			Texture = new Texture(copy.Texture);
			Color = new Color(copy.Color);
			SourceRect = copy.SourceRect;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Spooker.Graphics.Sprite"/> class.
		/// </summary>
		/// <param name="texture">Texture.</param>
		public Sprite (Texture texture)
		{
			Texture = new Texture (texture);
			Color = Color.White;
			SourceRect = new Rectangle (0, 0, (int)Texture.Size.X, (int)Texture.Size.Y);
		}

		#endregion

		#region Public methods

		/// <summary>
		/// Component uses this for drawing itself
		/// </summary>
		/// <param name="spriteBatch">Sprite batch.</param>
		/// <param name="effects">Effects.</param>
		public void Draw(SpriteBatch spriteBatch, SpriteEffects effects)
		{
			spriteBatch.Draw (
				Texture,
				Position,
				SourceRect,
				Color,
				Scale,
				Origin,
				Rotation,
				effects);
		}

		/// <summary>
		/// Intersects the specified sprite and alphaLimit.
		/// </summary>
		/// <param name="sprite">Sprite.</param>
		/// <param name="alphaLimit">Alpha limit.</param>
		public bool Intersects(Sprite sprite, float alphaLimit)
		{
			if (AABB.Intersects(sprite.AABB))
			{
				if (alphaLimit == 0) return true;

				var Intersection = Rectangle.Intersect (AABB, sprite.AABB);
				var o1SubRectSize = new Vector2(SourceRect.Width, SourceRect.Height);
				var o2SubRectSize = new Vector2(sprite.SourceRect.Width, sprite.SourceRect.Height);

				for (var i = Intersection.Left; i < Intersection.Left + Intersection.Width; i++)
				{
					for (var j = Intersection.Top; j < Intersection.Top + Intersection.Height; j++)
					{
						var o1v = Transform.TransformPoint(new Vector2(i, j));
						var o2v = sprite.Transform.TransformPoint(new Vector2(i, j));

						if (o1v.X > 0 && o1v.Y > 0 && o2v.X > 0 && o2v.Y > 0 &&
							o1v.X < o1SubRectSize.X && o1v.Y < o1SubRectSize.Y &&
							o2v.X < o2SubRectSize.X && o2v.Y < o2SubRectSize.Y)
						{
							var c1 = Texture.GetPixel((int)o1v.X, (int)o1v.Y);
							var c2 = sprite.Texture.GetPixel((int)o2v.X, (int)o2v.Y);
							if ((c1.A > alphaLimit) && (c2.A > alphaLimit))
								return true;
						}
					}
				}
			}

			return false;
		}

		#endregion

		#region Private methods

		private Vector2 RotatePoint(Vector2 vec, float angle)
		{
			angle = (float)MathHelper.ToDegrees(angle);
			return new Vector2(vec.X * (float)Math.Cos(angle) - vec.Y * (float)Math.Sin(angle),
				vec.X * (float)Math.Sin(angle) + vec.Y * (float)Math.Cos(angle));
		}

		#endregion
	}
}
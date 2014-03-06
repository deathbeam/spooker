using System;
using System.Collections.Generic;
using System.Diagnostics;
using SFML.Graphics;
using SFML.Window;
using SFGL.Utils;

namespace SFGL.Graphics
{
	////////////////////////////////////////////////////////////
	/// <summary>
	/// Provides optimized drawing of sprites
	/// </summary>
	////////////////////////////////////////////////////////////
	public class SpriteBatch : Drawable
	{
		#region Variables
		private struct BatchedTexture
		{
			public uint Count;
			public Texture Texture;
		}
		
		private List<BatchedTexture> textures = new List<BatchedTexture>();
		private Vertex[] vertices = new Vertex[100 * 4];
		private Texture activeTexture;
		private bool active;
		private uint queueCount;
		#endregion

		#region Properties
		////////////////////////////////////////////////////////////
		/// <summary>
		/// Returns count of all vertices in this vertex batch.
		/// </summary>
		////////////////////////////////////////////////////////////
		public int Count { get; private set; }

		////////////////////////////////////////////////////////////
		/// <summary>
		/// Returns or sets maximal possible number of verticles at
		/// once in this vertex batch. Max capacity is always divided
		/// by 4 (becouse of 4 verticle corners).
		/// </summary>
		////////////////////////////////////////////////////////////
		public int Max { get; set; }
		#endregion

		#region Constructors/Destructors
		////////////////////////////////////////////////////////////
		/// <summary>
		/// Creates new instance of SpriteBatch class.
		/// </summary>
		////////////////////////////////////////////////////////////
		public SpriteBatch()
		{
			Max = 40000;
		}

		////////////////////////////////////////////////////////////
		/// <summary>
		/// Creates new instance of SpriteBatch class.
		/// </summary>
		/// <param name="capacity">Maximal number of vertices in
		/// this vertex batch.</param>
		////////////////////////////////////////////////////////////
		public SpriteBatch(int capacity)
		{
			Max = capacity;
		}
		#endregion

		#region General functions
		////////////////////////////////////////////////////////////
		/// <summary>
		/// Begins this vertex batch, so we can draw sprites after.
		/// </summary>
		////////////////////////////////////////////////////////////
		public void Begin()
		{
			if (active) throw new Exception("Already active");
			Count = 0;
			textures.Clear();
			active = true;

			activeTexture = null;
		}

		////////////////////////////////////////////////////////////
		/// <summary>
		/// Ends this vertex batch, so we can not draw any more
		/// sprites.
		/// </summary>
		////////////////////////////////////////////////////////////
		public void End()
		{
			if (!active) throw new Exception("Call Begin first.");
			Enqueue();
			active = false;
		}

		////////////////////////////////////////////////////////////
		/// <summary>
		/// Draws all queued sprites for this vertex batch. Call
		/// this only after calling End().
		/// </summary>
		////////////////////////////////////////////////////////////
		public void Draw(RenderTarget target, RenderStates states)
		{
			if (active) throw new Exception("Call End first.");

			uint index = 0;
			foreach (var item in textures)
			{
				Debug.Assert(item.Count > 0);
				states.Texture = item.Texture;

				target.Draw(vertices, index, item.Count, PrimitiveType.Quads, states);
				index += item.Count;
			}
		}
		#endregion

		#region Drawing
		////////////////////////////////////////////////////////////
		/// <summary>
		/// Enqueue collection of sprites to this vertex batch.
		/// </summary>
		////////////////////////////////////////////////////////////
		public void Draw(IEnumerable<Sprite> sprites, SpriteEffects effects = SpriteEffects.None)
		{
			if (!active) throw new Exception("Call Begin first.");
			foreach (var s in sprites)
				Draw(s, effects);
		}

		////////////////////////////////////////////////////////////
		/// <summary>
		/// Enqueue sprite to this vertex batch.
		/// </summary>
		////////////////////////////////////////////////////////////
		public void Draw(Sprite sprite, SpriteEffects effects = SpriteEffects.None)
		{
			if (!active) throw new Exception("Call Begin first.");
			sprite.Scale = new Vector2f (
				sprite.Scale.X * ScaleEffectMultiplier.Get (effects).X,
				sprite.Scale.Y * ScaleEffectMultiplier.Get (effects).Y);
			WriteQuad(sprite.Texture, sprite.Position, sprite.TextureRect, sprite.Color, sprite.Scale, sprite.Origin,
				sprite.Rotation);
		}
		#endregion

		#region Helpers
		private void Enqueue()
		{
			if (queueCount > 0)
				textures.Add(new BatchedTexture
					{
						Texture = activeTexture,
						Count = queueCount
					});
			queueCount = 0;
		}

		private int Create(Texture texture)
		{
			if (!active) throw new Exception("Call Begin first.");

			if (texture != activeTexture)
			{
				Enqueue();
				activeTexture = texture;
			}

			if (Count >= (vertices.Length / 4))
			{
				if (vertices.Length < Max)
					Array.Resize(ref vertices, Math.Min(vertices.Length * 2, Max));
				else throw new Exception("Too many items");
			}

			queueCount += 4;
			return 4 * Count++;
		}

		private unsafe void WriteQuad(Texture texture, Vector2f position, IntRect rec, Color color, Vector2f scale,
			Vector2f origin, float rotation = 0)
		{

			var index = Create(texture);
			var sin = 0.0f;
			var cos= 1.0f;
			var pX = -origin.X * scale.X;
			var pY = -origin.Y * scale.Y;

			rotation = FloatMath.ToRadians(rotation);
			FloatMath.SinCos(rotation, out sin, out cos);
			scale.X *= rec.Width;
			scale.Y *= rec.Height;

			fixed (Vertex* fptr = vertices)
			{
				var ptr = fptr + index;

				ptr->Position.X = pX * cos - pY * sin + position.X;
				ptr->Position.Y = pX * sin + pY * cos + position.Y;
				ptr->TexCoords.X = rec.Left;
				ptr->TexCoords.Y = rec.Top;
				ptr->Color = color;
				ptr++;

				pX += scale.X;
				ptr->Position.X = pX * cos - pY * sin + position.X;
				ptr->Position.Y = pX * sin + pY * cos + position.Y;
				ptr->TexCoords.X = rec.Left + rec.Width;
				ptr->TexCoords.Y = rec.Top;
				ptr->Color = color;
				ptr++;

				pY += scale.Y;
				ptr->Position.X = pX * cos - pY * sin + position.X;
				ptr->Position.Y = pX * sin + pY * cos + position.Y;
				ptr->TexCoords.X = rec.Left + rec.Width;
				ptr->TexCoords.Y = rec.Top + rec.Height;
				ptr->Color = color;
				ptr++;

				pX -= scale.X;
				ptr->Position.X = pX * cos - pY * sin + position.X;
				ptr->Position.Y = pX * sin + pY * cos + position.Y;
				ptr->TexCoords.X = rec.Left;
				ptr->TexCoords.Y = rec.Top + rec.Height;
				ptr->Color = color;
			}
		}
		#endregion
	}
}
using System;
using System.Collections.Generic;
using SFML.Graphics;
using SFGL.Time;

namespace SFGL.Graphics
{
	public class AnimatedSprite : Sprite, IUpdateable
	{
		private int activeFrame;
		private List<Rectangle> frames;
		private Rectangle currentFrame;
		private float frameDelay;

		// how long it has been since initialize was called
		private float timeSinceStart;

		/// <summary>
		/// List with the frames of the animation
		/// </summary>
		public List<Rectangle> Frames
		{
			get { return frames; }
			set { frames = value; }
		}

		/// <summary>
		/// Defines for how long will be one frame drawn
		/// </summary>
		public float FrameDelay
		{
			get { return frameDelay; }
			set { frameDelay = value; }
		}

		public AnimatedSprite (Texture texture, IntRect rectangle) : base (texture, rectangle)
		{
		}

		public AnimatedSprite (Sprite copy) : base (copy)
		{
		}

		public AnimatedSprite () : base ()
		{
		}

		public AnimatedSprite (Texture texture) : base (texture)
		{
		}

		/// <summary>
		/// Allows the game component to update itself.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		public void Update(GameTime gameTime)
		{
			// calculate dt, the change in the since the last frame. the particle
			// updates will use this value.
			float dt = (float)gameTime.ElapsedGameTime.TotalMilliseconds;

			timeSinceStart += dt;

			// it's time to a next frame?
			if (timeSinceStart > frameDelay)
			{
				timeSinceStart -= frameDelay;
				activeFrame++;
				if (activeFrame == frames.Count)
				{
					activeFrame = 0;
				}
				// Get the current frame
				currentFrame = frames[activeFrame];
				base.TextureRect = new IntRect (
					currentFrame.X,
					currentFrame.Y,
					currentFrame.Width,
					currentFrame.Height);
			}
		}
	}
}
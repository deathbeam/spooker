//-----------------------------------------------------------------------------
// SpriteCollider.cs
//
// Spooker Open Source Game Framework
// Copyright (C) Indie Armory. All rights reserved.
// Website: http://indiearmory.com
// Other Contributors: exploit3r
// License: MIT
//-----------------------------------------------------------------------------

using System;
using Spooker.Graphics;

namespace Spooker.Physics
{
	public static class SpriteCollider
	{
		public static Rectangle GetAABB(Sprite sp)
		{
			// Store the sprite position
			var pos = sp.Transform.TransformPoint(Vector2.Zero);

			// Store the size so we can calculate the other corners
			var size = new Vector2(sp.SourceRect.Width, sp.SourceRect.Height);

			// Store the sprite current rotation
			var angle = sp.Rotation;

			// Bail out early if the sprite isn't rotated
			if (angle == 0.0f)
				return new Rectangle((int)pos.X, (int)pos.Y, (int)size.X, (int)size.Y);

			// Calculate the other points as vectors from (0,0)
			// Imagine sf::Vector2f A(0,0); but its not necessary
			// as rotation is around this point and rotate the points to
			// match the sprite rotation
			var b = RotatePoint (new Vector2(size.X, 0), angle);
			var c = RotatePoint (new Vector2(size.X, size.Y), angle);
			var d = RotatePoint (new Vector2(0, size.Y), angle);

			// Round off to int and set the four corners of our Rect
			var left = (int)MathHelper.Min (0.0f, b.X, c.X, d.X);
			var top = (int)MathHelper.Min (0.0f, b.Y, c.Y, d.Y);
			var right = (int)MathHelper.Max (0.0f, b.X, c.X, d.X);
			var bottom = (int)MathHelper.Max (0.0f, b.Y, c.Y, d.Y);

			// Create a Rect from out points and move it back to the correct position on the screen
			return new Rectangle(left + (int)pos.X, top + (int)pos.Y, right - left, bottom - top);
		}

		public static bool PixelIntersects(Sprite sp1, Sprite sp2, float alphaLimit)
		{
			// Get AABBs of the two sprites
			var object1Aabb = GetAABB(sp1);
			var object2Aabb = GetAABB(sp2);

			if (object1Aabb.Intersects(object2Aabb))
			{
				// Bail out now if AlphaLimit = 0
				if (alphaLimit == 0) return true;

				// We've got an intersection we need to process the pixels in that Rect.
				Rectangle Intersection;
				RectCollider.Intersect(ref object1Aabb, ref object2Aabb, out Intersection);

				// Get size of texture source rectangles
				var o1SubRectSize = new Vector2(sp1.SourceRect.Width, sp1.SourceRect.Height);
				var o2SubRectSize = new Vector2(sp2.SourceRect.Width, sp2.SourceRect.Height);

				// Loop through our pixels
				for (var i = Intersection.Left; i < Intersection.Left + Intersection.Width; i++)
				{
					for (var j = Intersection.Top; j < Intersection.Top + Intersection.Height; j++)
					{
						var o1v = sp1.Transform.TransformPoint(new Vector2(i, j));
						var o2v = sp2.Transform.TransformPoint(new Vector2(i, j));

						// Hack to make sure pixels fall within the Sprite's Image
						if (o1v.X > 0 && o1v.Y > 0 && o2v.X > 0 && o2v.Y > 0 &&
							o1v.X < o1SubRectSize.X && o1v.Y < o1SubRectSize.Y &&
							o2v.X < o2SubRectSize.X && o2v.Y < o2SubRectSize.Y)
						{
							// If both sprites have opaque pixels at the same point we've got a hit
							var c1 = sp1.Texture.GetPixel((int)o1v.X, (int)o1v.Y);
							var c2 = sp2.Texture.GetPixel((int)o2v.X, (int)o2v.Y);
							if ((c1.A > alphaLimit) && (c2.A > alphaLimit))
								return true;
						}
					}
				}
			}

			return false;
		}

		public static bool BoundingBoxTest(Sprite sp1, Sprite sp2)
		{

			Vector2 A, B, BL, TR;
			float t, x, a, dx, ext1, ext2;

			var halfSize1 = new Vector2(sp1.SourceRect.Width /2, sp1.SourceRect.Height /2);
			var halfSize2 = new Vector2(sp2.SourceRect.Width /2, sp2.SourceRect.Height /2);

			//Get the Angle we're working on
			var angle = sp1.Rotation - sp2.Rotation;
			var cosA = (float)Math.Cos(MathHelper.ToRadians(angle));
			var sinA = (float)Math.Sin(MathHelper.ToRadians(angle));

			//Normalise the Center of Object2 so its axis aligned an represented in
			//relation to Object 1 and get the Corners
			BL = TR = RotatePoint (sp2.Position - sp1.Position, sp2.Rotation);
			BL -= halfSize2;
			TR += halfSize2;

			//Calculate the vertices of the rotate Rect
			B = A = new Vector2 (-halfSize1.Y * sinA, halfSize1.Y * cosA);

			t = halfSize1.X * cosA;
			A.X += t;
			B.X -= t;

			t = halfSize1.X * sinA;
			A.Y += t;
			B.Y -= t;

			t = sinA * cosA;

			// verify that A is vertical min/max, B is horizontal min/max
			if (t < 0)
			{
				t = A.X;
				A.X = B.X;
				B.X = t;
				t = A.Y;
				A.Y = B.Y;
				B.Y = t;
			}

			// verify that B is horizontal minimum (leftest-vertex)
			if (sinA < 0)
			{
				B.X = -B.X;
				B.Y = -B.Y;
			}

			// if rr2(ma) isn't in the horizontal range of
			// colliding with rr1(r), collision is impossible
			if (B.X > TR.X || B.X > -BL.X) return false;

			// if rr1(r) is axis-aligned, vertical min/max are easy to get
			if (t == 0)
			{
				ext1 = A.Y;
				ext2 = -ext1;
			}// else, find vertical min/max in the range [BL.x, TR.x]
			else
			{
				x = BL.X - A.X;
				a = TR.X - A.X;
				ext1 = A.Y;
				// if the first vertical min/max isn't in (BL.x, TR.x), then
				// find the vertical min/max on BL.x or on TR.x
				if (a * x > 0)
				{
					dx = A.X;
					if (x < 0)
					{
						dx -= B.X;
						ext1 -= B.Y;
						x = a;
					}
					else
					{
						dx += B.X;
						ext1 += B.Y;
					}
					ext1 *= x;
					ext1 /= dx;
					ext1 += A.Y;
				}

				x = BL.X + A.X;
				a = TR.X + A.X;
				ext2 = -A.Y;
				// if the second vertical min/max isn't in (BL.x, TR.x), then
				// find the local vertical min/max on BL.x or on TR.x
				if (a * x > 0)
				{
					dx = -A.X;
					if (x < 0)
					{
						dx -= B.X;
						ext2 -= B.Y;
						x = a;
					}
					else
					{
						dx += B.X;
						ext2 += B.Y;
					}
					ext2 *= x;
					ext2 /= dx;
					ext2 -= A.Y;
				}
			}

			// check whether rr2(ma) is in the vertical range of colliding with rr1(r)
			// (for the horizontal range of rr2)
			return !((ext1 < BL.Y && ext2 < BL.Y) ||
				(ext1 > TR.Y && ext2 > TR.Y));

		}
		
		private static Vector2 RotatePoint(Vector2 vec, float angle)
		{
			angle = (float)MathHelper.ToDegrees(angle);
			return new Vector2(vec.X * (float)Math.Cos(angle) - vec.Y * (float)Math.Sin(angle),
				vec.X * (float)Math.Sin(angle) + vec.Y * (float)Math.Cos(angle));
		}
	}
}


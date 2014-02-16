/* File Description
 * Original Works/Author: eXpl0it3r
 * Other Contributors: Thomas Slusny
 * Author Website: http://sfml-dev.org
 * License: Zlib
*/

using SFML.Graphics;
using SFML.Window;
using System;
using System.Collections;

namespace SFGL.Physics
{

	public class CollisionDetection
	{

		public const float RADIANS_PER_DEGREE = (float)Math.PI / 180f;

		private static Hashtable images = new Hashtable();

		public static IntRect GetAABB(Sprite sp)
		{
			Vector2f pos = sp.Transform.TransformPoint(new Vector2f(0, 0));

			//Store the size so we can calculate the other corners
			Vector2f size = new Vector2f(sp.TextureRect.Width, sp.TextureRect.Height);

			float Angle = sp.Rotation;

			//Bail out early if the sprite isn't rotated
			if (Angle == 0.0f)
			{
				return new IntRect((int)pos.X, (int)pos.Y, (int)(sp.TextureRect.Width),
					(int)(sp.TextureRect.Height));
			}

			//Calculate the other points as vectors from (0,0)
			//Imagine sf::Vector2f A(0,0); but its not necessary
			//as rotation is around this point.
			Vector2f B = new Vector2f(sp.TextureRect.Width, 0);
			Vector2f C = new Vector2f(sp.TextureRect.Width, sp.TextureRect.Height);
			Vector2f D = new Vector2f(0, sp.TextureRect.Height);

			//Rotate the points to match the sprite rotation
			B = RotatePoint(B, Angle);
			C = RotatePoint(C, Angle);
			D = RotatePoint(D, Angle);

			//Round off to int and set the four corners of our Rect
			int Left = (int)(MinValue(0.0f, B.X, C.X, D.X));
			int Top = (int)(MinValue(0.0f, B.Y, C.Y, D.Y));
			int Right = (int)(MaxValue(0.0f, B.X, C.X, D.X));
			int Bottom = (int)(MaxValue(0.0f, B.Y, C.Y, D.Y));

			//Create a Rect from out points and move it back to the correct position on the screen
			IntRect AABB = new IntRect(Left, Top, Right - Left, Bottom - Top);
			AABB.Left += (int)pos.X;
			AABB.Top += (int)pos.Y;

			//AABB.Offset((int)(pos.X), (int)(pos.Y));
			return AABB;
		}

		public static float MinValue(float a, float b, float c, float d)
		{
			float min = a;

			min = (b < min ? b : min);
			min = (c < min ? c : min);
			min = (d < min ? d : min);

			return min;
		}

		public static float MaxValue(float a, float b, float c, float d)
		{
			float max = a;

			max = (b > max ? b : max);
			max = (c > max ? c : max);
			max = (d > max ? d : max);

			return max;
		}

		public static Vector2f RotatePoint(Vector2f p, float Angle)
		{
			Angle *= RADIANS_PER_DEGREE;
			return new Vector2f(p.X * (float)Math.Cos(Angle) - p.Y * (float)Math.Sin(Angle),
				p.X * (float)Math.Sin(Angle) + p.Y * (float)Math.Cos(Angle));
		}

		public static Vector2f ScalePoint(Vector2f p, float scale)
		{
			return new Vector2f(p.X * scale, p.Y * scale);
		}

		public static bool PixelPerfectTest(Sprite sp1, Sprite sp2, byte AlphaLimit)
		{
			//Get AABBs of the two sprites
			IntRect Object1AABB = GetAABB(sp1);
			IntRect Object2AABB = GetAABB(sp2);

			IntRect Intersection;

			if (Object1AABB.Intersects(Object2AABB, out Intersection))
			{
				//We've got an intersection we need to process the pixels
				//In that Rect.

				//Bail out now if AlphaLimit = 0
				if (AlphaLimit == 0) return true;

				//There are a few hacks here, sometimes the TransformToLocal returns negative points
				//Or Points outside the image.  We need to check for these as they print to the error console
				//which is slow, and then return black which registers as a hit.

				IntRect O1SubRect = sp1.TextureRect;
				IntRect O2SubRect = sp2.TextureRect;

				//Vector2f O1SubRectSize = new Vector2f(sp1.Texture.Width, sp1.Texture.Height);
				//Vector2f O2SubRectSize = new Vector2f(sp2.Texture.Width, sp2.Texture.Height);
				Vector2i O1SubRectSize = new Vector2i(O1SubRect.Width, O1SubRect.Height);
				Vector2i O2SubRectSize = new Vector2i(O2SubRect.Width, O2SubRect.Height);

				Vector2f o1v;
				Vector2f o2v;

				if(!images.ContainsKey(sp1.Texture)) images.Add(sp1.Texture, sp1.Texture.CopyToImage());
				if(!images.ContainsKey(sp2.Texture)) images.Add(sp2.Texture, sp2.Texture.CopyToImage());

				Image im1 = (Image)images[sp1.Texture];
				Image im2 = (Image)images[sp2.Texture];

				//Loop through our pixels
				for (int i = Intersection.Left; i < Intersection.Left + Intersection.Width; i++)
				{
					for (int j = Intersection.Top; j < Intersection.Top + Intersection.Height; j++)
					{

						o1v = sp1.Transform.TransformPoint(new Vector2f(i, j)); //Creating Objects each loop :(
						o2v = sp2.Transform.TransformPoint(new Vector2f(i, j));

						//Hack to make sure pixels fall within the Sprite's Image
						if (o1v.X > 0 && o1v.Y > 0 && o2v.X > 0 && o2v.Y > 0 &&
							o1v.X < O1SubRectSize.X && o1v.Y < O1SubRectSize.Y &&
							o2v.X < O2SubRectSize.X && o2v.Y < O2SubRectSize.Y)
						{

							//If both sprites have opaque pixels at the same point we've got a hit
							Color c1 = im1.GetPixel((uint)(o1v.X), (uint)(o1v.Y));
							Color c2 = im2.GetPixel((uint)(o2v.X), (uint)(o2v.Y));
							if ((c1.A > AlphaLimit) &&
								(c2.A > AlphaLimit))
							{
								return true;
							}
						}
					}
				}
				return false;
			}
			return false;
		}

		public static bool CircleTest(Sprite sp1, Sprite sp2, float? scale)
		{
			//Simplest circle test possible
			//Distance between points <= sum of radius

			if(scale == null) scale = 1f;

			float Radius1 = (sp1.TextureRect.Width + sp1.TextureRect.Height) / 4;
			float Radius2 = ((sp2.TextureRect.Width + sp2.TextureRect.Height) / 4) * scale.Value;
			float xd = sp1.Position.X - sp2.Position.X;
			float yd = sp1.Position.Y - sp2.Position.Y;

			return (Math.Sqrt(xd * xd + yd * yd) <= (Radius1 + Radius2));
		}

		public static bool BoundingBoxTest(Sprite sp1, Sprite sp2)
		{

			Vector2f A = new Vector2f();
			Vector2f B = new Vector2f();
			Vector2f C = new Vector2f();
			Vector2f BL= new Vector2f();
			Vector2f TR = new Vector2f();
			Vector2f HalfSize1 = new Vector2f(sp1.TextureRect.Width, sp1.TextureRect.Height);
			Vector2f HalfSize2 = new Vector2f(sp2.TextureRect.Width, sp2.TextureRect.Height);

			//For somereason the Vector2d divide by operator
			//was misbehaving
			//Doing it manually
			HalfSize1.X /= 2;
			HalfSize1.Y /= 2;
			HalfSize2.X /= 2;
			HalfSize2.Y /= 2;

			//Get the Angle we're working on
			float Angle = sp1.Rotation - sp2.Rotation;
			float CosA = (float)Math.Cos(Angle * RADIANS_PER_DEGREE);
			float SinA = (float)Math.Sin(Angle * RADIANS_PER_DEGREE);

			float t, x, a, dx, ext1, ext2;

			//Normalise the Center of Object2 so its axis aligned an represented in
			//relation to Object 1
			C = sp2.Position;

			C.X -= sp1.Position.X;
			C.Y -= sp1.Position.Y;

			C = RotatePoint(C, sp2.Rotation);

			//Get the Corners
			BL = TR = C;
			BL.X -= HalfSize2.X;
			BL.Y -= HalfSize2.Y;

			TR.X += HalfSize2.X;
			TR.Y += HalfSize2.Y;

			//Calculate the vertices of the rotate Rect
			A.X = -HalfSize1.Y * SinA;
			B.X = A.X;
			t = HalfSize1.X * CosA;
			A.X += t;
			B.X -= t;

			A.Y = HalfSize1.Y * CosA;
			B.Y = A.Y;
			t = HalfSize1.X * SinA;
			A.Y += t;
			B.Y -= t;

			t = SinA * CosA;

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
			if (SinA < 0)
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

	}

}
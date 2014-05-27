//-----------------------------------------------------------------------------
// Rectangle.cs
//
// Copyright (C) 2006 The Mono.Xna Team. All rights reserved.
// Website: http://monogame.com
// Other Contributors: deathbeam @ http://indiearmory.com
// License: MIT
//-----------------------------------------------------------------------------

using System;

namespace Spooker
{
	[Serializable]
	public struct Rectangle : IEquatable<Rectangle>
	{
        #region Public Fields

        public int X;
        public int Y;
        public int Width;
        public int Height;

        #endregion Public Fields


        #region Public Properties

        public int Left
		{
            get { return X; }
        }

        public int Right
		{
            get { return (X + Width); }
        }

        public int Top
		{
            get { return Y; }
        }

        public int Bottom
		{
            get { return (Y + Height); }
        }

		public Vector2 Location
		{
			get { return new Vector2(X, Y); }
		}

		public Vector2 Size
		{
			get { return new Vector2(Width, Height); }
		}

		public Vector2 Center
		{
			get { return new Vector2(X + (Width / 2), Y + (Height / 2)); }
		}

		public bool IsEmpty
		{
			get { return ((((Width == 0) && (Height == 0)) && (X == 0)) && (Y == 0)); }
		}

        #endregion Public Properties


		#region SFML Helpers

		internal Rectangle(SFML.Graphics.IntRect copy)
		{
			X = copy.Left;
			Y = copy.Top;
			Width = copy.Width;
			Height = copy.Height;
		}

		internal SFML.Graphics.IntRect ToSfml()
		{
			return new SFML.Graphics.IntRect(X, Y, Width, Height);
		}

		#endregion SFML Helpers


        #region Constructors

        public Rectangle(int x, int y, int width, int height)
		{
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

		public static Rectangle Empty
		{
			get { return new Rectangle(0, 0, 0, 0); }
		}

        #endregion Constructors


		#region Static Methods

		public static Rectangle Union(Rectangle value1, Rectangle value2)
		{
			var x = Math.Min(value1.X, value2.X);
			var y = Math.Min(value1.Y, value2.Y);
			return new Rectangle(x, y, Math.Max(value1.Right, value2.Right) - x, Math.Max(value1.Bottom, value2.Bottom) - y);
		}

		#endregion Static Methods

		
        #region Public Methods

        public bool Contains(int x, int y)
		{
            return ((((X <= x) && (x < (X + Width))) && (Y <= y)) && (y < (Y + Height)));
        }

        public bool Contains(Point value)
		{
            return ((((X <= value.X) && (value.X < (X + Width))) && (Y <= value.Y)) && (value.Y < (Y + Height)));
        }

        public bool Contains(Rectangle value)
		{
            return ((((X <= value.X) && ((value.X + value.Width) <= (X + Width))) && (Y <= value.Y)) && ((value.Y + value.Height) <= (Y + Height)));
        }

        public void Offset(Point offset)
		{
            X += offset.X;
            Y += offset.Y;
        }

        public void Offset(int offsetX, int offsetY)
		{
            X += offsetX;
            Y += offsetY;
        }

        public void Inflate(int horizontalValue, int verticalValue)
		{
            X -= horizontalValue;
            Y -= verticalValue;
            Width += horizontalValue * 2;
            Height += verticalValue * 2;
        }

		public static Rectangle Intersect(Rectangle rectangle1, Rectangle rectangle2)
		{
		    if (rectangle1.Intersects(rectangle2))
			{
				var rightSide = Math.Min(rectangle1.X + rectangle1.Width, rectangle2.X + rectangle2.Width);
				var leftSide = Math.Max(rectangle1.X, rectangle2.X);
				var topSide = Math.Max(rectangle1.Y, rectangle2.Y);
				var bottomSide = Math.Min(rectangle1.Y + rectangle1.Height, rectangle2.Y + rectangle2.Height);
				return new Rectangle(leftSide, topSide, rightSide - leftSide, bottomSide - topSide);
			}
		    return Empty;
		}

	    public bool Intersects(Rectangle rectangle)
		{
			return rectangle.Left < Right &&
				   Left < rectangle.Right &&
				   rectangle.Top < Bottom &&
				   Top < rectangle.Bottom;
		}

        public bool Equals(Rectangle other)
		{
			return (X == other.X) &&
				   (Y == other.Y) &&
				   (Width == other.Width) &&
				   (Height == other.Height);
        }

        public override bool Equals(object obj)
		{
			return (obj is Rectangle) && Equals((Rectangle)obj);
        }

        public override string ToString()
		{
            return string.Format("{{X:{0} Y:{1} Width:{2} Height:{3}}}", X, Y, Width, Height);
        }

        public override int GetHashCode()
		{
			return unchecked((int)((uint)Left ^
				(((uint)Top << 13) | ((uint)Top >> 19)) ^
				(((uint)Width << 26) | ((uint)Width >>  6)) ^
				(((uint)Height <<  7) | ((uint)Height >> 25))));
        }

        #endregion Public Methods


		#region Operators

		public static bool operator ==(Rectangle a, Rectangle b)
		{
			return a.Equals (b);
		}

		public static bool operator !=(Rectangle a, Rectangle b)
		{
			return !a.Equals (b);
		}

		#endregion Operators
    }
}
/* File Description
 * Original Works/Author: Laurent Gomilla
 * Other Contributors: Thomas Slusny
 * Author Website: http://sfml-dev.org
 * License: 
*/

using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using SFML.Graphics;

namespace SFGL.Graphics
{
	////////////////////////////////////////////////////////////
	/// <summary>
    /// An implementation of SFML.Graphics.IntRect with extended
    /// functionality.
	/// </summary>
	////////////////////////////////////////////////////////////
	[StructLayout(LayoutKind.Sequential)]
	public struct Rectangle : IEquatable<Rectangle>
	{
		#region Static members
		/// <summary>Returns a Rectangle with all of its components set to zero.</summary>
		public static Rectangle Zero
		{
			get { return new Rectangle(0,0,0,0); }
		}

		/// <summary>Returns a Rectangle with both of its components set to one.</summary>
		public static Rectangle One
		{
			get { return new Rectangle(1,1,1,1); }
		}
		#endregion

		#region Constructors
		////////////////////////////////////////////////////////////
		/// <summary>
		/// Construct the rectangle from its coordinates
		/// </summary>
		/// <param name="x">X coordinate of the rectangle</param>
		/// <param name="y">Y coordinate of the rectangle</param>
		/// <param name="width">Width of the rectangle</param>
		/// <param name="height">Height of the rectangle</param>
		////////////////////////////////////////////////////////////
		public Rectangle(int x, int y, int width, int height)
		{
			X   = x;
			Y    = y;
			Width  = width;
			Height = height;
		}

		////////////////////////////////////////////////////////////
		/// <summary>
		/// Construct the rectangle from its coordinates
		/// </summary>
		/// <param name="x">X coordinate of the rectangle</param>
		/// <param name="y">Y coordinate of the rectangle</param>
		/// <param name="width">Width of the rectangle</param>
		/// <param name="height">Height of the rectangle</param>
		////////////////////////////////////////////////////////////
		public Rectangle(float x, float y, float width, float height)
		{
			X   = (int)x;
			Y    = (int)y;
			Width  = (int)width;
			Height = (int)height;
		}

		////////////////////////////////////////////////////////////
		/// <summary>
		/// Construct the rectangle from its coordinates
		/// </summary>
		/// <param name="x">X coordinate of the rectangle</param>
		/// <param name="y">Y coordinate of the rectangle</param>
		/// <param name="width">Width of the rectangle</param>
		/// <param name="height">Height of the rectangle</param>
		////////////////////////////////////////////////////////////
		public Rectangle(uint x, uint y, uint width, uint height)
		{
			X   = (int)x;
			Y    = (int)y;
			Width  = (int)width;
			Height = (int)height;
		}

		////////////////////////////////////////////////////////////
		/// <summary>
		/// Construct the rectangle from another rectangle
		/// </summary>
		/// <param name="copy">Rectangle from what will this
		/// rectangle constructs</param>
		////////////////////////////////////////////////////////////
		public Rectangle(Rectangle copy)
		{
			X   = copy.X;
			Y    = copy.Y;
			Width  = copy.Width;
			Height = copy.Height;
		}

		////////////////////////////////////////////////////////////
		/// <summary>
		/// Construct the rectangle from another rectangle
		/// </summary>
		/// <param name="copy">Rectangle from what will this
		/// rectangle constructs</param>
		////////////////////////////////////////////////////////////
		public Rectangle(IntRect copy)
		{
			X   = copy.Left;
			Y    = copy.Top;
			Width  = copy.Width;
			Height = copy.Height;
		}

		////////////////////////////////////////////////////////////
		/// <summary>
		/// Construct the rectangle from another rectangle
		/// </summary>
		/// <param name="copy">Rectangle from what will this
		/// rectangle constructs</param>
		////////////////////////////////////////////////////////////
		public Rectangle(FloatRect copy)
		{
			X   = (int)copy.Left;
			Y    = (int)copy.Top;
			Width  = (int)copy.Width;
			Height = (int)copy.Height;
		}
		#endregion

		#region Functions
		////////////////////////////////////////////////////////////
		/// <summary>
		/// Check if a point is inside the rectangle's area
		/// </summary>
		/// <param name="x">X coordinate of the point to test</param>
		/// <param name="y">Y coordinate of the point to test</param>
		/// <returns>True if the point is inside</returns>
		////////////////////////////////////////////////////////////
		public bool Contains(int x, int y)
		{
			return (x >= X) && (x < X + Width) && (y >= Y) && (y < Y + Height);
		}

		////////////////////////////////////////////////////////////
		/// <summary>
		/// Determines whether this Rectangle entirely contains a
		/// specified Rectangle.
		/// </summary>
		/// <param name="value">The Rectangle to evaluate.</param>
		////////////////////////////////////////////////////////////
		public bool Contains(Rectangle value)
		{
			return ((((X <= value.X) && ((value.X + value.Width) <= (X + Width))) && (Y <= value.Y)) &&
				((value.Y + value.Height) <= (Y + Height)));
		}

		////////////////////////////////////////////////////////////
		/// <summary>
		/// Check intersection between two rectangles
		/// </summary>
		/// <param name="rect"> Rectangle to test</param>
		/// <returns>True if rectangles overlap</returns>
		////////////////////////////////////////////////////////////
		public bool Intersects(Rectangle rect)
		{
			// Compute the intersection boundaries
			int x   = Math.Max(X,         rect.X);
			int y    = Math.Max(Y,          rect.Y);
			int right  = Math.Min(X + Width, rect.X + rect.Width);
			int bottom = Math.Min(Y + Height, rect.Y + rect.Height);

			return (x < right) && (y < bottom);
		}

		////////////////////////////////////////////////////////////
		/// <summary>
		/// Check intersection between two rectangles
		/// </summary>
		/// <param name="rect"> Rectangle to test</param>
		/// <param name="overlap">Rectangle to be filled with overlapping rect</param>
		/// <returns>True if rectangles overlap</returns>
		////////////////////////////////////////////////////////////
		public bool Intersects(Rectangle rect, out Rectangle overlap)
		{
			// Compute the intersection boundaries
			int x   = Math.Max(X,         rect.X);
			int y    = Math.Max(Y,          rect.Y);
			int right  = Math.Min(X + Width, rect.X + rect.Width);
			int bottom = Math.Min(Y + Height, rect.Y + rect.Height);

			// If the intersection is valid (positive non zero area), then there is an intersection
			if ((x < right) && (y < bottom))
			{
				overlap.X   = x;
				overlap.Y    = y;
				overlap.Width  = right - x;
				overlap.Height = bottom - y;
				return true;
			}
			else
			{
				overlap.X   = 0;
				overlap.Y    = 0;
				overlap.Width  = 0;
				overlap.Height = 0;
				return false;
			}
		}

		////////////////////////////////////////////////////////////
		/// <summary>Determines whether the specified Object is equal
		///  to the Rectangle.
		/// </summary>
		/// <param name="other">The Object to compare with the current
		/// Rectangle.</param>
		////////////////////////////////////////////////////////////
		public bool Equals(Rectangle other)
		{
			return ((((X == other.X) && (Y == other.Y)) && (Width == other.Width)) && (Height == other.Height));
		}

		////////////////////////////////////////////////////////////
		/// <summary>Returns a value that indicates whether the current
		/// instance is equal to a specified object.
		/// </summary>
		/// <param name="obj">Object to make the comparison with.</param>
		////////////////////////////////////////////////////////////
		public override bool Equals(object obj)
		{
			var flag = false;
			if (obj is Rectangle)
				flag = Equals((Rectangle)obj);
			return flag;
		}

		////////////////////////////////////////////////////////////
		/// <summary>
		/// Gets the hash code for this object.
		/// </summary>
		/// <returns>Hash code of the object</returns>
		////////////////////////////////////////////////////////////
		public override int GetHashCode()
		{
			return (((X.GetHashCode() + Y.GetHashCode()) + Width.GetHashCode()) + Height.GetHashCode());
		}

		////////////////////////////////////////////////////////////
		/// <summary>
		/// Provide a string describing the object
		/// </summary>
		/// <returns>String description of the object</returns>
		////////////////////////////////////////////////////////////
		public override string ToString()
		{
			return "[Rectangle]" +
				" X(" + X + ")" +
				" Y(" + Y + ")" +
				" Width(" + Width + ")" +
				" Height(" + Height + ")";
		}
		#endregion

		#region Operators
		////////////////////////////////////////////////////////////
		/// <summary>
		/// Compares two rectangles for equality.
		/// </summary>
		/// <param name="a">Source rectangle.</param>
		/// <param name="b">Source rectangle.</param>
		////////////////////////////////////////////////////////////
		public static bool operator ==(Rectangle a, Rectangle b)
		{
			return ((((a.X == b.X) && (a.Y == b.Y)) && (a.Width == b.Width)) && (a.Height == b.Height));
		}

		////////////////////////////////////////////////////////////
		/// <summary>
		/// Compares two rectangles for inequality.
		/// </summary>
		/// <param name="a">Source rectangle.</param>
		/// <param name="b">Source rectangle.</param>
		////////////////////////////////////////////////////////////
		public static bool operator !=(Rectangle a, Rectangle b)
		{
			if (((a.X == b.X) && (a.Y == b.Y)) && (a.Width == b.Width))
				return (a.Height != b.Height);
			return true;
		}
		#endregion

		#region Properties
		/// <summary>X coordinate of the rectangle</summary>
		public int X;

		/// <summary>Y coordinate of the rectangle</summary>
		public int Y;

		/// <summary>Width of the rectangle</summary>
		public int Width;

		/// <summary>Height of the rectangle</summary>
		public int Height;

		/// <summary>Returns the x-coordinate of the left side of the rectangle.</summary>
		public int Left
		{
			get { return X; }
		}

		/// <summary>Returns the x-coordinate of the right side of the rectangle.</summary>
		public int Right
		{
			get { return (X + Width); }
		}

		/// <summary>Returns the y-coordinate of the top of the rectangle.</summary>
		public int Top
		{
			get { return Y; }
		}

		/// <summary>Returns the y-coordinate of the bottom of the rectangle.</summary>
		public int Bottom
		{
			get { return (Y + Height); }
		}
		#endregion
	}
}
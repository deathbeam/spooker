//-----------------------------------------------------------------------------
// Point.cs
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
    public struct Point : IEquatable<Point>
	{
        #region Public Fields

        public int X;
        public int Y;

        #endregion Public Fields


        #region Properties

        public static Point Zero
		{
			get { return new Point(0, 0); }
        }

        #endregion Properties


        #region Constructors

        public Point(int x, int y)
		{
            X = x;
            Y = y;
        }

        #endregion Constructors


		#region Operators

        public static bool operator ==(Point a, Point b)
		{
            return a.Equals(b);
        }

        public static bool operator !=(Point a, Point b)
		{
            return !a.Equals(b);
        }

		#endregion Operators

		#region Public methods

        public bool Equals(Point other)
		{
            return ((X == other.X) && (Y == other.Y));
        }

        public override bool Equals(object obj)
		{
            return (obj is Point) && Equals((Point)obj);
        }

        public override int GetHashCode()
		{
            return X ^ Y;
        }

        public override string ToString()
		{
            return string.Format("{{X:{0} Y:{1}}}", X, Y);
        }

		#endregion Public methods
    }
}



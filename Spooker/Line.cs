//-----------------------------------------------------------------------------
// Line.cs
//
// Spooker Open Source Game Framework
// Copyright (C) Indie Armory. All rights reserved.
// Website: http://indiearmory.com
// Other Contributors: None
// License: MIT
//-----------------------------------------------------------------------------

using System;

namespace Spooker
{
	[Serializable]
	public struct Line : IEquatable<Line>
	{
		#region Public fields

		public float X1, X2, Y1, Y2;

		#endregion

		#region Constructors/Destructors

		public Line(float x1, float y1, float x2, float y2)
		{
			X1 = x1;
			X2 = x2;
			Y1 = y1;
			Y2 = y2;
		}

		#endregion

		#region Public methods

		public bool Equals(Line other)
		{
			return ((X1 == other.X1) && (X2 == other.X2) && (Y1 == other.Y1) && (Y2 == other.Y2));
		}

		public override bool Equals(object obj)
		{
			return (obj is Line) && Equals((Line)obj);
		}

		public override int GetHashCode()
		{
			return X1.GetHashCode() + X2.GetHashCode() + Y1.GetHashCode() + Y2.GetHashCode();
		}

		#endregion Public methods
	}
}
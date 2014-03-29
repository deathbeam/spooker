//-----------------------------------------------------------------------------
// Polygon.cs
//
// Spooker Open Source Game Framework
// Copyright (C) Indie Armory. All rights reserved.
// Website: http://indiearmory.com
// Other Contributors: None
// License: MIT
//-----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;

namespace Spooker
{
	[Serializable]
	public struct Polygon : IEquatable<Polygon>
	{
		#region Public fields

		public List<Line> Lines;

		#endregion

		#region Constructors/Destructors

		public Polygon (List<Line> lines)
		{
			Lines = lines;
		}

		#endregion

		#region Public methods

		public bool Equals(Polygon other)
		{
			return Lines.All(t => other.Lines.All(t.Equals));
		}

		public override bool Equals(object obj)
		{
			return (obj is Polygon) && Equals((Polygon)obj);
		}

		public override int GetHashCode()
		{
		    return Lines.Sum(line => line.GetHashCode());
		}

	    #endregion
	}
}
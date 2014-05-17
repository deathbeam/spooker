//-----------------------------------------------------------------------------
// GameSpan.cs
//
// Spooker Open Source Game Framework
// Copyright (C) Indie Armory. All rights reserved.
// Website: http://indiearmory.com
// Other Contributors: Zachariah Brown @ http://www.zbrown.net/projects
// License: MIT
//-----------------------------------------------------------------------------

using System;
using System.Diagnostics;

namespace Spooker.Time
{
	////////////////////////////////////////////////////////////
	/// <summary>
	/// Contains game time span.
	/// </summary>
	////////////////////////////////////////////////////////////
	public struct GameSpan : IEquatable<GameSpan>
	{
		#region Private Fields

		private long _elapsedticks;

		#endregion Private Fields


		#region Properties

		/// <summary>
		/// Gets or sets the ticks.
		/// </summary>
		/// <value>The ticks.</value>
		public long Ticks
		{
			get { return _elapsedticks; }
			set { _elapsedticks = value; }
		}

		/// <summary>
		/// Gets or sets the milliseconds.
		/// </summary>
		/// <value>The milliseconds.</value>
		public long Milliseconds
		{
			get { return (long)(Seconds * 1000d); }
			set { Seconds = value / 1000d; }
		}

		/// <summary>
		/// Gets or sets the seconds.
		/// </summary>
		/// <value>The seconds.</value>
		public double Seconds
		{
			get { return Ticks / (double)Frequency; }
			set { Ticks = (long)(value * Frequency); }
		}

		/// <summary>
		/// Gets or sets the minutes.
		/// </summary>
		/// <value>The minutes.</value>
		public float Minutes
		{
			get { return (float)Seconds / 60f; }
			set { Seconds = value * 60d; }
		}

		/// <summary>
		/// Gets the frequency.
		/// </summary>
		/// <value>The frequency.</value>
		public static long Frequency
		{
			get { return Stopwatch.Frequency; }
		}

		#endregion

		#region Contructors/Destructors

		private GameSpan(long totalTicks)
		{
			_elapsedticks = totalTicks;
		}

		/// <summary>
		/// Gets the zero.
		/// </summary>
		/// <value>The zero.</value>
		public static GameSpan Zero
		{
			get { return new GameSpan(0); }
		}

		/// <summary>
		/// Froms the ticks.
		/// </summary>
		/// <returns>The ticks.</returns>
		/// <param name="ticks">Ticks.</param>
		public static GameSpan FromTicks(long ticks)
		{
			return new GameSpan(ticks);
		}

		/// <summary>
		/// Froms the milliseconds.
		/// </summary>
		/// <returns>The milliseconds.</returns>
		/// <param name="milliseconds">Milliseconds.</param>
		public static GameSpan FromMilliseconds(long milliseconds)
		{
			return FromSeconds(milliseconds / 1000d);
		}

		/// <summary>
		/// Froms the seconds.
		/// </summary>
		/// <returns>The seconds.</returns>
		/// <param name="seconds">Seconds.</param>
		public static GameSpan FromSeconds(double seconds)
		{
			return FromTicks((long)(seconds * Frequency));
		}

		/// <summary>
		/// Froms the minutes.
		/// </summary>
		/// <returns>The minutes.</returns>
		/// <param name="minutes">Minutes.</param>
		public static GameSpan FromMinutes(float minutes)
		{
			return FromSeconds(minutes * 60d);
		}

		#endregion

		#region Functions

		public bool Equals(GameSpan other)
		{
			return other._elapsedticks == _elapsedticks;
		}

		public override bool Equals(object obj)
		{
			return (obj is GameSpan) && Equals((GameSpan)obj);
		}

		public override int GetHashCode()
		{
			return 0;
		}

		#endregion

		#region Operators
		
		public static bool operator ==(GameSpan a, GameSpan b)
		{
			return b != null && (a != null && (a._elapsedticks == b._elapsedticks));
		}

		public static bool operator !=(GameSpan a, GameSpan b)
		{
			return b != null && (a != null && (a._elapsedticks != b._elapsedticks));
		}

		public static bool operator <(GameSpan a, GameSpan b)
		{
			return (a._elapsedticks < b._elapsedticks);
		}

		public static bool operator >(GameSpan a, GameSpan b)
		{
			return (a._elapsedticks > b._elapsedticks);
		}

		public static bool operator <=(GameSpan a, GameSpan b)
		{
			return (a._elapsedticks <= b._elapsedticks);
		}

		public static bool operator >=(GameSpan a, GameSpan b)
		{
			return (a._elapsedticks >= b._elapsedticks);
		}

		public static GameSpan operator -(GameSpan a)
		{
			return new GameSpan(-a._elapsedticks);
		}

		public static GameSpan operator +(GameSpan a, GameSpan b)
		{
			return new GameSpan(a._elapsedticks + b._elapsedticks);
		}

		public static GameSpan operator -(GameSpan a, GameSpan b)
		{
			return new GameSpan(a._elapsedticks - b._elapsedticks);
		}

		public static GameSpan operator *(GameSpan a, GameSpan b)
		{
			return new GameSpan(a._elapsedticks * b._elapsedticks);
		}

		public static GameSpan operator *(GameSpan a, long b)
		{
			return new GameSpan(a._elapsedticks * b);
		}

		public static GameSpan operator *(GameSpan a, double b)
		{
			return new GameSpan((long)(a._elapsedticks * b));
		}

		public static GameSpan operator *(GameSpan a, float b)
		{
			return new GameSpan((long)(a._elapsedticks *(double)b));
		}

		public static GameSpan operator *(GameSpan a, int b)
		{
			return new GameSpan((long)(a._elapsedticks * (double)b));
		}

		public static GameSpan operator /(GameSpan a, GameSpan b)
		{
			return new GameSpan(a._elapsedticks / b._elapsedticks);
		}

		public static GameSpan operator /(GameSpan a, long b)
		{
			return new GameSpan(a._elapsedticks / b);
		}

		public static GameSpan operator /(GameSpan a, double b)
		{
			return new GameSpan((long)(a._elapsedticks / b));
		}

		public static GameSpan operator /(GameSpan a, float b)
		{
			return new GameSpan((long)(a._elapsedticks / (double)b));
		}

		public static GameSpan operator /(GameSpan a, int b)
		{
			return new GameSpan((long)(a._elapsedticks / (double)b));
		}

		#endregion
	}
}

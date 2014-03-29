//-----------------------------------------------------------------------------
// GameTime.cs
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
	/// Provides snapshot of timing values.
	/// </summary>
	////////////////////////////////////////////////////////////
	public class GameTime
    {
        #region Variables
		/// <summary>Returns how much time has passed since last update</summary>
		public TimeSpan ElapsedGameTime = TimeSpan.Zero;

		/// <summary>Returns how much time has passed since starting game</summary>
		public TimeSpan TotalElapsedGameTime = TimeSpan.Zero;
        
		private long _elapsedticks;
        #endregion

        #region Properties

		/// <summary>
		/// 
		/// </summary>
		public static GameTime Zero
        {
			get { return new GameTime(0); }
        }
        /// <summary>
        /// 
        /// </summary>
        public long Ticks
        {
			get { return _elapsedticks; }
			set { _elapsedticks = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public long Milliseconds
        {
			get { return (long)(Seconds * 1000d); }
			set { Seconds = value / 1000d; }
        }
        /// <summary>
        /// 
        /// </summary>
        public double Seconds
        {
			get { return Ticks / (double)Frequency; }
			set { Ticks = (long)(value * Frequency); }
        }
        /// <summary>
        /// 
        /// </summary>
        public float Minutes
        {
			get { return (float)Seconds / 60f; }
			set { Seconds = value * 60d; }
        }
        /// <summary>
        /// 
        /// </summary>
        public static long Frequency
        {
			get { return Stopwatch.Frequency; }
        }

        #endregion

        #region Contructors/Destructors

        private GameTime(long totalTicks)
        {
            _elapsedticks = totalTicks;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ticks"></param>
        /// <returns></returns>
        public static GameTime FromTicks(long ticks)
        {
            return new GameTime(ticks);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="milliseconds"></param>
        /// <returns></returns>
        public static GameTime FromMilliseconds(long milliseconds)
        {
            return FromSeconds(milliseconds / 1000d);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="seconds"></param>
        /// <returns></returns>
        public static GameTime FromSeconds(double seconds)
        {
            return FromTicks((long)(seconds * Frequency));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="minutes"></param>
        /// <returns></returns>
        public static GameTime FromMinutes(float minutes)
        {
            return FromSeconds(minutes * 60d);
        }

        #endregion

        #region Functions

		/// <summary>
		/// 
		/// </summary>
		/// <param name="dt"></param>
		public void Update(TimeSpan dt)
		{
			ElapsedGameTime = dt;
			TotalElapsedGameTime += dt;
		}

        public override bool Equals(object obj)
        {
            if (!(obj is GameTime))
				return false;
            return ((GameTime)obj)._elapsedticks == _elapsedticks;
        }

	    public override int GetHashCode()
		{
			return 0;
		}

        #endregion

        #region Operators

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator ==(GameTime a, GameTime b)
        {
            return b != null && (a != null && (a._elapsedticks == b._elapsedticks));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator !=(GameTime a, GameTime b)
        {
            return b != null && (a != null && (a._elapsedticks != b._elapsedticks));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator <(GameTime a, GameTime b)
        {
            return (a._elapsedticks < b._elapsedticks);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator >(GameTime a, GameTime b)
        {
            return (a._elapsedticks > b._elapsedticks);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator <=(GameTime a, GameTime b)
        {
            return (a._elapsedticks <= b._elapsedticks);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator >=(GameTime a, GameTime b)
        {
            return (a._elapsedticks >= b._elapsedticks);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public static GameTime operator -(GameTime a)
        {
            return new GameTime(-a._elapsedticks);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static GameTime operator +(GameTime a, GameTime b)
        {
            return new GameTime(a._elapsedticks + b._elapsedticks);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static GameTime operator -(GameTime a, GameTime b)
        {
            return new GameTime(a._elapsedticks - b._elapsedticks);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static GameTime operator *(GameTime a, GameTime b)
        {
            return new GameTime(a._elapsedticks * b._elapsedticks);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static GameTime operator *(GameTime a, long b)
        {
            return new GameTime(a._elapsedticks * b);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static GameTime operator *(GameTime a, double b)
        {
            return new GameTime((long)(a._elapsedticks * b));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static GameTime operator *(GameTime a, float b)
        {
            return new GameTime((long)(a._elapsedticks *(double)b));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static GameTime operator *(GameTime a, int b)
        {
            return new GameTime((long)(a._elapsedticks * (double)b));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static GameTime operator /(GameTime a, GameTime b)
        {
            return new GameTime(a._elapsedticks / b._elapsedticks);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static GameTime operator /(GameTime a, long b)
        {
            return new GameTime(a._elapsedticks / b);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static GameTime operator /(GameTime a, double b)
        {
            return new GameTime((long)(a._elapsedticks / b));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static GameTime operator /(GameTime a, float b)
        {
            return new GameTime((long)(a._elapsedticks / (double)b));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static GameTime operator /(GameTime a, int b)
        {
            return new GameTime((long)(a._elapsedticks / (double)b));
        }

        #endregion
    }
}

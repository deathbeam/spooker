/* File Description
 * Original Works/Author: Zachariah Brown
 * Other Contributors: Thomas Slusny
 * Author Website: http://www.zbrown.net/projects
 * License: MIT
*/

using System;
using System.Diagnostics;

namespace SFGL.Time
{
	public class GameTime
    {
        #region Variables

		public static GameTime ElapsedGameTime = GameTime.Zero; 
        private long _elapsedticks = 0;

        #endregion

        #region Properties

		public static GameTime Zero
        {
			get { return new GameTime(0); }
        }
        public long Ticks
        {
			get { return _elapsedticks; }
			set { _elapsedticks = value; }
        }
        public long Milliseconds
        {
			get { return (long)(Seconds * 1000d); }
			set { Seconds = (double)value / 1000d; }
        }
        public double Seconds
        {
			get { return (double)Ticks / (double)Frequency; }
			set { Ticks = (long)(value * (double)Frequency); }
        }
        public float Minutes
        {
			get { return (float)Seconds / 60f; }
			set { Seconds = (double)value * 60d; }
        }
        public static long Frequency
        {
			get { return Stopwatch.Frequency; }
        }

        #endregion

        #region Contructors/Destructors

        private GameTime(long TotalTicks)
        {
            _elapsedticks = TotalTicks;
        }
        public static GameTime FromTicks(long Ticks)
        {
            return new GameTime(Ticks);
        }
        public static GameTime FromMilliseconds(long Milliseconds)
        {
            return FromSeconds((double)Milliseconds / 1000d);
        }
        public static GameTime FromSeconds(double Seconds)
        {
            return FromTicks((long)(Seconds * (double)Frequency));
        }
        public static GameTime FromMinutes(float Minutes)
        {
            return FromSeconds((double)Minutes * 60d);
        }

        #endregion

        #region Functions

        public override bool Equals(object obj)
        {
            if (!(obj is GameTime))
				return false;
            else
				return ((GameTime)obj)._elapsedticks == _elapsedticks;
        }
		public override int GetHashCode()
		{
			return 0;
		}

        #endregion

        #region Operators

        public static bool operator ==(GameTime a, GameTime b)
        {
            return (a._elapsedticks == b._elapsedticks);
        }
        public static bool operator !=(GameTime a, GameTime b)
        {
            return (a._elapsedticks != b._elapsedticks);
        }
        public static bool operator <(GameTime a, GameTime b)
        {
            return (a._elapsedticks < b._elapsedticks);
        }
        public static bool operator >(GameTime a, GameTime b)
        {
            return (a._elapsedticks > b._elapsedticks);
        }
        public static bool operator <=(GameTime a, GameTime b)
        {
            return (a._elapsedticks <= b._elapsedticks);
        }
        public static bool operator >=(GameTime a, GameTime b)
        {
            return (a._elapsedticks >= b._elapsedticks);
        }
        public static GameTime operator -(GameTime a)
        {
            return new GameTime(-a._elapsedticks);
        }
        public static GameTime operator +(GameTime a, GameTime b)
        {
            return new GameTime(a._elapsedticks + b._elapsedticks);
        }
        public static GameTime operator -(GameTime a, GameTime b)
        {
            return new GameTime(a._elapsedticks - b._elapsedticks);
        }
        public static GameTime operator *(GameTime a, GameTime b)
        {
            return new GameTime(a._elapsedticks * b._elapsedticks);
        }
        public static GameTime operator *(GameTime a, long b)
        {
            return new GameTime(a._elapsedticks * b);
        }
        public static GameTime operator *(GameTime a, double b)
        {
            return new GameTime((long)((double)a._elapsedticks * b));
        }
        public static GameTime operator *(GameTime a, float b)
        {
            return new GameTime((long)((double)a._elapsedticks *(double)b));
        }
        public static GameTime operator *(GameTime a, int b)
        {
            return new GameTime((long)((double)a._elapsedticks * (double)b));
        }
        public static GameTime operator /(GameTime a, GameTime b)
        {
            return new GameTime(a._elapsedticks / b._elapsedticks);
        }
        public static GameTime operator /(GameTime a, long b)
        {
            return new GameTime(a._elapsedticks / b);
        }
        public static GameTime operator /(GameTime a, double b)
        {
            return new GameTime((long)((double)a._elapsedticks / b));
        }
        public static GameTime operator /(GameTime a, float b)
        {
            return new GameTime((long)((double)a._elapsedticks / (double)b));
        }
        public static GameTime operator /(GameTime a, int b)
        {
            return new GameTime((long)((double)a._elapsedticks / (double)b));
        }

        #endregion
    }
}

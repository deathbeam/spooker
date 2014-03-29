//-----------------------------------------------------------------------------
// Clock.cs
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
    /// <summary>
    /// 
    /// </summary>
    public class Clock
    {
        #region Variables
		private readonly Stopwatch _timer = Stopwatch.StartNew();
        #endregion

        #region Properties
		/// <summary>
		/// 
		/// </summary>
		public GameTime ElapsedTime
        {
			get { return GameTime.FromTicks(_timer.ElapsedTicks); }
        }

		/// <summary>
		/// 
		/// </summary>
		public TimeSpan ElapsedTimeFromSpan
		{
			get { return TimeSpan.FromTicks(_timer.ElapsedTicks); }
		}
        #endregion

        #region Functions
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public GameTime Restart()
        {
			var tm = GameTime.FromTicks(_timer.ElapsedTicks);
            _timer.Reset();
            _timer.Start();
            return tm;
        }

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public TimeSpan RestartFromSpan()
		{
			var tm = TimeSpan.FromTicks(_timer.ElapsedTicks);
			_timer.Reset();
			_timer.Start();
			return tm;
		}
        #endregion
    }
}
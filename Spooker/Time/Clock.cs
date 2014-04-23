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
		#region Private Fields

		private readonly Stopwatch _timer = Stopwatch.StartNew();

		#endregion Private Fields

        #region Properties

		public GameTime ElapsedTime
        {
			get { return GameTime.FromTicks(_timer.ElapsedTicks); }
        }
		
		public TimeSpan ElapsedTimeFromSpan
		{
			get { return TimeSpan.FromTicks(_timer.ElapsedTicks); }
		}

        #endregion

        #region Functions

		public GameTime Restart()
        {
			var tm = GameTime.FromTicks(_timer.ElapsedTicks);
            _timer.Reset();
            _timer.Start();
            return tm;
        }
		
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
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

		public GameSpan ElapsedTime
        {
			get { return GameSpan.FromTicks(_timer.ElapsedTicks); }
        }

        #endregion

        #region Functions

		public GameSpan Restart()
        {
			var tm = GameSpan.FromTicks(_timer.ElapsedTicks);
            _timer.Reset();
            _timer.Start();
            return tm;
        }

        #endregion
    }
}
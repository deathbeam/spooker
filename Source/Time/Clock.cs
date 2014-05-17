//-----------------------------------------------------------------------------
// Clock.cs
//
// Spooker Open Source Game Framework
// Copyright (C) Indie Armory. All rights reserved.
// Website: http://indiearmory.com
// Other Contributors: Zachariah Brown @ http://www.zbrown.net/projects
// License: MIT
//-----------------------------------------------------------------------------

using System.Diagnostics;

namespace Spooker.Time
{
	/// <summary>
	/// Clock.
	/// </summary>
    public class Clock
    {
		#region Private Fields

		private readonly Stopwatch _timer = Stopwatch.StartNew();

		#endregion Private Fields

        #region Properties

		/// <summary>
		/// Gets the elapsed time.
		/// </summary>
		/// <value>The elapsed time.</value>
		public GameSpan ElapsedTime
        {
			get { return GameSpan.FromTicks(_timer.ElapsedTicks); }
        }

        #endregion

        #region Functions

		/// <summary>
		/// Restart this instance.
		/// </summary>
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
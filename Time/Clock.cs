/* File Description
 * Original Works/Author: zsbzsb
 * Other Contributors: Thomas Slusny
 * Author Website: 
 * License: MIT
*/

using System;
using System.Diagnostics;

namespace SFGL.Time
{
    public class Clock
    {
        #region Variables
		private Stopwatch _timer = Stopwatch.StartNew();
        #endregion

        #region Properties
		public GameTime ElapsedTime
        {
			get { return GameTime.FromTicks(_timer.ElapsedTicks); }
        }
        #endregion

        #region Functions
		public GameTime Restart()
        {
			GameTime tm = GameTime.FromTicks(_timer.ElapsedTicks);
            _timer.Reset();
            _timer.Start();
            return tm;
        }
        #endregion
    }
}
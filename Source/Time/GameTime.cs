//-----------------------------------------------------------------------------
// GameTime.cs
//
// Spooker Open Source Game Framework
// Copyright (C) Indie Armory. All rights reserved.
// Website: http://indiearmory.com
// Other Contributors: Zachariah Brown @ http://www.zbrown.net/projects
// License: MIT
//-----------------------------------------------------------------------------

namespace Spooker.Time
{
	////////////////////////////////////////////////////////////
	/// <summary>
	/// Provides snapshot of timing values.
	/// </summary>
	////////////////////////////////////////////////////////////
	public class GameTime
    {
		#region Public Fields

		/// <summary>Returns how much time has passed since last update</summary>
		public GameSpan ElapsedGameTime = GameSpan.Zero;

		/// <summary>Returns how much time has passed since starting game</summary>
		public GameSpan TotalGameTime = GameSpan.Zero;

		#endregion Public Fields

        #region Contructors/Destructors

		/// <summary>
		/// Initializes a new instance of the <see cref="Spooker.Time.GameTime"/> class.
		/// </summary>
		public GameTime()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Spooker.Time.GameTime"/> class.
		/// </summary>
		/// <param name="elapsed">Elapsed time.</param>
		/// <param name="total">Total time.</param>
		public GameTime(GameSpan elapsed, GameSpan total)
        {
			ElapsedGameTime = elapsed;
			TotalGameTime = total;
        }

        #endregion

        #region Functions

		/// <summary>
		/// Update the specified updateable.
		/// </summary>
		/// <param name="updateable">Updateable.</param>
		public void Update(IUpdateable updateable)
		{
			updateable.Update (this);
		}

        #endregion
    }
}

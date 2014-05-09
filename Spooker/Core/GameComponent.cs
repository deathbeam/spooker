//-----------------------------------------------------------------------------
// GameComponent.cs
//
// Spooker Open Source Game Framework
// Copyright (C) Indie Armory. All rights reserved.
// Website: http://indiearmory.com
// Other Contributors: None
// License: MIT
//-----------------------------------------------------------------------------

using SFML.Graphics;
using Spooker.Audio;
using Spooker.Input;
using Spooker.GameStates;

namespace Spooker.Core
{
	////////////////////////////////////////////////////////////
	/// <summary>
	/// Abstract class what will enable you to use all Game 
	/// functions in derived classes.
	/// </summary>
	////////////////////////////////////////////////////////////
	public abstract class GameComponent
	{
		/// <summary>Heart of Spooker. All important operations are done here.</summary>
        protected GameWindow Game;

		/// <summary>Core rendering device what controls everything drawn to screen.</summary>
		protected RenderTarget GraphicsDevice
		{
			get { return Game.GraphicsDevice; }
		}

		/// <summary>Can play various audio files.</summary>
		protected AudioManager Audio
		{ 
			get { return Game.Audio; }
		}

		/// <summary>Handles user input from keyboard and mouse.</summary>
		protected GameInput GameInput
		{ 
			get { return Game.GameInput; }
		}

		/// <summary>Manages all present game states.</summary>
		protected StateFactory StateFactory
		{ 
			get { return Game.StateFactory; }
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Spooker.Core.GameComponent"/> class.
		/// </summary>
		/// <param name="game">Game.</param>
		protected GameComponent(GameWindow game)
		{
			Game = game;
		}
	}
}


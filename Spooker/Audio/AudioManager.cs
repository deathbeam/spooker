//-----------------------------------------------------------------------------
// AudioManager.cs
//
// Spooker Open Source Game Framework
// Copyright (C) Indie Armory. All rights reserved.
// Website: http://indiearmory.com
// Other Contributors: None
// License: MIT
//-----------------------------------------------------------------------------

using System;
using System.IO;
using System.Collections.Generic;
using SFML.Audio;

namespace Spooker.Audio
{
	////////////////////////////////////////////////////////////
	/// <summary>
	/// Can be used for managing, playing and loading of sounds
	/// and music.
	/// </summary>
	////////////////////////////////////////////////////////////
	public class AudioManager : ILoadable, IDisposable
	{
		private readonly Dictionary<string, SoundBuffer> _sounds = new Dictionary<string, SoundBuffer>();
		private Music _currentMusic;
		
		/// <summary>Directory from where will audio manager load sounds.</summary>
		public string SoundDirectory;
		
		/// <summary>Extension of sounds what will audio manager load and play.</summary>
		public string SoundExtension;
		
		////////////////////////////////////////////////////////////
		/// <summary>
		/// Loads audio files from specified audio folder to cache.
		/// </summary>
		////////////////////////////////////////////////////////////
		public void LoadContent()
		{
			if (!Directory.Exists(SoundDirectory)) return;

			FileInfo[] files = new DirectoryInfo(SoundDirectory)
				.GetFiles("*." + SoundExtension, SearchOption.AllDirectories);

			_sounds.Clear();
			foreach (var file in files)
			{
				_sounds.Add(file.Name.Remove(file.Name.Length - 4, 4),
					new SoundBuffer(file.FullName));
			}
		}

		////////////////////////////////////////////////////////////
		/// <summary>
		/// Loads and plays music from specified music path.
		/// </summary>
		////////////////////////////////////////////////////////////
		public void PlayMusic(string musicName)
		{
			if(_currentMusic != null) _currentMusic.Dispose();
			_currentMusic = new Music(musicName);
			_currentMusic.Play();
		}

		////////////////////////////////////////////////////////////
		/// <summary>
		/// Stops current music.
		/// </summary>
		////////////////////////////////////////////////////////////
		public void StopMusic()
		{
			if (_currentMusic == null)
				return;
			_currentMusic.Stop();
			_currentMusic.Dispose ();
		}

		////////////////////////////////////////////////////////////
		/// <summary>
		/// Pause current music.
		/// </summary>
		////////////////////////////////////////////////////////////
		public void PauseMusic()
		{
			if (_currentMusic == null)
				return;
			_currentMusic.Pause();
		}

		////////////////////////////////////////////////////////////
		/// <summary>
		/// Resumes current paused music.
		/// </summary>
		////////////////////////////////////////////////////////////
		public void ResumeMusic()
		{
			if (_currentMusic == null)
				return;
			_currentMusic.Play();
		}

		////////////////////////////////////////////////////////////
		/// <summary>
		/// Plays sound stored in sound cache once.
		/// </summary>
		////////////////////////////////////////////////////////////
		public void PlaySound(string soundName)
		{
			var sound = new Sound(_sounds[soundName]) {Loop = false};
		    sound.Play();
		}

		////////////////////////////////////////////////////////////
		/// <summary>
		/// Play sound stored in sound cache once or repeated.
		/// </summary>
		////////////////////////////////////////////////////////////
		public void PlaySound(string soundName, bool repeat)
		{
			var sound = new Sound(_sounds[soundName]) {Loop = repeat};
		    sound.Play();
		}

		////////////////////////////////////////////////////////////
		/// <summary>
		/// Stops all sounds
		/// </summary>
		////////////////////////////////////////////////////////////
		public void StopSounds()
		{
			//TODO: Actually stop all sounds, not only dispose them...
			foreach (var sound in _sounds.Values)
				sound.Dispose ();
		}

		////////////////////////////////////////////////////////////
		/// <summary>
		/// Returns sound from sound cache.
		/// </summary>
		////////////////////////////////////////////////////////////
		public Sound GetSound(string soundName)
		{
			return new Sound(_sounds[soundName]);
		}

		////////////////////////////////////////////////////////////
		/// <summary>
		/// Disposes this instance of audio manager class.
		/// </summary>
		////////////////////////////////////////////////////////////
		public void Dispose()
		{
			StopMusic ();
			StopSounds ();
			_sounds.Clear ();
		}
	}
}


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
using Spooker.Content;

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

		/// <summary>
		/// Component uses this for loading itself
		/// </summary>
		/// <param name="content">Content.</param>
		public void LoadContent(ContentManager content)
		{
			if (!Directory.Exists(SoundDirectory)) return;

			FileInfo[] files = new DirectoryInfo(SoundDirectory).GetFiles(
				"*." + SoundExtension,
				SearchOption.AllDirectories);

			_sounds.Clear();
			foreach (var file in files)
				_sounds.Add(
					file.Name.Remove(file.Name.Length - 4, 4),
					new SoundBuffer(file.FullName));
		}

		/// <summary>
		/// Loads and plays music from specified music path.
		/// </summary>
		/// <param name="musicName">Music name.</param>
		public void PlayMusic(string musicName)
		{
			if(_currentMusic != null) _currentMusic.Dispose();
			_currentMusic = new Music(musicName);
			_currentMusic.Play();
		}

		/// <summary>
		/// Stops the music.
		/// </summary>
		public void StopMusic()
		{
			if (_currentMusic == null)
				return;
			_currentMusic.Dispose ();
		}

		/// <summary>
		/// Pauses the music.
		/// </summary>
		public void PauseMusic()
		{
			if (_currentMusic == null)
				return;
			_currentMusic.Pause();
		}

		/// <summary>
		/// Resumes the music.
		/// </summary>
		public void ResumeMusic()
		{
			if (_currentMusic == null)
				return;
			_currentMusic.Play();
		}

		/// <summary>
		/// Play sound stored in sound cache once or repeated.
		/// </summary>
		/// <param name="soundName">Sound name.</param>
		/// <param name="repeat">If set to <c>true</c> repeat.</param>
		public void PlaySound(string soundName, bool repeat = false)
		{
			var sound = new Sound(_sounds[soundName]) {Loop = repeat};
		    sound.Play();
		}

		/// <summary>
		/// Stops the sounds.
		/// </summary>
		public void StopSounds()
		{
			//TODO: Actually stop all sounds, not only dispose them...
			foreach (var sound in _sounds.Values)
				sound.Dispose ();
		}

		/// <summary>
		/// Gets the sound from sound cache.
		/// </summary>
		/// <returns>The sound.</returns>
		/// <param name="soundName">Sound name.</param>
		public Sound GetSound(string soundName)
		{
			return new Sound(_sounds[soundName]);
		}

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		/// <filterpriority>2</filterpriority>
		/// <remarks>Call <see cref="Dispose"/> when you are finished using the <see cref="Spooker.Audio.AudioManager"/>. The
		/// <see cref="Dispose"/> method leaves the <see cref="Spooker.Audio.AudioManager"/> in an unusable state. After
		/// calling <see cref="Dispose"/>, you must release all references to the <see cref="Spooker.Audio.AudioManager"/> so
		/// the garbage collector can reclaim the memory that the <see cref="Spooker.Audio.AudioManager"/> was occupying.</remarks>
		public void Dispose()
		{
			StopMusic ();
			StopSounds ();
			_sounds.Clear ();
		}
	}
}


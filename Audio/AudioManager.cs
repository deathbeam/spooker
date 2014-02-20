using SFML.Audio;
using System;
/* File Description
 * Original Works/Author: Thomas Slusny
 * Other Contributors: None
 * Author Website: http://indiearmory.com
 * License: MIT
*/

using System.IO;
using System.Collections.Generic;
using SFGL.Window;

namespace SFGL.Audio
{
	/// <summary>
	/// Can be used for managing, playing and loading of sound and music.
	/// </summary>
	public class AudioManager : GameComponent, ILoadable, IDisposable
	{
		private Dictionary<string, SoundBuffer> _sounds = new Dictionary<string, SoundBuffer>();
		private Music _currentMusic;

		/// <summary>
		/// Directory from where will audio manager load sounds.
		/// </summary>
		public string SoundDirectory = "Sounds";

		/// <summary>
		/// Extension of sounds what will audio manager load and play.
		/// </summary>
		public string SoundExtension = "ogg";

		/// <summary>
		/// Creates new instance of audio manager.
		/// </summary>
		public AudioManager(GameWindow game) : base(game) { }


		/// <summary>
		/// Loads audio files from specified audio folder to cache.
		/// </summary>
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

		/// <summary>
		/// Loads and plays music from specified music path.
		/// </summary>
		public void PlayMusic(string musicName)
		{
			if(_currentMusic != null) _currentMusic.Dispose();
			_currentMusic = new Music(musicName);
			_currentMusic.Play();
		}

		/// <summary>
		/// Stops current music.
		/// </summary>
		public void StopMusic()
		{
			_currentMusic.Stop();
		}

		/// <summary>
		/// Pause current music.
		/// </summary>
		public void PauseMusic()
		{
			_currentMusic.Pause();
		}

		/// <summary>
		/// Resumes current paused music.
		/// </summary>
		public void ResumeMusic()
		{
			_currentMusic.Play();
		}

		/// <summary>
		/// Plays sound stored in sound cache once.
		/// </summary>
		public void PlaySound(string soundName)
		{
			Sound sound = new Sound(_sounds[soundName]);
			sound.Loop = false;
			sound.Play();
		}

		/// <summary>
		/// Play sound stored in sound cache once or repeated.
		/// </summary>
		public void PlaySound(string soundName, bool repeat)
		{
			Sound sound = new Sound(_sounds[soundName]);
			sound.Loop = repeat;
			sound.Play();
		}

		/// <summary>
		/// Returns sound from sound cache.
		/// </summary>
		public Sound GetSound(string soundName)
		{
			return new Sound(_sounds[soundName]);
		}

		/// <summary>
		/// Disposes this instance of audio manager class.
		/// </summary>
		public void Dispose()
		{
			StopMusic();
			foreach (var sound in _sounds.Values)
				sound.Dispose ();
			_sounds.Clear();
			_currentMusic.Dispose();
		}
	}
}


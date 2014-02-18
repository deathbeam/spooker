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
	public class AudioManager : GameComponent, ILoadable, IDisposable
	{
		private Dictionary<string, SoundBuffer> _sounds = new Dictionary<string, SoundBuffer>();
		private Music _currentMusic;
		public string SoundDirectory = "Sounds";
		public string SoundExtension = "ogg";

		public AudioManager(GameWindow game) : base(game) { }

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

		public void PlayMusic(string musicName)
		{
			if(_currentMusic != null) _currentMusic.Dispose();
			_currentMusic = new Music(musicName);
			_currentMusic.Play();
		}

		public void StopMusic()
		{
			_currentMusic.Stop();
		}

		public void PauseMusic()
		{
			_currentMusic.Pause();
		}

		public void ResumeMusic()
		{
			_currentMusic.Play();
		}

		public void PlaySound(string soundName)
		{
			Sound sound = new Sound(_sounds[soundName]);
			sound.Loop = false;
			sound.Play();
		}

		public void PlaySound(string soundName, bool repeat)
		{
			Sound sound = new Sound(_sounds[soundName]);
			sound.Loop = repeat;
			sound.Play();
		}

		public Sound GetSound(string soundName)
		{
			return new Sound(_sounds[soundName]);
		}

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


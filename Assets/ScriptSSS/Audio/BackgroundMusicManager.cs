using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

namespace ScriptSSS.Audio
{
	public class BackgroundMusicManager : MonoBehaviour
	{
		[Header("Playlist")]
		[SerializeField] private AudioClip[] playlist;
		[SerializeField] private bool shuffle = false;
		[SerializeField] private bool loopPlaylist = true;

		[Header("Playback")]
		[SerializeField] private bool playOnStart = true;
		[SerializeField] private float crossfadeSeconds = 1.5f;
		[SerializeField] private AudioMixerGroup musicMixerGroup; // optional
		[SerializeField] private bool persistAcrossScenes = true;

		private AudioSource sourceA;
		private AudioSource sourceB;
		private bool usingA = true;
		private int currentIndex = -1;
		private float fadeTimer = 0f;
		private bool isFading = false;

		private static BackgroundMusicManager instance;

		private void Awake()
		{
			// Simple singleton to avoid duplicates if set to persist
			if (persistAcrossScenes)
			{
				if (instance != null && instance != this)
				{
					Destroy(gameObject);
					return;
				}
				instance = this;
				DontDestroyOnLoad(gameObject);
			}

			// Create two audio sources for crossfading
			sourceA = CreateSource("Music_A");
			sourceB = CreateSource("Music_B");
		}

		private void OnEnable()
		{
			SceneManager.sceneLoaded += OnSceneLoaded;
		}

		private void OnDisable()
		{
			SceneManager.sceneLoaded -= OnSceneLoaded;
		}

		private void Start()
		{
			if (playOnStart)
			{
				PlayNext();
			}
		}

		private void Update()
		{
			if (isFading)
			{
				fadeTimer += Time.unscaledDeltaTime;
				float t = Mathf.Clamp01(fadeTimer / Mathf.Max(0.01f, crossfadeSeconds));
				ActiveSource().volume = Mathf.Lerp(0f, 1f, t);
				InactiveSource().volume = Mathf.Lerp(1f, 0f, t);
				if (t >= 1f)
				{
					isFading = false;
					InactiveSource().Stop();
					InactiveSource().clip = null;
				}
			}

			// If current finished and not fading, advance
			if (!isFading && !ActiveSource().isPlaying && (playlist != null && playlist.Length > 0))
			{
				PlayNext();
			}
		}

		private AudioSource CreateSource(string name)
		{
			var go = new GameObject(name);
			go.transform.SetParent(transform);
			var src = go.AddComponent<AudioSource>();
			src.playOnAwake = false;
			src.loop = false;
			src.spatialBlend = 0f;
			if (musicMixerGroup != null) src.outputAudioMixerGroup = musicMixerGroup;
			src.volume = 0f;
			return src;
		}

		private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
		{
			// Optional: choose a new track per scene automatically
			// For now: do nothing automatically; user can call PlayNext/PlayIndex from scene events if needed
		}

		private AudioSource ActiveSource() => usingA ? sourceA : sourceB;
		private AudioSource InactiveSource() => usingA ? sourceB : sourceA;

		public void PlayNext()
		{
			if (playlist == null || playlist.Length == 0) return;
			int next = NextIndex();
			PlayIndex(next);
		}

		public void PlayIndex(int index)
		{
			if (playlist == null || playlist.Length == 0) return;
			index = Mathf.Clamp(index, 0, playlist.Length - 1);
			currentIndex = index;
			CrossfadeTo(playlist[currentIndex]);
		}

		public void StopAll()
		{
			sourceA.Stop(); sourceB.Stop();
			sourceA.clip = null; sourceB.clip = null;
		}

		private int NextIndex()
		{
			if (playlist == null || playlist.Length == 0) return -1;
			if (shuffle)
			{
				if (playlist.Length == 1) return 0;
				int rnd = Random.Range(0, playlist.Length);
				if (rnd == currentIndex) rnd = (rnd + 1) % playlist.Length;
				return rnd;
			}
			else
			{
				int next = currentIndex + 1;
				if (next >= playlist.Length)
				{
					return loopPlaylist ? 0 : currentIndex; // if not looping, stay on last
				}
				return next;
			}
		}

		private void CrossfadeTo(AudioClip clip)
		{
			if (clip == null) return;
			var inactive = InactiveSource();
			inactive.clip = clip;
			inactive.volume = 0f;
			inactive.Play();
			usingA = !usingA; // swap active
			fadeTimer = 0f;
			isFading = true;
		}
	}
}



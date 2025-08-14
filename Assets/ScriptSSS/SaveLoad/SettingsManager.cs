using UnityEngine;
using UnityEngine.Audio;

namespace ScriptSSS.SaveLoad
{
	public class SettingsManager : MonoBehaviour
	{
		[SerializeField] private AudioMixer audioMixer; // Optional, assign if using mixers
		[SerializeField] private string masterVolumeParam = "MasterVolume";
		[SerializeField] private string musicVolumeParam = "MusicVolume";
		[SerializeField] private string sfxVolumeParam = "SFXVolume";

		private SettingsData current = new SettingsData();

		public void Apply(SettingsData data)
		{
			current = data ?? new SettingsData();
			if (audioMixer != null)
			{
				SetMixerLinear(masterVolumeParam, current.masterVolume);
				SetMixerLinear(musicVolumeParam, current.musicVolume);
				SetMixerLinear(sfxVolumeParam, current.sfxVolume);
			}
			if (current.qualityLevel >= 0)
			{
				QualitySettings.SetQualityLevel(current.qualityLevel);
			}
			Application.targetFrameRate = current.targetFrameRate;
		}

		private void SetMixerLinear(string param, float linear01)
		{
			if (string.IsNullOrEmpty(param)) return;
			// Convert linear 0..1 to dB; assume -80dB..0dB
			float dB = Mathf.Approximately(linear01, 0f) ? -80f : Mathf.Lerp(-20f, 0f, Mathf.Clamp01(linear01));
			audioMixer.SetFloat(param, dB);
		}

		public SettingsData Capture()
		{
			return current;
		}
	}
}



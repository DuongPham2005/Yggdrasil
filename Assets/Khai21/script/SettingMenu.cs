using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Audio;


public class SettingMenu : MonoBehaviour 
{
    public AudioMixer mainMixer;
    public void SetVolume(float volume)
    {
        mainMixer.SetFloat("volume", volume);
    }
}

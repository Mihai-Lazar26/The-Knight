using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class VolumeSettings : MonoBehaviour
{
    [SerializeField] private AudioMixer _audioMixer;
    [SerializeField] private Slider _musicSlider;
    [SerializeField] private Slider _sfxSlider;
    [SerializeField] private SettingsData _settingsData;

    public void UpdateVolumeSettings() {
        _musicSlider.value = _settingsData.musicVolume;
        _sfxSlider.value = _settingsData.sfxVolume;
    }

    public void SetMusicVolume(float value) {
        _settingsData.musicVolume = value;
        _audioMixer.SetFloat("MusicVolume", Mathf.Log10(value) * 20);
    }
    public void SetSFXVolume(float value) {
        _settingsData.sfxVolume = value;
        _audioMixer.SetFloat("SFXVolume", Mathf.Log10(value) * 20);
    }
}

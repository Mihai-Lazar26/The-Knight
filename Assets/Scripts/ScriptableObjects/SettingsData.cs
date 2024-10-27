using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
[CreateAssetMenu]
public class SettingsData : ScriptableObject
{
    private bool loaded = false;
    public string saveName = "";

    public float musicVolume = 1;
    public float sfxVolume = 1;

    public AudioMixer mixer;

    public void SaveSettings() {
        SaveSystem.SaveSettings(saveName, this);
    }

    public void LoadSettings() {
        if (loaded)
            return;
        loaded = true;
        SettingsDataSerialized data = SaveSystem.LoadSettings(saveName);
        if (data == null) {
            SetDefault();
            return;
        }
        musicVolume = data.musicVolume;
        sfxVolume = data.sfxVolume;
        LoadVolume();
    }

    public void SetDefault() {
        musicVolume = 1;
        sfxVolume = 1;
        LoadVolume();
    }

    public void LoadVolume() {
        mixer.SetFloat("MusicVolume", Mathf.Log10(musicVolume) * 20);
        mixer.SetFloat("SFXVolume", Mathf.Log10(sfxVolume) * 20);
    }
}

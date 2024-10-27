using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SettingsDataSerialized
{
    public float musicVolume;
    public float sfxVolume;

    public SettingsDataSerialized(SettingsData settingsData) {
        musicVolume = settingsData.musicVolume;
        sfxVolume = settingsData.sfxVolume;
    }
}

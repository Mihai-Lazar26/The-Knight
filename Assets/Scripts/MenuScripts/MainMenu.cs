using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private PlayerData _playerData1;
    [SerializeField] private PlayerData _playerData2;
    [SerializeField] private PlayerData _playerData3;
    private GameObject _activeMenu;
    [SerializeField] private GameObject _mainMenu;
    [SerializeField] private GameObject _playMenu;
    [SerializeField] private GameObject _optionsMenu;
    [SerializeField] private SettingsData _settingsData;
    [SerializeField] private VolumeSettings _volumeSettings;


    private void Awake() {
        _settingsData.LoadSettings();

        _playerData1.LoadPlayer();
        _playerData2.LoadPlayer();
        _playerData3.LoadPlayer();
    }
    private void Start() {
        _activeMenu = _mainMenu;
        _activeMenu.SetActive(true);
        _playMenu.SetActive(false);
        _optionsMenu.SetActive(false);

        _volumeSettings.UpdateVolumeSettings();
        Time.timeScale = 0;
    }

    public void PlayButton() {
        _activeMenu.SetActive(false);
        _activeMenu = _playMenu;
        _activeMenu.SetActive(true);
    }

    public void OptionsButton() {
        _activeMenu.SetActive(false);
        _activeMenu = _optionsMenu;
        _activeMenu.SetActive(true);
    }

    public void ExitButton() {
        Application.Quit();
    }

    public void BackButton() {
        _activeMenu.SetActive(false);
        _activeMenu = _mainMenu;
        _activeMenu.SetActive(true);
    }

    public void SaveSettings() {
        _settingsData.SaveSettings();
    }
}

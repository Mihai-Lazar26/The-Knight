using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class OptionsMenu : MonoBehaviour
{
    private GameObject _activeMenu;
    [SerializeField] private GameObject _optionsMenu;
    [SerializeField] private GameObject _soundsMenu;
    [SerializeField] private GameObject _keyBindsMenu;

    private void Start() {
        _activeMenu = _optionsMenu;
        _activeMenu.SetActive(true);
        _soundsMenu.SetActive(false);
        _keyBindsMenu.SetActive(false);
    }

    private void OnEnable() {
        Start();
    }

    public void VolumeButton() {
        _activeMenu.SetActive(false);
        _activeMenu = _soundsMenu;
        _activeMenu.SetActive(true);
    }

    public void KeybindsButton() {
        _activeMenu.SetActive(false);
        _activeMenu = _keyBindsMenu;
        _activeMenu.SetActive(true);
    }

    public void BackButton() {
        _activeMenu.SetActive(false);
        _activeMenu = _optionsMenu;
        _activeMenu.SetActive(true);
    }
}

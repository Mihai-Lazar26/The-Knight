using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveSlotLogic : MonoBehaviour
{
    [SerializeField] private PlayerData _playerData;
    [SerializeField] private GameObject _foundLayout;
    [SerializeField] private GameObject _notFoundLayout;
    [SerializeField] private LoadedData _loadedData;

    private void Start() {
        if (_playerData.Status()) {
            _foundLayout.SetActive(true);
            _notFoundLayout.SetActive(false);
        }
        else {
            _foundLayout.SetActive(false);
            _notFoundLayout.SetActive(true);
        }
    }

    public void NewGameButton() {
        _loadedData.loadedData = _playerData;
        _playerData.SetDefault();
        _playerData.SavePlayer();
        SceneManager.LoadScene(_playerData.sceneIndex, LoadSceneMode.Single);
    }

    public void LoadSaveButton() {
        _loadedData.loadedData = _playerData;
        SceneManager.LoadScene(_playerData.sceneIndex, LoadSceneMode.Single);
    }

    public void DeleteSaveButton() {
        if (_playerData.DeleteData()) {
            _playerData.SetDefault();
            _foundLayout.SetActive(false);
            _notFoundLayout.SetActive(true);
        }
    }
}

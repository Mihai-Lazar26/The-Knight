using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    private bool _pauseMenuToggle = false;
    public bool IsPaused {get => _pauseMenuToggle;}
    [SerializeField] private GameObject _pauseMenu;
    [SerializeField] private LoadedData _loadedData;

    private GameObject _player;

    private void Start() {
        _player = GameObject.FindWithTag("Player");
        Time.timeScale = 1;
        _pauseMenuToggle = false;
        _pauseMenu.SetActive(false);
    }

    public void Pause(InputAction.CallbackContext context) {
        if (context.performed){
            if (_pauseMenuToggle) {
                Time.timeScale = 1;
            }
            else {
                Time.timeScale = 0;
            }
            _pauseMenuToggle = !_pauseMenuToggle;
            _pauseMenu.SetActive(_pauseMenuToggle);
        }
    }

    public void ContinueButton() {
        Time.timeScale = 1;
        _pauseMenuToggle = false;
        _pauseMenu.SetActive(false);
    }

    public void MainMenuButton() {
        SceneManager.LoadScene(0, LoadSceneMode.Single);
    }
}

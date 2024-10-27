using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelMove : MonoBehaviour
{
    [SerializeField] private int _sceneBuildIndex;
    [SerializeField] private Vector2 _playerPositon;
    [SerializeField] private LoadedData _playerData;


    private void OnTriggerEnter2D(Collider2D collider) {
        if (collider.tag == "Player") {
            EntityHealthSystem playerHealth = collider.GetComponent<EntityHealthSystem>();
            _playerData.loadedData.playerMaxHealth = playerHealth.MaxHealth;
            _playerData.loadedData.playerCurrentHealth = playerHealth.CurrentHealth;
            _playerData.loadedData.playerPosition = _playerPositon;
            _playerData.loadedData.sceneIndex = _sceneBuildIndex;
            SceneManager.LoadScene(_sceneBuildIndex, LoadSceneMode.Single);
        }
    }
}

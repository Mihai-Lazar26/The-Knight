using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class CheckpointScript : MonoBehaviour
{
    [SerializeField] private LoadedData _loadeData;
    [SerializeField] private AudioSource _saveSFX;
    private bool _inRange = false;

    private void OnTriggerEnter2D(Collider2D collider) {
        if (collider.tag == "Player") {
            _inRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collider) {
        if (collider.tag == "Player") {
            _inRange = false;
        }
    }

    public void Save(InputAction.CallbackContext context) {
        if (_inRange && context.performed) {
            if (_saveSFX != null) {
                _saveSFX.Play();
            }
            GameObject player = GameObject.FindWithTag("Player");
            _loadeData.loadedData.playerPosition = player.transform.position;

            EntityHealthSystem playerHealth = player.GetComponent<EntityHealthSystem>();
            playerHealth.MaxHeal();

            _loadeData.loadedData.playerCurrentHealth = playerHealth.CurrentHealth;
            _loadeData.loadedData.playerMaxHealth = playerHealth.MaxHealth;
            _loadeData.loadedData.sceneIndex = SceneManager.GetActiveScene().buildIndex;

            _loadeData.loadedData.SavePlayer();
        }
    }


}

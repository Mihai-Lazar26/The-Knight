using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class EntityHealthSystem : MonoBehaviour
{
    private Animator _animator;
    [SerializeField] private int _maxHealth = 100;
    [SerializeField] private bool _destroyObject = false;
    private int _currentHealth;

    public int MaxHealth {get => _maxHealth; set => _maxHealth = value;}
    public int CurrentHealth {get => _currentHealth; set => _currentHealth = value;}

    private bool _isDead = false;
    public bool IsDead {get => _isDead;}

    [SerializeField] private HealthBar _healthBar;
    [SerializeField] private AudioSource _hitSFX;
    [SerializeField] private AudioSource _deathSFX;
    [SerializeField] private LoadedData _playerData;
    private void Start()
    {
        _animator = GetComponent<Animator>();
        _currentHealth = _maxHealth;
        if (tag == "Player") {
            _currentHealth = _playerData.loadedData.playerCurrentHealth;
            _maxHealth = _playerData.loadedData.playerMaxHealth;
        }
        if (_healthBar != null) {
            _healthBar.SetMaxHealth(_maxHealth);
            _healthBar.SetHealth(_currentHealth);
        }
    }

    public void MaxHeal() {
        _currentHealth = _maxHealth;
        ReloadHealth();
    }

    public void ReloadHealth() {
        if (_healthBar != null) {
            _healthBar.SetMaxHealth(_maxHealth);
            _healthBar.SetHealth(_currentHealth);
        }
    }

    public void TakeDamage(int damage) {
        _currentHealth -= damage;
        if (_healthBar != null)
            _healthBar.SetHealth(_currentHealth);
        
        if (_hitSFX != null)
            _hitSFX.Play();
        _animator.SetTrigger("hurt");

        if (_currentHealth <= 0) {
            Die();
        }
    }

    private void Die() {
        _isDead = true;
        if (_deathSFX != null)
            _deathSFX.Play();
        if (GetComponentInParent<PlayerCombat>() != null) 
            GetComponentInParent<PlayerCombat>().enabled = false;
        if (GetComponentInParent<PlayerMovement>() != null) {
            PlayerMovement pl = GetComponentInParent<PlayerMovement>();
            pl.setMovementVelocity0();
            pl.enabled = false;
        }
            
        if (GetComponentInParent<EnemyPatrol>() != null) 
            GetComponentInParent<EnemyPatrol>().enabled = false;
        if (GetComponentInParent<MeleeEnemy>() != null) 
            GetComponentInParent<MeleeEnemy>().enabled = false;
        if (GetComponentInParent<EnemyContactDamage>() != null) 
            GetComponentInParent<EnemyContactDamage>().enabled = false;

        _animator.SetBool("isDead", true);
        if(tag != "Player"){
            if (GetComponent<Collider2D>() != null)
                GetComponent<Collider2D>().enabled = false;
            if (GetComponent<Rigidbody2D>() != null)
                GetComponent<Rigidbody2D>().isKinematic = true;
        }
        else{
            _playerData.loadedData.LoadPlayer();
            // SceneManager.LoadScene(_playerData.loadedData.sceneIndex, LoadSceneMode.Single);
            StartCoroutine(LoadLevelAfterDelay(1f));
            Physics2D.IgnoreLayerCollision(7, 9);
        }
        if (_destroyObject) {
            if (_deathSFX != null) {
                Destroy(gameObject, _deathSFX.clip.length);
                GetComponent<SpriteRenderer>().enabled = false;
            }
            else
                Destroy(gameObject);
        }
        
        this.enabled = false;
    }
    IEnumerator LoadLevelAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(_playerData.loadedData.sceneIndex, LoadSceneMode.Single);
    }
}

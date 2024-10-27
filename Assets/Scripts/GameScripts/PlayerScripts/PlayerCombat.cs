using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCombat : MonoBehaviour
{
    private Animator _animator;
    [SerializeField] private Transform _attackPoint;
    [SerializeField] private float _attackRange = .5f;
    [SerializeField] private LayerMask _enemyLayer;
    [SerializeField] private int _attackDamage = 20;
    [SerializeField] private float _attackCooldown = .5f;
    private float _nextAttackTime = 0;
    [SerializeField] private AudioSource _attackSFX;

    private EntityHealthSystem _playerHealth;

    [SerializeField] private PauseMenu _pauseMenu;
    [SerializeField] public BoxCollider2D shieldCollider;
    [SerializeField] private float _blockCooldown= .5f;
    private float _nextBlockTime = 0;
    private void Start()
    {
        shieldCollider.enabled = false;
        _animator = GetComponent<Animator>();
        _playerHealth = GetComponent<EntityHealthSystem>();
    }

    public void Block(InputAction.CallbackContext context) {
        if (!_pauseMenu.IsPaused && !_playerHealth.IsDead && Time.time >= _nextBlockTime && context.performed) {
            _attackSFX.Play();
            _animator.SetTrigger("block");
            _nextBlockTime = Time.time + _blockCooldown;
        }
    }

    public void Attack(InputAction.CallbackContext context) {
        if (!_pauseMenu.IsPaused && !_playerHealth.IsDead && Time.time >= _nextAttackTime && context.performed) {
            _attackSFX.Play();
            _animator.SetTrigger("attack1");
            _nextAttackTime = Time.time + _attackCooldown;
        }
    }

    private void DamageEnemies() {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(_attackPoint.position, _attackRange, _enemyLayer);

        foreach(Collider2D enemy in hitEnemies) {
            enemy.GetComponent<EntityHealthSystem>().TakeDamage(_attackDamage);
        }
    }

    private void OnDrawGizmosSelected() {
        if (_attackPoint == null)
            return;
        
        Gizmos.DrawWireSphere(_attackPoint.position, _attackRange);
    }

    
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyContactDamage : MonoBehaviour
{
    private GameObject _player;
    private EntityHealthSystem _playerHealth;
    [SerializeField] private int _damage = 20;
    private PlayerMovement _playerMovement;
    private BoxCollider2D _collider;
    private BoxCollider2D _playerCollider;
    private PlayerCombat _playerCombat;


    private void Start() {
        _collider = GetComponent<BoxCollider2D>();
        _player = GameObject.FindWithTag("Player");
        _playerCollider = _player.GetComponent<BoxCollider2D>();
        _playerCombat = _player.GetComponent<PlayerCombat>();
        _playerHealth = _player.GetComponent<EntityHealthSystem>();
        _playerMovement = _player.GetComponent<PlayerMovement>();
    }

    private bool IsColliderBetween(BoxCollider2D colliderA, BoxCollider2D colliderB, BoxCollider2D colliderToCheck)
    {
        // Get the center positions of the colliders along the desired axis
        float positionA = colliderA.bounds.center.x;
        float positionB = colliderB.bounds.center.x;
        float positionToCheck = colliderToCheck.bounds.center.x;

        // Check if colliderToCheck is between colliderA and colliderB
        return (positionA < positionToCheck && positionToCheck < positionB) || (positionB < positionToCheck && positionToCheck < positionA);
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.tag == "Player") {
            if (collision.transform.position.x < transform.position.x)
                _playerMovement.StartKnockBack(true);
            else
                _playerMovement.StartKnockBack(false);
            if (!_playerCombat.shieldCollider.enabled || (_playerCombat.shieldCollider.enabled && !IsColliderBetween(_collider, _playerCollider, _playerCombat.shieldCollider)))
                _playerHealth.TakeDamage(_damage);
        }
    }
}

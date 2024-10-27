using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemy : MonoBehaviour
{
    private GameObject _player;
    [SerializeField] private Transform _attackPoint;
    private BoxCollider2D _boxCollider;
    [SerializeField] private LayerMask _playerLayer;
    [SerializeField] private float _attackCooldown;
    private float _nextAttackTime = 0;
    [SerializeField] private int _attackDamage;
    [SerializeField] private float _attackRange;
    [SerializeField] private float _detectionRange;
    [SerializeField] private float _distance;
    private Animator _animator;
    private EntityHealthSystem _playerHealth;
    private EnemyPatrol _enemyPatrol;

    [SerializeField] private float _movementSpeed;

    private FlippingGameObject _flippingLogic;

    [SerializeField] private float _followDuration;
    private float _followTimer;

    [SerializeField] private Transform _groundDetectionFront;
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private float _groundDetectionRange = 1;
    [SerializeField] private LayerMask _terrainLayer;

    private PlayerCombat _playerCombat;
    private BoxCollider2D _playerCollider;
    private EntityHealthSystem _enemyHealth;
    private int _lastHealth;

    private void Start()
    {
        _boxCollider = GetComponent<BoxCollider2D>();
        _player = GameObject.FindWithTag("Player");
        _playerCollider = _player.GetComponent<BoxCollider2D>();
        _animator = GetComponent<Animator>();
        _enemyPatrol = GetComponentInParent<EnemyPatrol>();
        _playerHealth = _player.GetComponent<EntityHealthSystem>();
        _playerCombat = _player.GetComponent<PlayerCombat>();
        _flippingLogic = GetComponent<FlippingGameObject>();
        _followTimer = _followDuration;

        _enemyHealth = GetComponent<EntityHealthSystem>();
        _lastHealth = _enemyHealth.CurrentHealth;

        Physics2D.IgnoreLayerCollision(8, 8);
        Physics2D.IgnoreLayerCollision(8, 9);
    }

    private void Update()
    {
        _followTimer += Time.deltaTime;
        if(_lastHealth > _enemyHealth.CurrentHealth) {
            _lastHealth = _enemyHealth.CurrentHealth;
            _followTimer = 0;
        }
        if ((PlayerInSight() || _followTimer < _followDuration) && !_playerHealth.IsDead) {
            SetPatrolActive(false);
            if (PlayerInAttackRange()){
                _animator.SetInteger("movingState", (int) MovementState.idle);
                if (Time.time >= _nextAttackTime) {
                    _animator.SetTrigger("attack");
                    _nextAttackTime = Time.time + _attackCooldown;
                }
            }
            else{
                if (DetectGroundFront())
                    _animator.SetInteger("movingState", (int) MovementState.moving);
                else
                    _animator.SetInteger("movingState", (int) MovementState.idle);
                PlayerFollow();
            }
            
        }
        else if (_enemyPatrol != null) {
            SetPatrolActive(true);
        }
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




    private void DamagePlayer() {
        if (PlayerInAttackRange()) {
            bool shouldDealDamage = true;

            if (_playerCombat.shieldCollider.enabled) {
                shouldDealDamage = !IsColliderBetween(_boxCollider, _playerCollider, _playerCombat.shieldCollider);
                print(shouldDealDamage);
            }

            if (shouldDealDamage)
                _playerHealth.TakeDamage(_attackDamage);
        }
    }

    private bool PlayerInSight() {
        
        RaycastHit2D hit = Physics2D.BoxCast(_boxCollider.bounds.center + transform.right * _detectionRange * transform.localScale.x * _distance,
        new Vector3(_boxCollider.bounds.size.x * _detectionRange, _boxCollider.bounds.size.y, _boxCollider.bounds.size.z), 0, Vector2.left, 0, _playerLayer);
        if (hit.collider != null)
            _followTimer = 0;
        return hit.collider != null && CheckIfRayHitsPlayer();
    }

    private bool CheckIfRayHitsPlayer() {
        Vector2 direction = _player.GetComponent<PlayerMovement>().GetPlayerPositionOffset() - transform.position;
            RaycastHit2D rayhit = Physics2D.Raycast(_boxCollider.bounds.center, direction, _detectionRange, _terrainLayer);
            if (rayhit) {
                Debug.DrawRay(_boxCollider.bounds.center, direction * rayhit.distance, Color.blue);
            }
            return rayhit && rayhit.transform.tag == "Player";
    }

    private bool PlayerInAttackRange() {
        Collider2D hit = Physics2D.OverlapCircle(_attackPoint.position, _attackRange, _playerLayer);
        return hit != null;
    }

    private bool DetectGroundFront() {
        Collider2D hit = Physics2D.OverlapCircle(_groundDetectionFront.position, _groundDetectionRange, _groundLayer);
        return hit != null;
    }

    private void PlayerFollow() {
        if (_player.transform.position.x <= transform.position.x) {
            _flippingLogic.FlipLeft();
            if (DetectGroundFront())
                transform.position += Vector3.left * _movementSpeed * Time.deltaTime;
        }
        else {
            _flippingLogic.FlipRight();
            if (DetectGroundFront())
                transform.position += Vector3.right *_movementSpeed * Time.deltaTime;
        }
    }

    private void SetPatrolActive(bool value) {
        if (_enemyPatrol != null)
                _enemyPatrol.enabled = value;
    }


    private void OnDrawGizmosSelected() {
        _boxCollider = GetComponent<BoxCollider2D>();
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(_boxCollider.bounds.center + transform.right * _detectionRange * transform.localScale.x * _distance, 
        new Vector3(_boxCollider.bounds.size.x * _detectionRange, _boxCollider.bounds.size.y, _boxCollider.bounds.size.z));
        Gizmos.color = Color.blue;

        if (_attackPoint == null)
            return;
        
        Gizmos.DrawWireSphere(_attackPoint.position, _attackRange);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(_groundDetectionFront.position, _groundDetectionRange);

    }

}

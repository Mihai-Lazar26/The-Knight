using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemy : MonoBehaviour
{
    [SerializeField] private BoxCollider2D _boxCollider;
    [SerializeField] LayerMask _playerLayer;
    private EntityHealthSystem _playerHealth;
    [SerializeField] private float _detectionRange;
    [SerializeField] private float _distance;
    [SerializeField] private Transform _firePoint;
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private float _attackCooldown;
    private float _nextAttackTime = 0;
    private Animator _animator;
    private GameObject _player;
    private FlippingGameObject _flipLogic;
    [SerializeField] private LayerMask _terrainLayer;
    [SerializeField] private AudioSource _shootSFX;
    void Start()
    {
        _player = GameObject.FindWithTag("Player");
        _boxCollider = GetComponent<BoxCollider2D>();
        _animator = GetComponent<Animator>();
        _flipLogic = GetComponent<FlippingGameObject>();
        _playerHealth = _player.GetComponent<EntityHealthSystem>();
    }
    void Update()
    {
        if (!_playerHealth.IsDead && PlayerInSight()) {
           
            if (_player.transform.position.x <= transform.position.x) {
                _flipLogic.FlipLeft();
            }
            else {
                _flipLogic.FlipRight();
            }
            if (Time.time >= _nextAttackTime) {
                _nextAttackTime = Time.time + _attackCooldown;
                _animator.SetTrigger("attack");
                // Shoot();
            }
            
        }
        
    }

    public void Shoot() {
        _shootSFX.Play();
        Instantiate(_bulletPrefab, _firePoint.position, _firePoint.rotation);
    }

    private bool PlayerInSight() {
        
        RaycastHit2D hit = Physics2D.BoxCast(_boxCollider.bounds.center + transform.right * _detectionRange * transform.localScale.x * _distance,
        new Vector3(_boxCollider.bounds.size.x * _detectionRange, _boxCollider.bounds.size.y * _detectionRange / 6, _boxCollider.bounds.size.z), 0, Vector2.left, 0, _playerLayer);
        return hit.collider != null && CheckIfRayHitsPlayer();
    }

    private bool CheckIfRayHitsPlayer() {
        Vector2 direction = _player.GetComponent<PlayerMovement>().GetPlayerPositionOffset(0, 1) - transform.position;
            RaycastHit2D rayhit = Physics2D.Raycast(_boxCollider.bounds.center, direction, _detectionRange, _terrainLayer);
            if (rayhit) {
                Debug.DrawRay(_boxCollider.bounds.center, direction * rayhit.distance, Color.blue);
            }
            return rayhit && rayhit.transform.tag == "Player";
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(_boxCollider.bounds.center + transform.right * _detectionRange * transform.localScale.x * _distance, 
        new Vector3(_boxCollider.bounds.size.x * _detectionRange, _boxCollider.bounds.size.y * _detectionRange / 6, _boxCollider.bounds.size.z));
    }

}

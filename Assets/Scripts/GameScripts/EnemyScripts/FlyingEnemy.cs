using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class FlyingEnemy : MonoBehaviour
{
    private BoxCollider2D _boxCollider;
    [SerializeField] private float _movementSpeed;
    [SerializeField] private float _detectionRange;
    [SerializeField] private LayerMask _playerLayer;
    private GameObject _player;
    private BoxCollider2D _playerBoxCollider;
    private FlippingGameObject _flippingLogic;
    private Vector3 _initialPosition;
    private Animator _animator;
    private EntityHealthSystem _playerHealth;
    [SerializeField] private float _nextWaypointDistance = 3f;
    private Path _path;
    private int _currentWaypoint = 0;
    private Seeker _seeker;
    private Rigidbody2D _rb;
    [SerializeField] private LayerMask _terrainLayer;

    private enum PathTarget {
        initial,
        player
    }
    private PathTarget _currentTarget = PathTarget.initial;
    private Vector3 _target;

    private void Start()
    {
        _boxCollider = GetComponent<BoxCollider2D>();
        _player = GameObject.FindWithTag("Player");
        _animator = GetComponent<Animator>();
        _flippingLogic = GetComponent<FlippingGameObject>();
        _playerBoxCollider = _player.GetComponent<BoxCollider2D>();
        _initialPosition = new Vector2(transform.position.x, transform.position.y);
        _playerHealth = _player.GetComponent<EntityHealthSystem>();

        _seeker = GetComponent<Seeker>();
        _rb = GetComponent<Rigidbody2D>();
        _target = _initialPosition;

        InvokeRepeating("UpdatePath", 0f, .5f);
    }

    private void SelectPath(PathTarget target) {
        _currentTarget = target;
        if (_currentTarget == PathTarget.initial) {
            _target = _initialPosition;
        }
        else if (_currentTarget == PathTarget.player) {
            _target = _player.GetComponent<PlayerMovement>().GetPlayerPositionOffset(0, 1);
        }
    }

    private void UpdatePath(){
        if (_seeker.IsDone()) {
            _seeker.StartPath(_rb.position, _target, OnPathComplete);
        }
        
    }

    private void OnPathComplete(Path p) {
        if (!p.error) {
            _path = p;
            _currentWaypoint = 0;
        }
    }

    private void Update()
    {
        if (PlayerInSight() && !_playerHealth.IsDead) {
            _animator.SetInteger("movingState", (int) MovementState.moving);
            SelectPath(PathTarget.player);
        }
        else {
            SelectPath(PathTarget.initial);
            Vector2.Distance(transform.position, _initialPosition);
            float distanceFromInitial = Vector2.Distance(transform.position, _initialPosition);
            if (distanceFromInitial <= 2f) {
                transform.position = Vector2.MoveTowards(transform.position, _initialPosition, 1f * Time.deltaTime);
            }
            if (transform.position == _initialPosition) {
                _animator.SetInteger("movingState", (int) MovementState.idle);
            }
        }

        if (_path == null || _currentWaypoint >= _path.vectorPath.Count)
            return;

        Vector2 direction = ((Vector2) _path.vectorPath[_currentWaypoint] - _rb.position).normalized;
        Vector2 force = direction *_movementSpeed * Time.deltaTime;

        if (_animator.GetBool("isMoving")) {
            _rb.AddForce(force);
        }

        float distance = Vector2.Distance(transform.position, _path.vectorPath[_currentWaypoint]);
        if (distance < _nextWaypointDistance) {
            _currentWaypoint++;
        }
        if(_animator.GetInteger("movingState") == (int) MovementState.idle)
            return;
        if (force.x > 0.2f) {
            _flippingLogic.FlipRight();
        }
        else if (force.x < -0.2f) {
            _flippingLogic.FlipLeft();
        }
    }

    private bool PlayerInSight() {
        Collider2D hit = Physics2D.OverlapCircle(_boxCollider.bounds.center, _detectionRange, _playerLayer);
        if (hit != null) {
            
        }
        return hit != null && CheckIfRayHitsPlayer();
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
        _boxCollider = GetComponent<BoxCollider2D>();
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_boxCollider.bounds.center, _detectionRange);
    }
}

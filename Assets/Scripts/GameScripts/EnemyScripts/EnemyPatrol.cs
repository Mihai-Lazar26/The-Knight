using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    [Header ("Patrol Points")]
    [SerializeField] private Transform _leftEdge;
    [SerializeField] private Transform _rightEdge;

    [Header ("Enemy")]
    [SerializeField] private Transform _enemy;

    [Header ("Movement Parameters")]
    [SerializeField] private float _speed = 10;
    private bool _movingLeft;
    [SerializeField] private float _idleDuration;
    private float _idleTimer;
    private Animator _animator;


    private FlippingGameObject _flippingLogic;

    
    private void Start()
    {
        _animator = _enemy.GetComponent<Animator>();
        _flippingLogic = _enemy.GetComponent<FlippingGameObject>();
    }

    private void OnDisable() {
        _animator.SetInteger("movingState", (int) MovementState.idle);
    }
    private void Update()
    {
        if (_movingLeft) {
            _flippingLogic.FlipLeft();
            if (_enemy.position.x >= _leftEdge.position.x) {         
                MoveInDirection(-1);
            }
            else {
                ChangeDirection();
            }
        }
        else {
            _flippingLogic.FlipRight();
            if (_enemy.position.x <= _rightEdge.position.x) {
                MoveInDirection(1);
            }
            else {
                ChangeDirection();
            }
        }
    }

    private void ChangeDirection() {
        _animator.SetInteger("movingState", (int) MovementState.idle);
        _idleTimer += Time.deltaTime;
        if (_idleTimer > _idleDuration){
            _movingLeft = !_movingLeft;
        }
            
    }

    private void MoveInDirection(int direction) {

        _idleTimer = 0;

        _animator.SetInteger("movingState", (int) MovementState.moving);

        _enemy.position = new Vector3(_enemy.position.x + Time.deltaTime * direction * _speed, _enemy.position.y, _enemy.position.z);

    }
}

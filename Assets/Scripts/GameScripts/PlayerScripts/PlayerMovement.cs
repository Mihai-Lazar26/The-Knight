using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D _rb;
    private BoxCollider2D _boxCollider2D;
    private SpriteRenderer _spriteRenderer;
    private Animator _animator;
    [SerializeField] private LayerMask _jumpableGround; 
    private float _dirX;
    [SerializeField] private float _moveSpeed = 7f;
    [SerializeField] private float _jumpForce = 14f;
    [SerializeField] private float _wallSlideSpeed = 4f;
    [SerializeField] private Vector2 _wallJumpPower;
    [SerializeField] private float _wallJumpDuration;
    private float _wallJumpTimer;

    private bool _isWallSliding = false;
    private FlippingGameObject _flipLogic;

    [SerializeField] private float _kbDuration;
    private float _kbTimer;
    [SerializeField] private float _kbForce;
    private bool _kbFromRight;
    private EntityHealthSystem _playerHealth;

    [SerializeField] private AudioSource _walkSFX;
    [SerializeField] private AudioSource _jumpingSFX;
    [SerializeField] private LoadedData _playerData;

    [SerializeField] private PauseMenu _pauseMenu;
    [SerializeField] private Transform _groundDetector;
    [SerializeField] private Transform _wallDetector;
    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _boxCollider2D = GetComponent<BoxCollider2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
        _flipLogic = GetComponent<FlippingGameObject>();
        _playerHealth = GetComponent<EntityHealthSystem>();

        _kbTimer = _kbDuration;
        _wallJumpTimer = _wallJumpDuration;
        Physics2D.IgnoreLayerCollision(7, 8);
        Physics2D.IgnoreLayerCollision(7, 9, false);

        transform.position = _playerData.loadedData.playerPosition;
    }

    private void Update()
    {
        if (transform.position.y < -100) {
            _playerHealth.TakeDamage(_playerHealth.MaxHealth);
        }
        
        WallSlide();
        if (_kbTimer < _kbDuration) {
            if (_kbFromRight) {
                _rb.velocity = new Vector2(-_kbForce, _kbForce);
            }
            else {
                _rb.velocity = new Vector2(_kbForce, _kbForce);
            }
            _kbTimer += Time.deltaTime;
        }
        else if (_wallJumpTimer < _wallJumpDuration) {
            _wallJumpTimer += Time.deltaTime;
        }
        else {
            _rb.velocity = new Vector2(_dirX * _moveSpeed, _rb.velocity.y);
        }
        UpdateAnimationState();
        UpdateSounds();
    }

    private void WallSlide() {
        if (IsWalled() && !IsGrounded()) {
            _animator.SetBool("walled", true);
            _rb.velocity = new Vector2(_rb.velocity.x, Mathf.Clamp(_rb.velocity.y, -_wallSlideSpeed, float.MaxValue));
            _isWallSliding = true;
        }
        else {
            _animator.SetBool("walled", false);
            _isWallSliding = false;
        }
    }

    public void Movement(InputAction.CallbackContext context) {
        if (!_playerHealth.IsDead && !_pauseMenu.IsPaused) {
            _dirX = context.ReadValue<float>();
            // _rb.velocity = new Vector2(_dirX * _moveSpeed, _rb.velocity.y);
        }
    }

    public void Jump(InputAction.CallbackContext context) {
        if (_playerHealth.IsDead || _pauseMenu.IsPaused)
            return;
        
        
        if (context.performed && IsGrounded()) {
            _rb.velocity = new Vector2(_rb.velocity.x, _jumpForce);
            _jumpingSFX.Play();
        }
        else if(context.performed && _isWallSliding) {
            _isWallSliding = false;
            float direction = -Mathf.Sign(transform.rotation.y);
            _rb.velocity = new Vector2(direction * _wallJumpPower.x, _wallJumpPower.y);
            _wallJumpTimer = 0;
        }

        if (context.canceled && _rb.velocity.y > 0f) {
            _rb.velocity = new Vector2(_rb.velocity.x, _rb.velocity.y * 0.5f);
        }
    }

    private void UpdateSounds() {
        if (IsGrounded()) {
            if (_rb.velocity.x != 0 && !_walkSFX.isPlaying) {
                _walkSFX.Play();
            }
        }
        else {
            _walkSFX.Stop();
        }
    }
    private void UpdateAnimationState() {
        MovementState state;
        if (_dirX > 0) {
            state = MovementState.moving;
            _flipLogic.FlipRight();
        }
        else if (_dirX < 0) {
            state = MovementState.moving;
            _flipLogic.FlipLeft();
        }
        else {
            state = MovementState.idle;
        }

        if (_rb.velocity.y > .1f) {
            state = MovementState.jumping;
        } 
        else if (_rb.velocity.y < -.1f) {
            state = MovementState.falling;
        }

        _animator.SetInteger("movingState", (int) state);
    }

    private bool IsGrounded() {
        return Physics2D.BoxCast(_groundDetector.position, new Vector3(_boxCollider2D.bounds.size.x, 0.5f, 0), 0f, Vector2.down, .0f, _jumpableGround);
    }

    private bool IsWalled() {
        return Physics2D.BoxCast(_wallDetector.position, new Vector3(1, _boxCollider2D.bounds.size.y * 0.8f, 0), 0f, Vector2.right, 0f, _jumpableGround);
    }

    public void StartKnockBack(bool direction) {
        _kbFromRight = direction;
        _kbTimer = 0;
    }

    public void setMovementVelocity0() {
        _rb.velocity = new Vector2();
    }

    public Vector3 GetPlayerPositionOffset(float offSetX = 0, float offSetY = 0) {
        return new Vector3(transform.position.x + offSetX, transform.position.y + offSetY, transform.position.z);
    }

    private void OnDrawGizmosSelected() {
        _boxCollider2D = GetComponent<BoxCollider2D>();
        Gizmos.DrawWireCube(_groundDetector.position,new Vector3(_boxCollider2D.bounds.size.x, 0.5f, 0));
        Gizmos.DrawWireCube(_wallDetector.position, new Vector3(1, _boxCollider2D.bounds.size.y * 0.8f, 0));

    }

}

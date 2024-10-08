using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class BirdMover : MonoBehaviour
{
    [SerializeField] InputManagerBase _inputManager;
    [SerializeField] Animator _animator;
    [SerializeField] float _flapForce = 1.5f, _gravity = 2f, _moveSpeed = 3f, _flapDelay = 0.25f;
    [SerializeField] float _raycastDistance = 0.52f, _skidTime = 1.1f;
    [SerializeField] LayerMask _groundedMask;
    [SerializeField] SpriteRenderer _spriteRenderer;

    float _flapTime, _currentSkidTime, _speedFactor = 1f;
    float _flapVelocity, _moveVelocity;
    RaycastHit2D[] _raycastHit;
    bool _flapping;
    Vector3 _direction;
    bool _skidding;
    static readonly int Flapping = Animator.StringToHash("Flap");
    static readonly int Flying = Animator.StringToHash("Flying");
    static readonly int Speed = Animator.StringToHash("Speed");
    static readonly int Skidding = Animator.StringToHash("Skidding");
    bool _isInvulnerable;

    bool CanFlap => !_skidding && Time.time >= _flapTime;

    IInputManager InputManager => _inputManager.InputManager;
    bool IsPlayer => TryGetComponent<PlayerInputManager>(out var playerInputManager);

    bool IsMount =>
        gameObject.layer == LayerMask.NameToLayer("Player Mount") ||
        gameObject.layer == LayerMask.NameToLayer("Enemy Mount");

    bool IsGrounded
    {
        get
        {
            if (_flapping) return false;
            return Physics2D.RaycastNonAlloc(transform.position, Vector2.down, _raycastHit, _raycastDistance,
                _groundedMask) > 0;
        }
    }

    bool InputReceived => _flapping || !Mathf.Approximately(InputManager.Movement.x, 0f);

    bool IsFlying => !IsGrounded;

    public void Init(Vector3 direction, float speedFactor = 1f)
    {
        SetInvulnerability(true);
        _direction = direction;
        _speedFactor = speedFactor;
    }

    public IEnumerator ShowSpawnEffect(float spawnLength)
    {
        SetInvulnerability(true);
        var originalBirdColor = _spriteRenderer.material.color;

        float time = spawnLength;
        transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        while (_spriteRenderer && time > 0f && !InputReceived)
        {
            time -= Time.deltaTime;
            _spriteRenderer.material.color = Random.ColorHSV();
            if (transform.localScale.x < 1f)
            {
                transform.localScale = Vector3.Lerp(transform.localScale, Vector3.one, Time.deltaTime);
            }

            yield return null;
        }

        transform.localScale = Vector3.one;
        _spriteRenderer.material.color = originalBirdColor;
        SetInvulnerability(false);
    }

    void SetInvulnerability(bool invulnerable)
    {
        _isInvulnerable = invulnerable;
        _inputManager.enabled = !invulnerable;

        if (IsMount)
        {
            Debug.Log($"Mount {name} is invulnerable.");
            Physics2D.IgnoreLayerCollision(gameObject.layer,
                LayerMask.NameToLayer("Player"), true);
            Physics2D.IgnoreLayerCollision(gameObject.layer,
                LayerMask.NameToLayer("Enemy Bird"), true);
            return;
        }
       
        Physics2D.IgnoreLayerCollision(gameObject.layer,
            IsPlayer ? LayerMask.NameToLayer("Enemy Bird") :
                       LayerMask.NameToLayer("Player"), invulnerable);
    }

    void Awake()
    {
        InputManager.OnFlapPressed += OnFlapPressed;
        _raycastHit = new RaycastHit2D[10];
        Init(Vector3.right);
        SetInvulnerability(false);
    }

    void OnDisable()
    {
        InputManager.OnFlapPressed -= OnFlapPressed;
    }

    void Start()
    {
        _flapTime = Time.time;
    }

    void FixedUpdate()
    {
        HandleVerticleMovement();
        HandleHorizontalMovement();
    }

    void HandleVerticleMovement()
    {
        if (_flapping)
        {
            Flap();
            transform.position += Vector3.up * (_flapVelocity * Time.deltaTime);
            return;
        }

        if (!IsGrounded)
        {
            _animator.SetBool(Flying, true);
            ApplyGravity();
        }
        else
        {
            _animator.SetBool(Flying, false);
            _flapVelocity = 0f;
        }

        transform.position += Vector3.up * (_flapVelocity * Time.deltaTime);
    }

    void ApplyGravity()
    {
        _flapVelocity -= _gravity * Time.deltaTime;
    }

    void Flap()
    {
        if (_isInvulnerable) SetInvulnerability(false);
        _flapTime = Time.time + _flapDelay;
        _animator.SetBool(Flying, true);
        _animator.SetTrigger(Flapping);
        _flapVelocity = _flapForce;
        _flapping = false;
        _skidding = false;
    }

    bool ShouldSkid
    {
        get
        {
            if (_skidding || IsFlying) return false;
            if (_moveVelocity <= 0.75f) return false;
            if (_direction == Vector3.right && InputManager.Movement.x <= 0f) return true;
            return _direction == Vector3.left && InputManager.Movement.x >= 0f;
        }
    }

    void HandleHorizontalMovement()
    {
        if (ShouldSkid)
        {
            StartSkid();
        }

        if (_skidding)
        {
            HandleSkidMovement();
            return;
        }

        if (ShouldChangeDirection)
        {
            ChangeDirection();
        }

        if (_isInvulnerable && !Mathf.Approximately(InputManager.Movement.x, 0f))
        {
            SetInvulnerability(false);
        }
        
        _moveVelocity = Mathf.Abs(InputManager.Movement.x) * (_moveSpeed * _speedFactor);
        transform.position += _direction * (_moveVelocity * Time.deltaTime);
        _animator.SetFloat(Speed, Mathf.Abs(InputManager.Movement.x));
    }

    bool ShouldChangeDirection
    {
        get
        {
            if (_direction == Vector3.right && InputManager.Movement.x < 0f) return true;
            //_moveVelocity = 0f;
            return _direction == Vector3.left && InputManager.Movement.x > 0f;
        }
    }

    void ChangeDirection()
    {
        _direction = _direction == Vector3.right ? Vector3.left : Vector3.right;
        transform.localEulerAngles = new Vector3(0f, _direction == Vector3.right ? 0f : 180f, 0f);
    }
    
    void HandleSkidMovement()
    {
        transform.position += _direction * (_moveVelocity * Time.deltaTime);
        _moveVelocity = Mathf.Max(_moveSpeed - (_moveSpeed * 0.15f * Time.deltaTime), 0f);
        _currentSkidTime += Time.deltaTime;
        if (_currentSkidTime >= _skidTime)
        {
            StopSkid();
        }
    }

    void StopSkid()
    {
        _moveVelocity = 0f;
        _skidding = false;
        _animator.SetBool(Skidding, false);
    }

    void StartSkid()
    {
        _skidding = true;
        _animator.SetFloat(Speed, 0f);
        _animator.SetBool(Skidding, true);
        _currentSkidTime = 0f;
    }

    void OnFlapPressed()
    {
        if (CanFlap) _flapping = true;
    }
}

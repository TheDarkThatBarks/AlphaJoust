using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyInputManager : InputManagerBase, IInputManager
{
    public Vector2 Movement { get; private set; }
    public event Action<Vector2> OnMoveReceived = delegate(Vector2 vector2) {  };
    public event Action OnFlapPressed = delegate {  };

    [SerializeField] Vector2 _flapIntervalRange = new Vector2(0.5f, 3f);
    [SerializeField] Vector2 _directionChangeIntervalRange = new Vector2(5f, 10f);
    [SerializeField] float _maxHeight = 3.75f, _kickbackBoundary = 5f;

    float _nextFlapTime;
    Vector2 _direction;
    float _nextDirectionChange;
    Thunk _thunk;
    readonly Vector2 _moveLeft = new Vector2(-1f, 0f);
    readonly Vector2 _moveRight = new Vector2(1f, 0f);

    bool ShouldFlap => transform.position.y < _maxHeight && Time.time >= _nextFlapTime;
    bool ShouldChangeDirection => Time.time >= _nextDirectionChange;
    
    void OnEnable()
    {
        SetNextDirectionChangeTime();
        SubscribeToThunkEvent();
        _direction = transform.localEulerAngles == Vector3.zero ? Vector2.right : Vector2.left;
    }

    void OnDisable()
    {
        UnsubscribeFromThunkEvent();
    }

    void Update()
    {
        if (ShouldFlap)
        {
            Flap();
        }

        if (ShouldChangeDirection)
        {
            ChangeDirection();
        }

        Move();
    }

    void Move()
    {
        Movement = GetMoveDirection();
        OnMoveReceived(Movement);
    }

    Vector2 GetMoveDirection()
    {
        return _direction == Vector2.right ? _moveRight : _moveLeft;
    }


    void ChangeDirection()
    {
        SetNextDirectionChangeTime();
        _direction = _direction == Vector2.right ? Vector2.left : Vector2.right;
    }

    void Flap()
    {
        SetNextFlapTime();
        OnFlapPressed();
    }

    void SetNextFlapTime()
    {
        _nextFlapTime = Random.Range(_flapIntervalRange.x, _flapIntervalRange.y) + Time.time;
    }

    void OnThunk()
    {
        ChangeDirection();
        ApplyKnockBack();
    }

    void ApplyKnockBack()
    {
        var position = transform.position;
        if (Mathf.Abs(position.x) > _kickbackBoundary) return;
        position.x += GetMoveDirection().x * 0.5f;
        transform.position = position;
    }

    void SetNextDirectionChangeTime()
    {
        _nextDirectionChange = Random.Range(_directionChangeIntervalRange.x, _directionChangeIntervalRange.y) + Time.time;
    }

    void SubscribeToThunkEvent()
    {
        TryGetComponent<Thunk>(out _thunk);
        if (_thunk)
        {
            _thunk.OnThunk += OnThunk;
        }
    }

    void UnsubscribeFromThunkEvent()
    {
        if (_thunk)
        {
            _thunk.OnThunk -= OnThunk;
        }
    }
}

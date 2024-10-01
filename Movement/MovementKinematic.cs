using Stats;
using System.Collections.Generic;
using UnityEngine;

public enum MovementAction
{
    MoveLeft = -1,
    MoveRight = 1,
    Stop = 0
}

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CapsuleCollider2D))]
public class MovementKinematic : MonoBehaviour
{
    private enum MovementStates
    {
        MoveLeft = -1,
        MoveRight = 1,
        Stand = 0,
        Jump = 3,
        Fall = 4
    }

    #region Fields
    [Header("Ground")]
    [SerializeField]
    private Transform _feetPos;
    [SerializeField]
    private float _feetRadius = 0.2f;

    [Header("Jump")]
    [SerializeField]
    float gravity = -9.81f;
    [SerializeField]
    float gravityScale = 1;
    [SerializeField]
    protected float _jumpForce = 1f;

    [Header("Barrier")]
    [SerializeField]
    private bool _canBypassBarrier = true;
    [SerializeField]
    private ContactFilter2D _movementFilter;
    [SerializeField]
    private float _collisionOffset = 0.05f;

    private LayerMask _groundLayer;

    private Rigidbody2D _rigidBody;
    private Speed _speed;
    private bool _facingRight = true;

    private float _jumpVelocity = 0;

    public MovementAction CurrentAction { get; private set; }
    private MovementStates _currentState;

    private int _direction;

    private bool _isGrounded => Physics2D.OverlapCircle(_feetPos.position, _feetRadius, _groundLayer);
    #endregion

    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        _rigidBody.isKinematic = true;
        _rigidBody.constraints = RigidbodyConstraints2D.FreezeRotation;

        _groundLayer = LayerMask.GetMask("Ground");
        _currentState = MovementStates.Stand;
    }

    private void Start()
    {
        _speed = GetComponent<CharacterStats>().Speed;
    }

    private void FixedUpdate()
    {
        if(IsBarrierAhead() && _isGrounded) 
        {
            if (_canBypassBarrier)
            {
                _jumpVelocity = _jumpForce;
                _currentState = MovementStates.Jump;
            }
            else
                CurrentAction = MovementAction.Stop;
        }
        if(!_isGrounded)
        {
            _currentState = MovementStates.Fall;
        }
        _rigidBody.MovePosition(GetMovementVector());
    }

    private Vector2 GetMovementVector()
    {
        Vector2 _movementVector = _rigidBody.position;
        _movementVector.x = _rigidBody.position.x + _direction * _speed.CurrentValue * Time.fixedDeltaTime;

        switch (_currentState)
        {
            case MovementStates.MoveRight:
                if(!_facingRight)
                {
                    Flip();
                    _facingRight = true;
                }
                break;
            case MovementStates.MoveLeft:
                if(_facingRight)
                {
                    Flip();
                    _facingRight= false;
                }
                break;
            case MovementStates.Stand:
                break;
            case MovementStates.Jump:
                {
                    _jumpVelocity += (gravity * gravityScale) * Time.fixedDeltaTime;
                    _movementVector.y = _rigidBody.position.y + _jumpVelocity * Time.fixedDeltaTime;
                    if (IsBarrierAhead())
                        _movementVector.x = _rigidBody.position.x;
                    if (_jumpVelocity < 0)
                        _currentState = MovementStates.Fall;
                    break;
                }
            case MovementStates.Fall:
                {
                    _jumpVelocity += (gravity * gravityScale) * Time.fixedDeltaTime;
                    _movementVector.y = _rigidBody.position.y + _jumpVelocity * Time.fixedDeltaTime;
                    if (IsBarrierAhead())
                        _movementVector.x = _rigidBody.position.x;
                    if (_isGrounded)
                    {
                        _jumpVelocity = 0;
                        UpdateStateOnAction();
                        if (IsBarrierAhead())
                            _currentState = MovementStates.Stand;
                    }
                    break;
                }   
        }

        return _movementVector;
    }

    public void DoAction(MovementAction action)
    {
        CurrentAction = action;
        if (_currentState != MovementStates.Fall && _currentState != MovementStates.Jump)
        {
            UpdateStateOnAction();
        }
    }

    private void UpdateStateOnAction()
    {
        _currentState = (MovementStates)CurrentAction;
        _direction = (int)CurrentAction;
    }

    private void Flip()
    {
        transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
    }

    public bool IsBarrierAhead()
    {
        List<RaycastHit2D> castCollisions = new List<RaycastHit2D>();

        int countCollision = _rigidBody.Cast(Vector2.right * (_facingRight ? 1 : -1), _movementFilter, castCollisions, _speed.CurrentValue * Time.fixedDeltaTime + _collisionOffset);

        return countCollision > 0;
    }
}

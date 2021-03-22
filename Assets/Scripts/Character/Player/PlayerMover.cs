using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InputManagement;
using StatusManagement;
using UnityEngine.AI;

public class PlayerMover : MonoBehaviour, IPlayerSubClass
{
    [SerializeField] private float _moveSpeed = 2;
    private Rigidbody _rigid;
    private NavMeshAgent _navMeshAgent;
    private Vector2 _moveDirection;
    private IPlayerInput _playerInput;
    private PlayerAnimator _playerAnimator;
    private Status _status;

    void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _rigid = GetComponent<Rigidbody>();
        _playerAnimator = GetComponent<PlayerAnimator>();
        _status = GetComponent<Status>();
    }

    void Update()
    {
        _moveDirection = _playerInput.MoveDirection;
    }

    void FixedUpdate()
    {
        _Move();
    }

    void OnDisable()
    {
        _navMeshAgent.velocity = Vector3.Scale(_rigid.velocity, new Vector3(0, 1, 0));
    }

    public void SetInputter(IPlayerInput inputter)
    {
        _playerInput = inputter;
    }

    private void _Move()
    {
        Vector3 moveForward = transform.forward.normalized * Mathf.Clamp(_moveDirection.sqrMagnitude, 0, 1);
        moveForward *= _moveSpeed;
        if (_status.SP.Value < 20) moveForward *= 0.5f;
        if (_playerAnimator.IsEquiping) moveForward *= 0.7f;
        _navMeshAgent.velocity = moveForward;
    }

}

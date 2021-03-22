using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StatusManagement;
using InputManagement;
using Item;
using System;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Status))]
[RequireComponent(typeof(PlayerMover))]
[RequireComponent(typeof(PlayerRotator))]
[RequireComponent(typeof(PlayerAnimator))]
public class PlayerBehaviour : MonoBehaviour, IDamageable
{

    private Controller CurrentInputter = Controller.KeyboardMouse;
    private Controller NewInputter => InputManager.Instance.Inputter;

    #region property
    public Type InputMode { get => _playerInput.GetType(); }
    public Transform Transform { get => transform; }
    public Status Status { get; private set; }
    #endregion

    #region serialize field
    [SerializeField]
    private Transform cameraTarget;
    [SerializeField]
    private Transform cameraStartPosition;
    [SerializeField]
    private GameObject damageEffect;
    #endregion

    #region private field
    private IPlayerInput _playerInput;
    private PlayerMover _playerMover;
    private PlayerRotator _playerRotator;
    private PlayerAnimator _playerAnimator;
    private CameraController _cameraController;
    #endregion



    #region unity function
    void Awake()
    {
        Init();
    }

    void Update()
    {
        _CheckInputter();

        _playerMover.enabled = _playerAnimator.IsMoving && !GameManager.Instance.IsStageClear;
        _playerRotator.enabled = _playerAnimator.IsMoving && !GameManager.Instance.IsStageClear;

        Action();


        if (_playerAnimator.IsMoving && _playerInput.MoveDirection.sqrMagnitude > 0.99)
            Status.SP.Use(10 * Time.deltaTime);
        else
            Status.SP.Heal(15 * Time.deltaTime);

        if (Status.HP.Value <= 0) Die();
    }
    #endregion



    #region private function
    private void Init()
    {
        Status = GetComponent<Status>();
        _playerInput = new KeyBoardInput();
        _playerMover = GetComponent<PlayerMover>();
        _playerRotator = GetComponent<PlayerRotator>();
        _playerAnimator = GetComponent<PlayerAnimator>();
        _playerMover.SetInputter(_playerInput);
        _playerRotator.SetInputter(_playerInput);
        _playerAnimator.SetInputter(_playerInput);
        _cameraController = FindObjectOfType<CameraController>();
        _cameraController.SetInputter(_playerInput);
        _cameraController.SetTarget(cameraTarget);
        _cameraController.SetStartTransform(cameraStartPosition);
    }

    private void _CheckInputter()
    {

        if (CurrentInputter != NewInputter)
        {
            switch (NewInputter)
            {
                case Controller.KeyboardMouse:
                    _playerInput = new KeyBoardInput();
                    break;
                case Controller.GamePad:
                    _playerInput = new PadInput();
                    break;
            }

            _playerMover.SetInputter(_playerInput);
            _playerRotator.SetInputter(_playerInput);
            _playerAnimator.SetInputter(_playerInput);
            _cameraController.SetInputter(_playerInput);
            CurrentInputter = NewInputter;
        }
    }

    private void Action()
    {
        if (_playerInput.LockOnSwitch) _cameraController.LockOnSwitch(!_cameraController.IsLock);
        if (_playerInput.Avoid && Status.SP.Value > 20) _playerAnimator.Avoid();
        if (_playerInput.Equip) _playerAnimator.Equip();
        if (_playerInput.UnEquip) _playerAnimator.UnEquip();
        if (_playerInput.Attack1) _playerAnimator.Attack1();
        if (_playerInput.Attack1) _playerAnimator.DashAttack1();
        if (_playerInput.Attack2) _playerAnimator.Attack2();
        if (_playerInput.Attack3) _playerAnimator.Attack3();
        _playerAnimator.Attack4(_playerInput.Attack4);
    }

    private void Die()
    {
        _playerAnimator.Die();
        _playerMover.enabled = false;
        _playerRotator.enabled = false;
        _playerAnimator.enabled = false;
    }
    #endregion

    #region public function
    public void UseItem(BaseItem item)
    {
        if(item.GetType() == typeof(HandGrenade))
        {
            _playerAnimator.Throw(item);
        }
        else
        {
            _playerAnimator.Drink(item);
        }
    }

    public bool TryGetDamage(int damage, Vector3 hitPosition, bool isKnock)
    {
        //回避中・ふっとび中じゃないとき
        if (!_playerAnimator.IsAvoiding && !_playerAnimator.IsInvincible)
        {
            if(Status.IsDefenseUpping) Status.HP.Damage(damage / 2);
            else Status.HP.Damage(damage);

            Status.EP.Heal(damage / 3);
            Instantiate(damageEffect, transform.position + Vector3.up, Quaternion.identity);
            if (isKnock)
            {
                _playerRotator.LookAt(hitPosition);
                _playerAnimator.KnockDown();
            }
            _cameraController.ShortShake();
            return true;
        }
        else return false;
    }
    #endregion
}

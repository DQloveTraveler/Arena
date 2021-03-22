using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StatusManagement;
using Effect;
using Item;
using UnityEngine.AI;

public class PlayerAnimator : MonoBehaviour, IPlayerSubClass
{
    #region define
    [Serializable]
    private class Effects
    {
        public GameObject NormalSlash;
        public GameObject SuperSlash;
    }

    private enum AudioEnum { FootStep, Avoid, Swing_A, Equip, Slide, Swing_B, Kick, Drink }
    [System.Serializable]
    private class AudioPlayer
    {
        [SerializeField] private AudioSet<AudioEnum>[] audios;
        private Dictionary<AudioEnum, AudioSource> audioDic = new Dictionary<AudioEnum, AudioSource>();
        public Dictionary<AudioEnum, AudioSource> audioSources => audioDic;

        public void Init()
        {
            foreach (var a in audios)
            {
                audioDic.Add(a.Name, a.Source);
            }
        }

        public void Play(AudioEnum name)
        {
            var AS = audioDic[name];
            AS.PlayOneShot(AS.clip);
        }
    }
    #endregion

    #region property
    public bool IsMoving { get => _animator.GetCurrentAnimatorStateInfo(0).IsTag("Moving"); }
    public bool IsAvoiding { get => _animator.GetCurrentAnimatorStateInfo(0).IsName("Avoid"); }
    public bool IsInvincible { get; private set; } = false;
    public bool IsEquiping => currentState == State.Equiping;
    #endregion

    #region serialize field
    [SerializeField]
    private GameObject unEquipingWeapon;
    [SerializeField]
    private GameObject equipingWeapon_Right;
    [SerializeField]
    private Transform rightWeaponParent_DashAttack;
    [SerializeField]
    private Transform rightWeaponParent_SuperSkill;
    [SerializeField]
    private Transform rightWeaponParent_Attack2Combo;
    [SerializeField]
    private Transform leftWeaponParent;
    [SerializeField]
    private Transform throwPoint;
    [SerializeField]
    private PhysicsAttackable[] attackables;
    [SerializeField]
    private Transform[] effectGeneratePoint;
    [SerializeField]
    private Effects effects = new Effects();
    [SerializeField]
    private AudioPlayer audioPlayer;
    #endregion

    #region private field
    private Transform rightWeaponParent_Start;

    private Animator _animator;
    private NavMeshAgent _navMeshAgent;
    private ChargingEffect _chargingEffect;
    private IPlayerInput _playerInput;
    private Status _status;
    private BaseItem _nextUseItem;
    private enum State
    {
        Equiping, UnEquiping, Animating
    }
    private State currentState = State.UnEquiping;
    #endregion



    #region unity function
    void Awake()
    {
        _animator = GetComponent<Animator>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _chargingEffect = GetComponentInChildren<ChargingEffect>();
        _status = GetComponent<Status>();
        audioPlayer.Init();
        rightWeaponParent_Start = equipingWeapon_Right.transform.parent;
    }

    void Update()
    {
        if (GameManager.Instance.IsStageClear) _animator.SetFloat("Run", 0);
        else RunAnimControl();
        _animator.SetBool("IsEquiping", currentState == State.Equiping);
    }

    void LateUpdate()
    {
        _animator.SetBool("Equip", false);
        _animator.SetBool("UnEquip", false);
        _animator.SetBool("Avoid", false);
        _animator.SetBool("Drink", false);
        _animator.SetBool("Throw", false);
        _animator.SetBool("Attack1", false);
        _animator.SetBool("Attack2", false);
        _animator.SetBool("Attack3", false);
        _animator.SetBool("DashAttack1", false);
    }
    #endregion



    #region private function
    private void RunAnimControl()
    {
        if (Time.deltaTime > 0.01f)
        {
            float runSpeed = 1;
            if (_status.SP.Value < 20) runSpeed = 0.5f;
            _animator.SetFloat("RunSpeed", runSpeed);
            _animator.SetFloat("Run", _playerInput.MoveDirection.magnitude);
        }
    }

    private void _NavMeshON()
    {
        _navMeshAgent.Warp(transform.position);
        _navMeshAgent.isStopped = true;
        _navMeshAgent.updatePosition = true;
        _navMeshAgent.updateRotation = true;
    }
    #endregion

    #region public function
    public void SetInputter(IPlayerInput inputter)
    {
        _playerInput = inputter;
    }

    public void Equip()
    {
        if (currentState == State.UnEquiping)
        {
            _animator.SetBool("Equip", true);
        }
    }

    public void UnEquip()
    {
        if (currentState == State.Equiping)
        {
            _animator.SetBool("UnEquip", true);
        }
    }

    public void Avoid()
    {
        _animator.SetBool("Avoid", true);
    }

    public void Drink(BaseItem item)
    {
        _animator.SetBool("Drink", true);
        _nextUseItem = item;
    }

    public void Throw(BaseItem item)
    {
        _animator.SetBool("Throw", true);
        _nextUseItem = item;
    }

    public void Attack1()
    {
        _animator.SetBool("Attack1", true);
        if (_animator.GetCurrentAnimatorStateInfo(0).IsTag("BaseAttack"))
        {
            _animator.SetTrigger("Combo1");
        }
        else if (_animator.GetCurrentAnimatorStateInfo(0).IsTag("Combo1"))
        {
            _animator.SetTrigger("Combo2");
        }
    }

    public void Attack2()
    {
        _animator.SetBool("Attack2", true);
        if (_animator.GetCurrentAnimatorStateInfo(0).IsTag("BaseAttack"))
        {
            _animator.SetTrigger("Combo1");
        }
    }

    public void Attack3()
    {
        _animator.SetBool("Attack3", true);
        if (_animator.GetCurrentAnimatorStateInfo(0).IsTag("BaseAttack"))
        {
            _animator.SetTrigger("Combo1");
        }
        if (_animator.GetCurrentAnimatorStateInfo(0).IsTag("Combo1"))
        {
            _animator.SetTrigger("Combo2");
        }
    }

    public void Attack4(bool isButtonDown)
    {
        if (isButtonDown)
        {
            if(_status.EP.Value == _status.EP.Max)
                _animator.SetBool("Attack4", true);
        }
        else
        {
            _animator.SetBool("Attack4", false);
        }

    }

    public void DashAttack1()
    {
        _animator.SetBool("DashAttack1", true);
    }

    public void KnockDown()
    {
        _animator.SetTrigger("KnockDown");
        IsInvincible = true;
    }

    public void Die()
    {
        _animator.SetBool("IsDead", true);
    }
    #endregion



    //以下、アニメーションイベント用メソッド
    #region animation event function
    public void StateChange(string setState)
    {
        switch (setState)
        {
            case "Equiping":
                currentState = State.Equiping;
                break;
            case "UnEquiping":
                currentState = State.UnEquiping;
                break;
            case "Animating":
                currentState = State.Animating;
                break;
        }
    }
    public void WeaponSwitch(int pattern)
    {
        audioPlayer.Play(AudioEnum.Equip);
        switch (pattern)
        {
            case 1:
                equipingWeapon_Right.SetActive(true);
                unEquipingWeapon.SetActive(false);
                break;
            case 0:
                equipingWeapon_Right.SetActive(false);
                unEquipingWeapon.SetActive(true);
                break;
        }
    }
    public void WeaponParentChange(int pattern)
    {
        switch (pattern) 
        {
            case -1:
                equipingWeapon_Right.transform.parent = leftWeaponParent;
                equipingWeapon_Right.transform.localPosition = Vector3.zero;
                equipingWeapon_Right.transform.localEulerAngles = Vector3.zero;
                break;
            case 0:
                equipingWeapon_Right.transform.parent = rightWeaponParent_Start;
                equipingWeapon_Right.transform.localPosition = Vector3.zero;
                equipingWeapon_Right.transform.localEulerAngles = Vector3.zero;
                break;
            case 1:
                equipingWeapon_Right.transform.parent = rightWeaponParent_DashAttack;
                equipingWeapon_Right.transform.localPosition = Vector3.zero;
                equipingWeapon_Right.transform.localEulerAngles = Vector3.zero;
                break;
            case 2:
                equipingWeapon_Right.transform.parent = rightWeaponParent_SuperSkill;
                equipingWeapon_Right.transform.localPosition = Vector3.zero;
                equipingWeapon_Right.transform.localEulerAngles = Vector3.zero;
                break;
            case 3:
                equipingWeapon_Right.transform.parent = rightWeaponParent_Attack2Combo;
                equipingWeapon_Right.transform.localPosition = Vector3.zero;
                equipingWeapon_Right.transform.localEulerAngles = Vector3.zero;
                break;
        }
    }
    public void SlideForward(float power)
    {
        _NavMeshON();
        _navMeshAgent.velocity = transform.forward * power;
    }
    public void Decelerate()
    {
        StartCoroutine(_Decelerate(0.8f, 20, 0));
    }
    private IEnumerator _Decelerate(float dampen, int durationFrame, float delay)
    {
        yield return new WaitForSeconds(delay);
        for(int i = 0; i < durationFrame; i++)
        {
            _navMeshAgent.velocity = Vector3.Scale(_navMeshAgent.velocity, new Vector3(dampen, 1, dampen));
            float horizontalMag = new Vector2(_navMeshAgent.velocity.x, _navMeshAgent.velocity.z).sqrMagnitude;
            if (horizontalMag < 0.1f)
            {
                _navMeshAgent.velocity = Vector3.zero;
                _NavMeshON();
                yield break;
            }
            yield return null;
        }
    }
    public void Knock()
    {
        _chargingEffect.gameObject.SetActive(false);
        _NavMeshON();
        var forceVector = transform.forward * -9 + Vector3.up * 4f;
        _navMeshAgent.velocity = Vector3.zero;
        _navMeshAgent.velocity = forceVector;
        StartCoroutine(_Decelerate(0.97f, 60, 0.4f));
    }
    public void SlideGround()
    {
        audioPlayer.Play(AudioEnum.Slide);
    }
    public void Roll()
    {
        audioPlayer.Play(AudioEnum.Avoid);
        _status.SP.Use(20);
    }
    public void Kick()
    {
        audioPlayer.Play(AudioEnum.Kick);
    }
    public void SwingWeapon()
    {
        audioPlayer.Play(AudioEnum.Swing_A);
    }
    public void SwingWeapon_Dash()
    {
        audioPlayer.Play(AudioEnum.Swing_B);
    }
    public void DrinkSomething(int num) 
    {
        switch (num)
        {
            case 0:
                audioPlayer.Play(AudioEnum.Drink);
                break;
            case 1:
                _nextUseItem.Use(_status);
                ItemSlotCtrl.HavingNumList[_nextUseItem]--;
                Instantiate(_nextUseItem.GetEffect(), transform.position, Quaternion.identity);
                break;
        }
    }
    public void ThrowSomething()
    {
        ItemSlotCtrl.HavingNumList[_nextUseItem]--;
        Instantiate(_nextUseItem.GetEffect(), throwPoint.position,throwPoint.rotation);
    }
    public void FootL()
    {
        audioPlayer.Play(AudioEnum.FootStep);
    }
    public void FootR()
    {
        audioPlayer.Play(AudioEnum.FootStep);
    }
    public void ChargeStart()
    {
        _chargingEffect.gameObject.SetActive(true);
        _chargingEffect.LevelUp();
    }
    public void ChargeLevelUp()
    {
        _chargingEffect.LevelUp();
    }
    public void NormalSlash()
    {
        //Instantiate(effects.NormalSlash, effectGeneratePoint[0].position, effectGeneratePoint[0].rotation);
    }
    public void SuperSlash()
    {
        _status.EP.Use(_status.EP.Max);
        _chargingEffect.gameObject.SetActive(false);
        var instance = Instantiate(effects.SuperSlash, effectGeneratePoint[1].position, effectGeneratePoint[1].rotation);
        instance.GetComponent<SuperSlash>().PowerLevel = _chargingEffect.Level;
        
    }
    public void EndInvincible() 
    {
        IsInvincible = false;
    }
    public void ResetWeaponList(int num)
    {
        attackables[num].ResetAttackedList();
    }
    public void WeaponColliderOFF(int num)
    {
        attackables[num].ColliderOFF();
    }
    #endregion

}


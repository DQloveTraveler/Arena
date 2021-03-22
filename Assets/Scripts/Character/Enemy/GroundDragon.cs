using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundDragon : Enemy, IDamageable
{
    #region define
    private const int useSP_Attack1 = 15;
    private const int useSP_Attack2 = 20;
    private const int useSP_Attack3 = 30;
    private const int useSP_Attack4 = 50;
    private const int useSP_JumpAttack = 20;
    private const int breakingTime = 5;
    private const float stoppingDistance_Attack1 = 4.1f;
    private const float stoppingDistance_Attack2 = 4.1f;
    private const float stoppingDistance_Attack3 = 8;
    private const float stoppingDistance_Attack4 = 10;

    private enum AudioEnum { FootStep, Bite, Swing, Slide, Rush, Roar}
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

    #region serialize field
    [SerializeField]
    private PlayerChecker playerCheckerFront;
    [SerializeField]
    private GameObject damageEffect;
    [SerializeField]
    private AudioPlayer audioPlayer = null;
    #endregion

    #region private field
    private float HealSPCounter = 0;
    //private TaskList<TaskEnum> _taskList = new TaskList<TaskEnum>();
    #endregion

    #region property
    public Transform Transform { get => transform; }
    #endregion
    


    #region unity function
    void Start()
    {
        Init();
        audioPlayer.Init();
        StartCoroutine(_Loop());
    }

    protected override void Update()
    {
        base.Update();
        if (Status.HP.Value <= 0)
        {
            _Die();
            currentLargeState = LargeState.Dead;
        }
    }

    #endregion



    #region public function
    public bool TryGetDamage(int damage, Vector3 hitPosition, bool isKnock)
    {
        Status.HP.Damage(damage);
        Instantiate(damageEffect, hitPosition, Quaternion.identity);
        return true;
    }
    #endregion

    #region private enumerator
    private IEnumerator _Loop()
    {
        for(int loopCount = 0; currentLargeState != LargeState.Dead; loopCount++)
        {
            while (currentMiddleState == MiddleState.Animating) yield return null;

            yield return SPJudge();

            yield return _SelectActionRandom(loopCount);

            for (int j = 0; j < 10; j++) yield return null;
        }
    }

    private IEnumerator _SelectActionRandom(int currentloopCount)
    {
        int random = Random.Range(1, 100);
        if (random <= 30)
        {
            yield return _Attack1State();
        }
        else if (30 < random && random <= 55)
        {
            yield return _Attack2State();
        }
        else if (55 < random && random <= 80)
        {
            yield return _Attack3State();
        }
        else if (80 < random && random <= 95)
        {
            yield return _Attack4State();
        }
        else if (95 < random && random <= 100 && currentloopCount > 4)
        {
            _Break();
        }
    }

    private IEnumerator _Attack1State() //かみつき
    {
        currentMiddleState = MiddleState.Attack1;
        navMeshAgent.stoppingDistance = stoppingDistance_Attack1;
        while (true)
        {
            switch (currentSmallState)
            {
                case SmallState.Default:
                    currentSmallState = SmallState.Leave;
                    break;
                case SmallState.Leave:
                    yield return _Leave();
                    break;
                case SmallState.Chase:
                    yield return _Chase();
                    break;
                case SmallState.Search:
                    if (!playerCheckerFront.IsDetecting)
                    {
                        _JumpAttack();
                        yield break;
                    }
                    currentSmallState = SmallState.Attack;
                    break;
                case SmallState.Attack:
                    navMeshAgent.isStopped = true;
                    enemyAnimator.Attack1();
                    Status.SP.Use(useSP_Attack1);
                    currentMiddleState = MiddleState.Animating;
                    yield break;
            }
            yield return null;
        }
    }

    private IEnumerator _Attack2State() //ひっかき
    {
        currentMiddleState = MiddleState.Attack2;
        navMeshAgent.stoppingDistance = stoppingDistance_Attack2;
        while (true)
        {
            switch (currentSmallState)
            {
                case SmallState.Default:
                    currentSmallState = SmallState.Leave;
                    break;
                case SmallState.Leave:
                    yield return _Leave();
                    break;
                case SmallState.Chase:
                    yield return _Chase();
                    break;
                case SmallState.Search:
                    StartCoroutine(enemyRotator.LookPlayer(2, 1.5f));
                    currentSmallState = SmallState.Attack;
                    break;
                case SmallState.Attack:
                    navMeshAgent.isStopped = true;
                    enemyAnimator.Attack2();
                    Status.SP.Use(useSP_Attack2);
                    currentMiddleState = MiddleState.Animating;
                    yield break;
            }
            yield return null;
        }
    }

    private IEnumerator _Attack3State() //突進攻撃
    {
        currentMiddleState = MiddleState.Attack3;
        navMeshAgent.stoppingDistance = stoppingDistance_Attack3;
        while (true)
        {
            switch (currentSmallState)
            {
                case SmallState.Default:
                    currentSmallState = SmallState.Leave;
                    break;
                case SmallState.Leave:
                    yield return _Leave();
                    break;
                case SmallState.Chase:
                    yield return _Chase();
                    break;
                case SmallState.Search:
                    yield return enemyRotator.LookPlayer(1.5f, 1.5f);
                    if (DistanceToPlayer > Mathf.Pow(navMeshAgent.stoppingDistance, 2))
                    {
                        StateReset();
                        yield break;
                    }
                    if (Mathf.Abs(enemyRotator.DeltaAngle) > 5)
                    {
                        _JumpAttack();
                        yield break;
                    }
                    currentSmallState = SmallState.Attack;
                    break;
                case SmallState.Attack:
                    navMeshAgent.isStopped = true;
                    enemyAnimator.Attack3();
                    Status.SP.Use(useSP_Attack3);
                    currentMiddleState = MiddleState.Animating;
                    yield break;
            }
            yield return null;
        }
    }

    private IEnumerator _Attack4State() //ブレス攻撃
    {
        currentMiddleState = MiddleState.Attack4;
        navMeshAgent.stoppingDistance = stoppingDistance_Attack4;
        while (true)
        {
            switch (currentSmallState)
            {
                case SmallState.Default:
                    currentSmallState = SmallState.Leave;
                    break;
                case SmallState.Leave:
                    yield return _Leave();
                    break;
                case SmallState.Chase:
                    yield return _Chase();
                    break;
                case SmallState.Search:
                    yield return enemyRotator.LookPlayer(1.5f, 1.5f);
                    if (DistanceToPlayer > Mathf.Pow(navMeshAgent.stoppingDistance, 2))
                    {
                        StateReset();
                        yield break;
                    }
                    if (Mathf.Abs(enemyRotator.DeltaAngle) > 10)
                    {
                        _JumpAttack();
                        yield break;
                    }
                    currentSmallState = SmallState.Attack;
                    break;
                case SmallState.Attack:
                    navMeshAgent.isStopped = true;
                    enemyAnimator.Attack4();
                    Status.SP.Use(useSP_Attack4);
                    currentMiddleState = MiddleState.Animating;
                    yield break;
            }
            yield return null;
        }
    }

    private IEnumerator SPJudge()
    {
        while (Status.SP.Value <= 1)
        {
            HealSPCounter += Time.deltaTime;
            if (HealSPCounter > breakingTime) Status.SP.Reset();
            yield return null;
        }
        HealSPCounter = 0;
    }

    private IEnumerator _Leave()
    {
        yield return null;
        if (DistanceToPlayer < Mathf.Pow(navMeshAgent.stoppingDistance, 2))
        {
            StartCoroutine(enemyRotator.LookPlayer(4, 2));
            yield return enemyMover.MoveBack(2, 1);
        }
        currentSmallState = SmallState.Chase;
    }

    private IEnumerator _Chase()
    {
        yield return null;
        while (!(enemyMover.NavMeshVelocity <= 0 &&
            DistanceToPlayer <= Mathf.Pow(navMeshAgent.stoppingDistance, 2)))
        {
            navMeshAgent.isStopped = false;
            enemyMover.ChasePlayer();
            yield return null;
        }
        currentSmallState = SmallState.Search;
    }

    #endregion

    #region private function
    private void _JumpAttack() //ジャンプ攻撃
    {
        if(DistanceToPlayer < 4 * 4)
        {
            currentMiddleState = MiddleState.Animating;
            navMeshAgent.isStopped = true;
            enemyAnimator.Attack5();
            Status.SP.Use(useSP_JumpAttack);
        }
    }
    private void _Break()
    {
        navMeshAgent.isStopped = true;
        enemyAnimator.Break();
        Status.SP.Reset();
        currentMiddleState = MiddleState.Animating;
    }
    private void _Die()
    {
        StopAllCoroutines();
        enemyAnimator.Die();
        navMeshAgent.isStopped = true;
        navMeshAgent.updatePosition = false;
        navMeshAgent.updateRotation = false;
    }
    #endregion


    #region task function

    private void OnTask_Leave_Enter()
    {
        if (DistanceToPlayer < Mathf.Pow(navMeshAgent.stoppingDistance, 2))
        {
            StartCoroutine(enemyRotator.LookPlayer(4, 2));
            StartCoroutine(enemyMover.MoveBack(2, 1));
        }
    }

    private bool OnTask_Leave_Update()
    {
        return false;
    }

    private void OnTask_Leave_Exit()
    {

    }


    private void OnTask_Chase_Enter()
    {

    }

    private bool OnTask_Chase_Update()
    {
        return true;
    }

    private void OnTask_Chase_Exit()
    {

    }


    private void OnTask_Search_Enter()
    {

    }

    private bool OnTask_Search_Update()
    {
        return true;
    }

    private void OnTask_Search_Exit()
    {

    }


    private void OnTask_Attack_Enter()
    {

    }

    private bool OnTask_Attack_Update()
    {
        return true;
    }

    private void OnTask_Attack_Exit()
    {

    }

    #endregion



    //以下、アニメーションイベント用メソッド
    #region animation event function
    public void Foot() => audioPlayer.Play(AudioEnum.FootStep);
    public void SlideForward(float power) => enemyAnimator.SlideForward(power);
    public void Decelerate() => enemyAnimator.Decelerate();
    public void GenerateEffect(int num) => enemyAnimator.GenerateEffect(num);
    public void ResetAttackedList(int num) => enemyAnimator.ResetAttackedList(num);
    public void AttackableColliderOFF(int num) => enemyAnimator.AttackableColliderOFF(num);
    public void StateReset()
    {
        currentMiddleState = MiddleState.Default;
        currentSmallState = SmallState.Default;
        navMeshAgent.Warp(transform.position);
    }
    public void Sound_Bite()
    {
        audioPlayer.Play(AudioEnum.Bite);
    }
    public void Sound_HornRush()
    {
        audioPlayer.Play(AudioEnum.Slide);
        audioPlayer.Play(AudioEnum.Rush);
    }
    public void Sound_SwingArm()
    {
        audioPlayer.Play(AudioEnum.Swing);
    }
    public void Bark()
    {
        audioPlayer.audioSources[AudioEnum.Roar].pitch = Random.Range(0.6f, 0.9f);
        audioPlayer.Play(AudioEnum.Roar);
    }
    #endregion

}

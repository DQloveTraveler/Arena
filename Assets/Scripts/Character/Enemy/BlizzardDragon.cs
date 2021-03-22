using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlizzardDragon : Enemy, IDamageable
{
    #region define
    private const int useSP_Attack1 = 15;
    private const int useSP_Attack2 = 20;
    private const int useSP_Attack3 = 30;
    private const int useSP_Attack4 = 50;
    private const int useSP_Attack5 = 40;
    private const int useSP_Attack6 = 30;
    private const int useSP_Attack7 = 50;
    private const int breakingTime = 5;
    private const float stoppingDistance_Attack1 = 4.5f;
    private const float stoppingDistance_Attack2 = 7;
    private const float stoppingDistance_Attack3 = 15;
    private const float stoppingDistance_Attack4 = 14;
    private const float stoppingDistance_Attack5 = 12;
    private const float stoppingDistance_Attack6 = 6.5f;
    private const float stoppingDistance_Attack7 = 6;


    [System.Serializable]
    private class Effects
    {
        public GameObject damage;
        public GameObject iceSpike;
    }

    private enum AudioEnum { FootStep, Bite, Swing, Slide, Impact, Bark_A, Bark_B, Wing }
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
    private Effects effects;
    [SerializeField]
    private AudioPlayer audioPlayer;
    #endregion

    #region property
    public Transform Transform => transform;
    #endregion

    #region private field
    private int _loopCounter = 0;
    private float HealSPCounter = 0;
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
        Instantiate(effects.damage, hitPosition, Quaternion.identity);
        return true;
    }
    #endregion

    #region private enumerator
    private IEnumerator _Loop()
    {
        while (currentLargeState != LargeState.Dead)
        {
            while (currentMiddleState == MiddleState.Animating)
            {
                yield return null;
            }

            yield return _SPJudge();

            switch (currentLargeState)
            {
                case LargeState.Default:
                    yield return _OnGroundState();
                    break;
                case LargeState.Flying:
                    yield return _FlyingState();
                    break;
            }

            yield return _StateInterval(0.3f);

            _loopCounter++;
        }
    }

    private IEnumerator _OnGroundState()
    {
        int random = Random.Range(1, 100);
        if (random <= 20)
        {
            navMeshAgent.stoppingDistance = stoppingDistance_Attack1;
            yield return _Attack1State();
        }
        else if (20 < random && random <= 40)
        {
            navMeshAgent.stoppingDistance = stoppingDistance_Attack2;
            yield return _Attack2State();
        }
        else if (40 < random && random <= 55)
        {
            navMeshAgent.stoppingDistance = stoppingDistance_Attack3;
            yield return _Attack3State();
        }
        else if (55 < random && random <= 70)
        {
            navMeshAgent.stoppingDistance = stoppingDistance_Attack4;
            yield return _Attack4State();
        }
        else if (70 < random && Status.HP.Ratio < 0.5f)
        {
            _TakeOff();
        }
    }

    private IEnumerator _FlyingState()
    {
        int random = Random.Range(1, 100);
        if (random <= 25)
        {
            navMeshAgent.stoppingDistance = stoppingDistance_Attack5;
            yield return _Attack5State();
        }
        else if (25 < random && random <= 50)
        {
            navMeshAgent.stoppingDistance = stoppingDistance_Attack6;
            yield return _Attack6State();
        }
        else if (50 < random && random <= 90)
        {
            navMeshAgent.stoppingDistance = stoppingDistance_Attack7;
            yield return _Attack7State();
        }
        else if (90 < random && random <= 100 && _loopCounter > 20)
        {
            _Land();
        }
    }

    private IEnumerator _StateInterval(float interval)
    {
        yield return new WaitForSeconds(interval);
    }

    private IEnumerator _Attack1State() //かみつき
    {
        currentMiddleState = MiddleState.Attack1;
        currentSmallState = SmallState.Default;
        while (true)
        {
            switch (currentSmallState)
            {
                case SmallState.Default:
                    currentSmallState = SmallState.Leave;
                    break;
                case SmallState.Leave:
                    StartCoroutine(enemyRotator.LookPlayer(1.5f, 1));
                    yield return enemyMover.MoveBack(2, 1);
                    currentSmallState = SmallState.Chase;
                    break;
                case SmallState.Chase:
                    while (!(enemyMover.NavMeshVelocity <= 0 &&
                        DistanceToPlayer <= Mathf.Pow(navMeshAgent.stoppingDistance, 2)))
                    {
                        enemyMover.ChasePlayer();
                        yield return null;
                    }
                    currentSmallState = SmallState.Search;
                    break;
                case SmallState.Search:
                    StartCoroutine(enemyRotator.LookPlayer(2, 1));
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
        currentSmallState = SmallState.Default;
        while (true)
        {
            switch (currentSmallState)
            {
                case SmallState.Default:
                    currentSmallState = SmallState.Leave;
                    break;
                case SmallState.Leave:
                    if (DistanceToPlayer < Mathf.Pow(navMeshAgent.stoppingDistance, 2))
                    {
                        StartCoroutine(enemyRotator.LookPlayer(1.5f, 1));
                        yield return enemyMover.MoveBack(2, 1);
                    }
                    currentSmallState = SmallState.Chase;
                    break;
                case SmallState.Chase:
                    while (!(enemyMover.NavMeshVelocity <= 0 &&
                        DistanceToPlayer <= Mathf.Pow(navMeshAgent.stoppingDistance, 2)))
                    {
                        enemyMover.ChasePlayer();
                        yield return null;
                    }
                    currentSmallState = SmallState.Search;
                    break;
                case SmallState.Search:
                    StartCoroutine(enemyRotator.LookPlayer(2, 1));
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

    private IEnumerator _Attack3State() //咆哮攻撃
    {
        currentMiddleState = MiddleState.Attack3;
        currentSmallState = SmallState.Default;
        while (true)
        {
            switch (currentSmallState)
            {
                case SmallState.Default:
                    currentSmallState = SmallState.Leave;
                    break;
                case SmallState.Leave:
                    currentSmallState = SmallState.Chase;
                    break;
                case SmallState.Chase:
                    while (!(enemyMover.NavMeshVelocity <= 0 &&
                        DistanceToPlayer <= Mathf.Pow(navMeshAgent.stoppingDistance, 2)))
                    {
                        enemyMover.ChasePlayer();
                        yield return null;
                    }
                    currentSmallState = SmallState.Search;
                    break;
                case SmallState.Search:
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
        currentSmallState = SmallState.Default;
        while (true)
        {
            switch (currentSmallState)
            {
                case SmallState.Default:
                    currentSmallState = SmallState.Leave;
                    break;
                case SmallState.Leave:
                    if (DistanceToPlayer < Mathf.Pow(navMeshAgent.stoppingDistance, 2))
                    {
                        StartCoroutine(enemyRotator.LookPlayer(1.5f, 1));
                        yield return enemyMover.MoveBack(3.5f, 1);
                    }
                    currentSmallState = SmallState.Chase;
                    break;
                case SmallState.Chase:
                    while (!(enemyMover.NavMeshVelocity <= 0 &&
                        DistanceToPlayer <= Mathf.Pow(navMeshAgent.stoppingDistance, 2)))
                    {
                        enemyMover.ChasePlayer();
                        yield return null;
                    }
                    currentSmallState = SmallState.Search;
                    break;
                case SmallState.Search:
                    yield return enemyRotator.LookPlayer(1.5f, 1.5f);
                    if (DistanceToPlayer > Mathf.Pow(navMeshAgent.stoppingDistance, 2))
                    {
                        StateReset();
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

    private IEnumerator _Attack5State() //空中ブレス攻撃
    {
        currentMiddleState = MiddleState.Attack5;
        currentSmallState = SmallState.Default;
        while (true)
        {
            switch (currentSmallState)
            {
                case SmallState.Default:
                    currentSmallState = SmallState.Leave;
                    break;
                case SmallState.Leave:
                    if (DistanceToPlayer < Mathf.Pow(navMeshAgent.stoppingDistance, 2))
                    {
                        enemyRotator.LookPlayer(1.5f, 1);
                        yield return enemyMover.MoveBack(6, 1);
                    }
                    currentSmallState = SmallState.Chase;
                    break;
                case SmallState.Chase:
                    while (!(enemyMover.NavMeshVelocity <= 0 &&
                        DistanceToPlayer <= Mathf.Pow(navMeshAgent.stoppingDistance, 2)))
                    {
                        enemyMover.ChasePlayer();
                        yield return null;
                    }
                    currentSmallState = SmallState.Search;
                    break;
                case SmallState.Search:
                    StartCoroutine(enemyRotator.LookPlayer(2, 2));
                    currentSmallState = SmallState.Attack;
                    break;
                case SmallState.Attack:
                    navMeshAgent.isStopped = true;
                    enemyAnimator.Attack5();
                    Status.SP.Use(useSP_Attack5);
                    currentMiddleState = MiddleState.Animating;
                    yield break;
            }
            yield return null;
        }
    }

    private IEnumerator _Attack6State() //空中咆哮攻撃
    {
        currentMiddleState = MiddleState.Attack6;
        currentSmallState = SmallState.Default;
        while (true)
        {
            switch (currentSmallState)
            {
                case SmallState.Default:
                    currentSmallState = SmallState.Leave;
                    break;
                case SmallState.Leave:
                    currentSmallState = SmallState.Chase;
                    break;
                case SmallState.Chase:
                    while (!(enemyMover.NavMeshVelocity <= 0 &&
                        DistanceToPlayer <= Mathf.Pow(navMeshAgent.stoppingDistance, 2)))
                    {
                        enemyMover.ChasePlayer();
                        yield return null;
                    }
                    currentSmallState = SmallState.Search;
                    break;
                case SmallState.Search:
                    currentSmallState = SmallState.Attack;
                    break;
                case SmallState.Attack:
                    navMeshAgent.isStopped = true;
                    enemyAnimator.Attack6();
                    Status.SP.Use(useSP_Attack6);
                    currentMiddleState = MiddleState.Animating;
                    yield break;
            }
            yield return null;
        }
    }

    private IEnumerator _Attack7State() //滑空攻撃
    {
        currentMiddleState = MiddleState.Attack7;
        currentSmallState = SmallState.Default;
        while (true)
        {
            switch (currentSmallState)
            {
                case SmallState.Default:
                    currentSmallState = SmallState.Leave;
                    break;
                case SmallState.Leave:
                    if (DistanceToPlayer < Mathf.Pow(navMeshAgent.stoppingDistance, 2))
                    {
                        StartCoroutine(enemyRotator.LookPlayer(1.5f, 1));
                        yield return enemyMover.MoveBack(3, 1);
                    }
                    currentSmallState = SmallState.Chase;
                    break;
                case SmallState.Chase:
                    while (!(enemyMover.NavMeshVelocity <= 0 &&
                        DistanceToPlayer <= Mathf.Pow(navMeshAgent.stoppingDistance, 2)))
                    {
                        enemyMover.ChasePlayer();
                        yield return null;
                    }
                    currentSmallState = SmallState.Search;
                    break;
                case SmallState.Search:
                    yield return enemyRotator.LookPlayer(3, 1);
                    StartCoroutine(enemyRotator.LookPlayer(4, 0.3f));
                    currentSmallState = SmallState.Attack;
                    break;
                case SmallState.Attack:
                    enemyAnimator.Attack7();
                    Status.SP.Use(useSP_Attack7);
                    currentMiddleState = MiddleState.Animating;
                    yield break;
            }
            yield return null;
        }
    }

    private IEnumerator _SPJudge()
    {
        while (Status.SP.Value <= 1)
        {
            HealSPCounter += Time.deltaTime;
            if (HealSPCounter > breakingTime) Status.SP.Reset();
            yield return null;
        }
        HealSPCounter = 0;
    }
    #endregion

    #region private function
    private void _Land()
    {
        currentLargeState = LargeState.Default;
        currentMiddleState = MiddleState.Default;
        navMeshAgent.isStopped = true;
        enemyAnimator.Land();
    }

    private void _TakeOff()
    {
        currentMiddleState = MiddleState.Animating;
        currentLargeState = LargeState.Flying;
        navMeshAgent.isStopped = true;
        _loopCounter = 0;
        enemyAnimator.TakeOff();
    }

    private void _Die()
    {
        enemyAnimator.Die();
        navMeshAgent.isStopped = true;
        navMeshAgent.updatePosition = false;
        navMeshAgent.updateRotation = false;
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
    public void SetLargeState(string stateName)
    {
        switch (stateName)
        {
            case "Default":
                currentLargeState = LargeState.Default;
                currentMiddleState = MiddleState.Default;
                break;
            case "Flying":
                currentLargeState = LargeState.Flying;
                currentMiddleState = MiddleState.Default;
                break;
        }
    }
    public void Sound_Bite()
    {
        audioPlayer.Play(AudioEnum.Bite);
    }
    public void Sound_SwingArm()
    {
        audioPlayer.Play(AudioEnum.Swing);
    }
    public void Sound_Wing()
    {
        audioPlayer.Play(AudioEnum.Wing);
    }
    public void Bark(int pattern)
    {
        switch (pattern) 
        {
            case 0:
                audioPlayer.audioSources[AudioEnum.Bark_A].pitch = Random.Range(0.6f, 0.9f);
                audioPlayer.Play(AudioEnum.Bark_A);
                break;
            case 1:
                audioPlayer.audioSources[AudioEnum.Bark_B].pitch = Random.Range(1f, 1.2f);
                audioPlayer.Play(AudioEnum.Bark_B);
                break;
        }
    }
    public void Scream()
    {
        var ray = new Ray(transform.position, Vector3.up);
        var hits = Physics.SphereCastAll(ray, 20, 0.01f, LayerMask.GetMask("Player"));
        if (hits.Length > 0)
        {
            ray = new Ray(hits[0].transform.root.position + Vector3.up, Vector3.down);
            if (Physics.Raycast(ray, out var groundHit, 100, LayerMask.GetMask("Ground")))
            {
                Instantiate(effects.iceSpike, groundHit.point, Quaternion.identity);
            }
        }
    }
    #endregion

}

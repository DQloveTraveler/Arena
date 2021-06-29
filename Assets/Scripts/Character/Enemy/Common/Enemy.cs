using StatusManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Status))]
public abstract class Enemy : MonoBehaviour
{
    #region define
    protected enum LargeState { Default, Flying, Dead }
    protected enum MiddleState { Default, Attack1, Attack2, Attack3, Attack4, Attack5, Attack6, Attack7, Break, Animating }
    protected enum SmallState { Default, Leave, Chase, Search, Attack }

    [System.Serializable]
    protected class AudioPlayer<T>
    {
        [SerializeField] private AudioSet<T>[] audios;
        private Dictionary<T, AudioSource> audioDic = new Dictionary<T, AudioSource>();
        public Dictionary<T, AudioSource> audioSources => audioDic;

        public void Init()
        {
            foreach (var a in audios)
            {
                audioDic.Add(a.Name, a.Source);
            }
        }

        public void Play(T name)
        {
            var AS = audioDic[name];
            AS.PlayOneShot(AS.clip);
        }
    }

    #endregion

    #region serialize field
    [SerializeField]
    private EffectGenerator[] effectGenerators = null;
    [SerializeField]
    private PhysicsAttackable[] attackables;
    #endregion

    #region property
    public Status Status { get; private set; }
    protected float DistanceToPlayer 
    { get => (player.transform.position - transform.position).sqrMagnitude; }
    #endregion

    #region protected field
    protected NavMeshAgent navMeshAgent;
    protected PlayerBehaviour player;
    protected EnemyMover enemyMover;
    protected EnemyRotator enemyRotator;
    protected EnemyAnimator enemyAnimator;
    protected LargeState currentLargeState = LargeState.Default;
    protected MiddleState currentMiddleState = MiddleState.Default;
    protected SmallState currentSmallState = SmallState.Default;
    #endregion

    protected virtual void Init()
    {
        Status = GetComponent<Status>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        player = FindObjectOfType<PlayerBehaviour>();
        enemyMover = new EnemyMover(transform, navMeshAgent, player.transform);
        enemyRotator = new EnemyRotator(transform, navMeshAgent, player.transform);
        var animator = GetComponent<Animator>();
        enemyAnimator = new EnemyAnimator(animator, enemyMover, enemyRotator, effectGenerators, attackables);
    }

    protected virtual void Update()
    {
        enemyAnimator.Update();
        enemyRotator.Update();
    }

    protected virtual void LateUpdate()
    {
        enemyRotator.LateUpdate();
    }
}

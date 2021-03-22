using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMover
{
    #region field
    private readonly Transform transform = null;
    private readonly NavMeshAgent navMeshAgent = null;
    private readonly Transform playerTransform = null;
    #endregion

    #region property
    public bool IsBackking { get; private set; } = false;
    public float NavMeshVelocity { get => navMeshAgent.velocity.sqrMagnitude; }
    #endregion

    #region constructor
    public EnemyMover(Transform transform, NavMeshAgent navMeshAgent, Transform playerTransform)
    {
        this.transform = transform;
        this.navMeshAgent = navMeshAgent;
        this.playerTransform = playerTransform;
    }
    #endregion

    #region public function
    public void ChasePlayer()
    {
        navMeshAgent.isStopped = false;
        navMeshAgent.updatePosition = true;
        navMeshAgent.updateRotation = true;
        navMeshAgent.destination = playerTransform.position;
    }

    public void AddForceForward(float power)
    {
        navMeshAgent.velocity = transform.forward * power;
        CoroutineHandler.StartStaticCoroutine(_Decelerate(0.98f, 150, 0.05f));
    }
    #endregion

    #region public enumerator
    public IEnumerator _Decelerate(float dampen, int durationFrame)
    {
        for (int i = 0; i < durationFrame; i++)
        {
            navMeshAgent.velocity = Vector3.Scale(navMeshAgent.velocity, new Vector3(dampen, 1, dampen));
            if (navMeshAgent.velocity.sqrMagnitude < 0.1f)
            {
                navMeshAgent.velocity = Vector3.zero;
                yield break;
            }
            yield return null;
        }
    }

    public IEnumerator _Decelerate(float dampen, int durationFrame, float delay)
    {
        yield return new WaitForSeconds(delay);
        for (int i = 0; i < durationFrame; i++)
        {
            navMeshAgent.velocity = Vector3.Scale(navMeshAgent.velocity, new Vector3(dampen, 1, dampen));
            if(navMeshAgent.velocity.sqrMagnitude < 0.1f)
            {
                navMeshAgent.velocity = Vector3.zero;
                yield break;
            }
            yield return null;
        }
    }

    public IEnumerator MoveBack(float speed, float duration)
    {
        IsBackking = true;
        yield return MoveForward(-speed, duration);
        IsBackking = false;
    }

    public IEnumerator MoveForward(float speed, float duration)
    {
        navMeshAgent.updatePosition = true;
        Vector3 moveVector;

        for (float i = 0; i < duration; i += Time.deltaTime)
        {
            moveVector = transform.forward * speed;
            navMeshAgent.velocity = new Vector3(moveVector.x, navMeshAgent.velocity.y, moveVector.z);
            yield return null;
        }
    }
    #endregion
}

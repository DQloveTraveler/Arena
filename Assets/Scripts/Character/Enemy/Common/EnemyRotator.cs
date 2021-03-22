using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum Axis { X, Y, Z }

public class EnemyRotator
{
    #region field
    private Transform transform = null;
    private Transform playerLooker = null;
    private Transform playerTransform = null;
    private NavMeshAgent navMeshAgent = null;
    private float prevAngle;
    private float currentAngle;
    #endregion

    #region property
    public float DeltaAngle
    {
        get => Mathf.DeltaAngle(transform.eulerAngles.y, playerLooker.eulerAngles.y);
    }
    public bool IsRotating { get => Mathf.Abs(currentAngle - prevAngle) > 0.05f; }
    #endregion

    #region constructor
    public EnemyRotator(Transform transform, NavMeshAgent navMeshAgent, Transform playerTransform)
    {
        this.transform = transform;
        this.navMeshAgent = navMeshAgent;
        this.playerTransform = playerTransform;
        playerLooker = new GameObject("PlayerLooker").transform;
        playerLooker.parent = transform;
        playerLooker.localPosition = Vector3.zero;
        prevAngle = transform.eulerAngles.y;
        currentAngle = transform.eulerAngles.y;
    }
    #endregion

    public void Update()
    {
        currentAngle = transform.eulerAngles.y;
        playerLooker.LookAt(playerTransform.position);
    }

    public void LateUpdate()
    {
        prevAngle = currentAngle;
    }

    public IEnumerator LookPlayer(float speed, float duration)
    {
        navMeshAgent.updateRotation = false;
        for(float i = 0; i < duration; i+= Time.deltaTime)
        {
            var nextAngle = Mathf.MoveTowardsAngle(transform.eulerAngles.y, playerLooker.eulerAngles.y, speed);
            transform.eulerAngles = Vector3.up * nextAngle;
            var deltaAngle = Mathf.DeltaAngle(transform.eulerAngles.y, playerLooker.eulerAngles.y);
            if (Mathf.Abs(deltaAngle) < 0.5f) yield break;
            yield return null;
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InputManagement;

public class CameraController : SingletonMonoBehaviour<CameraController>
{
    #region serialize field
    [SerializeField] private Transform enemyLooker;
    #endregion

    #region private field
    private IPlayerInput _playerInput;
    private Vector3 _targetPos;
    private float _startDistance;
    private Animator _animator;
    private RaycastHit _frontHit;
    private RaycastHit _backHit;
    private Transform _enemyTransform;
    private RadialBlurEffect _radialBlurEffect;
    private Coroutine _blurCorutine;
    #endregion

    #region property
    public bool IsLock { get; private set; } = false;

    public Camera Camera { get; private set; }
    public Transform StartTrans { get; private set; } = null;
    public Transform Target { get; private set; } = null;
    #endregion

    #region unity function
    void Start()
    {
        Initialize();
    }

    void FixedUpdate()
    {
        AvoidObstacle();
    }

    void Update()
    {
        if (IsLock)
        {
            LockOn();
        }
        OperatedByPlayer();
    }
    #endregion

    #region private function
    private void Initialize()
    {
        Camera = GetComponentInChildren<Camera>();
        _animator = GetComponentInChildren<Animator>();
        _radialBlurEffect = GetComponentInChildren<RadialBlurEffect>();
        Camera.depthTextureMode = DepthTextureMode.Depth;
        transform.position = StartTrans.position;
        transform.eulerAngles = StartTrans.eulerAngles;
        _targetPos = Target.position;
        _startDistance = (transform.position - _targetPos).sqrMagnitude;
    }

    private void OperatedByPlayer()
    {
        float inputH = _playerInput.RotateDirection.x;
        float inputV = _playerInput.RotateDirection.y;

        if (_playerInput.GetType() == typeof(KeyBoardInput))
        {
            if (!Input.GetMouseButton(0))
            {
                inputH = 0;
                inputV = 0;
            }
        }

        if (!IsLock)
        {
            transform.RotateAround(_targetPos, Vector3.up, inputH * Time.deltaTime * 200f);
            transform.RotateAround(_targetPos, transform.right, inputV * Time.deltaTime * 100f);
            transform.LookAt(_targetPos);
        }
        transform.position += Target.position - _targetPos;
        _targetPos = Target.position;

    }

    private void AvoidObstacle()
    {
        var distance = (transform.position - _targetPos).sqrMagnitude;

        if (FrontRayCheck())
        {
            transform.position = _frontHit.point;
        }
        else
        {
            if (!BackRayCheck())
            {
                if (distance < _startDistance)
                {
                    transform.position += -transform.forward.normalized * Time.deltaTime * 2;
                }
            }
        }
    }

    private bool FrontRayCheck()
    {
        return Physics.Linecast(Target.position, transform.position, out _frontHit, LayerMask.GetMask("Ground", "NPC"));
    }

    private bool BackRayCheck()
    {
        return Physics.Raycast(transform.position, -transform.forward, out _backHit, 0.3f, LayerMask.GetMask("Ground", "NPC"));
    }

    private Transform GetLockOnTarget()
    {
        var hits = Physics.SphereCastAll(_targetPos, 100, Vector3.up, 0.01f, LayerMask.GetMask("Enemy"));

        return hits[0].transform.root;
    }

    private void LockOn()
    {
        enemyLooker.LookAt(_enemyTransform.position + Vector3.up * transform.position.y);
        float angleDiffX = Mathf.DeltaAngle(transform.eulerAngles.x, enemyLooker.eulerAngles.x);
        float angleDiffY = Mathf.DeltaAngle(transform.eulerAngles.y, enemyLooker.eulerAngles.y);
        transform.RotateAround(_targetPos, Vector3.up, angleDiffY);
        transform.RotateAround(_targetPos, transform.right, angleDiffX);
    }
    #endregion

    #region public function
    public void SetInputter(IPlayerInput inputter)
    {
        _playerInput = inputter;
    }

    public void SetStartTransform(Transform startTransform)
    {
        StartTrans = startTransform;
    }

    public void SetTarget(Transform tage)
    {
        Target = tage;
    }

    public void Shake()
    {
        _animator.SetTrigger("Shake");
    }

    public void RadialBlurEffect()
    {
        if (_blurCorutine != null) StopCoroutine(_blurCorutine);
        _blurCorutine = StartCoroutine(_RadialBlurEffect(0.2f));
    }

    public void LockOnSwitch(bool s)
    {
        IsLock = s;
        if (IsLock)
        {
            _enemyTransform = GetLockOnTarget();
        }
    }

    public void ShortShake()
    {
        StartCoroutine(_ShortShake(0.1f, 0.2f, 0.2f));
    }

    #endregion

    #region private enumerator

    private IEnumerator _ShortShake(float duration, float magnitudeX, float magnitudeY)
    {
        int loopNum = 10;
        var waitForSeconds = new WaitForSeconds(duration / loopNum);

        for(int i = 0; i < loopNum; i++)
        {
            var x = Random.Range(-1f, 1f) * magnitudeX;
            var y = Random.Range(-1f, 1f) * magnitudeY;
            Camera.transform.localPosition = new Vector3(x, y, 0);
            yield return waitForSeconds;
        }

        Camera.transform.localPosition = Vector3.zero;
    }


    private IEnumerator _RadialBlurEffect(float duration)
    {
        _radialBlurEffect.blurDegree = 0.02f;
        Time.timeScale = 0.2f;
        float loopNum = 10;
        var waitForSeconds = new WaitForSeconds(duration / loopNum);
        for (int i = 0; i < loopNum; i++)
        {
            _radialBlurEffect.blurDegree -= 0.02f / loopNum;
            yield return waitForSeconds;
        }
        _radialBlurEffect.blurDegree = 0;
        Time.timeScale = 1;
    }
    #endregion
}

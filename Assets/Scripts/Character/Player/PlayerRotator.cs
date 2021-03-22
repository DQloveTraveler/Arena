using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InputManagement;

public class PlayerRotator : MonoBehaviour, IPlayerSubClass
{
    [SerializeField] private float rotateSpeed = 3;
    private IPlayerInput _playerInput;
    private GameObject _cameraObj;

    private void Start()
    {
        _cameraObj = FindObjectOfType<CameraController>().gameObject;
    }

    private void Update()
    {
        RotateHorizontal();
    }

    public void SetInputter(IPlayerInput inputter)
    {
        _playerInput = inputter;
    }

    private void RotateHorizontal()
    {
        var inputH = _playerInput.MoveDirection.x;
        var inputV = _playerInput.MoveDirection.y;


        if (inputH != 0 || inputV != 0)
        {
            // カメラの方向から、X-Z平面の単位ベクトルを取得
            var cameraForward = Vector3.Scale(_cameraObj.transform.forward, new Vector3(1, 0, 1)).normalized;
            //回転方向を算出
            var nextVector = cameraForward * inputV + _cameraObj.transform.right * inputH;

            var targetAngle = Mathf.Atan2(nextVector.x, nextVector.z) * Mathf.Rad2Deg;
            var nextAngle = Vector3.up * Mathf.MoveTowardsAngle(transform.eulerAngles.y, targetAngle, rotateSpeed * Time.deltaTime * 100);

            //回転処理
            transform.eulerAngles = nextAngle;
        }
    }

    public void LookAt(Vector3 targetPos)
    {
        var targetVector = Vector3.Scale(targetPos - transform.position, new Vector3(1, 0, 1));
        transform.forward = targetVector;
    }

}

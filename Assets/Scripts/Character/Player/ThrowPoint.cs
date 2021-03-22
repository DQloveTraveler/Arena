using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowPoint : MonoBehaviour
{
    private Transform _camera;
    private Vector3 newAngle;
    // Start is called before the first frame update
    void Awake()
    {
        _camera = FindObjectOfType<CameraController>().transform;
    }

    // Update is called once per frame
    void Update()
    {
        transform.eulerAngles = _camera.eulerAngles;
        transform.localEulerAngles = Vector3.Scale(transform.localEulerAngles, Vector3.right);
    }
}

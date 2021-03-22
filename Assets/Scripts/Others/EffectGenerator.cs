using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectGenerator : MonoBehaviour
{
    [SerializeField] private GameObject effectPrefab;
    [SerializeField] private bool instanceIsChild = false;
    [SerializeField] private bool birthOnGround = false;

    void Awake()
    {
        if (instanceIsChild) birthOnGround = false;
    }

    public void Generate()
    {
        if (instanceIsChild)
        {
            Instantiate(effectPrefab, transform);
        }
        else
        {
            if (birthOnGround)
            {
                var ray = new Ray(transform.position + Vector3.up * 10, Vector3.down);
                if(Physics.Raycast(ray, out var hit, 100, LayerMask.GetMask("Ground")))
                {
                    Instantiate(effectPrefab, hit.point, Quaternion.Euler(hit.normal));
                }
            }
            else
            {
                Instantiate(effectPrefab, transform.position, transform.rotation);
            }
        }
    }

}

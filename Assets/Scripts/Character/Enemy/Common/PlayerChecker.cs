using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerChecker : MonoBehaviour
{
    [SerializeField] private float checkBoxSize = 2;
    private Vector3 boxSize;
    private RaycastHit[] hits = new RaycastHit[0];

    public bool IsDetecting 
    { 
        get 
        {
            boxSize = new Vector3(checkBoxSize, checkBoxSize, checkBoxSize) / 2;
            hits = Physics.BoxCastAll(
                transform.position, boxSize, transform.forward,
                Quaternion.identity, 0.001f, LayerMask.GetMask("Player"));

            return hits.Length > 0;
        }
    }

}

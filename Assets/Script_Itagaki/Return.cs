using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Return : MonoBehaviour
{
    [SerializeField] private GameObject ConfirmUI;
    
    public void OnClick(){
        ConfirmUI.SetActive(false);
    }
}

using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using StatusManagement;

public class EpGaugeCtrl: MonoBehaviour {
    Image fill;
    private Status status;
    private float valueRatio;
    void Start(){
        status = FindObjectOfType<PlayerBehaviour>().Status;
        fill = GetComponent<Image>();
    }
    
    void Update() {
        valueRatio = (float)status.EP.Value / status.EP.Max;
        fill.fillAmount = valueRatio;
    }
}

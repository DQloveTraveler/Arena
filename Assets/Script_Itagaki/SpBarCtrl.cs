using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using StatusManagement;

public class SpBarCtrl: MonoBehaviour {
    Slider _slider;
    private Status status;
    void Start(){
        status = FindObjectOfType<PlayerBehaviour>().Status;
        _slider = GameObject.Find("SPBar").GetComponent<Slider>();
        _slider.maxValue = status.SP.Max;
        _slider.value = status.SP.Value;
    }
    void Update() {
        _slider.value = status.SP.Value;
    }
}

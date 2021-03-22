using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using StatusManagement;

public class HpBarCtrl: MonoBehaviour {
    Slider _slider;
    private Status status;
    void Start(){
        status = FindObjectOfType<PlayerBehaviour>().Status;
        _slider = GameObject.Find("HPBar").GetComponent<Slider>();
        _slider.maxValue = status.HP.Max;
        _slider.value = status.HP.Value;
    }
    void Update() {
        _slider.value = status.HP.Value;
    }
}

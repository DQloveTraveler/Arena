using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class PauseCtrl : MonoBehaviour {
    
    [SerializeField] private GameObject pauseUI;
    [SerializeField] private GameObject mainUI;
    [SerializeField] private UnityEvent onPauseDisable = new UnityEvent();

    void Awake()
    {
        pauseUI.SetActive(false);
        mainUI.SetActive(true);
    }

    void Update () {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetButtonDown("Menu")) {
            pauseUI.SetActive (!pauseUI.activeSelf);
            mainUI.SetActive (!mainUI.activeSelf);
            if(pauseUI.activeSelf) {
                Time.timeScale = 0;
            } else {
                Time.timeScale = 1f;
                onPauseDisable.Invoke();
            }
        }
    }
}

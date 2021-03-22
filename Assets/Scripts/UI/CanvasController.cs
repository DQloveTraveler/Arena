using InputManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasController : MonoBehaviour
{
    [SerializeField] private GameObject[] gamePadOnlys;
    [SerializeField] private GameObject[] mouseOnlys;
    [SerializeField] private GameObject stageClearPanel;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private Image black;


    private Controller CurrentInputter = Controller.KeyboardMouse;
    private Controller NewInputter => InputManager.Instance.Inputter;

    private PauseCtrl _pauseCtrl;

    void Awake()
    {
        _pauseCtrl = GetComponent<PauseCtrl>();
        foreach (var obj in gamePadOnlys) obj.SetActive(false);
        stageClearPanel.SetActive(false);
        gameOverPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        _CheckInputter();
        if(black != null)
        {
            black.color -= Color.black * 0.01f;
            if (black.color.a < 0.001f) Destroy(black.gameObject);
        }
    }

    private void _CheckInputter()
    {

        if (CurrentInputter != NewInputter)
        {
            switch (NewInputter)
            {
                case Controller.KeyboardMouse:
                    foreach(var obj in gamePadOnlys) obj.SetActive(false);
                    foreach(var obj in mouseOnlys) obj.SetActive(true);
                    break;
                case Controller.GamePad:
                    foreach (var obj in gamePadOnlys) obj.SetActive(true);
                    foreach (var obj in mouseOnlys) obj.SetActive(false);
                    break;
            }
            CurrentInputter = NewInputter;
        }
    }

    public void ActivateStageClearPanel()
    {
        if (!stageClearPanel.activeSelf)
        {
            _pauseCtrl.enabled = false;
            stageClearPanel.SetActive(true);
        }
    }

    public void ActivateGameOverPanel()
    {
        if (!gameOverPanel.activeSelf)
        {
            _pauseCtrl.enabled = false;
            gameOverPanel.SetActive(true);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using InputManagement;

public class TitleManager : MonoBehaviour
{
    private readonly float screenSizeMultipier = 0.85f;
    void Awake()
    {
        int screenW = (int)(1920 * screenSizeMultipier);
        int screenH = (int)(1080 * screenSizeMultipier);
        Screen.SetResolution(screenW, screenH, false, 60);
    }

    void Update()
    {
        if (Input.anyKeyDown) SceneManager.LoadScene(1);
    }

}

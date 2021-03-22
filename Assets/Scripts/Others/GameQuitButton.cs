using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameQuitButton : MonoBehaviour
{
    void Awake()
    {
        DontDestroyOnLoad(transform.root.gameObject);
        SceneManager.LoadScene("Title");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}

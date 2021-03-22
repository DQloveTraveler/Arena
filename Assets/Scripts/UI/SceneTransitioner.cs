using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class SceneTransitioner : MonoBehaviour
{
    private static EventSystem eventSystem;

    void Awake()
    {
        eventSystem = FindObjectOfType<EventSystem>();
    }

    void OnEnable()
    {
        eventSystem.SetSelectedGameObject(gameObject);
    }

    public void Transition()
    {
        switch (LevelSelector.SettingLevel)
        {
            case "Level 1":
                SceneManager.LoadScene("Level 1");
                break;
            case "Level 2":
                SceneManager.LoadScene("Level 2");
                break;
            case "Level 3":
                SceneManager.LoadScene("Level 3");
                break;
        }
    }

    public void Transition(string sceneName)
    {
        if( sceneName == "Title" ||
            sceneName == "LevelSelect" ||
            sceneName == "Level 1" ||
            sceneName == "Level 2" ||
            sceneName == "Level 3")
        {
            SceneManager.LoadScene(sceneName);
        }
    }

}

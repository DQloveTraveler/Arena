using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class LevelSelector : MonoBehaviour {
    
    [SerializeField] private TextMeshProUGUI checkText;

    public static string SettingLevel { get; private set; } = string.Empty;
    

    public void SelectLevel()
    {

        SettingLevel = transform.name;

        switch (SettingLevel)
        {
            case "Level 1":
                checkText.text = "レベル 1\nに挑戦しますか?";
                break;
            case "Level 2":
                checkText.text = "レベル 2\nに挑戦しますか?";
                break;
            case "Level 3":
                checkText.text = "レベル 3\nに挑戦しますか?";
                break;
        }
    }
}

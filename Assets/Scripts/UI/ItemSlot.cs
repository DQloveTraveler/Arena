using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Item;
using UnityEngine.UI;
using TMPro;

public class ItemSlot : MonoBehaviour
{
    public BaseItem Item { get; set; }
    public int Num { get; set; }
    private TextMeshProUGUI _name;
    private TextMeshProUGUI _num;
    private Image _image;


    void Awake()
    {
        _name = transform.Find("Name").GetComponent<TextMeshProUGUI>();
        _num = transform.Find("Num").GetComponent<TextMeshProUGUI>();
        _image = transform.Find("Image").GetComponent<Image>();
    }

    void Start()
    {
        Num = Item.MaxNum;
    }

    // Update is called once per frame
    void Update()
    {
        _name.text = Item.Name;
        _image.sprite = Item.Sprite;
        Num = ItemSlotCtrl.HavingNumList[Item];
        _num.text = Num.ToString();
    }
}

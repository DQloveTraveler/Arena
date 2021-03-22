using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Item;
using InputManagement;

public class ItemSlotCtrl : MonoBehaviour
{
    [SerializeField] private BaseItem[] itemList;
    [SerializeField] private float slideSpeed = 5;
    [SerializeField] private GameObject firstSelected;

    public static Dictionary<BaseItem, int> HavingNumList { get; } = new Dictionary<BaseItem, int>();

    private PlayerBehaviour _player;
    private EventSystem _eventSystem;
    private GameObject _selectedObj = null;
    private List<ItemSlot> _slotList = new List<ItemSlot>();

    void Awake(){
        _player = FindObjectOfType<PlayerBehaviour>();
        _eventSystem = FindObjectOfType<EventSystem>();
        _InitializeSlot();
    }

    private void _InitializeSlot()
    {
        Transform child = transform.GetChild(0);
        HavingNumList.Clear();
        for(int i = 0; i < child.childCount; i++)
        {
            _slotList.Add(child.GetChild(i).GetComponentInChildren<ItemSlot>());
            _slotList[i].Item = itemList[i];
            HavingNumList.Add(itemList[i], itemList[i].MaxNum);
        }
    }

    void Start()
    {
        if (_player.InputMode == typeof(PadInput))
        {
            _eventSystem.SetSelectedGameObject(_selectedObj ?? _eventSystem.currentSelectedGameObject ?? null);
        }
    }

    void OnEnable()
    {
        _eventSystem.SetSelectedGameObject(firstSelected ?? null);
    }

    void Update(){
        if (Mathf.Approximately(Time.timeScale, 0f)) return;

        if (_player.InputMode == typeof(PadInput)) _UpdateSlotPosition();

    }

    private void _UpdateSlotPosition()
    {
        _selectedObj = _eventSystem.currentSelectedGameObject ?? null;

        if (_selectedObj != null)
        {
            var _rootObj = _selectedObj.transform.parent.gameObject;

            if (_selectedObj.transform.name == "Item1") _rootObj.transform.localPosition = Vector2.Lerp(_rootObj.transform.localPosition, Vector2.right * 250, slideSpeed * Time.deltaTime);
            if (_selectedObj.transform.name == "Item2") _rootObj.transform.localPosition = Vector2.Lerp(_rootObj.transform.localPosition, Vector2.right * 125, slideSpeed * Time.deltaTime);
            if (_selectedObj.transform.name == "Item3") _rootObj.transform.localPosition = Vector2.Lerp(_rootObj.transform.localPosition, Vector2.zero, slideSpeed * Time.deltaTime);
            if (_selectedObj.transform.name == "Item4") _rootObj.transform.localPosition = Vector2.Lerp(_rootObj.transform.localPosition, Vector2.right * -125, slideSpeed * Time.deltaTime);
            if (_selectedObj.transform.name == "Item5") _rootObj.transform.localPosition = Vector2.Lerp(_rootObj.transform.localPosition, Vector2.right * -250, slideSpeed * Time.deltaTime);

        }
    }

    public void UseItem(int itemNum)
    {
        var currentSlot = _slotList[itemNum];
        if (currentSlot.Num > 0)
        {
            _player.UseItem(itemList[itemNum]);
        }
        
    }
}

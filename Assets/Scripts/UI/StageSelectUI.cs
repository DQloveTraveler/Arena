using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StageSelectUI : MonoBehaviour
{
    private readonly static float scaleMultiplier = 1.2f;
    private static EventSystem _eventSystem = null;
    private Collider2D _collider;
    private Button _button;

    void Awake()
    {
        _eventSystem = FindObjectOfType<EventSystem>();
        _collider = GetComponent<Collider2D>();
        _button = GetComponent<Button>();
    }

    void Start()
    {
        _eventSystem.SetSelectedGameObject(_eventSystem.firstSelectedGameObject);
    }

    // Update is called once per frame
    void Update()
    {
        //transform.LookAt(transform.position + (transform.position - Camera.main.transform.position));
        _collider.enabled = _button.interactable;
        if (_eventSystem.currentSelectedGameObject == gameObject) transform.localScale = Vector3.one * scaleMultiplier;
        else transform.localScale = Vector3.one;
    }
}

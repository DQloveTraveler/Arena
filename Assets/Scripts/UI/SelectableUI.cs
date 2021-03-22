using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Collider2D))]
public class SelectableUI : MonoBehaviour
{
    public static SelectableUI currentSelectedInstance = null;
    private readonly static float _scaleMultiplier = 1.2f;
    private static EventSystem _eventSystem = null;
    private Button _button;
    private Collider2D _collider;

    void Awake()
    {
        _eventSystem = _eventSystem ?? FindObjectOfType<EventSystem>();
        _collider = GetComponent<Collider2D>();
        _button = GetComponent<Button>();
    }

    // Update is called once per frame
    void Update()
    {
        _collider.enabled = _button.interactable;
        //transform.LookAt(transform.position + (transform.position - Camera.main.transform.position));
        if (_eventSystem.currentSelectedGameObject == gameObject ||
            currentSelectedInstance == this
            ) 
            transform.localScale = Vector3.one * _scaleMultiplier;
        else
            transform.localScale = Vector3.one;
    }
}

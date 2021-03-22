using InputManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GamePadCursor : MonoBehaviour
{
    [SerializeField] private float circleCastRadius = 25;
    [SerializeField] private bool useEventSystem = true;
    [SerializeField] private string submitInputName = "B";

    private RectTransform rectTransform;
    private EventSystem eventSystem;
    private readonly int sensitivity = 10;
    private float sensiMultiplier = 1;
    private Image cusorImage;
    private Vector2 minSize = new Vector2(0.7f, 0.7f);

    private int layerMask;


    void Awake()
    {
        useEventSystem = TryGetComponent<Collider2D>(out _);
        rectTransform = GetComponent<RectTransform>();
        eventSystem = FindObjectOfType<EventSystem>();
        cusorImage = GetComponent<Image>();
        cusorImage.enabled = InputManager.Instance.Inputter == Controller.GamePad;
        layerMask = LayerMask.GetMask("UI");
    }

    void Update()
    {
        _Move();
        if(!useEventSystem) _CollisionCheck();
    }

    private void _Move()
    {
        if (InputManager.Instance.Inputter == Controller.GamePad)
        {
            cusorImage.enabled = true;
            var horizontal = Input.GetAxis("L_Stick Horizontal");
            var vertical = Input.GetAxis("L_Stick Vertical");
            rectTransform.localPosition += new Vector3(horizontal, vertical, 0).normalized
                * sensitivity * 100 * Time.unscaledDeltaTime * sensiMultiplier;
            rectTransform.localPosition = new Vector2(
                Mathf.Clamp(rectTransform.localPosition.x, -400, 400),
                Mathf.Clamp(rectTransform.localPosition.y, -225, 225));
        }
        else
        {
            cusorImage.enabled = false;
            rectTransform.localPosition = Vector2.zero;
        }
    }

    private void _CollisionCheck()
    {
        var hits = Physics2D.CircleCastAll(
            rectTransform.position,
            25 * rectTransform.localScale.x,
            Vector2.up, 0, layerMask
            );
        if (hits.Length > 0)
        {
            SelectableUI.currentSelectedInstance = hits[0].transform.GetComponent<SelectableUI>();
            sensiMultiplier = 0.5f;
            rectTransform.localScale = Vector2.MoveTowards(rectTransform.localScale, minSize, 0.05f);
            Submit();
        }
        else
        {
            sensiMultiplier = 1;
            rectTransform.localScale = Vector2.one;
            if(useEventSystem) eventSystem.SetSelectedGameObject(null);
            else SelectableUI.currentSelectedInstance = null;
        }
    }

    private void Submit()
    {
        if (Input.GetButtonDown(submitInputName))
        {
            if(SelectableUI.currentSelectedInstance != null)
            {
                SelectableUI.currentSelectedInstance.GetComponent<Button>().onClick.Invoke();
            }
        }

    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        sensiMultiplier = 0.4f;
        rectTransform.localScale = Vector2.MoveTowards(rectTransform.localScale, minSize, 0.05f);
        eventSystem.SetSelectedGameObject(collision.transform.gameObject);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        sensiMultiplier = 1;
        rectTransform.localScale = Vector2.one;
        eventSystem.SetSelectedGameObject(null);
    }
}

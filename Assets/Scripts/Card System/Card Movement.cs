using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardMovement : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] RectTransform rectTransform;
    [SerializeField] Canvas canvas;
    [SerializeField] GameObject highlight;
    [SerializeField] float verticalHoverMovement = 75f;
    [SerializeField] float dragThreshold = 100f;
    [SerializeField] Transform playPosition;
    [SerializeField] ArcRenderer arcRenderer;
    Vector3 originalScale;
    Vector3 originalPosition;
    Vector2 originalPointerPosition;
    Vector2 originalPanelPosition;

    public bool dragging = false;
    public bool hovering = false;

    private void Awake()
    {
        //rectTransform = GetComponent<RectTransform>();
        //canvas = GetComponent<Canvas>();
        //canvas.renderMode.
        canvas.overrideSorting = true;
        originalScale = transform.localScale;
        arcRenderer.gameObject.SetActive(false);
    }
    public void Update()
    {

    }
    // Hovering over card
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (dragging) return;
        hovering = true;
        Debug.Log("Mouse entered: " + gameObject.name);
        canvas.sortingOrder = 10;
        highlight.SetActive(true);
        transform.localScale = originalScale * 1.2f;
        originalPosition = transform.localPosition;
        transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y + verticalHoverMovement, transform.localPosition.z);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if (dragging) return;
        hovering = false;
        Debug.Log("Mouse exited: " + gameObject.name);
        canvas.sortingOrder = 0;
        highlight.SetActive(false);
        transform.localScale = originalScale;
        transform.localPosition = Vector3.Lerp(transform.localPosition, originalPosition, 2.0f);

    }

    // Dragging and targetting cards
    public void OnDrag(PointerEventData eventData)
    {
        dragging = true;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.GetComponent<RectTransform>(), eventData.position, eventData.pressEventCamera, out Vector2 localPointerPosition);
        float distance = Vector2.Distance(originalPanelPosition, eventData.position);
        if (distance > dragThreshold)
        {
            Debug.Log("Dragged past threshold!");
            arcRenderer.gameObject.SetActive(true);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.GetComponent<RectTransform>(), eventData.position, eventData.pressEventCamera, out originalPointerPosition); //Using the event system to detect what is clicked on
        originalPanelPosition = eventData.position;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        arcRenderer.gameObject.SetActive(false);
        dragging = false;
        canvas.sortingOrder = 0;
        highlight.SetActive(false);
        transform.localScale = originalScale;
        transform.localPosition = Vector3.Lerp(transform.localPosition, originalPosition, 2.0f);
    }
}

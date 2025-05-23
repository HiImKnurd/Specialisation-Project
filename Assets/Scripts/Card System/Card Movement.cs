using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardMovement : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IDragHandler, IPointerDownHandler
{
    [SerializeField] RectTransform rectTransform;
    [SerializeField] Canvas canvas;
    [SerializeField] GameObject highlight;
    [SerializeField] float verticalHoverMovement = 100f;
    Vector3 originalScale;
    Vector3 originalPosition;
    Vector2 originalPointerPosition;
    Vector2 originalPanelPosition;

    private void Awake()
    {
        //rectTransform = GetComponent<RectTransform>();
        //canvas = GetComponent<Canvas>();
        //canvas.renderMode.
        canvas.overrideSorting = true;
        originalScale = transform.localScale;
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("Mouse entered: " + gameObject.name);
        canvas.sortingOrder = 10;
        highlight.SetActive(true);
        transform.localScale = originalScale * 1.2f;
        originalPosition = transform.localPosition;
        transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y + verticalHoverMovement, transform.localPosition.z);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("Mouse exited: " + gameObject.name);
        canvas.sortingOrder = 0;
        highlight.SetActive(false);
        transform.localScale = originalScale;
        transform.localPosition = originalPosition;
    }

    public void OnDrag(PointerEventData eventData)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.GetComponent<RectTransform>(), eventData.position, eventData.pressEventCamera, out Vector2 localPointerPosition);

    }

    public void OnPointerDown(PointerEventData eventData)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.GetComponent<RectTransform>(), eventData.position, eventData.pressEventCamera, out originalPointerPosition); //Using the event system to detect what is clicked on
        originalPanelPosition = rectTransform.localPosition;
    }
}

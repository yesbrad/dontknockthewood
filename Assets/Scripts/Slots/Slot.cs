using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public interface IDropSlot
{
    public void OnDraggedOnToo(Slot incomingSlot);
}

public class Slot : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropSlot, IPointerEnterHandler
{
    public Image slotImage;
    internal Item slotItem;
    [SerializeField] private bool dragging;

    public bool HasItem => slotItem != null;

    public void Set(Item item)
    {
        slotItem = item;
        RefreshUI();
    }

    public void UnSet()
    {
        slotItem = null;
        RefreshUI();
    }

    private void Update()
    {
        if (dragging) {
            slotImage.transform.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        }
    }

    public void RefreshUI()
    {
        slotImage.color = Color.white;
        
        if (slotItem != null)
        {
            slotImage.sprite = slotItem.data.slotImageSprite;
            return;
        }

        slotImage.color = Color.clear;
        slotImage.sprite = null;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        //Debug.Log("DRAG THE DOG:" + gameObject.name, gameObject);
        dragging = true;
        UI.instance.OnDragItem(this);
    }

    public void OnDrag(PointerEventData eventData)
    {
        //Debug.LogWarning("DRAG THE DOG");
        //slotImage.rectTransform.anchoredPosition = eventData.pressPosition + eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        ResetPosition();
        UI.instance.CheckForSlotOnRelease(eventData);
        //Debug.Log("EENNND THE DOG: "+ gameObject.name, gameObject);
    }

    public void ResetPosition()
    {
        dragging = false;
        slotImage.transform.position = transform.position;
    }

    public void OnDraggedOnToo(Slot incomingSlot)
    {
        
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(UI.instance.currentSelection == false)
            UI.instance.SetHoverText(slotItem == null ? "Empty" : slotItem.data.name);
    }
}

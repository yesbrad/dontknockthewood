using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public interface IDropSlot
{
    public void OnDraggedOnToo(Slot incomingSlot);
}

public class Slot : MonoBehaviour, IBeginDragHandler, IPointerDownHandler, IDragHandler, IEndDragHandler, IDropSlot
{
    private Image slotImage;
    public Item currentItem;

    public bool HasItem => currentItem != null;

    private void Awake()
    {
        slotImage = GetComponentInChildren<Image>();
    }

    public void Set(Item item)
    {
        currentItem = item;
        RefreshUI();
    }

    public void UnSet()
    {
        currentItem = null;
        RefreshUI();
    }

    public void RefreshUI()
    {
        if (currentItem)
        {
            slotImage.sprite = currentItem.data.slotImageSprite;
            return;
        }

        slotImage.sprite = null;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
       // UI.instance.OnDragItem(this);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (UI.instance.HasDragItem)
        {
            Debug.Log("COMBINE: " + UI.instance.DragSlot.currentItem.data.name + " : " + currentItem.data.name, gameObject);
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("DRAG THE DOG:" + gameObject.name, gameObject);
        UI.instance.OnDragItem(this);
    }

    public void OnDrag(PointerEventData eventData)
    {
        Debug.LogWarning("DRAG THE DOG");

    }

    public void OnEndDrag(PointerEventData eventData)
    {
        UI.instance.CheckForSlotOnRelease(eventData);
        Debug.Log("EENNND THE DOG: "+ gameObject.name, gameObject);
    }

    public void OnDraggedOnToo(Slot incomingSlot)
    {
        if (UI.instance.HasDragItem)
        {
            UI.instance.Combine(currentItem, incomingSlot.currentItem);
                
            //Debug.Log("COMBINE: " + incomingSlot.currentItem.data.name + " : " + currentItem.data.name, gameObject);
        }
    }
}

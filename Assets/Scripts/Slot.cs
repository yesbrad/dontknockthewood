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
    public Sprite defaultSprite;
    public Image slotImage;
    internal Item currentItem;

    public bool HasItem => currentItem != null;

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
        slotImage.color = Color.white;
        
        if (currentItem)
        {
            if (currentItem.data.slotImageSprite == null)
            {
                slotImage.sprite = defaultSprite;
            }
            else
            {
                slotImage.sprite = currentItem.data.slotImageSprite;
            }
            return;
        }

        slotImage.color = Color.clear;
        slotImage.sprite = null;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        //Debug.Log("DRAG THE DOG:" + gameObject.name, gameObject);
        UI.instance.OnDragItem(this);
    }

    public void OnDrag(PointerEventData eventData)
    {
        //Debug.LogWarning("DRAG THE DOG");

    }

    public void OnEndDrag(PointerEventData eventData)
    {
        UI.instance.CheckForSlotOnRelease(eventData);
        Debug.Log("EENNND THE DOG: "+ gameObject.name, gameObject);
    }

    public void OnDraggedOnToo(Slot incomingSlot)
    {
        if (currentItem == null)
        {
            UI.instance.ClearSelection();
            return;
        }
        
        if (UI.instance.HasDragItem)
        {
            UI.instance.Combine(currentItem, incomingSlot.currentItem);
                
            //Debug.Log("COMBINE: " + incomingSlot.currentItem.data.name + " : " + currentItem.data.name, gameObject);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        UI.instance.SetHoverSlot(this);
    }
}

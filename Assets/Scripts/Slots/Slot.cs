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
        UI.instance.OnDragItem(this);
    }

    public void OnDrag(PointerEventData eventData)
    {
        //Debug.LogWarning("DRAG THE DOG");

    }

    public void OnEndDrag(PointerEventData eventData)
    {
        UI.instance.CheckForSlotOnRelease(eventData);
        //Debug.Log("EENNND THE DOG: "+ gameObject.name, gameObject);
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

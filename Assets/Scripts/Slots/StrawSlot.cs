using System;
using Items;
using Slots;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StrawSlot : MonoBehaviour, IDropSlot, IPointerEnterHandler, IPointerExitHandler
{
    public Image equippedImage;
    
    public void OnDraggedOnToo(Slot incomingSlot)
    {
        OnDrag(incomingSlot.slotItem);
    }

    public void OnDrag(Item slotItem)
    {
        if (GameManager.instance.IsEquipped) return;

        if (slotItem.data.canGoInStraw)
        {
            //Play Loading Sound
            GameManager.instance.AddBall(slotItem);
            RefreshUI();
            UI.instance.UnSetSlot(slotItem);
            FindObjectOfType<ComboSlot>().UnSet();
        }
        else
        {
            UI.instance.DeSelect();
        }
    }

    public void RefreshUI()
    {
        if (GameManager.instance.IsEquipped && GameManager.instance.CurrentStrawItem.HasAmmo)
        {
            equippedImage.color = Color.white;
            equippedImage.sprite = GameManager.instance.CurrentStrawItem.data.slotImageSprite;
        }
        else
        {
            equippedImage.sprite = null;
            equippedImage.color = Color.clear;
        }
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        var currentSelection = UI.instance.currentSelection;

        if (GameManager.instance.IsEquipped)
        {
            UI.instance.SetHoverText("Loaded");
            return;
        }

        if (currentSelection)
        {
            //UI.instance.SetHoverText("Use " + UI.instance.currentSelection.slotItem.data.name);
        }
        else
        {
        }
        
        UI.instance.SetHoverText("Straw");
        
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (UI.instance.currentSelection != null)
        {
            UI.instance.SetHoverText(UI.instance.currentSelection.slotItem.data.name);
        }
        else
        {
            UI.instance.SetHoverText("");
        }
    }
}

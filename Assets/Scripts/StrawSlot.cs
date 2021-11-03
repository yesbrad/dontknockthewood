using System;
using Items;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StrawSlot : MonoBehaviour, IDropSlot, IPointerEnterHandler, IPointerExitHandler
{
    public void OnDraggedOnToo(Slot incomingSlot)
    {
        //print("Draggin on m0uth");

       // OnDrag(incomingSlot);
    }

    public void OnDrag(Item slotItem)
    {
        if (GameManager.instance.IsEquipped) return;

        if (slotItem.data.canGoInStraw)
        {
            //Play Loading Sound
            GameManager.instance.AddBall(slotItem);
            UI.instance.UnSetSlot(slotItem);
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

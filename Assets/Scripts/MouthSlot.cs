using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MouthSlot : MonoBehaviour, IDropSlot, IPointerEnterHandler
{
    [SerializeField] private Image firstSlotImage;
    [SerializeField] private Image secondSlotImage;
    
    internal Item firstSlot;
    internal Item secondSlot;
    
    public void OnDraggedOnToo(Slot incomingSlot)
    {
//        print("Draggin on m0uth");
        
        if (UI.instance.HasDragItem)
        {
            if (incomingSlot.slotItem != null)
            {
                //Need to be done in staw
                //GameManager.instance.AddBall(incomingSlot.currentItem);
                
                if (firstSlot != null)
                {
                    // Add UI second slot UI
                    secondSlot = incomingSlot.slotItem;
                    print("Eat Second slot");

                    //Play mouth animation
                    
                    // Change text to easting
                    
                    // after animation combine
                    if (UI.instance.Combine(firstSlot, secondSlot))
                    {
                        // YUM Test
                        UI.instance.UnSetSlot(secondSlot);
                        firstSlot = null;
                        secondSlot = null;
                    }
                    else
                    {
                        //Cant eat that text
                        secondSlot = null;
                    }
                    
                    //Refresh MOuth UI
                }
                else
                {
                    firstSlot = incomingSlot.slotItem;
                    UI.instance.UnSetSlot(firstSlot);
                    print("Eat first slot");
                    //add to first slot
                    //Update Mouth Slot UI
                }
                
                RefreshUI();
                
            }
        }
    }

    public void RefreshUI()
    {
        if (firstSlot != null)
        {
            firstSlotImage.color = Color.white;
            firstSlotImage.sprite = firstSlot.data.slotImageSprite;
        }
        else
        {
            firstSlotImage.color = Color.clear;
        }
        
        if (secondSlot != null)
        {
            secondSlotImage.color = Color.white;
            secondSlotImage.sprite = secondSlot.data.slotImageSprite;
        }
        else
        {
            secondSlotImage.color = Color.clear;
        }
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        UI.instance.SetMouthHoverText(this);
    }
}

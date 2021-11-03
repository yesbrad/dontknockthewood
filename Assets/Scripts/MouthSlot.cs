using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MouthSlot : MonoBehaviour, IDropSlot, IPointerEnterHandler
{
    public Item firstSlot;
    public Item secondSlot;
    
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
                
            }
        }
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        UI.instance.SetMouthHoverText(this);
    }
}

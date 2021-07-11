using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MouthSlot : MonoBehaviour, IDropSlot
{
    public void OnDraggedOnToo(Slot incomingSlot)
    {
        print("Draggin on m0uth");
        
        if (UI.instance.HasDragItem)
        {
            if (incomingSlot.currentItem.data.spitball != null)
            {
                GameManager.instance.AddBall(incomingSlot.currentItem.data.spitball);
                Debug.Log("ADDING BALLS: " + incomingSlot.currentItem.data.name, gameObject);
                UI.instance.UnSetSlot(incomingSlot.currentItem);
            }
        }
    }
}

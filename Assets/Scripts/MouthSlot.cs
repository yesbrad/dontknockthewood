using System;
using Items;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MouthSlot : MonoBehaviour, IDropSlot, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Sprite openSprite;
    [SerializeField] private Sprite closedSprite;
    [SerializeField] private Image mouth;
    
    
    [SerializeField] private Image firstSlotImage;
    [SerializeField] private Image secondSlotImage;
    
    [Header("DEBBUG")]
    public Item firstSlot;
    public Item secondSlot;

    private void Awake()
    {
        SetMouthState(false);
    }

    public void OnDraggedOnToo(Slot incomingSlot)
    {
        //print("Draggin on m0uth");

        if (incomingSlot.slotItem == null) return;
        
        if(incomingSlot.slotItem.data.canGoInMouth == false) return;

        if (firstSlot != null) // Is there already an item in da mouth
        {
            secondSlot = incomingSlot.slotItem;
            print("Eat Second slot");

            //Play mouth animation
                
            // Change text to easting
                
            // after animation combine
            if (UI.instance.Combine(firstSlot, secondSlot))
            {
                // YUM Test
                firstSlot = null;
                secondSlot = null;
            }
            else
            {
                //Cant eat that text\
                UI.instance.SetHoverText(GameManager.instance.bible.eatNoCombo);
                UI.instance.DeSelect();
                secondSlot = null;
            }
                
            //Refresh MOuth UI
        }
        else
        {
            print("Eat first slot");
            firstSlot = incomingSlot.slotItem;
            UI.instance.UnSetSlot(firstSlot);
            UI.instance.SetHoverText(GameManager.instance.bible.eatCombo);
            //add to first slot
            //Update Mouth Slot UI
        }
            
        SetMouthState(false);

        RefreshUI();

    }

    public void SetMouthState(Item item)
    {
        if (UI.instance.currentSelection && UI.instance.currentSelection.slotItem.data.canGoInMouth && firstSlot == null)
        {
            SetMouthState(true);
            return;
        }
        
        ItemCombo combo = UI.instance.GetCombo(UI.instance.currentSelection.slotItem, firstSlot);

        SetMouthState(combo);
    }

    public void SetMouthState(bool item)
    {
        mouth.sprite = item ? openSprite : closedSprite;
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
            firstSlotImage.sprite = null;
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
            secondSlotImage.sprite = null;
        }
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        var currentSelection = UI.instance.currentSelection;

        if (currentSelection)
        {
            if (secondSlot == null)
            {
                UI.instance.SetHoverText("Eat");
                return;
            }
            
            ItemCombo combo = UI.instance.GetCombo(currentSelection.slotItem, firstSlot);

            if (combo)
            {
                // Open Mouth
                UI.instance.SetHoverText(currentSelection.slotItem.data.name + " + " + firstSlot.data.name + " = " + combo.comboItemData.name);
            }
            else
            {
                //close mouth
                UI.instance.SetHoverText(GameManager.instance.bible.dragNoCombo);
            }
                
        }
        else
        {
            UI.instance.SetHoverText("Mouth");
        }
        
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

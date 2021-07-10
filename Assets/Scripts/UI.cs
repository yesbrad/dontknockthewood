using System;
using System.Collections.Generic;
using System.Linq;
using Items;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI : MonoBehaviour
{
    public static UI instance;
    public Catalog comboCatalog;

    [Header("DONT TOUCH")]
    public List<Slot> slots = new List<Slot>();
    private Slot currentSelection;

    public bool HasDragItem => currentSelection != null;

    public Slot DragSlot => currentSelection;

    private void Awake()
    {
        instance = this;
        slots = GetComponentsInChildren<Slot>().ToList();
    }

    public void SetSlot(Item item)
    {
        foreach (Slot slot in slots)
        {
            if (slot.HasItem == false)
            {
                slot.Set(item);
                return;
            }
        }
    }
    
    public void UnSetSlot(Item item)
    {
        foreach (Slot slot in slots)
        {
            if (slot.currentItem == item)
            {
                slot.UnSet();
                return;
            }
        }
    }

    public bool HasSlot()
    {
        return !slots.TrueForAll(slot => slot.HasItem);
    }
    
    public void OnDragItem(Slot slot)
    {
        currentSelection = slot;
        Debug.Log("DragingItem: " + slot.currentItem.data.name);
    }
    
    
    public void CheckForSlotOnRelease(PointerEventData data)
    {
        print("Looking for the fucking  SLOT");
        RaycastHit hit;
        List<RaycastResult> res = new List<RaycastResult>();
        
        EventSystem.current.RaycastAll(data, res);

        if (res.Count > 0)
        {
            foreach (var uithing in res)
            {
                Slot dropSlot = uithing.gameObject.GetComponent<Slot>();

                if (dropSlot)
                {
                    print("FOUND SLOT");
                    dropSlot.OnDraggedOnToo(currentSelection);
                    return;
                }
            }
        }
    }
    
    
    public bool Combine(Item ingOne, Item ingTwo)
    {
            
        foreach (ItemCombo itemCombo in comboCatalog.Combos)
        {
            Debug.Log(itemCombo.firstIngredient.name);
            Debug.Log(itemCombo.secondIngredient.name);

            bool che1 = ingOne.data.name == itemCombo.firstIngredient.name &&
                        ingTwo.data.name == itemCombo.secondIngredient.name;
            
            bool che2 = ingTwo.data.name == itemCombo.firstIngredient.name &&
                        ingOne.data.name == itemCombo.secondIngredient.name;
            
            if (che1 || che2)
            {
                //Delete old 2
                UnSetSlot(ingOne);
                UnSetSlot(ingTwo);
                //Make new one
                Item comboItem = new GameObject("NewCombo: " + itemCombo.comboItem.name).AddComponent<Item>().Create(itemCombo.comboItem);
                SetSlot(comboItem);
                
                return true;
            }
        }

        Debug.Log("Combine Failed");
        return false;
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using Items;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    public static UI instance;
    public Catalog comboCatalog;

    public TextMeshProUGUI ballText;
    public TextMeshProUGUI ammoText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI comboText;

    [Header("DONT TOUCH")]
    public List<Slot> slots = new List<Slot>();
    internal Slot currentSelection;
    private Slot hoverSlot;

    public bool HasDragItem => currentSelection != null;

    public Slot DragSlot => currentSelection;

    private void Awake()
    {
        instance = this;
        slots = GetComponentsInChildren<Slot>().ToList();
        RefreshSpitUI(null);
        RefreshScoreUI(0);
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
        
        Debug.LogError("NO MORE SLOTS SOFT LOCKED");
    }

    public void UnSetSlot(Slot slot)
    {
        UnSetSlot(slot.slotItem);
    }
    
    public void UnSetSlot(Item item)
    {
        foreach (Slot slot in slots)
        {
            if (slot.slotItem.UUID == item.UUID)
            {
                slot.UnSet();
                return;
            }
        }
    }

    public void SetMouthHoverText(MouthSlot mouth)
    {
        if (mouth.firstSlot == null)
        {
            SetHoverText("Ready to Eat");
            return;
        }
        
        if (currentSelection)
        {
            ItemCombo combo = GetCombo(currentSelection.slotItem, mouth.firstSlot);
            
            if(combo)
                SetHoverText(currentSelection.slotItem.data.name + " + " + mouth.firstSlot.data.name + " = " + combo.comboItemData.name);
            else
                SetHoverText("YUCK! That's Not Gonna Work..");
                
        }
    }

    public void SetHoverSlot(Slot slot)
    {
     SetHoverText(currentSelection.slotItem.data.name);   
    /*
        hoverSlot = slot;
        string hoverText = slot.HasItem ? slot.slotItem.data.name : "Empty";

        if (currentSelection)
        {
            ItemCombo combo = GetCombo(currentSelection.slotItem, slot.slotItem);
            
            if(combo)
                hoverText = currentSelection.slotItem.data.name + " + " + (slot.HasItem ? slot.slotItem.data.name : "Empty") + " = " + combo.comboItem.name;
            else
                hoverText = currentSelection.slotItem.data.name + " + " + (slot.HasItem ? slot.slotItem.data.name : "Empty") + " = ?";
                
        }
        
        SetHoverText(hoverText);
    */
    }
    
    public void SetHoverText(string txt)
    {
        comboText.SetText($"{txt}");
    }

    public bool HasSlot()
    {
        return !slots.TrueForAll(slot => slot.HasItem);
    }
    
    public void OnDragItem(Slot slot)
    {
        currentSelection = slot;
        Debug.Log("DragingItem: " + slot.slotItem.data.name);
    }
    
    
    public void CheckForSlotOnRelease(PointerEventData data)
    {
        RaycastHit hit;
        List<RaycastResult> res = new List<RaycastResult>();
        
        EventSystem.current.RaycastAll(data, res);

        if (res.Count > 0)
        {
            foreach (var uithing in res)
            {
                IDropSlot dropSlot = uithing.gameObject.GetComponent<IDropSlot>();

                if (dropSlot != null)
                {
                    //print("FOUND SLOT");
                    dropSlot.OnDraggedOnToo(currentSelection);
                    DeSelect();
                    return;
                }
            }
        }
        
        DeSelect();
    }

    public void DeSelect()
    {
        // Move UI back to slot
        currentSelection = null;
    }

    public ItemCombo GetCombo(Item ingOne, Item ingTwo)
    {
        if (ingOne == null || ingTwo == null)
            return null;
        
        foreach (ItemCombo itemCombo in comboCatalog.Combos)
        {
            if(itemCombo == null)
                continue;
            
            //Debug.Log(itemCombo.firstIngredient.name);
            //Debug.Log(itemCombo.secondIngredient.name);

            bool che1 = ingOne.data.name == itemCombo.firstIngredient.name &&
                        ingTwo.data.name == itemCombo.secondIngredient.name;
            
            bool che2 = ingTwo.data.name == itemCombo.firstIngredient.name &&
                        ingOne.data.name == itemCombo.secondIngredient.name;
            
            if (che1 || che2)
            {
                return itemCombo;
            }
        }

        return null;
    }
    
    public bool Combine(Item ingOne, Item ingTwo)
    {
        ItemCombo combo = GetCombo(ingOne, ingTwo);

        if (combo != null)
        {
            //Delete old 2
            //UnSetSlot(ingOne);
            //UnSetSlot(ingTwo);
            //Make new one
            Item comboItem = new GameObject("NewCombo: " + combo.comboItemData.name).AddComponent<Item>().Create(combo.comboItemData);
            print("Created Combo: " + combo.comboItemData.name);
            SetSlot(comboItem);
            return true;
        }
        else
        {
            Debug.Log("Combine Failed");
            return false;
        }
    }

    public void RefreshSpitUI(Item item)
    {
        if (item == null)
        {
            ballText.SetText("");
            ammoText.SetText("");
            return;
        }
        
        ballText.SetText(item.data.name);
        ammoText.SetText($"{item.Ammo}/{item.data.startAmmo}");
    }

    public void RefreshScoreUI(int score)
    {
        scoreText.SetText("" + score);
    }
}

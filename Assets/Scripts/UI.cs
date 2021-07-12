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

    public void SetMouthHoverText(MouthSlot mouth)
    {
        if (currentSelection != null 
            && currentSelection.currentItem != null)
        {
            if (currentSelection.currentItem.data.spitball != null)
            {
                SetHoverText("Make " + currentSelection.currentItem.data.spitball.name + "s");
            }
            else
            {
                SetHoverText("YUCK! Not that!");
            }
            
            return;
        }
        
        SetHoverText("Ready to make Spitballs");
    }

    public void SetHoverSlot(Slot slot)
    {
        hoverSlot = slot;
        string hoverText = slot.HasItem ? slot.currentItem.data.name : "Empty";

        if (currentSelection)
        {
            ItemCombo combo = GetCombo(currentSelection.currentItem, slot.currentItem);
            
            if(combo)
                hoverText = currentSelection.currentItem.data.name + " + " + (slot.HasItem ? slot.currentItem.data.name : "Empty") + " = " + combo.comboItem.name;
            else
                hoverText = currentSelection.currentItem.data.name + " + " + (slot.HasItem ? slot.currentItem.data.name : "Empty") + " = ?";
                
        }
        
        SetHoverText(hoverText);
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
        Debug.Log("DragingItem: " + slot.currentItem.data.name);
    }
    
    
    public void CheckForSlotOnRelease(PointerEventData data)
    {
        RaycastHit hit;
        List<RaycastResult> res = new List<RaycastResult>();
        
        EventSystem.current.RaycastAll(data, res);
        print("ABOUT TO CHECK SLOT");

        if (res.Count > 0)
        {
            foreach (var uithing in res)
            {
                IDropSlot dropSlot = uithing.gameObject.GetComponent<IDropSlot>();

                if (dropSlot != null)
                {
                    print("FOUND SLOT");
                    dropSlot.OnDraggedOnToo(currentSelection);
                    ClearSelection();
                    return;
                }
            }
        }
        
        ClearSelection();
    }

    public void ClearSelection()
    {
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
            UnSetSlot(ingOne);
            UnSetSlot(ingTwo);
            //Make new one
            Item comboItem = new GameObject("NewCombo: " + combo.comboItem.name).AddComponent<Item>().Create(combo.comboItem);
            SetSlot(comboItem);
            return true;
        }
        else
        {
            Debug.Log("Combine Failed");
            return false;
        }
    }

    public void RefreshSpitUI(Spitball ball)
    {
        if (ball == null)
        {
            ballText.SetText("");
            ammoText.SetText("");
            return;
        }
        
        ballText.SetText(ball.data.name);
        ammoText.SetText($"{ball.ammo}/{ball.data.startAmmo}");
    }

    public void RefreshScoreUI(int score)
    {
        scoreText.SetText("" + score);
    }
}

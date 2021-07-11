using System;
using System.Collections.Generic;
using System.Linq;
using Items;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI : MonoBehaviour
{
    public static UI instance;
    public Catalog comboCatalog;

    public TextMeshProUGUI ballText;
    public TextMeshProUGUI ammoText;
    public TextMeshProUGUI scoreText;

    [Header("DONT TOUCH")]
    public List<Slot> slots = new List<Slot>();
    private Slot currentSelection;

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

        if (res.Count > 0)
        {
            foreach (var uithing in res)
            {
                IDropSlot dropSlot = uithing.gameObject.GetComponent<IDropSlot>();

                if (dropSlot != null)
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
        scoreText.SetText("Score: " + score);
    }
}

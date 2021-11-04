using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Items;
using Slots;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    public static UI instance;
    public Catalog combos;

    [SerializeField] private GameObject slotGroup;

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
        slots = slotGroup.GetComponentsInChildren<Slot>().ToList();
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

    public void UnSetSlot(Item item)
    {
        Slot[] allSlots = FindObjectsOfType<Slot>();
        
        foreach (Slot slot in allSlots)
        {
            if(slot.slotItem == null) continue;
            
            if (slot.slotItem.UUID == item.UUID)
            {
                slot.UnSet();
                return;
            }
        }
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
        SetHoverText(slot.slotItem.data.name);
        FindObjectOfType<MouthSlot>().SetMouthState(slot.slotItem);
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
        if(currentSelection != null)
            currentSelection.ResetPosition();
        
        currentSelection = null;
    }

    public ItemCombo GetCombo(Item ingOne, Item ingTwo)
    {
        if (ingOne == null || ingTwo == null)
            return null;
        
        foreach (ItemCombo itemCombo in combos.Combos)
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

        if (GameManager.instance.IsEquipped) return false;
        
        if (combo != null)
        {
            //Delete old 2
            UnSetSlot(ingOne);
            UnSetSlot(ingTwo);
            //Make new one
            Item comboItem = new GameObject("NewCombo: " + combo.comboItemData.name).AddComponent<BasicItem>().Create(combo.comboItemData);
            print("Created Combo: " + combo.comboItemData.name);
            //SetSlot(comboItem);
            FindObjectOfType<ComboSlot>().Set(comboItem);
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

    private float lerpedScore = 0;
    private float score = 0;

    private void Update()
    {
        lerpedScore = Mathf.Lerp(lerpedScore, score, Time.deltaTime * 10);
        scoreText.SetText("" + Mathf.Ceil(lerpedScore));
    }

    public void RefreshScoreUI(int scorer)
    {
        this.score = scorer;
    }
}

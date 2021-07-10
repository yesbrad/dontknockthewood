using System;
using UnityEngine;

public class Item : MonoBehaviour
{
    public ItemData data;

    private bool IsSelected;

    public Item Create(ItemData newItem)
    {
        data = newItem;
        return this;
    }
    
    private void Start()
    {
        if(!data)
            Debug.LogError("ITEM MISSING DATA", gameObject);
    }

    public bool Select()
    {
        if (IsSelected)
        {
            return false;
        }
        
        ToggleRenderer(false);
        IsSelected = true;
        return true;
    }

    private void ToggleRenderer(bool on)
    {
        var rens = GetComponentsInChildren<Renderer>();

        foreach (Renderer ren in rens)
        {
            ren.enabled = on;
        }
    }
}
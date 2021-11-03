using System;
using UnityEngine;

public abstract class Item : MonoBehaviour
{
    public ItemData data;
    public string UUID { get; private set; }
    
    private bool IsSelected;

    private int item;
    private int _ammo;

    public int Ammo => _ammo;

    public Item Create(ItemData newItem)
    {
        data = newItem;

        if (newItem.canGoInStraw)
        {
            _ammo = data.startAmmo;
        }

        UUID = Guid.NewGuid().ToString();
        
        return this;
    }
    
    private void Start()
    {
        if(!data)
            Debug.LogError("ITEM MISSING DATA", gameObject);
    }

    public virtual Item Select()
    {
        if (IsSelected)
        {
            return null;
        }

        ToggleRenderer(false);
        IsSelected = true;
        return this;
    }

    protected void ToggleRenderer(bool on)
    {
        var rens = GetComponentsInChildren<Renderer>();

        foreach (Renderer ren in rens)
        {
            ren.enabled = on;
        }
    }
    
    protected void ToggleRenderer(bool on, Renderer ren)
    {
        ren.enabled = on;
    }
    
    public bool Use()
    {
        _ammo--;

        if (_ammo <= 0)
        {
            return false;
        }

        return true;
    }
}
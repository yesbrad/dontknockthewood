using System;
using System.Collections;
using System.Collections.Generic;
using Items;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemManager : MonoBehaviour
{
    
    private void Start()
    {
    }

    private void Update()
    {
        RaycastHit hit;

        if(!Input.GetMouseButtonDown(0))
            return;
        
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
        {
            Item item = hit.transform.GetComponent<Item>();
            
            if (item)
            {
                SelectItem(item);
            }
        }
    }

    private void SelectItem(Item item)
    {
        if (!UI.instance.HasSlot())
        {
            Debug.Log("Trello");
            return;
        }
        
        if (item.Select())
        {
            UI.instance.SetSlot(item);
        }
    }


}

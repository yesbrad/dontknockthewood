using System;
using System.Collections;
using System.Collections.Generic;
using Items;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public Animator impulseAnimator;
    
    public GameObject baseSpitballHitPrefab;
    public GameObject straw;

    public GameObject spitballHitPFX;
    
    private Item currentStrawItem;
    public Item CurrentStrawItem => currentStrawItem;

    public DesignBible bible;

    public bool IsEquipped => currentStrawItem != null;

    private int score;
    
    private void Start()
    {
        instance = this;
        bible = FindObjectOfType<BibleManager>().bible;
    }

    private void Update()
    {
        straw.SetActive(Input.GetMouseButton(1));
        
        if (Input.GetMouseButton(1) && (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space)))
        {
            //FIRE!!!
            
            if (currentStrawItem != null)
            {
                if (currentStrawItem.Use())
                {
                    RaycastHit hit;

                    if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
                    {
                        ISpitballHit s = hit.transform.GetComponent<ISpitballHit>();

                        if (s != null)
                        {
                            s.OnHit(currentStrawItem);
                            impulseAnimator?.SetTrigger("Shoot");
                            Instantiate(spitballHitPFX, hit.point + (Vector3.forward * 0.2f), Quaternion.identity);                    
                        }
                    }
                    
                    UI.instance.RefreshSpitUI(currentStrawItem);
                }
                else
                {
                    // NO AMMO
                    currentStrawItem = null;
                    UI.instance.RefreshSpitUI(null);
                }
                
                FindObjectOfType<StrawSlot>().RefreshUI();
            }
        }

        PickupDetection();
    }

    private void PickupDetection()
    {
        if (!Input.GetMouseButtonDown(0))
            return;

        if (Input.GetMouseButton(1))
        {
            return;
        }
        
        RaycastHit hit;

        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
        {
            Item item = hit.transform.GetComponent<Item>();

            if (item)
            {
                if (!UI.instance.HasSlot())
                {
                    Debug.Log("NoSlots");
                    return;
                }

                Item selectedItem = item.Select();
        
                if (selectedItem != null)
                {
                    UI.instance.SetSlot(selectedItem);
                }
            }
        }
    }

    public void AddBall(Item item)
    {
        currentStrawItem = item;
        UI.instance.RefreshSpitUI(currentStrawItem);
    }

    public void AddScore(int amt)
    {
        score += amt;
        
        print("ADDING TEACH SCORE");
        
        UI.instance.RefreshScoreUI(score);

        if (score >= bible.scoreToBeat)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}

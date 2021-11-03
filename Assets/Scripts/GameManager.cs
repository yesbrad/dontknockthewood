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
    
    private Item strawItem;

    public DesignBible bible;

    public bool IsEquipped => strawItem != null;

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
            
            if (strawItem != null)
            {
                if (strawItem.Use())
                {
                    RaycastHit hit;

                    if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
                    {
                        ISpitballHit s = hit.transform.GetComponent<ISpitballHit>();

                        if (s != null)
                        {
                            s.OnHit(strawItem);
                            impulseAnimator?.SetTrigger("Shoot");
                            Instantiate(spitballHitPFX, hit.point + (Vector3.forward * 0.2f), Quaternion.identity);                    
                        }
                    }
                    
                    UI.instance.RefreshSpitUI(strawItem);
                }
                else
                {
                    // NO AMMO
                    strawItem = null;
                    UI.instance.RefreshSpitUI(null);
                }
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
        strawItem = item;
        UI.instance.RefreshSpitUI(strawItem);
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

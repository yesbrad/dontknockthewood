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
    public int scoreToBeat = 1000;
    
    public GameObject baseSpitballHitPrefab;
    public GameObject straw;

    public GameObject spitballHitPFX;
    
    private Spitball currentSpitBall;

    private int score;
    
    private void Start()
    {
        instance = this;
    }

    private void Update()
    {
        straw.SetActive(Input.GetMouseButton(1));
        
        if (Input.GetMouseButton(1) && (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space)))
        {
            //FIRE!!!
            
            if (currentSpitBall != null)
            {
                if (currentSpitBall.Use())
                {
                    RaycastHit hit;

                    if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
                    {
                        ISpitballHit s = hit.transform.GetComponent<ISpitballHit>();

                        if (s != null)
                        {
                            s.OnHit(currentSpitBall);
                            Instantiate(spitballHitPFX, hit.point + (Vector3.forward * 0.2f), Quaternion.identity);                    
                        }
                    }
                    
                    UI.instance.RefreshSpitUI(currentSpitBall);
                }
                else
                {
                    // NO AMMO
                    currentSpitBall = null;
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
                SelectItem(item);
            }
        }
    }

    private void SelectItem(Item item)
    {
        if (!UI.instance.HasSlot())
        {
            Debug.Log("NoSlots");
            return;
        }
        
        if (item.Select())
        {
            UI.instance.SetSlot(item);
        }
    }

    public void AddBall(SpitballData ball)
    {
        currentSpitBall = new Spitball(ball);
        UI.instance.RefreshSpitUI(currentSpitBall);
    }

    public void AddScore(int amt)
    {
        score += amt;
        
        print("ADDING TEACH SCORE");
        
        UI.instance.RefreshScoreUI(score);

        if (score >= scoreToBeat)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}

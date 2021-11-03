using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Teacher : MonoBehaviour
{
    public int baseHitScore = 100;
    public TextMeshProUGUI exlemationMarkText;
    [SerializeField] private float exlemationTextTimoutTime = 1f;

    private bool on;

    private void Awake()
    {
        exlemationMarkText.SetText("");
    }

    public void GotHit(float amt, Item item)
    {
        GameManager.instance.AddScore((int)(amt * baseHitScore * item.data.scoreMultiplier));
    }
    
    public void SetExclamation(string character = "!")
    {
        if(on)
            return;

        StartCoroutine(ShowX(character));
    }

    IEnumerator ShowX(string te)
    {
        exlemationMarkText.SetText(te);
        on = true;

        yield return new WaitForSeconds(exlemationTextTimoutTime);

        on = false;
        exlemationMarkText.SetText("");
    }
}

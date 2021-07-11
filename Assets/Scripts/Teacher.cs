using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teacher : MonoBehaviour
{
    public int baseHitScore = 100;
    
    public void GotHit(float amt, Spitball ball)
    {
        GameManager.instance.AddScore((int)(amt * baseHitScore * ball.data.scoreMultiplier));
    }
}

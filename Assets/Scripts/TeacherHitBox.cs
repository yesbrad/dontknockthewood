using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISpitballHit
{
    public void OnHit(Spitball ball);
}

[RequireComponent(typeof(Collider))]
public class TeacherHitBox : MonoBehaviour, ISpitballHit
{
    [Range(1, 5)]
    public float hitScore;
    
    public void OnHit(Spitball ball)
    {
        GetComponentInParent<Teacher>().GotHit(hitScore, ball);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISpitballHit
{
    public void OnHit(Item item);
}

[RequireComponent(typeof(Collider))]
public class TeacherHitBox : MonoBehaviour, ISpitballHit
{
    [Range(1, 5)]
    public float hitScore;
    
    public string exlemationText = "!";

    public void OnHit(Item item)
    {
        GetComponentInParent<Teacher>().GotHit(hitScore, item);
        GetComponentInParent<Teacher>().SetExclamation(exlemationText);
    }
}

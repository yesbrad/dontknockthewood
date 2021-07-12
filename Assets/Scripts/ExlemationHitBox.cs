using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class ExlemationHitBox : MonoBehaviour, ISpitballHit
{
    public string exlemationText = "!";
    
    public void OnHit(Spitball ball)
    {
        GetComponentInParent<Teacher>().SetExclamation(exlemationText);
    }
}

using System.Collections;
using UnityEngine;

public class InfinateItem : Item
{
    public bool hasTimeRendererTimeout = true;
    public float timoutForRenderer = 0.5f;

    public Renderer SelectRenderer;

    private bool waiting = false;
    
    public override Item Select()
    {
        if (waiting)
            return null;

        if(hasTimeRendererTimeout)
            StartCoroutine(Timeout());

        Item newItem = Instantiate(this, Vector3.down * 100, Quaternion.identity);
        newItem.Create(data);
        
        return newItem;
    }

    IEnumerator Timeout()
    {
        waiting = true;
        ToggleRenderer(false, SelectRenderer);

        yield return new WaitForSeconds(timoutForRenderer);
        ToggleRenderer(true, SelectRenderer);
        waiting = false;


    }
}
using System.Collections;
using UnityEngine;

public class InfinateItem : Item
{
    public bool hasTimeRendererTimeout = true;
    public float timoutForRenderer = 0.5f;

    public Renderer SelectRenderer;

    private bool waiting = false;
    
    public override bool Select()
    {
        if (waiting)
            return false;

        if(hasTimeRendererTimeout)
            StartCoroutine(Timeout());
        
        return true;
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
using System;
using UnityEngine;

public class LookAt : MonoBehaviour
{
    public float maxXLook = 40;
    public float maxYLook = 80;
    private Vector3 pos;

    private void Update()
    {
        pos += new Vector3(Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"), 0);
        pos.y = Mathf.Clamp(pos.y, -maxXLook, maxXLook);
        pos.x = Mathf.Clamp(pos.x, -maxYLook, maxYLook);
        transform.localRotation = Quaternion.Euler(pos);
    }
}

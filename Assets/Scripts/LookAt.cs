using System;
using UnityEngine;

public class LookAt : MonoBehaviour
{
    public float maxXLook = 40;
    public float maxYLook = 80;
    private Vector3 pos;

    private void Start()
    {
        //Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;
    }

    private void Update()
    {
        //pos += new Vector3(Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"), 0);
        pos += new Vector3(-Input.GetAxis("Vertical"), Input.GetAxis("Horizontal"), 0);
        //pos.y = Mathf.Clamp(pos.y, -maxXLook, maxXLook);
        pos.x = Mathf.Clamp(pos.x, -maxYLook, maxYLook);
        transform.localRotation = Quaternion.Euler(pos);
        
        
            Cursor.lockState = Input.GetMouseButton(1) ? CursorLockMode.Locked : CursorLockMode.None;
            Cursor.visible = !Input.GetMouseButton(1);

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //Cursor.visible = true;
        }
    }
}

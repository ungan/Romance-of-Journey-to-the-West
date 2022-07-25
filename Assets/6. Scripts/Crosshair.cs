using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crosshair : MonoBehaviour
{
    public Vector2 mouse;
    void Start()
    {
        Cursor.visible = false;
    }

    void FixedUpdate()
    {
        mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        gameObject.transform.position = mouse;
    }
}

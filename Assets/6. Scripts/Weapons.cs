using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapons : MonoBehaviour
{
    SpriteRenderer spriteRenderer;

    public Vector2 mouse;
    public float z;
    public PartyManager partyManager;
    public Character character;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    void Update()
    {
        Watch();

    }

    void Watch()
    {
        if (partyManager.charactersIndex == character.posValue && character.curHealth > 0)
        {
            spriteRenderer.color = new Color(1, 1, 1, 1);
            mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            z = (Mathf.Atan2(mouse.y, mouse.x) * Mathf.Rad2Deg);
            transform.rotation = Quaternion.Euler(0, 0, z);
            //무기 뒤집기
            spriteRenderer.flipY = (z > 90f || z < -90f);
        }
        else
        {
            spriteRenderer.color = new Color(1, 1, 1, 0);
            transform.rotation = Quaternion.Euler(0, 0, z);
        }
    }
}

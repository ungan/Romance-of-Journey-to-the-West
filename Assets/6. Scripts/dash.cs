﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dash : MonoBehaviour
{
    public Enemy_ai enemy_ai;
    public GameObject dash_effect;

    public int count = 0;
    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.tag == "Leader")
        {
            if(count <= 1)
            {
                count = 2;
                enemy_ai.enemy_atk();
                dash_effect.SetActive(false);
                enemy_ai.isdash_effect = false;
                enemy_ai.enemy_state = e_state.Follow;
            }
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Leader")
        {
 
        }
    }
    void OnEnable()
    {
        count = 0;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

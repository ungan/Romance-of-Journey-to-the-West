using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dash_range : MonoBehaviour
{
    public Enemy_ai enemy;
    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.tag == "Leader")
        {
            if (enemy.isdead == false)
            {
                enemy.inrange_dash = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Leader")
        {
            if (enemy.isdead == false)
            {
                enemy.inrange_dash = false;
                enemy.isdash = false;
            }
        }
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

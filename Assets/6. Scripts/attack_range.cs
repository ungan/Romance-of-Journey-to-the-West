using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class attack_range : MonoBehaviour
{
    public Enemy_ai enemy_ai;
    // Start is called before the first frame update

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.tag == "Leader")
        {
            enemy_ai.inrange = true;
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Leader")
        {
            enemy_ai.inrange = false;
        }

    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}

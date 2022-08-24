using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class attack_range : MonoBehaviour
{
    public Enemy_ai enemy_ai;
    public GameObject crash_point;
    public PolygonCollider2D pc2;
    // Start is called before the first frame update

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Leader")
        {
            enemy_ai.inrange = true;
            Debug.Log("trigger");
            
            //enemy_ai.enemy_atk();

        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        ContactPoint2D contact = collision.contacts[0];
        if (collision.gameObject.tag == "Leader")
        {
            enemy_ai.inrange = true;
            crash_point.transform.position = new Vector3(contact.point.x, contact.point.y, 0);
            if(enemy_ai.enemy_state == e_state.attack)
            {
                crash_point.SetActive(true);
                pc2.isTrigger = true;
            }
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Leader")
        {

            crash_point.SetActive(false);
            pc2.isTrigger = false;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Leader")
        {
            enemy_ai.inrange = false;
            crash_point.SetActive(false);
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

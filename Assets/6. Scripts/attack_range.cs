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
        crash_point.SetActive(false);
        if (collision.gameObject.tag == "Leader")
        {
            if (enemy_ai.isdead == false)
            {
                enemy_ai.inrange = true;

                crash_point.transform.position = collision.gameObject.GetComponent<Collider2D>().ClosestPoint(transform.position);
                if (enemy_ai.enemy_state == e_state.attack)
                {
                    enemy_ai.enemy_atk();
                    crash_point.SetActive(true);
                    gameObject.SetActive(false);
                }
            }
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Leader")
        {
            //crash_point.SetActive(false);
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

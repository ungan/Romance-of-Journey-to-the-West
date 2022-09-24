using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class nachal_godhand : MonoBehaviour
{
    public SpriteRenderer hand_sprite;

    public boss_nachal nachal;
    public GameObject hand;
    public GameObject hand_smoke;
    public GameObject godhand;

    float Dist = 2f;
    float Speed = 1f;
    float count = 0;

    Vector3 destination;



    private void OnEnable()     // 이 오브젝트가 켜질때 사용될 것
    {
        hand_smoke.SetActive(false);
        destination = new Vector3(hand.transform.position.x, hand.transform.position.y - 4f, 0f);
    }
    // Start is called before the first frame update
    void Start()
    {
        //sprite = GetComponent<SpriteRenderer>();
        nachal = GameObject.Find("boss_nachal").GetComponent<boss_nachal>();
        destination = new Vector3(hand.transform.position.x,hand.transform.position.y-4f,0f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        
        if(count<Dist)
        {
            count += Speed * Time.deltaTime;
            hand.transform.position = Vector3.MoveTowards(hand.transform.position, destination, Speed * Time.deltaTime);
            hand_sprite.color = new Color(hand_sprite.color.r, hand_sprite.color.g, hand_sprite.color.b, hand_sprite.color.a + Time.deltaTime);
        }
        else
        {
            hand_sprite.color = new Color(hand_sprite.color.r, hand_sprite.color.g, hand_sprite.color.b, hand_sprite.color.a - Time.deltaTime);
            hand_smoke.transform.position = new Vector3(transform.position.x, transform.position.y-0.3f, transform.position.z);
            hand_smoke.SetActive(true);
        }

        if (hand_sprite.color.a <= 0)
        {
            nachal.isboss_pattern_h = false;
            Destroy(godhand);
        }

        /*
        if (Mathf.Round(hand.transform.position.x * 100) == Mathf.Round(destination.x * 100) && Mathf.Round(hand.transform.position.y * 100) == Mathf.Round(destination.y * 100))
        {
            hand_sprite.color = new Color(hand_sprite.color.a - Time.deltaTime, hand_sprite.color.r, hand_sprite.color.g, hand_sprite.color.b);
        }
        else
        {
            hand_sprite.color = new Color(hand_sprite.color.a + Time.deltaTime, hand_sprite.color.r, hand_sprite.color.g, hand_sprite.color.b);
            hand.transform.position = Vector3.MoveTowards(hand.transform.position, destination, 1*Time.deltaTime);
        }*/

    }
}

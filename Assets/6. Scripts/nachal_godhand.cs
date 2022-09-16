using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class nachal_godhand : MonoBehaviour
{
    public SpriteRenderer hand_sprite;
    public GameObject hand;
    
    float Dist = 2f;
    float Speed = 1f;
    float count = 0;
    Vector3 destination;

    

    // Start is called before the first frame update
    void Start()
    {
        //sprite = GetComponent<SpriteRenderer>();
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

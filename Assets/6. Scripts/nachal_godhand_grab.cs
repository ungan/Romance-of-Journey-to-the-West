using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class nachal_godhand_grab : MonoBehaviour
{
    public SpriteRenderer hand_sprite;
    public GameObject hand;

    float Dist = 2f;
    float Speed = 1f;
    float count = 0;
    int click_count = 0;
    
    bool isgrab = false;

    Vector3 destination;

    public PartyManager partyManager;
    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.tag == "Leader")
        {
            partyManager.controlList[partyManager.charactersIndex] = false;
            isgrab = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {

        if (collision.gameObject.tag == "Leader")
        {

        }

    }

    // Start is called before the first frame update
    void Start()
    {
        partyManager = GameObject.Find("Party").GetComponent<PartyManager>();  //파티(플레이어)찾기 SJM
        destination = new Vector3(hand.transform.position.x, hand.transform.position.y + 4f, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (count < Dist)
        {
            count += Speed * Time.deltaTime;
            hand.transform.position = Vector3.MoveTowards(hand.transform.position, destination, Speed * Time.deltaTime);
            hand_sprite.color = new Color(hand_sprite.color.r, hand_sprite.color.g, hand_sprite.color.b, hand_sprite.color.a + Time.deltaTime);
        }
        if(isgrab == true)
        {
            if(Input.GetKeyDown(KeyCode.Mouse0))
            {
                click_count++;
            }

            if(click_count > 2)
            {
                Destroy(gameObject);
            }
        }

    }
}

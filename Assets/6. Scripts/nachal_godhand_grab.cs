using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class nachal_godhand_grab : MonoBehaviour
{
    public SpriteRenderer hand_sprite;
    public GameObject hand;

    public GameObject gound_split_1;
    public GameObject gound_split_2;
    public GameObject gound_split_3;
    public Character character;
    public boss_nachal nachal;

    public bool isLeavingSkill = false;

    float Dist = 15f;
    float Speed = 50f;
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

    private void OnEnable()     // 이 오브젝트가 켜질때 사용될 것
    {
        isgrab = false;
        partyManager = GameObject.Find("Party").GetComponent<PartyManager>();  //파티(플레이어)찾기 SJM
        destination = new Vector3(hand.transform.position.x, hand.transform.position.y + 3f, 0f);
    }

    // Start is called before the first frame update
    void Start()
    {
        partyManager = GameObject.Find("Party").GetComponent<PartyManager>();  //파티(플레이어)찾기 SJM
        destination = new Vector3(hand.transform.position.x, hand.transform.position.y + 3f, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        if (isgrab == true)
        {
            if(isLeavingSkill == false)
            {
                character = partyManager.characterScripts[partyManager.charactersIndex];
                partyManager.characterScripts[partyManager.charactersIndex].StartCoroutine("LeavingSkill");
                isLeavingSkill = true;
            }
            if (Input.GetMouseButtonDown(0))
            {
                click_count++;
            }

            if (click_count > 3)
            {
                partyManager.controlList[partyManager.charactersIndex] = true;
                isgrab = false;
                gound_split_3.SetActive(false);

                character.isLeaving = false;
                nachal.isboss_pattern_h = false;
                Destroy(gameObject);
            }
            if (click_count == 1)
            {
                gound_split_1.SetActive(true);
            }

            if (click_count == 2)
            {
                gound_split_1.SetActive(false);
                gound_split_2.SetActive(true);
            }

            if (click_count == 3)
            {
                gound_split_2.SetActive(false);
                gound_split_3.SetActive(true);
            }

        }
    }

    private void FixedUpdate()
    {
        if (count < Dist)
        {
            count += Speed * Time.deltaTime;
            hand.transform.position = Vector3.MoveTowards(hand.transform.position, destination, Speed * Time.deltaTime);
            hand_sprite.color = new Color(hand_sprite.color.r, hand_sprite.color.g, hand_sprite.color.b, hand_sprite.color.a + Time.deltaTime);
        }
        else
        {

                
        }
    }


    /*
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "PlayerBullet")
        {
            Bullet bullet = collision.gameObject.GetComponent<Bullet>();
            StartCoroutine(OnDamage(bullet.damage));
            //StartCoroutine(BePushed());
            //Destroy(collision.gameObject);
        }
        if (collision.gameObject.tag == "PlayerSwing")
        {
            //Debug.Log("닿음");
            Bullet bullet = collision.gameObject.GetComponent<Bullet>();
            StartCoroutine(OnDamage(bullet.damage));
            //StartCoroutine(BePushed());
            cam.Shake(0.12f, 1);
        }

        //장판 스킬
        if (collision.gameObject.tag == "Magicline")
        {
            Bullet bullet = collision.gameObject.GetComponent<Bullet>();
            switch (bullet.value)
            {
                case 1:
                    StartCoroutine(OnDamage(bullet.damage));
                    //StartCoroutine(BePushed());
                    break;
                case 30:
                    StartCoroutine(OnDamage(bullet.damage));
                    //StartCoroutine(BePushed());
                    break;
                case 20:
                    StartCoroutine(OnDamage(bullet.damage));
                    break;
            }
        }

        if (collision.gameObject.tag == "Explosive") //폭발
        {
            Bullet bullet = collision.gameObject.GetComponent<Bullet>();
            switch (bullet.value)
            {
                case 10: //속박됨
                    //isRooted = true;
                    break;
            }
        }
    }


    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Magicline")
        {
            Bullet bullet = collision.gameObject.GetComponent<Bullet>();
            switch (bullet.value)
            {
                case 0:
                    //ADS.target = party.transform; //SJM
                    break;
                case 11:
                    //aiPath.maxSpeed = maxSpeed;
                    break;
            }
        }
    }*/

}

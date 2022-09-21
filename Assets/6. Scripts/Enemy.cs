using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Enemy : MonoBehaviour
{
    CameraController cam;
    PartyManager party; //SJM
    EventManager eventManager;
    ObjectManager objectManager;
    AudioManager audioManager;

    //밸류
    public int value;

    //밀쳐지는 양
    public float pushAmount;

    public float maxHealth;
    public float curHealth;
    public float damage;
    public float defaultSpeed;
    public float maxSpeed;

    public bool a;

    //특수상태
    public bool isRooted = false; //속박됨
    public bool isDead = false; //사망

    //특수상태 카운트
    float curRootedDelay = 0f;
    float maxRootedDelay = 5f;

    AIPath aiPath;
    AIDestinationSetter ADS;
    Rigidbody2D rigid;

    void Start()
    {
        cam = GameObject.Find("Main Camera").GetComponent<CameraController>(); //게임오브젝트를 신 안에서 찾은 후 스크립트 연결(프리펩시 필수!)
        eventManager = GameObject.Find("EventManager").GetComponent<EventManager>();
        objectManager = GameObject.Find("ObjectManager").GetComponent<ObjectManager>();
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        party = GameObject.Find("Party").GetComponent<PartyManager>();  //SJM 씬 내 파티찾기

        rigid = GetComponent<Rigidbody2D>();
        aiPath = GetComponent<AIPath>();
        //seeker.StartPath(rigid.position, target.position, )
        ADS = GetComponent<AIDestinationSetter>();
        ADS.target = party.transform;  //SJM, 타겟 지정
        maxSpeed = aiPath.maxSpeed;
        defaultSpeed = maxSpeed;

        Stats();
    }
    private void OnEnable()
    {
        isDead = false;
        curHealth = maxHealth;
        maxSpeed = defaultSpeed;
        isRooted = false;
        a = true;
    }
    private void OnDisable()
    {

    }

    void Update()
    {
        if (isRooted)
        {
            aiPath.maxSpeed = 0;
            if (curRootedDelay >= maxRootedDelay)
            {
                isRooted = false; aiPath.maxSpeed = maxSpeed;
                curRootedDelay = 0;
            }
        }

        if (a) { aiPath.canMove = true; aiPath.maxSpeed = maxSpeed; a = false; }
        Delay();
    }

    void Stats()
    {
        switch (value)
        {
            case 0:
                //maxHealth = 1f;
                //curHealth = 100f;
                //damage = 20f;
                break;
        }
    }

    void Delay()
    {
        if (isRooted)
            curRootedDelay += Time.deltaTime;
    }

    IEnumerator OnDamage(int damage)
    {
        if (curHealth > 0)
            curHealth -= damage;
        if (curHealth <= 0)
        {
            curHealth = 0;
            eventManager.curMonsterCount--;
            //Destroy(gameObject);
            isDead = true;
            switch (value)
            {
                case 9999:
                    this.gameObject.SetActive(false);
                    break;
                default:
                    objectManager.MakeObj("Soul", this.transform.position, Quaternion.Euler(0, 0, 0));
                    StartCoroutine(objectManager.ObjReturn(this.gameObject));
                    break;

            }

            //사망, 누움
            //transform.rotation = Quaternion.Euler(0, 0, -90);
            //gameObject.layer = 17;
        }
        yield return null;
    }

    IEnumerator BePushed()
    {
        if (isRooted) //속박, 사망시 불가
            yield return null;
        else
        {
            aiPath.canMove = false;
            Vector2 direction = this.transform.position - ADS.target.position;
            direction = direction.normalized * (pushAmount / 2);
            rigid.AddForce(direction, ForceMode2D.Impulse);

            yield return new WaitForSeconds(0.2f);
            rigid.velocity = Vector3.zero;
            aiPath.canMove = true;
            aiPath.maxSpeed = 1f;

            yield return new WaitForSeconds(0.1f);
            aiPath.maxSpeed = maxSpeed;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "PlayerBullet")
        {
            Bullet bullet = collision.gameObject.GetComponent<Bullet>();
            if (!isDead)
            {
                StartCoroutine(BePushed());
                StartCoroutine(OnDamage(bullet.damage));
            }
        }
        if (collision.gameObject.tag == "PlayerSwing")
        {
            Bullet bullet = collision.gameObject.GetComponent<Bullet>();
            if (!isDead)
            {
                StartCoroutine(BePushed());
                StartCoroutine(OnDamage(bullet.damage));
            }
            cam.Shake(0.12f, 1);
        }

        //장판 스킬
        if (collision.gameObject.tag == "Magicline")
        {
            Bullet bullet = collision.gameObject.GetComponent<Bullet>();
            if (!isDead)
            {
                switch (bullet.value)
                {
                    case 1:
                        StartCoroutine(BePushed());
                        break;
                    case 30:
                        StartCoroutine(BePushed());
                        break;
                }
                StartCoroutine(OnDamage(bullet.damage));
            }
        }

        if (collision.gameObject.tag == "Explosive") //폭발
        {
            Bullet bullet = collision.gameObject.GetComponent<Bullet>();
            if (!isDead)
            {
                StartCoroutine(OnDamage(bullet.damage));
                switch (bullet.value)
                {
                    case 10: //속박됨
                        isRooted = true;
                        break;
                }
            }
        }
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Magicline") //마법진
        {
            Bullet bullet = collision.gameObject.GetComponent<Bullet>();
            switch (bullet.value)
            {
                case 0: //어그로
                    ADS.target = party.characters[1].transform; //SJM
                    break;
                case 11: //느려짐
                    aiPath.maxSpeed = 4f;
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
                    ADS.target = party.transform; //SJM
                    break;
                case 11:
                    aiPath.maxSpeed = maxSpeed;
                    break;
            }
        }
    }
}

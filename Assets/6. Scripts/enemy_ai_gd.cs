using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class enemy_ai_gd : MonoBehaviour
{
    CameraController cam;
    PartyManager party; //SJM
    EventManager eventManager;

    //밸류
    public int value;

    //밀쳐지는 양
    public float pushAmount;

    public float maxHealth;
    public float curHealth;
    public float damage;
    public float defaultSpeed;
    public float maxSpeed;

    //특수상태
    public bool isRooted = false; //속박됨
    public bool isrange = false;
    //특수상태 카운트
    float curRootedDelay = 0f;
    float maxRootedDelay = 5f;

    AIPath aiPath;
    AIDestinationSetter ADS;
    Rigidbody2D rigid;

    GameObject dash_range;
    GameObject sight_range;

    private void OnEnable()
    {
        curHealth = maxHealth;
    }

    void Start()
    {
        cam = GameObject.Find("Main Camera").GetComponent<CameraController>(); //게임오브젝트를 신 안에서 찾은 후 스크립트 연결(프리펩시 필수!)
        eventManager = GameObject.Find("EventManager").GetComponent<EventManager>();
        party = GameObject.Find("Party").GetComponent<PartyManager>();  //SJM 씬 내 파티찾기

        rigid = GetComponent<Rigidbody2D>();
        aiPath = GetComponent<AIPath>();
        //seeker.StartPath(rigid.position, target.position, )
        ADS = GetComponent<AIDestinationSetter>();
        ADS.target = party.transform;  //SJM, 타겟 지정
        maxSpeed = aiPath.maxSpeed;

        Stats();
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

        Delay();
    }

    private void FixedUpdate()
    {
        StartCoroutine("range");
    }

    IEnumerator range()
    {
        if (isrange == true) yield break;

        isrange = true;

        dash_range = DetectInRange(4, "Party");
        sight_range = DetectInRange(1.2f, "Party");

        yield return new WaitForSeconds(0.2f);
        isrange = false;
        yield return 0;
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

    GameObject DetectInRange(float range, string LayerName)       // 거리 내 감지되는 오브젝트를 반환
    {
        int layermask = 1 << LayerMask.NameToLayer(LayerName);
        //Collider[] cols = Physics.OverlapSphere(transform.position + Vector3.up * high, range, layermask);
        Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position,4f, layermask);
        if (cols.Length != 0)
        {
            Debug.Log("leader : " + cols[0].gameObject.name);

            return cols[0].gameObject;
        }
        return null;

    }



    IEnumerator OnDamage(int damage)
    {
        if (curHealth > 0)
            curHealth -= damage;
        if (curHealth <= 0)
        {
            curHealth = 0;
            eventManager.curMonsterCount--;
            Destroy(gameObject);
            //사망, 누움
            //transform.rotation = Quaternion.Euler(0, 0, -90);
            //gameObject.layer = 17;
        }
        yield return null;
    }

    IEnumerator BePushed()
    {
        if (isRooted) //속박시 불가
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
            StartCoroutine(OnDamage(bullet.damage));
            StartCoroutine(BePushed());
            //Destroy(collision.gameObject);
        }
        if (collision.gameObject.tag == "PlayerSwing")
        {
            //Debug.Log("닿음");
            Bullet bullet = collision.gameObject.GetComponent<Bullet>();
            StartCoroutine(OnDamage(bullet.damage));
            StartCoroutine(BePushed());
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
                    StartCoroutine(BePushed());
                    break;
                case 30:
                    StartCoroutine(OnDamage(bullet.damage));
                    StartCoroutine(BePushed());
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
                    isRooted = true;
                    break;
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
                    ADS.target = party.characters[3].transform; //SJM
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

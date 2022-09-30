using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class nachal_ball : MonoBehaviour
{

    public float speed = 1000000000f;
    public float x, y;
    public float z=0;
    public int damage = 20;

    public bool exphase = false;
    public bool isdead = false;
    public bool isexplosion = false;
    public boss_nachal nachal;

    public GameObject party;
    public PartyManager character;
    public GameObject explosion;
    public GameObject ball;
    public GameObject ball2;
    public GameObject ball3;
    public GameObject explosion2;
    public GameObject explosion3;
    public GameObject ball_collection;      // 밖을 직접 돌리면 문제가 생겨서 이걸 돌려서 문제를 해결함

    Vector3 destination;
    Vector3 dir;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Wall")
        {
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Party")
        {
            if(character.isDashing == false) character.onDamabe_bullet_party(damage);

            StartCoroutine("dead");
        }

        if (collision.gameObject.tag == "Wall" || collision.gameObject.tag == "Border")
        {
            StartCoroutine("dead");
        }
    }

    private void OnEnable()
    {
        transform.localScale = new Vector3(0.1f, 0.1f, 1);
    }
    // Start is called before the first frame update
    void Start()
    {
        //transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, 90);
        transform.localScale = new Vector3(0.1f, 0.1f, 1);
        party = GameObject.Find("Party");  //SJM 씬 내 파티찾기
        nachal = GameObject.Find("boss_nachal").GetComponent<boss_nachal>();
        character = party.GetComponent<PartyManager>();
        if(exphase == true)
        {
            damage = 30;        // 강화시의 데미지
        }
        else
        {
            damage = 20;        // 일반 상태의 데미지
        }
    }

    // Update is called once per frame
    void Update()
    {


        if (isdead == false)
        {
            if (transform.localScale.x < 1f || transform.localScale.y < 1f)
            {

            }
            else
            {
                //move();
                //nachal.StartCoroutine("boss_delay_b");
            }

        }


    }

    private void FixedUpdate()
    {
        if(isdead == false)
        {
            if(transform.localScale.x < 1f || transform.localScale.y < 1f)
            {
                transform.localScale = new Vector3(transform.localScale.x + Time.deltaTime, transform.localScale.y + Time.deltaTime, 1);
                destination = party.transform.position;
                inclination();
            }
            else
            {
                move();
                nachal.StartCoroutine("boss_delay_b");
            }

            if(exphase == true)
            {
                //Debug.Log("aaaQ");
            }
        }

        if (exphase == true && isexplosion == false)
        {
            z += 5f;
            ball_collection.transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, z);
        }
    }

    void move()
    {
        transform.position = Vector3.MoveTowards(transform.position, dir*1000f,  Time.deltaTime + 0.1f);
        
        //transform.Translate(dir * Time.deltaTime * speed);

    }

    IEnumerator dead()
    {
        isdead = true;
        ball.SetActive(false);
        explosion.transform.position = ball.transform.position;
        explosion.SetActive(true);
        isexplosion = true;
        if (exphase == true)
        {
            explosion2.transform.position = ball2.transform.position;
            ball2.SetActive(false);
            explosion2.SetActive(true);
            explosion3.transform.position = ball3.transform.position;
            ball3.SetActive(false);
            explosion3.SetActive(true);
        }
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }

    void inclination()
    {
        //y = destination.y - transform.position.y + 0.5f;
        //x = destination.x - transform.position.x;
        dir = destination - transform.position;
        dir = dir.normalized;
    }
}



using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class nachal_ball : MonoBehaviour
{

    public float speed = 0.01f;
    public float x, y;
    public float z=0;
    public int damage = 20;

    public bool exphase = false;
    public bool isdead = false;
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Wall")
        {
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Leader")
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
        if (exphase == true)
        {
            z += 5f;
            ball_collection.transform.rotation =  Quaternion.Euler(transform.rotation.x, transform.rotation.y, z);
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

    }

    void move()
    {
        //transform.position = Vector3.MoveTowards(transform.position, destination,  Time.deltaTime + 0.1f );
        transform.Translate(new Vector3(x, y, 0) * Time.deltaTime * speed);

    }

    IEnumerator dead()
    {
        //yield return;
        isdead = true;
        ball.SetActive(false);
        explosion.SetActive(true);
        if(exphase == true)
        {
            ball2.SetActive(false);
            explosion2.SetActive(true);
            ball3.SetActive(false);
            explosion3.SetActive(true);
        }
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }

    void inclination()
    {
        y = destination.y - transform.position.y + 0.5f;
        x = destination.x - transform.position.x;
    }
}



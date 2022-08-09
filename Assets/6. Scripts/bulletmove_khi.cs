using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletmove_khi : MonoBehaviour
{
    public PartyManager partymanager;       
    public GameObject player;       // 목적지 받아옴
    public Transform target;
    public Transform make_ball;
    public float speed;         // 속도
    public float damage;        // 데미지
    SpriteRenderer sprite;
    bool start = true;      //  true는 시작전 false 는 시작후 
    public bool can_move = false;
    public float x, y;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag =="Wall")
        {
            Destroy(gameObject);
        }

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Leader")
        {
            //Debug.Log("tri");
            partymanager.onDamabe_bullet_party(damage);
            Destroy(gameObject);
            
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        partymanager = GameObject.Find("Party").GetComponent<PartyManager>();
        player = GameObject.Find("Party");
        sprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if(start)           // start = true 는 첫시작(생성 직후)
        {
            start = false;
            sprite.color = new Color(255f,255f,255f,0);
        }
        //if(sprite.color.r == 255f)        // 움직이는 조건이 투명화에서 색이 다 채워 졌을때 기준임
        if(can_move)
        {
            move();   
        }
        else
        {
            target = player.transform;      // 렉걸릴시 상태변환시 한번만 돌아가도록 해줄것
            inclination();                  // 렉유발시 상태변환시 한번만 돌아가도록 해줄것
            transform.position = make_ball.position;
            sprite.color = new Color(255f, 255f, 255f, sprite.color.a+Time.deltaTime);
        }
    }
    void inclination()
    {
        y = target.position.y - transform.position.y+0.5f;
        x = target.position.x - transform.position.x;
    }
    void move()
    {
        float step = speed * Time.deltaTime;
        //transform.position = Vector3.MoveTowards(transform.position, target.position, step);
        transform.Translate(new Vector3(x, y, 0) * Time.deltaTime*speed);

    }

    public void Dead()
    {
        Destroy(gameObject);
    }
}

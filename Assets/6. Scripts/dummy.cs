using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dummy : MonoBehaviour
{
    public Vector3 destination;     // 목적지
    public Transform player;
    Rigidbody2D rb2D;
    public int phase = 0;
    public bool rush;
    float d_x;
    float d_y;
    float regular;
    float reqular_d;

    Vector3 lastVelocity;
    // Start is called before the first frame update
    void Start()
    {
        phase = 1;
        //rb2D = gameObject.AddComponent<Rigidbody2D>();
        rb2D = GetComponent<Rigidbody2D>();
    }
    
    // Update is called once per frame
    void Update()
    {
        //rb2D.AddForce(new Vector3(1 , 0, 0), ForceMode2D.Impulse);
        Vector3 dir = transform.position - player.transform.position;                                                                             // 목적 과 현재 지점의 벡터값을 정규화 시켜줌
        dir = dir.normalized;
        float angle = Mathf.Acos(dir.x);
        angle *= (180f / 3.141592f);
        Debug.Log("angle : " + angle);
        /*
        switch (phase)
        {
            case 1:
                StartCoroutine("Stop");
                break;
            case 2:
                player_d();
                break;
            case 3:
                move();
                break;
            case 4:
                //rb2D.AddForce(new Vector3(d_x, d_y, 0) , ForceMode2D.Impulse);
                break;
            default:
                break;
        }
        */
    }
    void player_d()     // player쪽으로 돌진할때 지점을 세팅해주는 함수
    {
        //Debug.Log("aa");
        d_x = player.transform.position.x * 2 - transform.position.x;
        d_y = player.transform.position.y * 2 - transform.position.y;
        regular = Mathf.Sqrt(d_x * d_x + d_y * d_y);
        reqular_d = 1f;                                                   // 이 값을 조절 해서 player 캐릭터와의 거리를 조절
        d_x = d_x / regular;
        d_y = d_y / regular;
        destination = new Vector3((d_x * reqular_d) + player.transform.position.x, (d_y * reqular_d) +( player.transform.position.y+1), 0);           // 점대칭을 통해서 뒤로 당겨야 하는 만큼 당김
        phase = 3;
    }

    void move()         // 돌진
    {
        StopCoroutine(Stop());
        rush = true;
       
        transform.position = Vector3.MoveTowards(transform.position, destination, 5f * Time.deltaTime);
        if (Mathf.Round(transform.position.x * 100) == Mathf.Round(destination.x * 100) && Mathf.Round(transform.position.y * 100) == Mathf.Round(destination.y * 100))
        {
            rush = false;
            phase = 0;
        }
        
    }

    void reaction()
    {
        d_x = transform.position.x * 2 - player.transform.position.x;
        d_y = transform.position.y * 2 - (player.transform.position.y+1);

        d_x = d_x - transform.position.x;
        d_y = d_y - transform.position.y;

        regular = Mathf.Sqrt(d_x * d_x + d_y * d_y);                        // 정규 벡터화
        d_x = d_x / regular;
        d_y = d_y / regular;

        phase = 4;
        //Debug.Log("d_x : " + d_x + "d_y : " + d_y);
        rb2D.AddForce(new Vector3(d_x, d_y, 0)*1.5f, ForceMode2D.Impulse);
        //rb2D.AddForce(new Vector3(d_x,d_y,0)*100);
    }

    IEnumerator Stop()
    {
        phase = 0;
        yield return new WaitForSeconds(1f);
        phase = 2;
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("col : " + collision.gameObject.tag);
        //Debug.Log(""collision.gameObject.tag == "border");
        if (collision.gameObject.tag == "Leader")           // 만났다는것 충돌이 났다는것
        {
              if(rush)  // 돌진상태
            {
                phase = 0;
                Debug.Log("col");
            }

        }

        if(collision.gameObject.tag == "Border")            // 현재 지금 border로 떠서 water tag 추가 안하고 border로 사용 이후에 벽 tag를 다른것으로 바꾸게 된다면 이부분 수정해줄것
        {
            phase = 0;
            //if(true)    // 이값은 추후에 이동중이 아닌경우로 바꿔 주어야함 non a* 의미
            lastVelocity = new Vector3(d_x,d_y,0);

            var speed = lastVelocity.magnitude;
            var dir = Vector2.Reflect(lastVelocity.normalized, collision.contacts[0].normal);

            rb2D.velocity = dir * Mathf.Max(speed, 0f);
            Debug.Log("rv2d" + rb2D.velocity);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.tag == "Leader")           // 만났다는것 충돌이 났다는것
        {
            if (rush)  // 돌진상태
            {
                phase = 0;
                Debug.Log("tri");
                reaction();
            }

        }

    }
}

/*
void dot_d()        // 뒤로 가는 목적지 배정
{
    d_x = transform.position.x * 2 - player.transform.position.x;
    d_y = transform.position.y * 2 - player.transform.position.y;
    regular = Mathf.Sqrt(d_x * d_x + d_y * d_y);
    reqular_d = 1f;                                                   // 이 값을 조절해서 뒤로 가는 차징거리 조절
    d_x = d_x / regular;
    d_y = d_y / regular;
    destination = new Vector3((d_x * reqular_d) + transform.position.x, (d_y * reqular_d) + transform.position.y, 0);           // 점대칭을 통해서 뒤로 당겨야 하는 만큼 당김
    phase = 1;
}

void charge()       // 충전 뒤로 가는 정도의 움직임
{

    transform.position = Vector3.MoveTowards(transform.position,destination,1f*Time.deltaTime);
    if (Mathf.Round(transform.position.x * 100) == Mathf.Round(destination.x * 100) && Mathf.Round(transform.position.y * 100) == Mathf.Round(destination.y * 100))
    {
        phase = 2;
    }
    //transform.position = Vector3.MoveTowards(transform.position, player.position, 1f * Time.deltaTime);

}*/

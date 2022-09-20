///////////////////////////////////////////////////////////////////주 의 사 항/////////////////////////////////////////////////////////////////////////
//ctrl+F 하고 SJM 치면 내가(신준문) 추가+수정한 스크립트들 확인할 수 있음. 그리고 앞으로 다른 사람이 작성한 스크립트에 손 댈 경우엔 꼭 수정한 스크립트에 본인의 이니셜 박아주길 바람//
//그리고 Debug는 한 번 확인했으면 바로 지워주셈. 괜히 렉잡아먹음
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public enum e_state
{
    Follow,     // plyaer 추격중
    attack,     // 공격
    attack_ready, // 공격 대기? 상태
    attack_ani,   // attack 애니메이션 나오고 있는 상태
    dash,           // dash 중인 상태
    dead            // 죽은 상태
}

public enum atk_type
{
    short_range,    // 근거리
    long_range,        // 원거리
    dash,               // 돌진
}
public class Enemy_ai : MonoBehaviour
{
    CameraController cam;
    PartyManager partyManager; //SJM
    EventManager eventManager; //SJM
    int r;                   // 납치할때 character 랜덤 배열값

    public int code;        // code enemy 고유값
    //밸류
    public int value;

    //밀쳐지는 양
    public float pushAmount;

    public float defaultSpeed; //기본 속력 SJM
    public float maxSpeed; //현재 속력(ai_Path의 maxSpeed의 변수를 저장. 속력을 복구시키고 싶을 때 사용바람) SJM
    public float maxHealth;
    public float curHealth;             // 현재 체력
    public float damage;                // damage
    public float damage_skill;          // skill damage?
    public float attack_cooltime = 3;   // 공격 cooltime 
    float curReadyDelay = 0;        // 현재 딜레이
    public float maxReadyDelay = 2;        // 최대 딜레이
    float curShotDelay = 0;     // 현재 딜레이
    public float maxShotDelay = 2; //    딜레이 총량?
    public float regular_d;
    public float stab_damge_Cycle = 1f;       // 몇초에 한번 데미지가 들어갈지 
    public float stab_damage_curCycle = 0;
    public float dash_speed = 10f;          // dash speed
    public GameObject player;       //      player 받아옴
    public SpriteRenderer sprite_render;        // 상태이상 또는 
    public Animator e_ani;         // enemy animation
    public bool can_attack = false;         // canattack 이 가능해지면 공격을 수행함
    public bool attack_inrange = false;     // attack range 내에 player 존재
    public bool stop = false;           // 모든것을 멈출때 사용하게 될것 stop
    public bool make_ball = true;      // true일때 생성가능 false일때 생성불가
    public bool stab_fire = false;      // 상태이상 fire
    public bool isRooted = false; //속박됨 SJM
    public bool inrange = false; // true 일때 공격 범위내에 캐릭터가 존재함
    public bool issight_range = false; // true 일때 sightragne 내에 존재하는것
    public bool bullet_attack = false;  // shot foxball 코루틴 여러번 돌지 않게 하기 위한 것
    public bool iskidnap = false;       // race 천요괴 납치시 true 아닐시 false
    public bool sight_right = true;     // 보는 방향 우측 좌측 true 시 우측 아닐시 좌측
    public bool inrange_dash = false;   // inrange_dash  true 대쉬 범위내 false 대쉬 범위 밖
    public bool isdash = false;     // dash 중에는 true 아닐경우 false
    public bool isdash_effect = false; // isdash_effect on true off false
    public bool isdead = false;     // 죽었을경우에는 true
    public bool isrange = false;

    public GameObject fox_ball;     // 구미호 bullet prefab
    public Transform fox_ball1;     // 구미호볼 1,2,3 
    public Transform fox_ball2;
    public Transform fox_ball3;
    public Vector3 dash_target;
    public bulletmove_khi bullet1;         // 여우볼 3개
    bulletmove_khi bullet2;
    bulletmove_khi bullet3;

    AIPath aiPath;                      // 길찾기
    AIDestinationSetter ADS;            // 길찾기
    Rigidbody2D rigid;                  // 2d 물리
    Animator anim;                      // ??

    public status_abnormality stab;        // 상태이상
    Enemy_ai enemy_ai;
    int stab_type = 0;                // 어떤 상태이상의 종류인지
    float holding_time = 10f;     // 얼마나 상태이상을 지속할것이냐
    float play_time = 11f;             // 얼마나 대기중이였는지
    float angle;                        // enemy to player angle 측정시 사용 
    //특수상태 카운트
    float curRootedDelay = 0f; //현재속박딜레이 SJM
    float maxRootedDelay = 5f; //최대속박딜레이 SJM
    bool damageon = false;
    public GameObject fire_effect;

    public GameObject imae_ew;
    public GameObject imae_sn;
    public GameObject imae_e;
    public GameObject imae_w;
    public GameObject imae_s;
    public GameObject imae_n;

    public GameObject destination;          // 목적지 오브젝트
    public AIDestinationSetter ai_destination;      // 목적지 오브젝트를 넣어주어야 할곳
    public e_state enemy_state;                 // enemy 상태 

    public GameObject atk_range_image;          // 공격 범위 이미지화
    public GameObject atk_range;                // 어택 ragne 회전을 위함임
    public GameObject objparty;
    public GameObject dash_effect;                     // dash_effect dash상태일때 켜주기 위함임
    public GameObject e_ghost;                      // 유령 오브젝트
    //public GameObject dummy;
    public GameObject dash_range;
    public GameObject sight_range;
    public GameObject obj;

    BoxCollider2D boxcollider2d;
    float[] e_angle = new float[6];
    int min_engle;

    Character nap;                              // 납치한 character의 script character 정보를 여기에 저장
    void Start()
    {
        cam = GameObject.Find("Main Camera").GetComponent<CameraController>(); //게임오브젝트를 신 안에서 찾은 후 스크립트 연결(프리펩시 필수!)
        eventManager = GameObject.Find("EventManager").GetComponent<EventManager>(); //이벤트 매니저 찾기 SJM
        partyManager = GameObject.Find("Party").GetComponent<PartyManager>();  //파티(플레이어)찾기 SJM
        boxcollider2d = GetComponent<BoxCollider2D>();
        rigid = GetComponent<Rigidbody2D>();
        aiPath = GetComponent<AIPath>();
        anim = GetComponent<Animator>();
        //seeker.StartPath(rigid.position, target.position, )
        ADS = GetComponent<AIDestinationSetter>();
        ADS.target = partyManager.transform;  //타겟 지정 SJM
        maxSpeed = aiPath.maxSpeed; //aiPath내 maxspeed 복사 SJM

        Stats(); //스텟

        enemy_state = e_state.Follow;       // enemy 처음 상태 지정 follow
        can_attack = true;
        destination = GameObject.Find("Party");
        if (code >= 104 && code <= 109)         // 
        {
            ai_destination = GetComponent<AIDestinationSetter>();
            ai_destination.target = destination.transform;
        }
    }

    void Update() //SJM
    {
        Delay_normal();
        if (isRooted) //속박시 
        {
            aiPath.maxSpeed = 0; //속력 = 0
            if (curRootedDelay >= maxRootedDelay) //속박 제한시간을 초과할 경우
            {
                isRooted = false; aiPath.maxSpeed = maxSpeed; //속박 풀기 + maxSpeed 복구
                curRootedDelay = 0;
            }
        }
        
        else if (isdead == false)
        {
            //rol();      // right or left
            ani_state();        // 상태에 따른 ani 기본 state와 따로 분류 해줌
        }
        
    }

    private void FixedUpdate()
    {
        /*
        Delay_fixed();
        enemy_ai = this;
        if (!stop)
        {
            special();
            //state();
        }
        else
        {
            sprite_render.color = new Color(sprite_render.color.r, sprite_render.color.g - Time.deltaTime, sprite_render.color.b - Time.deltaTime);
        }
        dead();
        StartCoroutine("range");
        */
    }

    void ani_state()
    {

        if (sight_right == false)
        {
            obj.transform.localScale = new Vector3(-1, 1, 1);
        }
        else
        {
            obj.transform.localScale = new Vector3(1, 1, 1);
        }

        if (enemy_state == e_state.Follow)
        {
            move_true();
            //e_ani.Play("walk");
            if (inrange == true)
            {
                enemy_state = e_state.attack_ready;
            }
        }
        else if (enemy_state == e_state.attack_ready)
        {
            //e_ani.Play("idle");
            move_false();
            if (can_attack)
            {
                ready_to_attack();
            }
        }
        else if (enemy_state == e_state.attack)
        {
            if (can_attack == true)
            {
                //e_ani.Play("attack");
            }
            rigid.constraints = RigidbodyConstraints2D.FreezeAll;       // 공격중일때 enemy를 고정 시켜줌

        }
        else if (enemy_state == e_state.dash)
        {
            if (isdash_effect == false && isdash == true)
            {
                isdash_effect = true;
                dash_effect.SetActive(true);
            }
            e_dash();
            //e_ani.Play("dash");
        }


        if (inrange_dash == true && inrange == false && isdash == false)        // 돌진 범위내 이면서 동시에 공격범위 밖이여야하고 dash를 쓰지 않은 상태에서 dash로의 상태변경이 가능함
        {
            enemy_state = e_state.dash;
        }

        if (enemy_state != e_state.dash)
        {
            isdash_effect = false;
            dash_effect.SetActive(false);
        }

        if (enemy_state != e_state.attack)
        {
            rigid.constraints = RigidbodyConstraints2D.FreezeRotation;      // 일반적인 상태일떄는 enemy 고정 해제
            min_engle = 1000;                                               // attack 아닐때는 값을 초기화 시켜줌
        }
        if (!can_attack)                                    // canattack 이 true가 아니라면 can attack으로 되돌리기 위해서 돌림
        {
            cooltime_to_attack();
        }
    }
    void state()
    {
        //Debug.Log("enemy_state : " + enemy_state);
        if (iskidnap && partyManager.curCharactersCount > 1)
        {
            kid_nap();
        }
        else if (iskidnap && partyManager.curCharactersCount <= 1)
        {
            iskidnap = false;
        }
        else if (enemy_state == e_state.Follow)
        {

        }
        //else if (enemy_state == e_state.attack_ani)
        else if (enemy_state == e_state.attack)
        {
            attack_direction();         // 공격할때 어디로 해야할지 방향을 탐지
            if (can_attack && inrange && !partyManager.isDashing)            // canattack + inrange  즉 공격은 당연히 시행을 하고 inrange 공격 범위내에 player 존재시 데미지가 들어감
            {

            }
            else if (can_attack && bullet1 != null)             // 여우볼이 생성이 되어있을시 inrange를 벗어난경우 공격을 해준다.
            {
                switch (code)
                {
                    case 101:
                        StartCoroutine("shot_foxball");
                        bullet1 = null;
                        if (!inrange)
                        {
                            enemy_state = e_state.Follow;      // attack range 밖으로 player 벗어남
                            move_true();
                            //player = null;
                        }
                        break;
                }
            }

            can_attack = false;

            //if(code != 101) enemy_state = e_state.attack_ready; // 여우볼이 다공격을 갈때까지 기다려야함
            //if(code != 101) enemy_state = e_state.attack_ani;   // 공격 모션이 다끝날때까지 기다려야함
        }
        else if (enemy_state == e_state.attack_ready)
        {
            //Debug.Log("지금은 state attack_ready");
            if (can_attack)
            {
                ready_to_attack();
            }
            if (bullet1 == null)         // 여우볼 생성 x일때 움직여 주는것을 허용해줌
            {

                //Debug.Log("bullet1 null");
                switch (code)
                {
                    case 101:
                        if (!inrange)
                        {
                            enemy_state = e_state.Follow;      // attack range 밖으로 player 벗어남
                            move_true();
                            //player = null;
                        }
                        break;
                }
            }
            else
            {
                Debug.Log("bullet1 :" + bullet1);
            }

        }
        else if (enemy_state != e_state.attack_ready)       // ready 상태일때 할일?
        {

        }
        if (!can_attack)                                    // canattack 이 true가 아니라면 can attack으로 되돌리기 위해서 돌림
        {
            cooltime_to_attack();
        }

        // 상태이상 이부분은 따로히 script화를 시키기 위해서 분리를 해둠

        stab_control();
        if (stab_fire == true)
        {
            fire_effect.SetActive(true);
        }

    }

    IEnumerator range()
    {
        if (isrange == true) yield break;

        isrange = true;

        //dash_range = DetectInRange(4, "Party");
        if(dash_range != null)
        {
            inrange_dash = true;
        }
        else
        {
            inrange_dash = false;
        }
        //sight_range = DetectInRange(1.2f, "Party");
        if (sight_range != null)
        {
            if (enemy_ai.enemy_state != e_state.dash)
            {
                enemy_ai.enemy_state = e_state.attack;      // attack range 안에 player 감지시 
            }
            enemy_ai.issight_range = true;
            enemy_ai.player = sight_range;
            enemy_ai.move_false();
            if (enemy_ai.code == 101)                        // 구미호는 attak range와 sight rage가 같아서 따로 이부분만 sight range에 넣어줌
            {
                enemy_ai.inrange = true;
            }
        }
        else
        {
            enemy_ai.issight_range = false;
            enemy_ai.inrange = false;
        }
        yield return new WaitForSeconds(0.2f);
        isrange = false;
        yield return 0;
    }

    GameObject DetectInRange(float range, string LayerName)       // 거리 내 감지되는 오브젝트를 반환
    {
        int layermask = 1 << LayerMask.NameToLayer(LayerName);
        Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, 4f, layermask);      // 중심지, 반지름 ,layermask 어떤걸 탐지?
        if (cols.Length != 0)
        {
            return cols[0].gameObject;
        }
        return null;

    }


    void e_dash()       // e_state dash 일때 사용될 함수
    {
        //ADS.target = partyManager.transform;
        if (isdash == false)
        {
            isdash = true;
            dash_target = partyManager.transform.position;
            move_false();       // a* 움직임 봉쇄
        }
        transform.position = Vector3.MoveTowards(transform.position, dash_target, 5f * Time.deltaTime);

        if (Mathf.Round(transform.position.x * 100) == Mathf.Round(dash_target.x * 100) && Mathf.Round(transform.position.y * 100) == Mathf.Round(dash_target.y * 100))        // 목적지 도착
        {
            move_true();        // a* 움직임 활성화
            if (issight_range == true)
            {
                Debug.Log("ready");
                enemy_state = e_state.attack_ready;
            }
            else if (issight_range == false)
            {
                Debug.Log("follow");
                enemy_state = e_state.Follow;
            }
        }
    }
    public void enemy_atk()
    {
        switch (code)                    // 여기서 코드에 따라서 다른 공격 시행
        {
            case 0:                                                     // 경기
                partyManager.StartCoroutine("onDamage_party", damage);
                break;
            case 2:                                                    // 짐조
                partyManager.StartCoroutine("onDamage_party", damage);
                break;
            case 5:                                                     // ?? 알수없음
                partyManager.StartCoroutine("onDamage_party", damage);
                break;
            case 100:                                                   // 이매망량 1페
                partyManager.StartCoroutine("onDamage_party", damage);
                break;
            case 101:                                                 // 구미호
                make_ball = true;
                StartCoroutine("shot_foxball");
                bullet1 = null;
                break;
            case 104:                                                  // 이매망량 2페 ew
                partyManager.StartCoroutine("onDamage_party", damage);
                break;
            case 105:                                                  // 이매망량 2페 ns
                partyManager.StartCoroutine("onDamage_party", damage);
                break;
            case 106:                                                   // 이매망량 3페 e
                partyManager.StartCoroutine("onDamage_party", damage);
                break;
            case 107:                                                   // 이매망량 3페 w 
                partyManager.StartCoroutine("onDamage_party", damage);
                break;
            case 108:                                                   // 이매망량 3페 n
                partyManager.StartCoroutine("onDamage_party", damage);
                break;
            case 109:                                                   // 이매망량 3페 s
                partyManager.StartCoroutine("onDamage_party", damage);
                break;
            case 1000:
                //partyManager.get_enemy(this);                           // 자폭맨 -> 폭죽
                partyManager.StartCoroutine("onDamage_party", damage);
                break;
            case 1001:                                                  // 천 요괴
                partyManager.StartCoroutine("onDamage_party", damage);
                iskidnap = true;
                break;
        }
    }

    public void dash_crash()       // dash 중 chracter(leader)와 부딫혔을때 발동될 함수 뒤로 물러나는 물리값 넣어 줄것
    {
        isRooted = true;
        if (sight_right)     // 우측을 보고 있을때
        {
            rigid.AddForce(new Vector3(-5, 0, 0));
        }
        else                // 좌측을 보고 있을때
        {
            rigid.AddForce(new Vector3(5, 0, 0));
        }

    }
    void e_to_p_angle()
    {
        Vector3 dir = transform.position - player.transform.position;                                                                             // 목적 과 현재 지점의 벡터값을 정규화 시켜줌
        dir = dir.normalized;
        angle = Mathf.Acos(dir.x);
        angle *= (180f / 3.141592f);

        // 0우,45우상,135우상,180좌,225좌하,315우하
        e_angle[0] = 0;
        e_angle[1] = 45;
        e_angle[2] = 135;
        e_angle[3] = 180;
        e_angle[4] = 225;
        e_angle[5] = 315;
        for (int i = 0; i < e_angle.Length - 1; i++)
        {
            e_angle[i] -= angle;
            Mathf.Abs(e_angle[i]);
            if (min_engle != 1000)
            {
                if (e_angle[i] < min_engle)
                {
                    min_engle = i;
                }
            }
            else
            {
                Debug.Log("e_angle.length : " + e_angle.Length);
                Debug.Log("i : " + i);
                if (e_angle[i] < e_angle[min_engle])
                {
                    min_engle = i;
                }
            }
        }
    }
    void rol()
    {
        if (ADS.target.position.x - transform.position.x > 0)        //  목적지가 우측에 존재
        {
            sight_right = true;
        }
        else                                                        // 목적지가 좌측에 존재
        {
            sight_right = false;
        }
    }

    void attack_direction()     // 어느 방향으로 공격을 해야할지 탐지
    {
        //atk_range_image.transform.rotation.z = Mathf.Atan2(player.transform.position.y - transform.position.y, player.transform.position.x - transform.position.x);
        player = GameObject.Find("Party");

        //atk_range.transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(player.transform.position.y - transform.position.y + 1f, player.transform.position.x - transform.position.x) * 57.2958f);
    }

    void make_foxball()
    {
        if (bullet1 == null && bullet2 == null && bullet3 == null)     // 여우볼 없을때만 생성할것
        {
            bullet1 = Instantiate(fox_ball, fox_ball1.position, Quaternion.identity).GetComponent<bulletmove_khi>();
            bullet2 = Instantiate(fox_ball, fox_ball2.position, Quaternion.identity).GetComponent<bulletmove_khi>();
            bullet3 = Instantiate(fox_ball, fox_ball3.position, Quaternion.identity).GetComponent<bulletmove_khi>();
            bullet1.make_ball = fox_ball1;                              // ball 소환 위치를 ball1,2,3 각각 정해줌
            bullet2.make_ball = fox_ball2;
            bullet3.make_ball = fox_ball3;
        }
    }
    void special()
    {
        switch (code)
        {
            case 1000:         // 자폭맨 => 폭죽
                if (curHealth <= (maxHealth / 10))
                {
                    //partyManager.get_enemy(this);
                    if (attack_inrange)
                    {
                        StartCoroutine("self_destruct");
                    }
                }
                break;
            case 101:         // 구미호
                if ((enemy_state == e_state.attack_ready) && make_ball)         // attack_ready 일때 make_ball on
                {
                    make_ball = false;
                    make_foxball();
                }
                break;
        }
    }

    public void move_false()
    {
        aiPath.canMove = false;
    }

    public void move_true()
    {
        aiPath.canMove = true;
    }

    public void kid_nap()
    {
        objparty = GameObject.Find("Party");
        //Debug.Log("objparty : " + objparty.transform.position);
        r = Random.Range(0, partyManager.curCharactersCount);
        if (partyManager.aliveList[r] == true && partyManager.characterLists[r].tag == "Leader")                                                     // leader 납치
        {
            partyManager.characterLists[r].transform.position = new Vector3(transform.position.x, transform.position.y - 2, transform.position.z);      // 납치한 player의 위치를 몬스터 아래에 옮김
            partyManager.controlList[r] = false;                                                                                                    // control 불가
            nap = partyManager.characterLists[r].GetComponent<Character>();
            partyManager.priCharIndex = -1; //퀵버튼 리셋
            nap.isLeaving = true;


            if (partyManager.curCharactersCount > 1)
            {
                partyManager.controlList[partyManager.charactersIndex] = false;
                partyManager.priCharIndex = -1; //퀵버튼 리셋

                while (!partyManager.controlList[partyManager.charactersIndex])
                {
                    if (partyManager.charactersIndex != partyManager.hasCharactersCount - 1)
                        partyManager.charactersIndex++;
                    else if (partyManager.charactersIndex == partyManager.hasCharactersCount - 1)
                        partyManager.charactersIndex = 0;
                }

            }
            //nap.StartCoroutine("LeavingSkill");
        }
        //partyManager.aliveList[r] = false;
        // 멤버를 납치하는 경우
        /*
        if(partyManager.aliveList[r] == true && partyManager.characterLists[r].tag == "Member")                                                     // 살아 있지만 동시에 leader가 아닌 캐릭터가 member 여야함
        {
            partyManager.characterLists[r].transform.position = new Vector3(transform.position.x,transform.position.y-2,transform.position.z);      // 납치한 player의 위치를 몬스터 아래에 옮김
            partyManager.controlList[r] = false;                                                                                                    // control 불가
            nap = partyManager.characterLists[r].GetComponent<Character>();
            nap.isLeaving = true;
            //partyManager.aliveList[r] = false;
        }
        */
        Vector3 dir = transform.position - objparty.transform.position;                                                                             // 목적 과 현재 지점의 벡터값을 정규화 시켜줌
        dir = dir.normalized;
        move_false();
        regular_d = 2f;
        //dummy.transform.position =transform.position + dir * regular_d;
        transform.position = Vector3.MoveTowards(transform.position, transform.position + dir * regular_d, 5f * Time.deltaTime);
    }
    public void dead()
    {
        if (curHealth <= 0)
        {
            enemy_state = e_state.dead;
            isdead = true;
            rigid.constraints = RigidbodyConstraints2D.FreezeAll;   // 사망시 시체가 움직이지 않게 해줌
            boxcollider2d.isTrigger = true;
            dash_effect.SetActive(false);
            if (code == 101)   // fox일 때 남은 fox 삭제 해줘야됨
            {
                bullet1.Dead();
                bullet2.Dead();
                bullet3.Dead();
            }
            else if (code == 1001)
            {
                Debug.Log("nap" + nap);
                if (nap != null)
                {
                    Debug.Log("돌았슈");
                    iskidnap = false;                       // 납치중 아님
                    nap.isLeaving = false;                  // leaving 상태 아님
                    partyManager.controlList[r] = true;     // control 허용
                    //partyManager.aliveList[r] = true;       //
                }
            }
            curHealth = 0;
            //Destroy(gameObject);
            e_ani.Play("dead");
            e_ghost.SetActive(true);
            //사망, 누움
            //transform.rotation = Quaternion.Euler(0, 0, -90);
            //gameObject.layer = 17;
        }
    }
    void cooltime_to_attack()
    {
        if (maxShotDelay <= curShotDelay)
        {
            curShotDelay = 0f;
            can_attack = true;
        }
        else
        {
            curShotDelay += Time.deltaTime;
        }
    }

    void ready_to_attack()
    {
        if (maxReadyDelay <= curReadyDelay)
        {
            curReadyDelay = 0f;
            enemy_state = e_state.attack;
        }
        else
        {
            curReadyDelay += Time.deltaTime;
        }
    }
    void Stats()
    {
        /*
        switch (value)
        {
            case 0:
                maxHealth = 100f;
                curHealth = 100f;
                damage = 0f;
                break;
        }*/
    }

    public void shut_down()
    {
        gameObject.SetActive(false);
    }
    IEnumerator shot_foxball()       // 여우구슬발사
    {
        if (bullet1 != null) bullet1.can_move = true;
        yield return new WaitForSeconds(0.2f);
        bullet2.can_move = true;
        yield return new WaitForSeconds(0.2f);
        bullet3.can_move = true;
        //enemy_state = e_state.attack_ready;
        enemy_state = e_state.attack_ani;
        yield return null;
    }
    IEnumerator atkr_image_time()       // 공격 범위 표시
    {
        yield return new WaitForSeconds(0.2f);
        atk_range_image.SetActive(false);
    }
    IEnumerator self_destruct()     // 자폭
    {

        stop = true;
        yield return new WaitForSeconds(1.4f);
        anim.SetTrigger("death");
        //partyManager.StartCoroutine("onDamage_bomb_party");
        partyManager.StartCoroutine("onDamage_party", damage_skill);
        yield return null;
    }
    IEnumerator attack_cool()
    {
        yield return new WaitForSeconds(3f);
        can_attack = false;
    }
    IEnumerator OnDamage(int damage)
    {
        play_time = 0f;         // play time 0f로 만들어줘서 일단 돌수 있게 만들어줌
        stab_type = 2;          // case number를 만들어서 다양한? 즉 종류 구별 가능하게 만들어 줄것 느려지게 하는거

        if (curHealth > 0)
            curHealth -= damage;
        //enemy_state = e_state.attack_ready;
        curShotDelay = 0;       // 공격시간 초기화
        if (curHealth <= 0)
        {
            if (code == 100)
            {

                Instantiate(imae_ew, new Vector3(transform.position.x + 3, transform.position.y, transform.position.z), Quaternion.identity);
                Instantiate(imae_sn, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);

            }
            else if (code == 105)
            {
                Instantiate(imae_e, new Vector3(transform.position.x + 3, transform.position.y, transform.position.z), Quaternion.identity);
                Instantiate(imae_w, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
            }
            else if (code == 104)
            {
                Instantiate(imae_s, new Vector3(transform.position.x + 3, transform.position.y, transform.position.z), Quaternion.identity);
                Instantiate(imae_n, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
            }
            else if (code == 101)   // fox일 때 남은 fox 삭제 해줘야됨
            {
                bullet1.Dead();
                bullet2.Dead();
                bullet3.Dead();
            }
            else if (code == 1001)
            {
                //Debug.Log("nap" + nap);
                if (nap != null)
                {
                    //Debug.Log("돌았슈");
                    iskidnap = false;                       // 납치중 아님
                    nap.isLeaving = false;                  // leaving 상태 아님
                    partyManager.controlList[r] = true;     // control 허용
                    //partyManager.aliveList[r] = true;       //
                }
            }
            curHealth = 0;

            eventManager.curMonsterCount--; //씬 안에 몬스터 카운팅 -1 SJM

            //Destroy(gameObject);
            //사망, 누움
            //transform.rotation = Quaternion.Euler(0, 0, -90);
            //gameObject.layer = 17;
        }
        yield return null;
    }

    IEnumerator BePushed()
    {
        if (isRooted) //속박시 이 함수 사용X SJM
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

    public void stab_control()          // 특정 stab 상태가 존재 한다면 그에 맞게 돌아감
    {
        switch (stab_type)
        {
            case 1:
                stab_behavior();            // 느려지는거
                break;
            case 2:
                stab_dot();                 // 불
                break;
            default:
                break;
        }
    }
    public void stab_behavior()     // 행동 제한 상태이상
    {

        if (holding_time >= play_time)
        {
            aiPath.maxSpeed = 1.5f;     // 느려지게 하는것
            play_time += Time.deltaTime;
            //sprite_render.color = new Color(sprite_render.color.r - Time.deltaTime, sprite_render.color.g - Time.deltaTime, sprite_render.color.b + Time.deltaTime);
            sprite_render.color = new Color(0, 0, 255);
        }
        else
        {
            sprite_render.color = new Color(255, 255, 255);
            aiPath.maxSpeed = maxSpeed;     // 원래 속도
            return;
        }
    }

    public void stab_dot()          // 도트데미지 상태이상
    {
        if (holding_time >= play_time)
        {
            stab_fire = true;
            play_time += Time.deltaTime;
            stab_damage();
        }
        else
        {
            stab_fire = false;
            return;
        }
    }

    void Delay_normal() //!모든 일반 딜레이 카운트(+=Time.deltaTime)은 여기에 넣으셈. fixedDeltaTime은 Delay_fixed()으로! SJM
    {
        if (isRooted)
            curRootedDelay += Time.deltaTime;
    }

    void Delay_fixed() //!모든 fixed 딜레이 카운트(+=Time.fixedDeltaTime)은 여기에 넣으셈. DeltaTime은 Delay_normal로! SJM
    {
        //참고로 fixedUpdate는 성능을 크게 잡아먹음. 그러니 이동같은 '섬세한 요소' 외에는 Update를 사용하길 권장함
    }

    public void stab_damage()     // 데미지 들어가는 부분
    {
        if (stab_damge_Cycle >= stab_damage_curCycle)
        {
            stab_damage_curCycle += Time.deltaTime;
        }
        else
        {
            stab_damage_curCycle = 0;
            curHealth = curHealth - 10;
        }
    }

    public IEnumerator Shake(float _amount, float _duration)
    {
        float timer = 0;
        while (timer <= _duration)
        {
            transform.localPosition = (Vector3)Random.insideUnitCircle * _amount + this.transform.position;

            timer += Time.deltaTime;
            yield return null;
        }
        //transform.localPosition = originPos;

    }

    void OnTriggerEnter2D(Collider2D collision) //피격 다양화 + 수정 SJM
    {
        Bullet bullet = collision.gameObject.GetComponent<Bullet>();

        if (collision.gameObject.tag == "PlayerBullet") //투사체
        {
            StartCoroutine(OnDamage(bullet.damage));
            StartCoroutine(BePushed());
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.tag == "PlayerSwing") //근접공격
        {
            StartCoroutine(OnDamage(bullet.damage));
            StartCoroutine(BePushed());
            cam.Shake(0.12f, 1);
        }

        //장판 스킬
        if (collision.gameObject.tag == "Magicline") //마법진
        {
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
            switch (bullet.value)
            {
                case 10: //속박됨
                    isRooted = true;
                    break;
            }
        }
    }

    void OnTriggerStay2D(Collider2D collision) //Crystal SJM
    {
        Bullet bullet = collision.gameObject.GetComponent<Bullet>();

        if (collision.gameObject.tag == "Magicline") //마법진
        {
            switch (bullet.value)
            {
                case 0: //어그로
                    ADS.target = partyManager.characters[3].transform; //SJM
                    break;
                case 11: //느려짐
                    aiPath.maxSpeed = 4f;
                    break;
            }
        }
    }

    void OnTriggerExit2D(Collider2D collision) //Crystal SJM
    {
        Bullet bullet = collision.gameObject.GetComponent<Bullet>();

        if (collision.gameObject.tag == "Magicline")
        {
            switch (bullet.value)
            {
                case 0:
                    ADS.target = partyManager.transform; //SJM
                    break;
                case 11:
                    aiPath.maxSpeed = maxSpeed;
                    break;
            }
        }
    }
}
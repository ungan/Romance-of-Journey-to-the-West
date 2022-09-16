using System.Collections;
using UnityEngine;

public class boss_nachal : MonoBehaviour
{
    public float radiius = 2.0f;
    public float maxlightning = 0.25f;
    public float curlightning = 0;
    public float curpatton_time = 0;
    public float maxpatton_time = 5f;
    public float curstoptime_patton = 0;
    public float maxstoptime_patton = 10f;

    public int numchild = 12;
    public int lightning_count=0;
    public int lightning_count_1 = 0;
    public int lightning_count_2 = 0;
    public int lightning_chess_downtoup_count = 0;      // downtoup count
    public int lightning_chess_uptodown_count = 0;      // uptodown count
    public int c = 0;
    public int n = 0;
    public bool can_lightning = true;
    public bool can_lightning_ready = false;     // false면 preview true면 진짜 번개
    public bool can_lightning_ready_1 = false;
    public bool can_lightning_ready_2 = false;
    public bool ex_phase = false;
    public bool isLight_circle_button= false;      // circle 시간차o 존재 공격
    public bool isLight_circle_button_ex_phase = false; // circle 시간차x로 공격
    public bool isLight_chess_button_downtoup = false;      // 체스모양 위에서 아래로 시간차 존재
    public bool isLight_chess_button_uptodown = false;      // 체스모양 위에서 아래로 시간차 존재
    public bool isLight_chess_button_vertical = false;     //  체스모양 위아래 동시공격
    public bool isLight_chess_button_ex_phase = false;      // 체스모양 전체 폭파 시간차 존재x
    public bool isLight_vertical_downtoup_button = false;        // 일자모양 아래에서 위로
    public bool isLight_vertical_uptodown_button = false;       // 일자 모양 위에서 아래로
    public bool isLight_vertical_synthesize_button = false;     // 일자 모양 위 그리고 아래 동시
    public bool isLight_horizontal_lefttoright_button = false;  // 일자 모양 좌에서 우로
    public bool isLight_horizontal_righttoleft_button = false;  // 일자 모양 우에서 좌로
    public bool isLight_horizontal_righttoleft_synthesize_button = false;       // 일자모양 좌우 동시
    public bool isbutton = false;
    public bool isgod_hand_button = false;              // god hand 소환
    public bool isgod_hand_grab = false;                // god hand grab

    public bool isLight_chess = false;
    public bool isboss_patton = false;
    public bool isLight_chess_uptodown = false;     // 코루틴 돌때 사용 
    public bool isLight_chess_downtoup = false;     // 코루틴 돌때 사용
    public bool isLight_vertical_downtoup = false;      // 코루틴 돌때 사용
    public bool isLight_vertical_uptodown = false;      // 코루틴 돌때 사용
    public bool isLight_horizontal_lefttoright = false;      // 직선 좌에서 우로 코루틴
    public bool isLight_horizontal_righttoleft = false;      // 직선 우에서 좌로 코루틴
    public bool isboss_delay = false;

    public bool iscooltime_to_lightning_co = false; // prewview -> lightning cooltime corutine 사용시 사용됨
    public bool isLight_chess_exphase = false;        //
    public bool reset = true;   // reset 용
    public bool reset_1 = true;
    public bool reset_2 = true;
    public bool isLight_chess_uptodown_reset = true;        //  체스모양 위에서 아래로 리셋
    public bool isLight_chess_downtoup_reset = true;        // 체스모양 아래서 위로 리셋
    public bool isLight_vertical_uptodown_reset = true;     // 직선 위서 아래 리셋 
    public bool isLight_vertical_downtoup_reset = true;     // 직선 아래서 위로 리셋
    public GameObject lightning;
    public GameObject lightning_preview;
    public GameObject go;
    public GameObject startpoint_left_up;        // 시작 지점 왼쪽 위   
    public GameObject endpoint_right_down;       // 아래 지점 왼쪽 아래
    public GameObject god_hand;
    public GameObject god_hand_grab_gob;
    public GameObject partyManager;

    // Start is called before the first frame update
    void Start()
    {
        partyManager = GameObject.Find("Party");  //파티(플레이어)찾기 SJM
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        //boss_patton();
        if(isLight_circle_button == true)       // 일반 lightning_circle 이 나감
        {
            Light_circle_button();
        }
        if(isLight_circle_button_ex_phase == true)      // ex_phase 강화 페이즈 lightning_circle이 나감
        {
            Light_circle_button_ex_phase();
        }
        if (isLight_chess_button_downtoup == true)      // chess 모양아래서 위로
        {
            Light_chess_button_downtoup();
        }
        if (isLight_chess_button_uptodown == true)      // chess 모양 위에서 아래로
        {
            Light_chess_button_uptodown();
        }
        if (isLight_chess_button_ex_phase == true)      // chess 모양강화 한번에 떨어지기
        {
            Light_chess_exphase_button();
        }
        if( isLight_chess_button_vertical == true)      // 체스 모양 위아래 동시에 나오기
        {
            Light_chess_button_downtoup();
            Light_chess_button_uptodown();
        }
        if(isLight_vertical_downtoup_button == true)        // 직선 아래에서 위로
        {
            Light_vertical_downtoup_button();
        }
        if(isLight_vertical_uptodown_button == true)        // 직선 위에서 아래로
        {
            Light_vertical_uptodown_button();
        }
        if(isLight_vertical_synthesize_button == true)      // 번개 직선 위에래 동시
        {
            Light_vertical_downtoup_button();
            Light_vertical_uptodown_button();
        }
        if(isLight_horizontal_lefttoright_button == true)   // 번개 직선 왼쪽에서 오른쪽으로
        {
            Light_horizontal_lefttoright_button();
        }
        if(isLight_horizontal_righttoleft_button == true)   // 번개 직선 오른쪽에서 왼쪽으로
        {
            Light_horizontal_righttoleft_button();
        }
        if(isLight_horizontal_righttoleft_synthesize_button == true)    // 번개 직선 오른쪽 왼쪽 동시
        {
            Light_horizontal_lefttoright_button();
            Light_horizontal_righttoleft_button();
        }
        if(isgod_hand_button == true)
        {
            god_hand_button();
        }
        if(isgod_hand_grab == true)
        {
            god_hand_grab();
        }
        if(isbutton == true)
        {
            button();
            isbutton = false;
        }
    }

    public void god_hand_grab()
    {
        
        Instantiate(god_hand, partyManager.transform.position , Quaternion.identity);
        isgod_hand_grab = false;
    }

    public void god_hand_button()
    {
        float x = Random.Range(-4, 4);
        float y = Random.Range(-4, 4);
        Vector3 v = new Vector3(transform.position.x + x, transform.position.y + y, 0);
        Instantiate(god_hand, v, Quaternion.identity);
        isgod_hand_button = false;
    }

    public void button()
    {

        Instantiate(god_hand, transform.position, Quaternion.identity);
        
    }

    public void boss_patton()
    {
        if (isboss_patton) return;
        isboss_patton = true;
        n = Random.Range(1, 12);
        patton();
    }

    IEnumerator boss_delay()
    {
        yield return null;
        if (isboss_delay) yield break;        // 코루틴이 실행중일경우에는 두개이상의 코루틴이 실행 불가능하도록 만들어줌
        isboss_delay = true;                  // 이게 코루틴 여러번 실행중인경우를 막아줌

        Debug.Log("boss_delay");
        yield return new WaitForSeconds(maxpatton_time);
        isboss_patton = false;
        isboss_delay = false;
        StopCoroutine("boss_delay");
    }

    void patton()
    {
        switch(n)
        {
            case 1:
                isLight_circle_button = true;
                break;
            case 2:
                isLight_circle_button_ex_phase = true;
                break;
            case 3:
                isLight_chess_button_downtoup = true;
                break;
            case 4:
                isLight_chess_button_uptodown = true;
                break;
            case 5:
                isLight_chess_button_vertical = true;
                break;
            case 6:
                isLight_chess_button_ex_phase = true;
                break;
            case 7:
                isLight_vertical_downtoup_button = true;
                break;
            case 8:
                isLight_vertical_uptodown_button = true;
                break;
            case 9:
                isLight_vertical_synthesize_button = true;
                break;
            case 10:
                isLight_horizontal_lefttoright_button = true;
                break;
            case 11:
                isLight_horizontal_righttoleft_button = true;
                break;
            case 12:
                isLight_horizontal_righttoleft_synthesize_button = true;
                break;
            default:
                break;
        }   


    }

    public void Palm_button()
    {
        StartCoroutine("Palm");
    }

    /*
    IEnumerator Palm()
    {
        yield return 0;
    }*/

    public void Light_horizontal_righttoleft_button()
    {
        if (reset_2 == true)
        {
            can_lightning_ready_2 = false;                  // ready 초기화
            reset_2 = false;                                // reset 초기화
            maxlightning = 0.25f;                           // 번개 내려치는 시간 초기화
            lightning_count_2 = 0;                          // 번개개수 카운트 초기화
        }
        StartCoroutine("Light_horizontal_righttoleft");     // 코루틴 시작
    }


    IEnumerator Light_horizontal_righttoleft()
    {
        yield return null;
        if (isLight_horizontal_righttoleft) yield break;        // 코루틴이 실행중일경우에는 두개이상의 코루틴이 실행 불가능하도록 만들어줌
        isLight_horizontal_righttoleft = true;                  // 이게 코루틴 여러번 실행중인경우를 막아줌

        while (lightning_count_2 < (int)(endpoint_right_down.transform.position.x - startpoint_left_up.transform.position.x))       // 좌우 이동이라서 x 값으로 거리 계싼
        {
            for (int i = 0; i < (int)(startpoint_left_up.transform.position.y - endpoint_right_down.transform.position.y) * 0.5; i++)       // y값으로 계산  
            {
                if (can_lightning_ready_1 == false)                                                                                         // ready= false -. preview true lightning
                {
                    Instantiate(lightning_preview, transform.position + (new Vector3(endpoint_right_down.transform.position.x - lightning_count_2, startpoint_left_up.transform.position.y - i * 2, 0)), Quaternion.identity);
                }
                else if (can_lightning_ready_1 == true)
                {
                    Instantiate(lightning, transform.position + (new Vector3(endpoint_right_down.transform.position.x - lightning_count_2, startpoint_left_up.transform.position.y - i * 2, 0)), Quaternion.identity);
                }
            }
            lightning_count_2++;
            yield return new WaitForSeconds(maxlightning);                  // 다음 번개 나올때까지 실행 시간 이를 조절해 실행 시간을 조절 해줄수 있음
            if (can_lightning_ready_1 == false && lightning_count_2 == (int)(endpoint_right_down.transform.position.x - startpoint_left_up.transform.position.x))       // preview 에서 real lightning으로 넘어갈때 쓰이는 것
            {
                can_lightning_ready_1 = true;
                lightning_count_2 = 0;
            }
        }
        reset_2 = true;                                                     // 리셋 초기화
        isLight_horizontal_righttoleft = false;                             // 코루틴 초기화
        isLight_horizontal_righttoleft_button = false;                      // 버튼 초기화
        isLight_horizontal_righttoleft_synthesize_button = false;           // synthesize 인경우 초기화
        isboss_patton = false;
        StartCoroutine("boss_delay");
        StopCoroutine("Light_horizontal_righttoleft");

    }

    public void Light_horizontal_lefttoright_button()
    {
        if (reset_1 == true)
        {
            can_lightning_ready_1 = false;
            reset_1 = false;
            maxlightning = 0.25f;                           // 번개 내려치는 시간 초기화
            lightning_count_1 = 0;
        }
        StartCoroutine("Light_horizontal_lefttoright");
    }

    IEnumerator Light_horizontal_lefttoright()
    {
        yield return null;
        if (isLight_horizontal_lefttoright) yield break;
        isLight_horizontal_lefttoright = true;

        while (lightning_count_1 < (int)(endpoint_right_down.transform.position.x - startpoint_left_up.transform.position.x))
        {
            //for (int i = 0; i < (int)((endpoint_right_down.transform.position.x - startpoint_left_up.transform.position.x) * 0.5); i++)
            for (int i = 0; i < (int)(startpoint_left_up.transform.position.y - endpoint_right_down.transform.position.y)*0.5; i++)
            {
                if (can_lightning_ready_1 == false)
                {
                    Instantiate(lightning_preview, transform.position + (new Vector3(startpoint_left_up.transform.position.x + lightning_count_1, startpoint_left_up.transform.position.y- i*2-1, 0)), Quaternion.identity);
                }
                else if (can_lightning_ready_1 == true)
                {
                    Instantiate(lightning, transform.position + (new Vector3(startpoint_left_up.transform.position.x + lightning_count_1, startpoint_left_up.transform.position.y - i*2-1, 0)), Quaternion.identity);
                }
            }
            lightning_count_1++;
            yield return new WaitForSeconds(maxlightning);
            if (can_lightning_ready_1 == false && lightning_count_1 == (int)(endpoint_right_down.transform.position.x - startpoint_left_up.transform.position.x))
            {
                can_lightning_ready_1 = true;
                lightning_count_1 = 0;
            }
        }
        reset_1 = true;
        isLight_horizontal_lefttoright = false;
        isLight_horizontal_lefttoright_button = false;
        isLight_horizontal_righttoleft_synthesize_button = false;
        StartCoroutine("boss_delay");
        StopCoroutine("Light_horizontal_lefttoright");

    }
    public void Light_vertical_uptodown_button()
    {
        if (isLight_vertical_uptodown_reset == true)
        {
            can_lightning_ready_1 = false;
            isLight_vertical_uptodown_reset = false;
            maxlightning = 0.25f;                           // 번개 내려치는 시간 초기화
            lightning_count_1 = 0;
        }
        StartCoroutine("Light_vertical_uptodown");
    }

    IEnumerator Light_vertical_uptodown()
    {
        yield return null;
        if (isLight_vertical_uptodown) yield break;
        isLight_vertical_uptodown = true;


        while (lightning_count_1 < (int)(startpoint_left_up.transform.position.y - endpoint_right_down.transform.position.y))
        {
            for (int i = 0; i < (int)((endpoint_right_down.transform.position.x - startpoint_left_up.transform.position.x) * 0.5); i++)
            {
                if (can_lightning_ready_1 == false)
                {
                    Instantiate(lightning_preview, transform.position + (new Vector3(i * 2 + startpoint_left_up.transform.position.x+1, startpoint_left_up.transform.position.y-lightning_count_1, 0)), Quaternion.identity);
                }
                else if (can_lightning_ready_1 == true)
                {
                    Instantiate(lightning, transform.position + (new Vector3(i * 2 + startpoint_left_up.transform.position.x+1, startpoint_left_up.transform.position.y- lightning_count_1, 0)), Quaternion.identity);
                }
            }
            lightning_count_1++;
            yield return new WaitForSeconds(maxlightning);
            if (can_lightning_ready_1 == false && lightning_count_1 == (int)(startpoint_left_up.transform.position.y - endpoint_right_down.transform.position.y))
            {
                can_lightning_ready_1 = true;
                lightning_count_1 = 0;
            }
        }
        isLight_vertical_uptodown_reset = true;
        isLight_vertical_uptodown = false;
        isLight_vertical_uptodown_button = false;
        isLight_vertical_synthesize_button = false;
        StartCoroutine("boss_delay");
        StopCoroutine("Light_vertical_uptodown");
    }

    public void Light_vertical_downtoup_button()
    {
        if (isLight_vertical_downtoup_reset == true)
        {
            can_lightning_ready_2 = false;
            isLight_vertical_downtoup_reset = false;
            maxlightning = 0.25f;                           // 번개 내려치는 시간 초기화
            lightning_count_2 = 0;
        }
        StartCoroutine("Light_vertical_downtoup");
    }


    IEnumerator Light_vertical_downtoup()        // 아래 -> 위
    {
        yield return null;
        if (isLight_vertical_downtoup) yield break;
        isLight_vertical_downtoup = true;

        
        while(lightning_count_2 < (int)(startpoint_left_up.transform.position.y - endpoint_right_down.transform.position.y))
        {
            for (int i = 0; i < (int)((endpoint_right_down.transform.position.x - startpoint_left_up.transform.position.x) * 0.5); i++)
            {
                if (can_lightning_ready_2 == false)
                {
                    Instantiate(lightning_preview, transform.position + (new Vector3(i * 2 + startpoint_left_up.transform.position.x, lightning_count_2 + endpoint_right_down.transform.position.y, 0)), Quaternion.identity);
                }
                else if (can_lightning_ready_2 == true)
                {
                    Instantiate(lightning, transform.position + (new Vector3(i * 2 + startpoint_left_up.transform.position.x, lightning_count_2 + endpoint_right_down.transform.position.y, 0)), Quaternion.identity);
                }
            }
            lightning_count_2++;
            yield return new WaitForSeconds(maxlightning);
            if (can_lightning_ready_2 == false && lightning_count_2 == (int)(startpoint_left_up.transform.position.y - endpoint_right_down.transform.position.y))
            {
                
                can_lightning_ready_2 = true;
                lightning_count_2 = 0;
            }
        }

        isLight_vertical_downtoup_reset = true;
        isLight_vertical_downtoup = false;
        isLight_vertical_downtoup_button = false;
        isLight_vertical_synthesize_button = false;
        StartCoroutine("boss_delay");
        StopCoroutine("Light_chess_downtoup");
    }
    /*
      IEnumerator Light_vertical_downup()        // 아래 -> 위
    {
        for (int i = 0; i < (int)(endpoint_right_down.transform.position.x - startpoint_left_up.transform.position.x); i++)
        {
            //Instantiate(lightning_preview, transform.position + (new Vector3(i * 2 - 6, lightning_count * 1 - 7, 0)), Quaternion.identity);
            Instantiate(lightning, transform.position + (new Vector3(i * 2 + startpoint_left_up.transform.position.x, lightning_count + endpoint_right_down.transform.position.y, 0)), Quaternion.identity);
        }
        lightning_count++;
        if (can_lightning_ready == false &&lightning_count == (int)(startpoint_left_up.transform.position.y - endpoint_right_down.transform.position.y))
        {
            yield return new WaitForSeconds(maxlightning);
            can_lightning_ready = true;
        }

        if (can_lightning_ready == true && lightning_count == (int)(startpoint_left_up.transform.position.y - endpoint_right_down.transform.position.y))
        {
            reset = true;
        }

        yield return 0;
    }
     */

    
    public void Light_chess_button_downtoup()
    {
        if (isLight_chess_downtoup_reset == true)
        {
            isLight_chess_downtoup_reset = false;
            maxlightning = 0.25f;                           // 번개 내려치는 시간 초기화
            lightning_chess_downtoup_count = 0;
        }
        StartCoroutine("Light_chess_downtoup");
    }

    public void Light_chess_button_uptodown()
    {
        if (isLight_chess_uptodown_reset == true)
        {
            isLight_chess_uptodown_reset = false;
            maxlightning = 0.25f;                           // 번개 내려치는 시간 초기화
            lightning_chess_uptodown_count = 0;
        }
        StartCoroutine("Light_chess_uptodown");
    }

    IEnumerator Light_chess_uptodown()
    {
        yield return null;
        if (isLight_chess_uptodown) yield break;
        isLight_chess_uptodown = true;
        while (lightning_chess_uptodown_count < (int)(startpoint_left_up.transform.position.y - endpoint_right_down.transform.position.y))
        {
            for (int i = 0; i < (int)((endpoint_right_down.transform.position.x - startpoint_left_up.transform.position.x) / 2); i++)
            {
                if (lightning_chess_uptodown_count % 2 == 0)
                {
                    Instantiate(lightning, transform.position + (new Vector3(i * 2 + startpoint_left_up.transform.position.x, (lightning_chess_uptodown_count * -1) + startpoint_left_up.transform.position.y, 0)), Quaternion.identity);
                    //Instantiate(lightning_preview, transform.position + (new Vector3(i*2-7, lightning_count*1-7, 0)), Quaternion.identity);
                }
                else if (lightning_chess_uptodown_count % 2 != 0)
                {
                    if (i == (int)(endpoint_right_down.transform.position.x - startpoint_left_up.transform.position.x - 1)) break;
                    //Instantiate(lightning_preview, transform.position + (new Vector3(i * 2 - 6, lightning_count * 1 - 7, 0)), Quaternion.identity);
                    Instantiate(lightning, transform.position + (new Vector3(i * 2 + startpoint_left_up.transform.position.x + 1, (lightning_chess_uptodown_count * -1) + startpoint_left_up.transform.position.y, 0)), Quaternion.identity);
                }
            }
            lightning_chess_uptodown_count++;  // 세로줄
            yield return new WaitForSeconds(maxlightning);
        }
        isLight_chess_button_uptodown = false;      // 위에서 아래 버튼 활성화
        isLight_chess_uptodown = false;             //  코루틴 진입 활성화
        isLight_chess_uptodown_reset = true;        // 리셋 활성화
        isLight_chess_button_vertical = false;      // 가로 세로 동시일때를 위해서 false값
        StartCoroutine("boss_delay");
        StopCoroutine("Light_chess_uptodown");
    }

    IEnumerator Light_chess_downtoup()
    {
        yield return null;
        if (isLight_chess_downtoup) yield break;
        isLight_chess_downtoup = true;
        while (lightning_chess_downtoup_count < (int)(startpoint_left_up.transform.position.y - endpoint_right_down.transform.position.y))
        {
            for (int i = 0; i < (int)((endpoint_right_down.transform.position.x - startpoint_left_up.transform.position.x)/2); i++)
            {
                if (lightning_chess_downtoup_count % 2 == 0)
                {
                    if (i == (int)(endpoint_right_down.transform.position.x - startpoint_left_up.transform.position.x - 1)) break;
                    Instantiate(lightning, transform.position + (new Vector3(i * 2 + startpoint_left_up.transform.position.x+1, lightning_chess_downtoup_count + endpoint_right_down.transform.position.y, 0)), Quaternion.identity);
                    //Instantiate(lightning_preview, transform.position + (new Vector3(i*2-7, lightning_count*1-7, 0)), Quaternion.identity);
                }
                else if (lightning_chess_downtoup_count % 2 != 0)
                {
                    //Instantiate(lightning_preview, transform.position + (new Vector3(i * 2 - 6, lightning_count * 1 - 7, 0)), Quaternion.identity);
                    Instantiate(lightning, transform.position + (new Vector3(i * 2 + startpoint_left_up.transform.position.x, lightning_chess_downtoup_count + endpoint_right_down.transform.position.y, 0)), Quaternion.identity);
                }
            }
            lightning_chess_downtoup_count++;  // 세로줄
            yield return new WaitForSeconds(maxlightning);
        }
        isLight_chess_button_downtoup = false;
        isLight_chess_downtoup = false;
        isLight_chess_downtoup_reset = true;
        isLight_chess_button_vertical = false;
        StartCoroutine("boss_delay");
        StopCoroutine("Light_chess_downtoup");
    }

    public void Light_chess_exphase_button()        // 체스 강화 공격 
    {
        if (reset == true)
        {
            reset = false;
            isLight_chess_exphase = false;
            can_lightning_ready = false;
            lightning_count = 0;
        }
        StartCoroutine("Light_chess_exphase");      // 체스 강화 공격 
    }
    IEnumerator Light_chess_exphase()
    {
        yield return null;
        if (isLight_chess_exphase) yield break;
        isLight_chess_exphase = true;

        while (lightning_count < (int)(startpoint_left_up.transform.position.y - endpoint_right_down.transform.position.y))
        {
            if(can_lightning == true)
            {
                for (int i = 0; i < (int)((endpoint_right_down.transform.position.x - startpoint_left_up.transform.position.x) / 2); i++)
                {
                    if (lightning_count % 2 == 0)
                    {
                        if(can_lightning_ready == false)
                        {
                            Instantiate(lightning_preview, transform.position + (new Vector3(i * 2 + startpoint_left_up.transform.position.x, lightning_count + endpoint_right_down.transform.position.y, 0)), Quaternion.identity);
                        }
                        else if(can_lightning_ready == true)
                        {
                            Instantiate(lightning, transform.position + (new Vector3(i * 2 + startpoint_left_up.transform.position.x, lightning_count + endpoint_right_down.transform.position.y, 0)), Quaternion.identity);
                        }
                    }
                    else if (lightning_count % 2 != 0)
                    {
                        if (i == (int)(endpoint_right_down.transform.position.x - startpoint_left_up.transform.position.x - 1)) break;
                        if (can_lightning_ready == false)
                        {
                            Instantiate(lightning_preview, transform.position + (new Vector3(i * 2 + startpoint_left_up.transform.position.x + 1, lightning_count + endpoint_right_down.transform.position.y, 0)), Quaternion.identity);
                        }
                        else if (can_lightning_ready == true)
                        {
                            Instantiate(lightning, transform.position + (new Vector3(i * 2 + startpoint_left_up.transform.position.x + 1, lightning_count + endpoint_right_down.transform.position.y, 0)), Quaternion.identity);
                        }
                    }
                }
                lightning_count++;  // 세로줄
                if(can_lightning_ready == false && lightning_count == (int)(startpoint_left_up.transform.position.y - endpoint_right_down.transform.position.y))
                {
                    Debug.Log("a : ");
                    lightning_count = 0;
                    can_lightning = false;
                    can_lightning_ready = true;
                    maxlightning = 2f;                      // 다음 번개가 나오는 타이밍 조절
                }
            }
            else if(can_lightning == false)
            {
                yield return new WaitForSeconds(maxlightning);
                can_lightning = true;
            }
        }
        isLight_chess_button_ex_phase = false;
        reset = true;
        StartCoroutine("boss_delay");
        StopCoroutine("Light_chess_exphase");
    }

    public void Light_circle_button()       // 일반 lightning circle 
    {
        if(reset == true)                   // 처음 시작할때 기본 값들 설정
        {
            reset = false;                  // reset false로 설정하여 한번만 돌게 해주고 light circle이 끝날때 다시 true값으로 되돌려줌
            radiius = 2f;                   // radiius =2f는 다시 시작원을 2f 로 만들어서 초기화
            maxlightning = 0.25f;           // 다음 번개가 나오는 타이밍 조절
            ex_phase = false;               // ex_phase fase
            can_lightning_ready = false;     // ready
            lightning_count = 0;            //  번개 줄 0으로 초기화
        }

        if(lightning_count<=5)              // 번개 줄이 5개 이하로 까지만 돌아가도록 설정
        {
            Light_Circle();
        }
    }

    public void Light_circle_button_ex_phase()      // 강화 light_circle
    {
        if(reset == true)                           // 기본값들 초기화
        {
            reset = false;                          // reset false로 설정하여 한번만 돌게 해주고 light circle이 끝날때 다시 true값으로 되돌려줌
            radiius = 2f;                           // radiius =2f는 다시 시작원을 2f 로 만들어서 초기화
            maxlightning = 1f;                      // 다음 번개가 나오는 타이밍 조절
            ex_phase = true;                        // ex_phase fase
            can_lightning_ready = false;            // ready
            lightning_count = 0;                    //  번개 줄 0으로 초기화
        }

        if (lightning_count <= 5)
        {
            Light_Circle();
        }
    }

    public void Light_Circle()     // 전기 파지직
    {

            if (can_lightning == true)              // 전기 파지직 가능
            {
                numchild = (int)(radiius * 3.14 * 2);       // 원하나의 바깥에 몇개의 번개가 들어가야하는지 계산 추후에 번개 크기가 달라진다면 이부분을 조절해줄것

                for (int i = 0; i < numchild; i++)      // 번개 소환 하는 for문
                {
                    float angle = i * (Mathf.PI * 2.0f) / numchild;     // 번개 개수를 360으로 나눠서 지들이 들어가 각도 계산
                    if(can_lightning_ready == true)                      // true면 번개 false 면 번개 예고
                    {
                        Instantiate(lightning, transform.position + (new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0)) * radiius, Quaternion.identity);   // 번개소환  cos sin + position으로 소환될 좌표 계산
                    }
                    else if(can_lightning_ready == false)
                    {
                        Instantiate(lightning_preview, transform.position + (new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0)) * radiius, Quaternion.identity);
                    }
                }
                radiius += 2.0f;                // 다음 소환에 반지름이 더 켜져서 바깥쪽에 번개를 소환해주는것 줄과 줄사이의 번개의 텀을 줄여주고 싶다면 이부분을 조절해줄것
                                                // 추가적으로 만약 번개 크기 조절시 이것도 같이 조절해줘야 거리 조절이 가능함
                lightning_count++;              // lightining_count는 소환 한줄 때마다 하나씩 늘어남 이것의 최대 크기를 조절 해줄경우
                
                if (ex_phase == true)           // ex_phase 강화 페이즈 일시 돌아가는 코드 강화 페이즈 시에 번개 소환에 텀이 존재 하지 않으며 예고뒤 바로 소환
                {
                    if (lightning_count == 6)
                    {
                        can_lightning = false;
                        maxlightning = 2f;      // 강화 페이즈 시 preview 이후에 소환되는 번개 텀을 조절해주는 부분 늘리면 preview 이후에 번개 늦게 소환  
                    }
                }
                else if (ex_phase == false)
                {
                    can_lightning = false;      // 일반 페이즈 시에 각 줄별 번개가 소환되도록 해줌
                }
                
                //can_lightning = false;      // 일반 페이즈 시에 각 줄별 번개가 소환되도록 해줌
            }
            else if(can_lightning == false)
            {
                cooltime_to_lightning();        // 텀 조절
            }

            if(can_lightning_ready == false && lightning_count == 6)     // preview 끝났음
            {
                can_lightning_ready = true;                             // for 문 instance에서 진짜 번개 소환으로 바꿔줌
                lightning_count = 0;                                    // 번개 소환 카운트 0로 소환 가능하게 해줌
                radiius = 2.0f;                                         // 반지름 초기화
            }

        if(can_lightning_ready == true && lightning_count == 6)
        {
            isLight_circle_button_ex_phase = false;
            isLight_circle_button = false;
            reset = true;
            StartCoroutine("boss_delay");
        }

    }
    void cooltime_to_lightning()
    {
        if (maxlightning <= curlightning)               // maxlightning이 curlightning보다 작아짓는 시점이오면 타이머가 끝나고 can_lightning true
        {
            curlightning = 0f;
            can_lightning = true;
        }
        else
        {
            curlightning += Time.deltaTime;
        }
    }
}

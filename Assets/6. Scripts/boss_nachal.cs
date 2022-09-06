using UnityEngine;

public class boss_nachal : MonoBehaviour
{
    public float radiius = 2.0f;
    public float maxlightning = 0.25f;
    public float curlightning = 0;

    public int numchild = 12;
    public int lightning_count=0;

    public bool can_lightning = true;
    public bool can_lightning_ready = false;     // false면 preview true면 진짜 번개
    public bool ex_phase = false;
    public bool isLight_circle_button= false;      // true로 만들어 주면 
    public bool isLight_circle_button_ex_phase = false; // 
    public bool isLight_chess_button = false;
    public bool reset = true;   // reset 용
    public GameObject lightning;
    public GameObject lightning_preview;
    public GameObject go;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if(isLight_circle_button == true)       // 일반 lightning_circle 이 나감
        {
            Light_circle_button();
        }
        if(isLight_circle_button_ex_phase == true)      // ex_phase 강화 페이즈 lightning_circle이 나감
        {
            Light_circle_button_ex_phase();
        }
        if(isLight_chess_button == true)
        {
            Light_chess_button();
        }
    }
    public void Light_chess_button()
    {
        if(reset == true)
        {
            reset = false;
            lightning_count = 0;
        }
        if(lightning_count<=15)
        {
            Light_chess();
        }
    }

    public void Light_chess()
    {
        for(int i=0; i<10;i++)
        {
            if(lightning_count%2 == 0)
            {
                Instantiate(lightning_preview, transform.position + (new Vector3(i*2-7, lightning_count*1-7, 0)), Quaternion.identity);
            }
            else if(lightning_count % 2 != 0)
            {
                if (i == 9) break;
                Instantiate(lightning_preview, transform.position + (new Vector3(i * 2 - 6, lightning_count * 1 - 7, 0)), Quaternion.identity);
            }
        }
        lightning_count++;
        if (lightning_count == 15)
        {
            isLight_chess_button = false;
            reset = true;
        }
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

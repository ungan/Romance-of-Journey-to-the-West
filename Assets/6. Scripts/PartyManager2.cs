using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyManager2 : MonoBehaviour
{
    [Header("매니저 & 관리")]
    float fixedDeltaTime; //fixedDeltaTime(시간)
    public GameManager gameManager; //게임메니저
    public ChoicePanel cPanel; //c판넬
    CameraController cam; //카메라 컨트롤

    //이동
    [Header("이동")]
    public float speed = 10f; //이동 스피드

    [Header("캐릭터 리스트")]
    public Character[] charactersList; //장착 가능한 캐릭터 목록
    public Character[] equipCharactersList; //장착한 캐릭터 목록

    //파티 내 포지션
    [Header("포지션")]
    public GameObject[] Position; //캐릭터 포지션 지정

    [Header("캐릭터 관리")]
    public Character equipCharacter; //장착 중인 캐릭터
    public int charactersIndex = 0; //현재 캐릭터 번호
    public int priCharIndex = -1; //이전 캐릭터 번호

    public int hasCharactersCount = 1; //현재 가진 최대 캐릭터 카운트
    public int curCharactersCount = 1; //현재 살아있는 캐릭터 카운트
    public int[] list;
    public bool[] aliveList; //현재 살아있는 캐릭터리스트
    public bool[] controlList; //현재 컨트롤 가능한 캐릭터리스트(파티에서 잠시 이탈했을 경우 false값)

    //조작
    bool fire2;
    bool iDown;
    bool iStay;
    bool sDown1;
    bool sDown2;
    bool sDown3;
    bool sDown4;
    bool qDown;
    bool fStay;
    bool fUp;
    bool t;
    bool tUp;

    //애니메이션
    [Header("bool값 관리")]
    public bool isRunning; //달릴 경우
    public bool isDashing; //대시할 경우
    public bool isSticking; //움직일 수 없을 경우
    public bool isAttacking; //공격할 경우

    //컨트롤
    [Header("스왑 관리")]
    public bool canSwap; //true시 스왑 가능
    bool swapDelayDone; //스왑 딜레이가 끝날 경우

    //딜레이
    float maxSwapDelay = 0.2f;
    float curSwapDelay = 0;
    float maxAliveDelay = 0.4f;
    float curAliveDelay = 0;
    float maxUseDelay = 0.4f;
    float curUseDelay = 0;

    //거리 계산
    float range;
    Vector3 tracePos;

    Rigidbody2D rigid;
    BoxCollider2D boxCol;
    float h, v;
    float mWheel;

    Vector3 dirXY;
    [Header("X, Y")]
    public float x;
    public float y;

    [Header("인터랙션")]
    public GameObject nearObject; //주변 오브젝트

    //표시
    public GameObject playerPosition; //플레이어 위치

    //여의주
    [Header("아이템")]
    public int curDragonBall = 0; //현재 가진 드래곤볼
    public int maxDragonBall = 3; //최대 가진 드래곤볼

    //기타
    bool fCheck = false;

    private void Awake()
    {
        cam = GameObject.Find("Main Camera").GetComponent<CameraController>(); //게임오브젝트를 신 안에서 찾은 후 스크립트 연결(프리펩시 필수!)
        rigid = GetComponent<Rigidbody2D>();
        this.fixedDeltaTime = Time.fixedDeltaTime;
        boxCol = GetComponent<BoxCollider2D>();
    }
    void Start()
    {
        for (int i = 0; i < equipCharactersList.Length; i++)
        {
            for(int l = 0; l < charactersList.Length; l++)
            {
                if (equipCharactersList[i] != null) break;

                if (charactersList[l].gameObject.activeSelf)
                {
                    equipCharactersList[i] = charactersList[l];
                    list[i] = equipCharactersList[i].value;
                    aliveList[i] = true;
                    controlList[i] = true;
                }
            }
        }

        for(int i = 0; i < equipCharactersList.Length; i++)
        {
            if(equipCharactersList[i] == null)
                list[i] = -1;
        }
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
        Interaction(); //상호작용
        Use(); //사용
        Swap(); //캐릭터 스왑
        Passive(); //패시브
        Delay(); //딜레이++
        GameOver(); //게임오버
    }

    void FixedUpdate()
    {
        if (gameObject.layer != 16)
        {
            Move();
            SlowDownStay();
        }
    }

    void GetInput()
    {
        if (!isDashing)
        {
            h = Input.GetAxisRaw("Horizontal");
            v = Input.GetAxisRaw("Vertical");
        }

        fire2 = Input.GetButtonDown("Fire2");
        mWheel = Input.GetAxis("Mouse ScrollWheel");
        iDown = Input.GetButtonDown("Interaction");
        iStay = Input.GetButton("Interaction");
        sDown1 = Input.GetButtonDown("Swap1");
        sDown2 = Input.GetButtonDown("Swap2");
        sDown3 = Input.GetButtonDown("Swap3");
        sDown4 = Input.GetButtonDown("Swap4");
        qDown = Input.GetButtonDown("PreChange");
        fStay = Input.GetButton("Use");
        fUp = Input.GetButtonUp("Use");
        t = cPanel.t;
        tUp = cPanel.tUp;
    }

    void Move()
    {
        //isRunning 구분
        if (dirXY.x != 0 || dirXY.y != 0 || dirXY.z != 0) isRunning = true;
        else isRunning = false;

        //방향키 이동
        dirXY = Vector3.right * h + Vector3.up * v;
        dirXY.Normalize();


        if (curCharactersCount > 0) //파티 살아있음
        {
            if (isSticking == true || !controlList[charactersIndex]) //굳었을 때
                rigid.velocity = Vector2.zero;
            else if (isRunning && !isDashing) //대시하지 않고 달리는 상태
            {
                rigid.velocity = new Vector2(dirXY.x, dirXY.y) * speed;
            }
            else if (isDashing) //대시 상태
            {
                rigid.velocity = new Vector2(dirXY.x, dirXY.y) * (speed * 4f);
            }
        }
        else if (curCharactersCount <= 0) //파티 터짐
        {
            this.gameObject.layer = 12;
            rigid.velocity = Vector3.zero;
        }

        //포지션 보정
        x = dirXY.x; y = dirXY.y;
        if (x == 0 && y == 0) { playerPosition.transform.localPosition = new Vector3(0, 0, 0); }//센터
        else if (x >= 1 && y == 0) { playerPosition.transform.localPosition = new Vector3(-0.08f, 0, 0); }//좌
        else if (x <= -1 && y == 0) { playerPosition.transform.localPosition = new Vector3(0.08f, 0, 0); }//우
        else if (x == 0 && y >= 1) { playerPosition.transform.localPosition = new Vector3(0, -0.08f, 0); }//상
        else if (x == 0 && y <= -1) { playerPosition.transform.localPosition = new Vector3(0, 0.08f, 0); }//하
        else if (x >= 0.5 && y >= 0.5) { playerPosition.transform.localPosition = new Vector3(-0.05f, -0.05f, 0); }//우상
        else if (x <= -0.5 && y >= 0.5) { playerPosition.transform.localPosition = new Vector3(0.05f, -0.05f, 0); }//좌상
        else if (x >= 0.5 && y <= -0.5) { playerPosition.transform.localPosition = new Vector3(-0.05f, 0.05f, 0); }//우하
        else if (x <= -0.5 && y <= -0.5) { playerPosition.transform.localPosition = new Vector3(0.05f, 0.05f, 0); }//좌하
    }

    void Swap()
    {
        tracePos = equipCharactersList[charactersIndex].transform.position; //장착한 캐릭터의 위치
        range = Vector3.Distance(tracePos, transform.position); //캐릭터 위치과 지정된 포지션 위치와의 거리

        //캐릭터 스왑(1, 2, 3, 4)
        if ((sDown1 || (cPanel.tUp && cPanel.cNum == 1)) && (hasCharactersCount < 1 || !aliveList[0] || !controlList[0]))
            return;
        if ((sDown2 || (cPanel.tUp && cPanel.cNum == 2)) && (hasCharactersCount < 2 || !aliveList[1] || !controlList[1]))
            return;
        if ((sDown3 || (cPanel.tUp && cPanel.cNum == 3)) && (hasCharactersCount < 3 || !aliveList[2] || !controlList[2]))
            return;
        if (sDown4 && (hasCharactersCount < 4 || !aliveList[3] || !controlList[3]))
            return;

        StartCoroutine("CanYouSwap");
        if (range < 0.5f && swapDelayDone) { canSwap = true; swapDelayDone = false; }

        if ((sDown1 || (cPanel.tUp && cPanel.cNum == 1)) && canSwap) { priCharIndex = charactersIndex; charactersIndex = 0; canSwap = false; curSwapDelay = 0; }
        else if ((sDown2 || (cPanel.tUp && cPanel.cNum == 2)) && canSwap) { priCharIndex = charactersIndex; charactersIndex = 1; canSwap = false; curSwapDelay = 0; }
        else if ((sDown3 || (cPanel.tUp && cPanel.cNum == 3)) && canSwap) { priCharIndex = charactersIndex; charactersIndex = 2; canSwap = false; curSwapDelay = 0; }
        else if (sDown4 && canSwap) { priCharIndex = charactersIndex; charactersIndex = 3; canSwap = false; curSwapDelay = 0; }

        //Q변경
        if (qDown && priCharIndex != -1 && canSwap)
        {
            int ci;
            ci = charactersIndex;
            charactersIndex = priCharIndex;
            priCharIndex = ci;
            curSwapDelay = 0;
            canSwap = false;
        }

        //휠변경
        if (curCharactersCount <= 1 && (mWheel > 0 || mWheel < 0)) //1이고 휠
        {
            //휠 안써짐
        }
        if (curCharactersCount > 1) //현재 인원이 1보다 클 때 + 휠
        {

            if (mWheel > 0 && canSwap) //휠업
            {
                do
                {
                    priCharIndex = charactersIndex;
                    if (charactersIndex != 0)
                        charactersIndex--;
                    else if (charactersIndex == 0)
                        charactersIndex = hasCharactersCount - 1;
                } while (!controlList[charactersIndex]);
                canSwap = false;
                curSwapDelay = 0;
            }
            if (mWheel < 0 && canSwap) //휠다운
            {
                do
                {
                    priCharIndex = charactersIndex;
                    if (charactersIndex != hasCharactersCount - 1)
                        charactersIndex++;
                    else if (charactersIndex == hasCharactersCount - 1)
                        charactersIndex = 0;
                } while (!controlList[charactersIndex]);
                canSwap = false;
                curSwapDelay = 0;
            }
        }

        if (!canSwap)
            equipCharacter = equipCharactersList[charactersIndex];
    }

    void Passive()
    {
        for (int i = 0; i < 4; i++)
        {
            if (aliveList[i] == true)
            {
                switch (list[i])
                {
                    case 0: //1번 패시브
                        break;
                    case 1: //2번 패시브

                        break;
                    case 2: //3번 패시브

                        break;
                    case 3: //4번 패시브

                        break;
                }
            }
        }
    }

    void Delay()
    {
        curSwapDelay += Time.deltaTime;
    }

    void Interaction()
    {
        //캐릭터 장착
        if (iDown && nearObject != null)
        {
            if (nearObject.tag == "Character" && hasCharactersCount < 4) //캐릭터 파티 편입
            {
                Item item = nearObject.GetComponent<Item>();
                int characterIndex = item.value;
                equipCharactersList[hasCharactersCount] = charactersList[characterIndex];
                list[hasCharactersCount] = characterIndex;
                equipCharactersList[hasCharactersCount].gameObject.SetActive(true);
                equipCharactersList[hasCharactersCount] = equipCharactersList[hasCharactersCount].GetComponent<Character>();
                hasCharactersCount++;
                curCharactersCount++;
                aliveList[hasCharactersCount - 1] = true;
                controlList[hasCharactersCount - 1] = true;
                Destroy(nearObject);
            }
            //아이템 먹기
            else if (nearObject.tag == "Item")
            {
                Item item = nearObject.GetComponent<Item>();
                switch (item.value)
                {
                    case 5:
                        if (curDragonBall < maxDragonBall)
                        {
                            curDragonBall++;
                            Destroy(nearObject);
                        }
                        break;
                }
            }
        }

        if (iStay && nearObject != null)
        {
            if (nearObject != null && nearObject.tag == "Downed") //살리기
            {
                curAliveDelay += Time.deltaTime;

                if (curAliveDelay >= maxAliveDelay)
                {
                    Character character = nearObject.GetComponent<Character>();
                    StartCoroutine(character.Alive(character.posValue));
                }
            }
        }
        else
        {
            curAliveDelay = 0;
        }
    }

    void Use()
    {
        if (fStay && fCheck == false)
        {
            Character curChar = equipCharactersList[charactersIndex];
            if (curDragonBall > 0 && curChar.curHealth < curChar.maxHealth)
            {
                curUseDelay += Time.deltaTime;

                if (curUseDelay >= maxUseDelay)
                {
                    StartCoroutine(curChar.Heal(curChar.maxHealth)); //전체회복

                    curDragonBall--;
                    curUseDelay = 0;
                    fCheck = true;
                }
            }
        }

        if (fUp)
        {
            fCheck = false;
        }
    }

    void SlowDownStay()
    {
        if (t && cam.dying == false)
        {
            cam.t = true;
            Time.timeScale = 0.2f;
            Time.fixedDeltaTime = this.fixedDeltaTime * Time.timeScale;
            gameManager.Choice();
        }
        else if (!t && cam.dying == false)
        {
            cam.t = false;
            Time.timeScale = 1f;
            Time.fixedDeltaTime = this.fixedDeltaTime * Time.timeScale;
            gameManager.ChoiceOut();
        }
    }


    public void CanSwapFalse()
    {
        curSwapDelay = 0;
        canSwap = false;
    }

    public IEnumerator Dash() //대시
    {
        isDashing = true;
        this.gameObject.layer = 12;

        yield return new WaitForSeconds(0.15f);
        isDashing = false;
        isSticking = true;

        yield return new WaitForSeconds(0.2f);
        isSticking = false;
        this.gameObject.layer = 10;
    }

    IEnumerator Push()
    {
        Vector2 direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - this.transform.position;
        direction = direction.normalized * 1;
        rigid.AddForce(direction, ForceMode2D.Impulse);

        yield return new WaitForSeconds(0.1f);
        rigid.velocity = Vector3.zero;
    }

    public IEnumerator SlowDown(float i, bool d)
    {
        gameManager.ChoiceOut();
        if (i > 0)
        {
            Time.timeScale = 0.2f;
            Time.fixedDeltaTime = this.fixedDeltaTime * Time.timeScale;
            if (d == true) cam.dying = true;
            yield return new WaitForSeconds(i);
            Time.timeScale = 1f;
            Time.fixedDeltaTime = this.fixedDeltaTime * Time.timeScale;
            cam.dying = false;
        }
        else
        {
            Time.timeScale = 1f;
            Time.fixedDeltaTime = this.fixedDeltaTime * Time.timeScale;
        }
    }

    IEnumerator CanYouSwap()
    {
        yield return new WaitForSeconds(0.2f);
        swapDelayDone = true;
    }

    void GameOver()
    {
        if (curCharactersCount <= 0)
        {
            gameManager.GameOver();
            gameObject.layer = 16;
        }
    }

    public IEnumerator onDamage_party(int e_damage)
    {
        equipCharactersList[charactersIndex].StartCoroutine("OnDamage", e_damage);
        yield return new WaitForSeconds(0.1f);
    }

    public IEnumerator onDamage_bomb_party()
    {
        //Debug.Log("bomb!");
        equipCharactersList[charactersIndex].StartCoroutine("OnDamage_bomb");
        yield return new WaitForSeconds(0.1f);
    }

    public void onDamabe_bullet_party(float damage)
    {
        equipCharactersList[charactersIndex].OnDamage_bullet(damage);
    }

    void OnTriggerStay2D(Collider2D collision)
    {

        if (collision.gameObject.tag == "Character" || collision.gameObject.tag == "Item" || collision.gameObject.tag == "Downed")
        {
            nearObject = collision.gameObject;
        }

    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Character" || collision.gameObject.tag == "Item" || collision.gameObject.tag == "Downed")
            nearObject = null;
    }
}

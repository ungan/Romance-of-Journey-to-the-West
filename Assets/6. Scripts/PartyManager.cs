using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyManager : MonoBehaviour
{
    float fixedDeltaTime;
    public GameManager gameManager;
    public ChoicePanel cPanel;
    CameraController cam;

    //이동
    public float speed = 10f;

    public GameObject[] characters;
    public GameObject[] characterLists;
    public Character[] characterScripts;

    //파티 내 포지션
    public GameObject[] Position;

    public Crosshair crosshair;
    public int charactersIndex = 0;
    public int priCharIndex = -1;

    public int hasCharactersCount = 1;
    public int curCharactersCount = 1;
    int equipCharacterIndex;
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
    public bool isRunning;
    public bool isDashing;
    public bool isStopping;
    public bool isAttacking;

    //컨트롤
    public bool canSwap; //true시 스왑 가능
    bool swapDelayDone;

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
    public float x, y;

    public GameObject nearObject;
    public GameObject equipCharacter;

    //표시
    public GameObject playerPosition;

    //여의주
    public int curDragonBall = 0;
    public int maxDragonBall = 3;

    //기타
    bool fCheck = false;

    //레벨
    public int maxLV = 50; //최대 레벨
    public int curLV = 0; //현재 레벨
    public int maxEXP = 500; //최대 경험치
    public int curEXP = 0; //현재 경험치 

    void Awake()
    {
        cam = GameObject.Find("Main Camera").GetComponent<CameraController>(); //게임오브젝트를 신 안에서 찾은 후 스크립트 연결(프리펩시 필수!)
        rigid = GetComponent<Rigidbody2D>();
        this.fixedDeltaTime = Time.fixedDeltaTime;
        boxCol = GetComponent<BoxCollider2D>();
    }

    void Start()
    {
        for (int index = 1; index < 4; index++)
            list[index] = -1;
    }

    void Update()
    {
        GetInput();
        Interaction(); //상호작용
        Use(); //사용
        Swap(); //캐릭터 스왑
        Passive(); //패시브
        EXPControl(); //업그레이드
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
        fire2 = Input.GetButtonDown("Fire2");
        if (!isDashing)
        {
            h = Input.GetAxisRaw("Horizontal");
            v = Input.GetAxisRaw("Vertical");
        }
        mWheel = Input.GetAxis("Mouse ScrollWheel");
        iDown = Input.GetButtonDown("Interaction");
        iStay = Input.GetButton("Interaction");
        if (!gameManager.isUpgrading)
        {
            sDown1 = Input.GetButtonDown("Swap1");
            sDown2 = Input.GetButtonDown("Swap2");
            sDown3 = Input.GetButtonDown("Swap3");
            sDown4 = Input.GetButtonDown("Swap4");
        }
        qDown = Input.GetButtonDown("PreChange");
        fStay = Input.GetButton("Use");
        fUp = Input.GetButtonUp("Use");
        t = cPanel.t;
        tUp = cPanel.tUp;
    }

    void Move()
    {
        //if (characterScripts[charactersIndex].isAttacking == true) isAttacking = true; //공격중 체크
        //else isAttacking = false;

        //방향키 이동
        dirXY = Vector3.right * h + Vector3.up * v;
        dirXY.Normalize();


        if (curCharactersCount != 0)
        {
            if (isAttacking == true && isDashing == false && isRunning)
                rigid.velocity = new Vector2(dirXY.x, dirXY.y) * (speed / 2);
            if (isAttacking == false && isDashing == false && isRunning)
                rigid.velocity = new Vector2(dirXY.x, dirXY.y) * speed;
            if (isDashing == true)
                rigid.velocity = new Vector2(dirXY.x, dirXY.y) * (speed * 4f);
            if (isStopping == true || !controlList[charactersIndex])
                rigid.velocity = Vector2.zero;
        }

        if (curCharactersCount == 0)
        {
            this.gameObject.layer = 12;
            rigid.velocity = Vector3.zero;
        }

        /*if (isAttacking)
        {
            StartCoroutine(Push());
        }*/

        if (dirXY.x != 0 || dirXY.y != 0 || dirXY.z != 0) isRunning = true;
        else isRunning = false;

        //포지션 보정
        x = dirXY.x; y = dirXY.y;
        if (x == 0 && y == 0) { playerPosition.transform.localPosition = new Vector3(0, 0, 0); }//센터
        if (x >= 1 && y == 0) { playerPosition.transform.localPosition = new Vector3(-0.08f, 0, 0); }//좌
        if (x <= -1 && y == 0) { playerPosition.transform.localPosition = new Vector3(0.08f, 0, 0); }//우
        if (x == 0 && y >= 1) { playerPosition.transform.localPosition = new Vector3(0, -0.08f, 0); }//상
        if (x == 0 && y <= -1) { playerPosition.transform.localPosition = new Vector3(0, 0.08f, 0); }//하
        if (x >= 0.5 && y >= 0.5) { playerPosition.transform.localPosition = new Vector3(-0.05f, -0.05f, 0); }//우상
        if (x <= -0.5 && y >= 0.5) { playerPosition.transform.localPosition = new Vector3(0.05f, -0.05f, 0); }//좌상
        if (x >= 0.5 && y <= -0.5) { playerPosition.transform.localPosition = new Vector3(-0.05f, 0.05f, 0); }//우하
        if (x <= -0.5 && y <= -0.5) { playerPosition.transform.localPosition = new Vector3(0.05f, 0.05f, 0); }//좌하

        //마우스 이동
    }

    void Swap()
    {
        tracePos = characterLists[charactersIndex].transform.position;
        range = Vector3.Distance(tracePos, transform.position);

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
        if ((sDown2 || (cPanel.tUp && cPanel.cNum == 2)) && canSwap) { priCharIndex = charactersIndex; charactersIndex = 1; canSwap = false; curSwapDelay = 0; }
        if ((sDown3 || (cPanel.tUp && cPanel.cNum == 3)) && canSwap) { priCharIndex = charactersIndex; charactersIndex = 2; canSwap = false; curSwapDelay = 0; }
        if (sDown4 && canSwap) { priCharIndex = charactersIndex; charactersIndex = 3; canSwap = false; curSwapDelay = 0; }

        if ((sDown1 || sDown2 || sDown3 || sDown4))
        {
            equipCharacterIndex = charactersIndex;
            equipCharacter = characterLists[charactersIndex];
        }

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
    }

    void Passive()
    {
        for (int i = 0; i < 4; i++)
        {
            if (aliveList[i] == true)
            {
                switch (list[i])
                {
                    case 0: //검사 패시브
                        break;
                    case 1: //기관단총 패시브

                        break;
                    case 2: //마법사 패시브

                        break;
                    case 3: //힐러 패시브

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
            if (nearObject.tag == "Character" && hasCharactersCount != 4) //캐릭터 파티 편입
            {
                Item item = nearObject.GetComponent<Item>();
                int characterIndex = item.value;
                characterLists[hasCharactersCount] = characters[characterIndex];
                list[hasCharactersCount] = characterIndex;
                characterLists[hasCharactersCount].SetActive(true);
                characterScripts[hasCharactersCount] = characterLists[hasCharactersCount].GetComponent<Character>();
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
            Character curChar = characterScripts[charactersIndex];
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

    void EXPControl()
    {
        if(curEXP >= maxEXP)
        {
            curLV++;
            curEXP = 0;
            maxEXP += 50;
            gameManager.UpgradeChoice();
        }
    }

    public IEnumerator Dash() //대시
    {
        isDashing = true;
        this.gameObject.layer = 12;

        yield return new WaitForSeconds(0.15f);
        isDashing = false;
        isStopping = true;

        yield return new WaitForSeconds(0.2f);
        isStopping = false;
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
        characterScripts[charactersIndex].StartCoroutine("OnDamage", e_damage);
        yield return new WaitForSeconds(0.1f);
    }

    public IEnumerator onDamage_bomb_party()
    {
        //Debug.Log("bomb!");
        characterScripts[charactersIndex].StartCoroutine("OnDamage_bomb");
        yield return new WaitForSeconds(0.1f);
    }

    public void onDamabe_bullet_party(float damage)
    {
        characterScripts[charactersIndex].OnDamage_bullet(damage);
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

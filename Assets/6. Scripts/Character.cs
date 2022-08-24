using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering;
using UnityEngine;

public class Character : MonoBehaviour
{
    Rigidbody2D rigid;
    SortingGroup sortingGroup;
    Animator anim;

    //조작키
    bool fire1; //마우스1
    bool fire2; //마우스2
    bool fire2Down; //마우스2down
    bool rDown; //r
    bool skill1Down; //스킬1
    bool skill2Down; //스킬2

    //상태
    bool isRunning; //이동중
    bool isReloading; //재장전중
    bool isDamage; //데미지입음
    bool isDowned; //누움
    bool isDead; //사망
    public bool isLeaving; //이탈중
    public bool isAttacking; //공격중

    //캐릭터 스텟
    public int value; //캐릭터 구분
    public float maxHealth; //최대체력
    public float curHealth; //현재체력
    public int maxAmmo; //최대탄약
    public int curAmmo; //현재탄약
    public float maxShield = 0; //최대쉴드(저팔계 전용) //SJM
    public float curShield = 0; //현재쉴드(저팔계 전용) //SJM

    public int followValue; //캐릭터의 기차 위치
    public int posValue; //
    public float reloadTime; //재장전시간

    //딜레이
    public float maxShotDelay; //최대사격딜레이
    public float curShotDelay; //현재사격딜레이
    public float maxDashDelay; //최대닷지딜레이
    public float curDashDelay; //현재닷지딜레이
    public float maxLSkillDelay; //최대-리더액티브스킬딜레이
    public float curLSkillDelay; //현재-리더액티브스킬딜레이
    public float maxMSkillDelay; //최대-맴버액티브스킬딜레이
    public float curMSkillDelay; //현재-맴버액티브스킬딜레이
    public float maxMemberShotDelay; //최대멤버공격딜레이
    public float curMemberShotDelay; //현재멤버공격딜레이

    public Vector3 followPos; //따라가려는 기차의 위치
    public Transform parent; //파티 중심(선봉)
    public Queue<Vector3> parentPos;
    public PartyManager partyManager; //파티매니저
    public Enemy_ai enemy; //적
    public CameraController cam; //카메라

    //스킬
    public GameObject[] skillObject; //스킬오브젝트
    public GameObject skillPositionCenter; //스킬포지션(센터)

    //닷지
    public int maxDashCount = 1; //최대닷지카운트
    public int curDashCount = 0; //현재닷지카운트
    public float maxDashCooldownDelay = 1; //최대닷지쿨다운딜레이
    public float curDashCooldownDelay = 0; //현재닷지쿨다운딜레이

    //매태리얼과 스프라이트
    public Material material; //매태리얼
    public GameObject unitRoot; //캐릭터 스프라이트 모음

    //마우스 커서
    Vector2 mouse; //마우스
    float h, v; //h->horizontal(가로) v->vertical(세로)
    float z; //z
    int lookingDirection; //바라보는 방향을 0~7까지로 표현한 것
    float rotateDg;
    Vector2 dir;
    Vector2 ran;

    //거리
    float range;

    //중복스킬 방지
    bool noMoreOverlapSkill = true;


    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        parentPos = new Queue<Vector3>();
        sortingGroup = GetComponentInChildren<SortingGroup>();
        anim = GetComponentInChildren<Animator>();
    }

    void Start()
    {
        Stats();
    }

    void Update()
    {
        GetInput(); //키 입력
        Watch(); //
        Follow();
        PositionValue();
        Skill(); //스킬(일반공격, 리더스킬)
        MemberSkill(); //멤버스킬
        MemberAI();
        Reload();
        Delay();
        Watch();
        Follow();
    }

    void FixedUpdate()
    {
        Vector3 velo = Vector3.zero;

        if (!isDead && !isLeaving)
        {
            if (followValue == 1)
            {
                transform.position = Vector3.MoveTowards(transform.position, followPos, (partyManager.speed * 4 * Time.fixedDeltaTime));
                if (range > 0.4f) anim.SetFloat("runningSpeed", 0.7f);
                if (partyManager.isRunning) isRunning = true;
                if (!partyManager.isRunning) isRunning = false;

                //if(isAttacking == true) //공격중
                //{
                if ((z > 0f && z < 90f) || (z <= 0f && z > -90f)) unitRoot.transform.localScale = new Vector3(-1, 1, 1);
                if ((z >= 90f && z < 180f) || (z <= -90 && z > -180f)) unitRoot.transform.localScale = new Vector3(1, 1, 1);
                //}
                /*if(isAttacking == false) //이동중
                {
                    if (partyManager.x > 0) unitRoot.transform.localScale = new Vector3(-1, 1, 1);
                    if (partyManager.x < 0) unitRoot.transform.localScale = new Vector3(1, 1, 1);
                }*/
            }
            if (followValue != 1)
            {
                transform.position = Vector3.SmoothDamp(transform.position, followPos, ref velo, 0.09f);
                if (range > 0.2f) isRunning = true;
                if (range <= 0.2f) isRunning = false;

                if (range > 0.4f) anim.SetFloat("runningSpeed", 0.7f);
                if (range <= 0.4f) anim.SetFloat("runningSpeed", 0.3f);

                if (this.transform.position.x < followPos.x) unitRoot.transform.localScale = new Vector3(-1, 1, 1);
                if (this.transform.position.x > followPos.x) unitRoot.transform.localScale = new Vector3(1, 1, 1);
            }
        }
        if (isLeaving) isRunning = false;

        switch (followValue)
        {
            case 1:
                followPos = partyManager.Position[0].transform.position;
                sortingGroup.sortingLayerName = "Leader";
                break;
            case 2:
                followPos = partyManager.Position[1].transform.position;
                sortingGroup.sortingLayerName = "Member1";
                break;
            case 3:
                followPos = partyManager.Position[2].transform.position;
                sortingGroup.sortingLayerName = "Member2";
                break;
            case 4:
                followPos = partyManager.Position[3].transform.position;
                sortingGroup.sortingLayerName = "Member3";
                break;
        }

        if (isRunning) anim.SetBool("isRunning", true);
        if (!isRunning) anim.SetBool("isRunning", false);
    }

    void GetInput()
    {
        //if (!partyManager.t)
        //{
        fire1 = Input.GetButton("Fire1");
        fire2 = Input.GetButton("Fire2");
        fire2Down = Input.GetButtonDown("Fire2");
        rDown = Input.GetButtonDown("Reload");
        skill1Down = Input.GetButtonDown("Skill1");
        skill2Down = Input.GetButtonDown("Skill2");
        //}
    }

    void Stats() //스탯
    {
        switch (value)
        {
            case 0: //손오공
                maxHealth = 100;
                curHealth = 100;
                maxAmmo = 2;
                curAmmo = 2;
                maxLSkillDelay = 5f;
                maxMSkillDelay = 5f;
                break;
            case 1: //저팔계
                maxHealth = 200;
                curHealth = 200;
                maxAmmo = 2;
                curAmmo = 2;
                maxLSkillDelay = 5f;
                maxMSkillDelay = 5f;
                maxShield = 1000;
                curShield = maxShield;
                break;
            case 2: //사오정
                maxHealth = 75;
                curHealth = 75;
                maxAmmo = 20;
                curAmmo = 20;
                maxLSkillDelay = 5f;
                maxMSkillDelay = 5f;
                break;
            case 3: //??
                break;

        }
    }

    void PositionValue()
    {
        if (this.gameObject == partyManager.characterLists[0])
            posValue = 0;
        if (this.gameObject == partyManager.characterLists[1])
            posValue = 1;
        if (this.gameObject == partyManager.characterLists[2])
            posValue = 2;
        if (this.gameObject == partyManager.characterLists[3])
            posValue = 3;
    }

    void Watch()
    {
        if (curHealth > maxHealth)
            curHealth = maxHealth;
        if (curHealth <= 0 && !isDead)
            StartCoroutine(DownednDead());

        //캐릭터 전환 밸류
        if (partyManager.hasCharactersCount == 2)
        {
            switch (partyManager.charactersIndex)
            {
                case 0:
                    if (posValue == 0)
                        followValue = 1;
                    else if (posValue == 1)
                        followValue = 2;
                    break;
                case 1:
                    if (posValue == 0)
                        followValue = 2;
                    else if (posValue == 1)
                        followValue = 1;
                    break;
            }
        }
        else if (partyManager.hasCharactersCount == 3)
        {
            switch (partyManager.charactersIndex)
            {
                case 0:
                    if (posValue == 0)
                        followValue = 1;
                    else if (posValue == 1)
                        followValue = 2;
                    else if (posValue == 2)
                        followValue = 3;
                    break;
                case 1:
                    if (posValue == 0)
                        followValue = 3;
                    else if (posValue == 1)
                        followValue = 1;
                    else if (posValue == 2)
                        followValue = 2;
                    break;
                case 2:
                    if (posValue == 0)
                        followValue = 2;
                    else if (posValue == 1)
                        followValue = 3;
                    else if (posValue == 2)
                        followValue = 1;
                    break;
            }
        }
        else if (partyManager.hasCharactersCount == 4)
        {
            switch (partyManager.charactersIndex)
            {
                case 0:
                    if (posValue == 0)
                        followValue = 1;
                    else if (posValue == 1)
                        followValue = 2;
                    else if (posValue == 2)
                        followValue = 3;
                    else if (posValue == 3)
                        followValue = 4;
                    break;
                case 1:
                    if (posValue == 0)
                        followValue = 4;
                    else if (posValue == 1)
                        followValue = 1;
                    else if (posValue == 2)
                        followValue = 2;
                    else if (posValue == 3)
                        followValue = 3;
                    break;
                case 2:
                    if (posValue == 0)
                        followValue = 3;
                    else if (posValue == 1)
                        followValue = 4;
                    else if (posValue == 2)
                        followValue = 1;
                    else if (posValue == 3)
                        followValue = 2;
                    break;
                case 3:
                    if (posValue == 0)
                        followValue = 2;
                    else if (posValue == 1)
                        followValue = 3;
                    else if (posValue == 2)
                        followValue = 4;
                    else if (posValue == 3)
                        followValue = 1;
                    break;
            }
        }

        //그림자
        if (followValue == 1 && curHealth > 0)
        {
            gameObject.tag = "Leader";
            if (!isDamage)
                gameObject.layer = 9;
            material.color = new Color(1, 1, 1, 1);
        }
        else if (followValue == 2 && curHealth > 0)
        {
            gameObject.tag = "Member";
            gameObject.layer = 13;
            if (!isLeaving)
                material.color = new Color(215 / 255f, 215 / 255f, 215 / 255f, 1f);
            //spriteRenderer.color = new Color(1, 1, 1, 0.6f);
        }
        else if (followValue == 3 && curHealth > 0)
        {
            gameObject.tag = "Member";
            gameObject.layer = 13;
            if (!isLeaving)
                material.color = new Color(175 / 255f, 175 / 255f, 175 / 255f, 1f);
            //spriteRenderer.color = new Color(1, 1, 1, 0.5f);
        }
        else if (followValue == 4 && curHealth > 0)
        {
            gameObject.tag = "Member";
            gameObject.layer = 13;
            if (!isLeaving)
                material.color = new Color(135 / 255f, 135 / 255f, 135 / 255f, 1f);
            //spriteRenderer.color = new Color(1, 1, 1, 0.3f);
        }

        range = Vector3.Distance(followPos, transform.position);
    }

    void Follow()
    {

        mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        z = (Mathf.Atan2(mouse.y, mouse.x) * Mathf.Rad2Deg);

        if (isDead) return;

        if ((z > 0f && z < 22.5f) || (z < 0f && z > -22.5f)) //동
        {
            lookingDirection = 1;
        }
        else if (z > 22.5f && z < 67.5) // 동북
        {
            lookingDirection = 2;
        }
        else if (z > 67.5f && z < 112.5f) // 북
        {
            lookingDirection = 3;
        }
        else if (z > 112.5f && z < 157.5f) //서북
        {
            lookingDirection = 4;
        }
        else if ((z > 157.5f && z < 180f) || (z < -157.5f && z > -180f)) //서
        {
            lookingDirection = 5;
        }
        else if (z > -157.5f && z < -112.5f) //서남
        {
            lookingDirection = 6;
        }
        else if (z > -112.5f && z < -67.5f) //남
        {
            lookingDirection = 7;

        }
        else if (z > -67.5f && z < -22.5f) // 동남
        {
            lookingDirection = 8;
        }

    }

    void Skill()
    {
        if (isDead) return;

        Vector2 playerPos = rigid.position;
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        float dy = mousePos.y - playerPos.y;
        float dx = mousePos.x - playerPos.x;

        rotateDg = Mathf.Atan2(dy, dx) * Mathf.Rad2Deg;

        dir = (mousePos - playerPos);
        ran = new Vector2(Random.Range(-0.3f, 0.3f), Random.Range(-0.3f, 0.3f));

        if (partyManager.charactersIndex == posValue)
        {
            //캐릭터 구분
            switch (value)
            {
                case 0: //손오공
                    if (fire1 && !isReloading) //일반공격(근접)
                    {
                        if (curShotDelay < maxShotDelay || !partyManager.canSwap)
                            return;

                        StartCoroutine(NormalAttack(rotateDg));
                    }
                    if (fire2Down) //리더 액티브 스킬-회피(대시)
                    {
                        if (curDashDelay < maxDashDelay || curDashCount == 0 || !partyManager.canSwap)
                            return;

                        curDashCooldownDelay = 0;
                        curDashCount--;
                        curDashDelay = 0;
                        partyManager.StartCoroutine("Dash"); //닷지
                        StartCoroutine(Push(0)); //닷지
                    }
                    break;
                case 1: //저팔계
                    if (fire1 && !isReloading) //화염구
                    {
                        if (curShotDelay < maxShotDelay || !partyManager.canSwap)
                            return;
                        StartCoroutine(NormalAttack(rotateDg));
                    }
                    if (fire2Down) //리더 액티브 스킬-이탈형 스킬, 어그로
                    {
                        if (!partyManager.canSwap)
                            return;
                        StartCoroutine(LeavingSkill());
                    }
                    break;
                case 2: //사오정
                    if (fire1 && !isReloading) //전체 힐
                    {
                        if (curShotDelay < maxShotDelay || curAmmo <= 0 || !partyManager.canSwap)
                            return;

                        if (curAmmo > 0)
                        {
                            StartCoroutine(NormalAttack(rotateDg));
                        }
                    }
                    if (fire2Down) //리더 액티브 스킬-일직선 밀쳐내기 + 저격
                    {
                        if (curLSkillDelay < maxLSkillDelay || !partyManager.canSwap)
                            return;
                        curLSkillDelay = 0;
                        StartCoroutine(Push(rotateDg));
                    }
                    if (skill1Down) //5초간 파티 스피드 +25% 올림
                    {
                        if (curLSkillDelay < maxLSkillDelay || !partyManager.canSwap)
                            return;
                        curLSkillDelay = 0;
                        StartCoroutine(SupportSkill());
                    }
                    break;
                case 3: //??
                    break;
            }
        }
        if (partyManager.isDashing)
        {
            switch (value)
            {
                case 0: //손오공
                    StartCoroutine("DashRail");
                        //curDashRailDelay = 0f;
                    //}
                    break;
            }
        }
    }

    void MemberSkill() //멤버스킬
    {
        GameObject bullet;

        for (int i = 0; i < 4; i++)
        {
            if (partyManager.aliveList[i] == true && partyManager.charactersIndex != posValue)
            {
                switch (value)
                {
                    case 0: //손오공
                        if (curMSkillDelay < maxMSkillDelay)
                            return;
                        else if (curMSkillDelay >= maxMSkillDelay)
                        {

                        }
                        break;
                    case 1: //저팔계
                        if (curMSkillDelay < maxMSkillDelay)
                            return;
                        else if (curMSkillDelay >= maxMSkillDelay)
                        {

                        }
                        break;
                    case 2: //사오정
                        if (curMSkillDelay < maxMSkillDelay)
                            return;
                        else if (curMSkillDelay >= maxMSkillDelay)
                        {
                            bullet = Instantiate(skillObject[3], this.transform.position, Quaternion.Euler(0, 0, 0));
                            curMSkillDelay = 0;
                        }
                        break;
                    case 3: //??
                        break;

                }
            }
        }
    }

    void MemberAI()
    {
        if (this.gameObject.tag == "Member" && isLeaving == false)
        {
            if (curMemberShotDelay >= maxMemberShotDelay)
            {
                StartCoroutine(NormalAttack(rotateDg));
                curMemberShotDelay = 0;
            }
        }
    }

    void Reload()
    {
        if ((rDown && partyManager.charactersIndex == posValue || curAmmo == 0) && !isReloading)
        {
            if (curAmmo >= maxAmmo)
                return;
            else if (curAmmo < maxAmmo)
            {
                isReloading = true;
                Invoke("ReloadOut", reloadTime);
            }
        }
    }

    void ReloadOut()
    {
        this.curAmmo = this.maxAmmo;
        isReloading = false;
    }

    void Delay()
    {
        curShotDelay += Time.deltaTime;
        curDashDelay += Time.deltaTime;
        curLSkillDelay += Time.deltaTime;
        curMSkillDelay += Time.deltaTime;

        if (curDashCount < maxDashCount) //닷지카운트
        {
            curDashCooldownDelay += Time.deltaTime;
        }

        if (curDashCooldownDelay >= maxDashCooldownDelay) //닷지 딜레이
        {
            if (curDashCount < maxDashCount)
            {
                curDashCount = maxDashCount;
                curDashCooldownDelay = 0;
            }
        }

        if (this.gameObject.tag == "Member")
        {
            curMemberShotDelay += Time.deltaTime;
        }
        else
        {
            curMemberShotDelay = 0f;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy" && isLeaving && value == 1)
        {
            StartCoroutine("ShieldDamaged");
        }
    }

    IEnumerator ShieldDamaged()
    {
        curShield -= enemy.damage;
        yield return null;
    }

    IEnumerator NormalAttack(float rotateDg) //일반공격
    {

        GameObject bullet;
        Rigidbody2D rigid;

        switch (value)
        {
            case 0: //손오공
                curAmmo--;
                skillObject[0].SetActive(true);
                skillObject[0].transform.rotation = Quaternion.Euler(0, 0, rotateDg);
                isAttacking = true;
                anim.SetTrigger("ClosedAttack");
                curShotDelay = 0;
                yield return new WaitForSeconds(0.25f);
                AttackFinished();
                break;
            case 1: //저팔계
                curAmmo--;
                for (int index = 0; index < 7; index++)
                {
                    bullet = Instantiate(skillObject[0], skillPositionCenter.transform.position, Quaternion.Euler(0, 0, rotateDg));
                    rigid = bullet.GetComponent<Rigidbody2D>();
                    dir += ran;
                    rigid.AddForce(dir.normalized * Random.Range(20f, 38f), ForceMode2D.Impulse);
                }
                isAttacking = true;
                anim.SetTrigger("MagicAttack");

                curShotDelay = 0;

                yield return new WaitForSeconds(0.4f);
                AttackFinished();
                break;
            case 2: //사오정
                isAttacking = true;
                anim.SetTrigger("MagicSkill");
                skillObject[0].SetActive(true);

                curShotDelay = 0;
                yield return new WaitForSeconds(0.4f);
                AttackFinished();
                break;
            case 3:
                break;
        }

        isAttacking = false;
    }

    IEnumerator LeavingSkill()
    {
        int leavingIndex = partyManager.charactersIndex; //인덱스 저장

        partyManager.CanSwapFalse();
        isLeaving = true;

        //캐릭터 변경 매커니즘
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
        if (partyManager.curCharactersCount <= 1)
        {
            //스킬 안써짐
            isLeaving = false;
            Debug.Log("혼자 남았기 때문에 스킬을 쓸 수 없습니다!");
        }

        //스킬 발동
        if (isLeaving)
        {
            if (value == 1) //저팔계
            {
                anim.SetTrigger("MagicSkill");
                yield return new WaitForSeconds(0.2f);
                skillObject[1].SetActive(true);
                cam.Shake(0.4f, 1);

                for (int i = 0; i < 5; i++)
                {
                    yield return new WaitForSeconds(1);
                    StartCoroutine(Check(leavingIndex));
                }

                isLeaving = false;
            }
        }
        if (!isLeaving) //파티에 돌아옴
        {
            skillObject[1].SetActive(false);
            partyManager.controlList[leavingIndex] = true;
            curShield = maxShield;
        }

        yield return null;
    } //이탈형 스킬

    IEnumerator Check(int leavingIndex)
    {
        if (partyManager.curCharactersCount <= 1)
        {
            //스킬 안써짐
            isLeaving = false;
            Debug.Log("혼자 남았기 때문에 스킬을 쓸 수 없습니다!");
        }
        if (curShield <= 0)
        {
            isLeaving = false;
            Debug.Log("쉴드 깨짐!");
        }

        if (!isLeaving)
        {
            skillObject[1].SetActive(false);
            partyManager.controlList[leavingIndex] = true;
        }

        yield return null;
    }

    IEnumerator SupportSkill()
    {
        if (value == 2)
        {
            partyManager.speed += (partyManager.speed / 4);
            yield return new WaitForSeconds(5f);
            partyManager.speed = 10f;
        }
    } //지원형 스킬

    IEnumerator Push(float rotateDg)
    {
        if (value == 0) //손오공
        {
            yield return new WaitForSeconds(0.2f);
            cam.Shake(0.3f, 1);
            skillObject[2].SetActive(true);
            yield return new WaitForSeconds(0.3f);
            skillObject[2].SetActive(false);
        }
        if (value == 2) //사오정
        {
            partyManager.isStopping = true;
            skillObject[1].SetActive(true);
            yield return new WaitForSeconds(0.3f);
            skillObject[1].SetActive(false);
            GameObject bullet = Instantiate(skillObject[2], skillPositionCenter.transform.position, Quaternion.Euler(0, 0, rotateDg));
            yield return new WaitForSeconds(0.1f);
            partyManager.isStopping = false;
            cam.Shake(0.6f, 2);
        }
        /*if(value == 2) //원으로 밀쳐내기
        {
            skillObject[1].SetActive(true);
            partyManager.gameObject.layer = 12;
            yield return new WaitForSeconds(0.2f);
            skillObject[1].SetActive(false);
            partyManager.gameObject.layer = 10;
        }*/
    }
    IEnumerator DashRail()
    {
        yield return new WaitForSeconds(0.01f);
        GameObject bullet = Instantiate(skillObject[1], skillPositionCenter.transform.position, Quaternion.Euler(0, 0, 0));
    }

    void AttackFinished()
    {
        isAttacking = false;

        if(value == 0)
        {
            skillObject[0].SetActive(false);
        }
        if (value == 2)
        {
            skillObject[0].SetActive(false);
        }
    }

    public IEnumerator OnDamage(int e_damage)
    {
        curHealth -= e_damage;
        cam.Shake(0.2f, 1);
        if (curHealth <= 0)
        {
            //사망, 누움
            //퀵버튼 리셋
            StartCoroutine(DownednDead());
        }

        yield return null;

    }

    public IEnumerator OnDamage_bomb()
    {
        curHealth -= enemy.damage_skill;
        cam.Shake(0.4f, 1);
        if (curHealth <= 0)
        {
            //사망, 누움
            //퀵버튼 리셋
            StartCoroutine(DownednDead());
        }
        yield return null;
    }

    public void OnDamage_bullet(float damage)
    {
        curHealth -= damage;
        cam.Shake(0.3f, 1);
        if (curHealth <= 0)
        {
            //사망, 누움
            //퀵버튼 리셋
            StartCoroutine(DownednDead());
        }
    }


    IEnumerator DownednDead() //사망
    {
        curHealth = 0;
        partyManager.priCharIndex = -1;

        isDead = true;
        partyManager.aliveList[partyManager.charactersIndex] = false;
        partyManager.controlList[partyManager.charactersIndex] = false;
        this.gameObject.layer = 14;
        this.gameObject.tag = "Downed";
        anim.SetBool("isDead", true);
        anim.SetTrigger("Death");

        //사망시 시간 느려지기
        StartCoroutine(partyManager.SlowDown(0.3f, true));

        partyManager.curCharactersCount--;

        if (partyManager.curCharactersCount > 0)
        {
            //사망 시 변경 매커니즘
            while (!partyManager.aliveList[partyManager.charactersIndex])
            {
                if (partyManager.charactersIndex != partyManager.hasCharactersCount - 1)
                    partyManager.charactersIndex++;
                else if (partyManager.charactersIndex == partyManager.hasCharactersCount - 1)
                    partyManager.charactersIndex = 0;
            }

        }
        else if (partyManager.curCharactersCount <= 0)
        {
            //Game Over
            partyManager.charactersIndex = 0;
            Debug.Log("Game Over!");
        }

        yield return null;
    }

    public IEnumerator Alive(int posV)
    {
        isDead = false;
        partyManager.aliveList[posV] = true;
        partyManager.controlList[posV] = true;
        this.gameObject.layer = 10;
        this.gameObject.tag = "Member";
        anim.SetBool("isDead", false);
        curHealth = maxHealth / 2;
        partyManager.curCharactersCount++;
        yield return null;
    }
}

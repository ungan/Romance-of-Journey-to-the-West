using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_Player : MonoBehaviour
{
    Rigidbody2D rigid;

    bool fire1; //마우스1
    bool fire2; //마우스2

    public float maxShotDelay; //최대사격딜레이
    public float curShotDelay; //현재사격딜레이

    public GameObject[] skillObject; //스킬오브젝트
    public GameObject skillPositionCenter; //스킬포지션(센터)

    //마우스 커서
    Vector2 mouse; //마우스
    float h, v; //h->horizontal(가로) v->vertical(세로)
    float z; //z
    int lookingDirection; //바라보는 방향을 0~7까지로 표현한 것
    float rotateDg;
    Vector2 dir;
    Vector2 ran;

    //오브젝트매니저
    ObjectManager objectManager;

    private void Awake()
    {
        objectManager = GameObject.Find("ObjectManager").GetComponent<ObjectManager>();
    }
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    
    void Update()
    {
        GetInput(); //키 입력
        Follow();
        Skill(); //스킬(일반공격, 리더스킬)
        Delay();
    }

    void GetInput()
    {
        fire1 = Input.GetButton("Fire1");
        fire2 = Input.GetButton("Fire2");
    }

    void Follow()
    {
        mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        z = (Mathf.Atan2(mouse.y, mouse.x) * Mathf.Rad2Deg);
    }

    void Skill()
    {
        Vector2 playerPos = rigid.position;
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        float dy = mousePos.y - playerPos.y;
        float dx = mousePos.x - playerPos.x;

        rotateDg = Mathf.Atan2(dy, dx) * Mathf.Rad2Deg;

        dir = (mousePos - playerPos);

        if (fire1) //원거리 공격
        {
            if (curShotDelay < maxShotDelay)
                return;
            StartCoroutine(RangeAttack(rotateDg)); //노말 어택 코루틴 실행
        }
        if (fire2) //근거리 공격
        {
            if (curShotDelay < maxShotDelay)
                return;
            StartCoroutine(ClosedAttack(rotateDg)); //노말 어택 코루틴 실행
        }
    }

    void Delay()
    {
        curShotDelay += Time.deltaTime;
    }

    IEnumerator RangeAttack(float rotateDg)
    {
        GameObject bullet;
        Rigidbody2D rigid;

        bullet = objectManager.MakeObj("Shotgun Bullet", skillPositionCenter.transform.position, Quaternion.Euler(0, 0, rotateDg));
        rigid = bullet.GetComponent<Rigidbody2D>();
        rigid.AddForce(dir.normalized * 20, ForceMode2D.Impulse); //탄알 날려보내기

        curShotDelay = 0;

        yield return null;
    }

    IEnumerator ClosedAttack(float rotateDg)
    {
        skillObject[0].SetActive(true);
        skillObject[0].transform.rotation = Quaternion.Euler(0, 0, rotateDg);

        curShotDelay = 0;
        yield return new WaitForSeconds(0.25f);
        skillObject[0].SetActive(false);
    }
}

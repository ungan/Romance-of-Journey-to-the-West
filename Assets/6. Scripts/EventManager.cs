using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public MusicManager musicManager;
    public ObjectManager objectManager;

    //Event
    public float curEChangeDelay = 0f; //현재 노말->호드 이벤트 체인지 딜레이
    public float maxEChangeDelay = 20f; //최대 노말->호드 이벤트 체인지 딜레이
    public float curSpawnDelay = 0f; //현재 스폰 딜레이
    public float maxSpawnDelay = 1f; //최대 스폰 딜레이
    public float curEliteSpawnDelay = 0f; //현재 엘리트 스폰 딜레이
    public float maxEliteSpawnDelay = 10f; //최대 엘리트 스폰 딜레이
    public bool normalEvent; //일반 이벤트
    public bool hordeEventIntro; //호드 이벤트 인트로(전주)
    public bool hordeEvent; //호드 이벤트
    public bool bossEvent; //보스 이벤트
    //Normal

    //Horde
    //Delay
    public float curHordeDelayIntro = 0f; //현재 호드 이벤트 딜레이
    public float maxHordeDelayIntro = 10f; //최대 호드 이벤트 딜레이
    public float curHordeDelay = 0f; //현재 호드 딜레이
    public float maxHordeDelay = 60f; //최대 호드 딜레이
    public float curBreakTimeDelay = 0f; //현재 호드 딜레이
    public float maxBreakTimeDelay = 15f; //최대 호드 딜레이
    //HordeBalance
    public int curPhase = 0; //현재 페이즈
    public int maxPhase = 4; //최대 페이즈
    public int curMonsterCount = 0;
    public int maxMonsterCount = 30;
    public bool hordeBreakTime;
    public int ranZone; //스폰 지점
    //Boss 

    //SpawnZone
    public GameObject[] enemySpawnZone;
    //public Transform[] enemySpawnZone; //적스폰존

    //Enemy
    public GameObject[] enemies_Normal;
    public GameObject[] enemies_Elite;
    public GameObject[] enemies_Special;
    public List<int> enemyList;

    void Start()
    {
        objectManager = GameObject.Find("ObjectManager").GetComponent<ObjectManager>();

        enemyList = new List<int>();
    }
    void Update()
    {
        Event();
        Delay();
    }

    void Event()
    {
        if(curEChangeDelay >= maxEChangeDelay) //Normal -> HordeIntro
        {
            ActiveHordeEventIntro(); //호드 인트로 전환
            musicManager.playMusic = true;
            curEChangeDelay = 0;
        }
        if(curHordeDelayIntro >= maxHordeDelayIntro) //HordeIntro -> Horde
        {
            ranZone = Random.Range(0, enemySpawnZone.Length);
            ActiveHordeEvent(); //호드 전환
            musicManager.playMusic = true;
            curHordeDelayIntro = 0;
        }
        if(curHordeDelay >= maxHordeDelay || curPhase >= maxPhase) //Horde -> Normal
        {
            ActiveNormalEvent(); //노말 전환
            musicManager.playMusic = false;
            curHordeDelay = 0;
            curPhase = 0;
        }

        if (normalEvent)
        {
            NormalSpawn();
        }
        else if (hordeEventIntro)
        {
            //NormalSpawn();
        }
        else if (hordeEvent)
        {
            HordeSpawn();
        }
        else if (bossEvent)
        {
            BossSpawn();
        }
    }

    void Delay()
    {
        if(hordeEvent) { curHordeDelay += Time.deltaTime; curSpawnDelay += Time.deltaTime; curEliteSpawnDelay += Time.deltaTime; }
        if(normalEvent) curEChangeDelay += Time.deltaTime;
        if(hordeEventIntro) curHordeDelayIntro += Time.deltaTime;
        if(hordeBreakTime && hordeEvent) curBreakTimeDelay += Time.deltaTime;
    }

    void NormalSpawn() 
    {

    }

    void HordeSpawn()
    {
        if (!hordeBreakTime) curBreakTimeDelay = 0;

        //페이즈 넘김(쉬는시간 돌입)
        if (curMonsterCount >= maxMonsterCount) { 
            hordeBreakTime = true;
            curPhase++;
            curMonsterCount = 0;
        }
        //쉬는시간 끝
        if (curBreakTimeDelay >= maxBreakTimeDelay) {
            ranZone = Random.Range(0, enemySpawnZone.Length);
            hordeBreakTime = false;
        }

        //스폰
        if(curSpawnDelay >= maxSpawnDelay && !hordeBreakTime)
        {
            int ranPosition = Random.Range(0, enemySpawnZone[ranZone].transform.childCount);
            int ran = Random.Range(0, enemies_Normal.Length);
            GameObject instantEnemy = objectManager.MakeObj(enemies_Normal[ran].name, enemySpawnZone[ranZone].transform.GetChild(ranPosition).position, Quaternion.Euler(0, 0, 0));
            Enemy_ai enemy = instantEnemy.GetComponent<Enemy_ai>();
            //GameObject instantEnemy = Instantiate(enemies_Normal[ran], enemySpawnZone[ranZone]);
            //Enemy_ai enemy = instantEnemy.GetComponent<Enemy_ai>();
            curSpawnDelay = 0f;
            curMonsterCount++;
        }

        if(curEliteSpawnDelay >= maxEliteSpawnDelay)
        {
            int ranPosition = Random.Range(0, enemySpawnZone[ranZone].transform.childCount);
            int ran = Random.Range(0, enemies_Normal.Length);
            //GameObject instantEnemy = objectManager.MakeObj(enemies_Elite[ran].name, enemySpawnZone[ranZone].transform.GetChild(ranPosition).position, Quaternion.Euler(0, 0, 0));
            //Enemy_ai enemy = instantEnemy.GetComponent<Enemy_ai>();
            curEliteSpawnDelay = 0f;
        }
    }
    void BossSpawn()
    {

    }
    void ActiveNormalEvent()
    {
        hordeEventIntro = false;
        hordeEvent = false;
        bossEvent = false;
        normalEvent = true;
        hordeBreakTime = false;
    }
    void ActiveHordeEventIntro()
    {
        hordeEventIntro = true;
        hordeEvent = false;
        bossEvent = false;
        normalEvent = false;
    }
    void ActiveHordeEvent()
    {
        hordeEventIntro = false;
        hordeEvent = true;
        bossEvent = false;
        normalEvent = false;
        hordeBreakTime = false;
    }
    void ActiveBossEvent()
    {
        hordeEventIntro = false;
        hordeEvent = false;
        bossEvent = true;
        normalEvent = false;
    }
}
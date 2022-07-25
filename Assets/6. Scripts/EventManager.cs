using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    //Event
    public float curEChangeDelay = 0f; //현재 노말->호드 이벤트 체인지 딜레이
    public float maxEChangeDelay = 20f; //최대 노말->호드 이벤트 체인지 딜레이
    public float curSpawnDelay = 0f; //현재 스폰 딜레이
    public float maxSpawnDelay = 1f; //최대 스폰 딜레이
    public bool normalEvent; //일반 이벤트
    public bool hordeEvent; //호드 이벤트
    public bool bossEvent; //보스 이벤트
    //Normal

    //Horde
    public float curHordeDelay = 0f; //현재 호드 딜레이
    public float maxHordeDelay = 60f; //최대 호드 딜레이
    public float curBreakTimeDelay = 0f; //현재 호드 딜레이
    public float maxBreakTimeDelay = 10f; //최대 호드 딜레이
    public int curMonsterCount = 0;
    public int maxMonsterCount = 20;
    public bool hordeBreakTime;
    //Boss 

    //SpawnZone
    public Transform[] enemySpawnZone; //적스폰존

    //Enemy
    public GameObject[] enemies;
    public List<int> enemyList;

    void Start()
    {
        enemyList = new List<int>();
    }
    void Update()
    {
        Event();
        Delay();
    }

    void Event()
    {
        if(curEChangeDelay >= maxEChangeDelay)
        {
            ActiveHordeEvent();
            curEChangeDelay = 0;
        }
        if(curHordeDelay >= maxHordeDelay)
        {
            ActiveNormalEvent();
            curHordeDelay = 0;
        }

        if (normalEvent)
        {
            NormalSpawn();
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
        if(hordeEvent) { curHordeDelay += Time.deltaTime; curSpawnDelay += Time.deltaTime; }
        if(normalEvent) curEChangeDelay += Time.deltaTime;
        if (hordeBreakTime) curBreakTimeDelay += Time.deltaTime;
    }

    void NormalSpawn() 
    { 

    }

    void HordeSpawn()
    {
        //호드 쉬는시간
        if (curMonsterCount >= maxMonsterCount) hordeBreakTime = true;
        if (curBreakTimeDelay >= maxBreakTimeDelay) { hordeBreakTime = false; curBreakTimeDelay = 0; }

        //스폰
        if(curSpawnDelay >= maxSpawnDelay && !hordeBreakTime)
        {
            int ran = Random.Range(0, enemies.Length);
            int ranZone = Random.Range(0, enemySpawnZone.Length);
            GameObject instantEnemy = Instantiate(enemies[ran], enemySpawnZone[ranZone]);
            Enemy_ai enemy = instantEnemy.GetComponent<Enemy_ai>();
            curSpawnDelay = 0f;
            curMonsterCount++;
        }
    }
    void BossSpawn()
    {

    }
    void ActiveNormalEvent()
    {
        hordeEvent = false;
        bossEvent = false;
        normalEvent = true;
    }
    void ActiveHordeEvent()
    {
        hordeEvent = true;
        bossEvent = false;
        normalEvent = false;
    }
    void ActiveBossEvent()
    {
        hordeEvent = false;
        bossEvent = true;
        normalEvent = false;
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class status_abnormality : MonoBehaviour
{
    float holding_time = 5f;     // 얼마나 상태이상을 지속할것이냐
    float play_time=0;             // 얼마나 대기중이였는지
    Vector3 originPos;          //  물체 위치
    bool damageon = false;
    Enemy_ai enemy;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void stab_behavior(Enemy_ai e)     // 행동 제한 상태이상
    {
        enemy = e;
        originPos = e.transform.position;

        if (holding_time>=play_time)
        {
            play_time += Time.deltaTime;
            if(damageon == false)
            {
                StartCoroutine("self_destruct");
                //StartCoroutine("atab_damage");
            }
        }
        else
            
        {
            damageon = false;
            //StopCoroutine(atab_damage());
            return;
        }
    }

    IEnumerator self_destruct()     // 자폭
    {

        /*stop = true;
        yield return new WaitForSeconds(1.4f);
        anim.SetTrigger("death");
        //Debug.Log("aaa!");
        partyManager.StartCoroutine("onDamage_bomb_party");
        Debug.Log("stop : " + stop);
        yield return null;*/
        yield return new WaitForSeconds(0.1f);
        yield return null;
    }

    public IEnumerator atab_damage()     // 데미지 들어가는 부분
    {

        damageon = true;
        
        yield return new WaitForSeconds(0.1f);
        enemy.curHealth = enemy.curHealth - 10;
        yield return null;
    }

    public IEnumerator Shake(float _amount, float _duration)
    {
        float timer = 0;
        while (timer <= _duration)
        {
            enemy.transform.localPosition = (Vector3)Random.insideUnitCircle * _amount + originPos;

            timer += Time.deltaTime;
            yield return null;
        }
        enemy.transform.localPosition = originPos;

    }

    void stab_damage()      // 데미지가 들어오는 상태이상
    {

    }
}
